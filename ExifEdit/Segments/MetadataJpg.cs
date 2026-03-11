
using ExifReader.Exif;
using ExifEdit.Tags;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExifEdit.Segments
{
    class MetadataJpg
    {

        byte[] b;
        int app13 = 0;
        int app1 = -1;
        public SortedList<int, byte[]> Liste { get; set; } = null;



        /// <summary>
        /// CDheck ob das APP1 Segment auch exif daten enthält
        /// </summary>
        /// <param name="b">Segmentarray</param>
        /// <returns></returns>
        private bool checkExif(byte[] b)
        {
            String s = dumpStr(b, 4, 4);
            if (s.Equals("Exif"))
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

            String s = dumpStr(b, 4, 13); //
            String t = dumpStr(b, 18, 4);
            uint kenn = toInt(b[22], b[23]); //
            if (s.Equals("Photoshop 3.0") && t.Equals("8BIM") && kenn == (uint)0x0404)
            {
                return true;

            }
            else { return false; }
        }






        public void parsefile(String filename)
        {
            //Dictionary<uint, ExifEntrie> exifEntryList = new Dictionary<uint, ExifEntrie>();
            //b = File.ReadAllBytes(filename);




            //createSegemnts();

            ////  segListe = jpgsep.ListeJpegSegments;
            //byte[] bx;

            //SeperatorJpg seperatorJpg = new SeperatorJpg(b);
            //seperatorJpg.createSegemnts();
            //for (int i = 0; i < seperatorJpg.ListeJpegSegments.Count; i++)
            //{
            //    bx = seperatorJpg.ListeJpegSegments[i];
            //    ushort seg = toInt(bx[0], bx[1]);//
            //    //  listBox1.Items.Add("" + i + ": " + seperatorJpg.getSegmentTyp(seg));

            //}


            //if (seperatorJpg.app13 != 255)
            //{
            //    IPTCSeg IptcSeg = new IPTCSeg(seperatorJpg.ListeJpegSegments[seperatorJpg.app13]);
            //    IptcSeg.parse();

            //    IptcTag[] tags = IptcSeg.getTagsList();
            //    for (uint i = 0; i < tags.Length; i++)
            //    {
            //        Console.WriteLine(tags[i].ToString() + " " + IptcSeg.getElement(tags[i]));
            //    }
            //}

            //if (seperatorJpg.app1 != 255)
            //{
            //    //            parseExif = new ParamterParser(filename);
            //    ExifParser exifParser = new ExifParser();
            //    exifEntryList = exifParser.analyse(seperatorJpg.ListeJpegSegments[seperatorJpg.app1]);
            //    foreach (ExifEntrie ef in exifEntryList.Values)
            //    {
            //        Console.WriteLine($"({ef.ifdTyp}:{ef.id:X4}) {ef.tagtyp.ToString()} as {ef.typ.ToString()} : {ef.value} ");
            //    }

            //}
            //Console.WriteLine("done");
        }




        public void createSegemnts()
        {


            SortedList<int, byte[]> Liste = new SortedList<int, byte[]>();

            ulong ptr = 2; // das erste Sgment ist bei m offset 2 FF d8
            int nr = 0;
            ushort size;
            ushort seg;

            if (b.Length > 2 && toInt(b[0], b[1]) == 0xFFD8)
            {

                Liste.Add(nr, Tools.subArray(b, 0, 2));
                seg = toInt(b[2], b[3]);
                while ((SEG)seg != SEG.EOI) //nach SOS kommen die kodierten Bilddaten und dann ein EOI
                {

                    nr++;
                    seg = toInt(b[ptr], b[ptr + 1]);     // 2 Byte Kennung
                    size = toInt(b[ptr + 2], b[ptr + 3]); // 2 Byte Länge

                    //byte[] bx = new byte[size+2];                           // datenbereich kopieren
                    //Array.Copy(b, (long)ptr, bx, 0, (long)size+2);   //TODO
                    byte[] bx = Tools.subArray(b, (int)ptr, (int)size + 2);

                    if ((SEG)seg == SEG.APP13)
                    {
                        //gucken ob es wirklich IPTC ist
                        if (checkIptc(bx))
                        { app13 = nr; }

                    }

                    if ((SEG)seg == SEG.APP1)
                    {
                        //gucken ob es wirklich Exif ist
                        if (checkExif(bx))
                        { app1 = nr; }

                    }


                    if ((SEG)seg != SEG.SOS)
                    {
                        Liste.Add(nr, bx);
                        ptr = ptr + size + 2;

                    }
                    else
                    {
                        // rest einfach ins DIE lISTE PACKEN
                        bx = Tools.subArray(b, (int)ptr, b.Length - (int)ptr);
                        Liste.Add(nr, bx);
                        seg = (ushort)SEG.EOI;
                    } // keine Ahnung warum SOS nicht über die Länge bekommt 

                }
            }
            else { }
        }



        private ushort toInt(byte b0, byte b1)
        { ushort r = b1;
            r =(ushort) ( 256* r) ;
            return (ushort) (r + b1);
        }


  

        /**
         * Dump as a String
         * example "ABC.12"
         * @param b
         * @param start
         * @param size
         * @return
         */
        public static String dumpStr(byte[] b, ulong start, ulong size)
        {
            String s = "";
            ulong end = start + size;
            if (end >= (ulong)b.Length) { end = (ulong)b.Length - 1; }
            for (ulong i = start; i < end; i++)
            {
                if (b[i] >= 0x20 && b[i] < 0xFF)
                { s += (char)b[i]; }
                else
                { s += "."; }

            }
            return s;

        }





    }
}
