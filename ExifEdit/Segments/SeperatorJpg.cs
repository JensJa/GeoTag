using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ExifEdit.Helper;
using ExifEdit.Exif;
using log4net;

namespace ExifEdit.Segments
{
    /// <summary>
    /// Separiert ein Jpeg in dei einzelenen Segmente
    /// </summary>
    class SeperatorJpg
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SeperatorJpg));

        public byte[] FileAsByteStream { get; set; }

        public SeperatorJpg(byte[] byteStream)
        {
            this.FileAsByteStream = byteStream; 
        }

        private const byte noSegmentFound = 255;   //soviel Segmente sollte es nie geben

        public int app13 { get; private set; } = noSegmentFound; 
        public int app1 { get; private set; } = noSegmentFound;
        public  SortedList<int, byte[]> ListeJpegSegments { get; set; } = null;


        public void createSegemnts()
        {
            
            ListeJpegSegments = new SortedList<int, byte[]>();
            byte[] b = FileAsByteStream;

          
            ulong ptr = 2; // das erste Sgment ist beim offset 2 FF d8
            int nr = 0;
            ushort size;
            ushort seg;

            if (b.Length > 2 &&Convert2.toInt(b[0], b[1]) == 0xFFD8)
            {
                
                ListeJpegSegments.Add(nr, Tools.subArray(b, 0, 2));
                 seg = Convert2.toInt(b[2], b[3]);
                while ((SEG) seg != SEG.EOI  && (ptr+3 < (ulong) b.Length)) //nach SOS kommen die kodierten Bilddaten und dann ein EOI
                {

                    nr++;
                     seg = Convert2.toInt(b[ptr], b[ptr + 1]);     // 2 Byte Kennung
                     size = Convert2.toInt(b[ptr + 2], b[ptr + 3]); // 2 Byte Länge

                    //byte[] bx = new byte[size+2];                           // datenbereich kopieren
                    //Array.Copy(b, (long)ptr, bx, 0, (long)size+2);   //TODO
                    byte[] bx = Tools.subArray(b, (int)ptr, (int)size + 2);

                    if ((SEG)seg == SEG.APP13 && app13 == noSegmentFound)
                    {
                        //gucken ob es wirklich IPTC ist
                        if (checkIptc(bx))
                        { app13 = nr; }
                        
                    }

                    if ((SEG)seg == SEG.APP1 && app1 == noSegmentFound)
                    {
                        //gucken ob es wirklich Exif ist
                        if (checkExif(bx))
                        { app1 = nr; }

                    }


                    if ((SEG) seg != SEG.SOS)
                    {
                        ListeJpegSegments.Add(nr, bx);
                        ptr = ptr + size + 2;

                    }
                    else {
                        // rest einfach ins DIE lISTE PACKEN
                         bx = Tools.subArray(b, (int)ptr, b.Length-(int) ptr);
                        ListeJpegSegments.Add(nr, bx);
                        seg = (ushort) SEG.EOI;
                    } // keine Ahnung warum SOS nicht über die Länge bekommt 
                  
                }
            }
            else { new ArgumentException("no jpeg file", "FileAsByteStream"); }
        }

        /// <summary>
         /// Wenn noch kein backup existiert wird die Datei in .bak umebannt
         /// So wird  immer das Original aufgehoben
        /// </summary>
        /// <param name="Filename"></param>
        private void checkforBackup(String filename, bool central, String centralDir)
        {
            String backupdir="";
            FileInfo fi = new FileInfo(filename);

            if (central )
            {
                backupdir = centralDir + @"\"+fi.DirectoryName.Replace(":",@"\").Replace(@"\\",@"\");
            }
            else
            {
                backupdir = fi.DirectoryName + "\\Backup";
            }
           
            if (!File.Exists(backupdir +"\\"+fi.Name) && File.Exists(filename))
            {
                try
                {
                    Directory.CreateDirectory(backupdir);
                    File.Copy(filename, backupdir + @"\" + fi.Name);
                }
                catch (Exception ex) {
                //ToDO im Hintergrund verschieben und Asncron, mehrere Versuche
                }
            }
            //if (!File.Exists(filename + ".bak") && File.Exists(filename))
            //{
            //    File.Move(filename, filename + ".bak");
            //}
        }

        public void writeIptc2File(String filename, IPTCSeg iptc, bool backup, bool central, string centralDir)
        {

            if (backup) { checkforBackup(filename,central, centralDir); }
            byte count = 0;
            bool isWritten = false;
            do
            {
                isWritten = writeFile(filename, iptc);
                

            } while (! isWritten && count++ <10);

            if (!isWritten) { new IOException("cant' write :" + filename); }


        }

        private bool writeFile(String filename, IPTCSeg iptc)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter w = new BinaryWriter(fs))
                    {

                        for (int i = 0; i < ListeJpegSegments.Count; i++)
                        {
                            if (i == 1 && app13 == noSegmentFound) { iptc.toFile(w); }  //wenn im original keine APT13 dann schreiben wir es als ersten Block nach dem Header
                            if (i != app13)
                            { w.Write(ListeJpegSegments[i]); }
                            else
                            { iptc.toFile(w); }  //hier schreiben wir den neuen APT13 Block
                        }
                        w.Close();

                    }

                    fs.Close();
                }
                return true;
            }
            catch (IOException ex)
            {
                log.Error(ex.Message, ex);
                
                return false;
            }


            
        }


        /// <summary>
        /// CDheck ob das APP1 Segment auch exif daten enthält
        /// </summary>
        /// <param name="b">Segmentarray</param>
        /// <returns></returns>
        private bool checkExif(byte[] b)
        {
            String s = Convert2.dumpStr(b, 4, 4);
            if (s.Equals("Exif") )
            {
                return true;

            }
            else { return false; }
        }




        /// <summary>
        /// check ob das App13 Segment IPTC Daten enthaelt
        /// </summary>
        /// <param name="b">Segmentarray</param>
        /// <returns></returns>
        private bool checkIptc(byte[] b)
        {
         
            String s = Convert2.dumpStr(b, 4, 13);
            String t = Convert2.dumpStr(b, 18, 4);
            uint kenn = Convert2.toInt(b[22], b[23]);
            if (s.Equals("Photoshop 3.0") && t.Equals("8BIM") && kenn == (uint)  0x0404)
            {
                return true;

            }
            else { return false; }
        }

        public  SEG getSegmentTyp(ushort kennung)
        {
            SEG seg;
            if (Enum.IsDefined(typeof(SEG), kennung))
            {
                seg = (SEG)kennung;
            }
            else
            {
                seg = SEG.NIX;
            }
            return seg;
        }

    }

   

    public enum SEG : ushort
    {
        NIX = 0x000,    // nix bekanntes dürfte nie vorkommen
        SOI = 0xFFD8,   //Start Of Image
                        // 	SOFn = 0xFFCn,	//Start of Frame Marker, legt Art der Kompression fest:
        SOF0 = 0xFFC0,  //Baseline DCT
        SOF1 = 0xFFC1,  //Extended sequential DCT
        SOF2 = 0xFFC2,  //Progressive DCT
        SOF3 = 0xFFC3,  //Lossless (sequential)
        SOF5 = 0xFFC5,  //Differential sequential DCT
        SOF6 = 0xFFC6,  //Differential progressive DCT
        SOF7 = 0xFFC7,  //Differential lossless (sequential)
        JPG = 0xFFC8,   //reserviert für JPEG extensions
        SOF9 = 0xFFC9,  //Extended sequential DCT
        SOF10 = 0xFFCA, //Progressive DCT
        SOF11 = 0xFFCB, //Lossless (sequential)
        SOF13 = 0xFFCD, //Differential sequential DCT
        SOF14 = 0xFFCE, //Differential progressive DCT
        SOF15 = 0xFFCF, //Differential lossless (sequential)
        DHT = 0xFFC4,   //Definition der Huffman-Tabellen
        DAC = 0xFFCC,   //Definition der arithmetischen Codierung
        DQT = 0xFFDB,   //Definition der Quantisierungstabellen

        APP0 = 0xFFE0,  //JFIF tag
        APP1 = 0xFFE1,  //Exif-Daten
        APP2 = 0xFFE2,  //meist ICC_Profil
        APP3 = 0xFFE3,  //n=3..F 
        APP4 = 0xFFE4,  //
        APP5 = 0xFFE5,  //
        APP6 = 0xFFE6,  //
        APP7 = 0xFFE7,  //
        APP8 = 0xFFE8,  //
        APP9 = 0xFFE9,  //
        APP10 = 0xFFEA, //
        APP11 = 0xFFEB, //
        APP12 = 0xFFEC, //
        APP13 = 0xFFED, //IPTC
        APP14 = 0xFFEE, //Oft für Copyright-Einträge
        APP15 = 0xFFEF, //

        COM = 0xFFFE,   //Kommentare

        SOS = 0xFFDA,   //Start of Scan
        EOI = 0xFFD9,   //End of Image

        RST0 = 0xFFD0,  //Restart 0-7  
        RST1 = 0xFFD1,  //Restart 0-7 
        RST2 = 0xFFD2,  //Restart 0-7 
        RST3 = 0xFFD3,  //Restart 0-7 
        RST4 = 0xFFD4,  //Restart 0-7 
        RST5 = 0xFFD5,  //Restart 0-7 
        RST6 = 0xFFD6,  //Restart 0-7 
        RST7 = 0xFFD7   //Restart 0-7 
    }

}

  

