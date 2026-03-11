using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExifEdit.Helper;
using ExifEdit.Tags;


namespace ExifEdit.Segments
{
    public class IPTCSeg
    {    
        const byte DELIMITER = 0x1C;

        byte[] b = null;
        public static byte delimiterDefault { get { return 0x1c; } } // Definitin des Delimiter, wird gebraucht, wenn man den Tag aus dem Programm neu erzeuht
        public  Encoding localEncoding = null;

        ushort kennung = 0;
        ushort size;
        byte[] Datenkennung;
        byte[] DataDescriptor;
        ushort TypeFiled;
        byte Stringleng;
        byte Filler;
        uint sizeOfData;

        public Dictionary<IptcTag, List<IPTCField>> IptcList { get; set; }

        /// <summary>
        /// Initalis wiith data for parse
        /// </summary>
        /// <param name="b"></param>
        public IPTCSeg(byte[] b)
        {
            this.b = b;
            localEncoding = Encoding.Default;
            IptcList = new Dictionary<IptcTag, List<IPTCField>>();
        }

        /// <summary>
        /// initialize without data - when no apt13 block found
        /// </summary>
        public IPTCSeg()
        {

            IptcList = new Dictionary<IptcTag, List<IPTCField>>();
            kennung = 0xFFED;  //APP13 = 0xFFED
            Datenkennung = new byte[] { 0x50, 0x68, 0x6F, 0x74,  0x6f, 0x73, 0x68,0x6f,0x70, 0x20, 0x33, 0x2e, 0x30,0x00 };
            DataDescriptor = new byte[] { 0x38, 0x42, 0x49, 0x4d };  //8BIM
            TypeFiled = 0x0404;
            localEncoding = Encoding.Default; // use as default

            //Add the first 2 Segments
            //Encoding
             byte[] dataEncoding = { 0x01b, 0x25, 0x47 };
             List<IPTCField> list2 = new List<IPTCField>();
             list2.Add(new IPTCField(delimiterDefault, (ushort) IptcTag.CODED_CHARACTER_SET, 3, dataEncoding, Encoding.ASCII));
             IptcList.Add(IptcTag.CODED_CHARACTER_SET, list2);
            
            //Recorded Version
            byte[] dataRecordedVersion = { 0x01b, 0x04 };
            List<IPTCField> list3 = new List<IPTCField>();
            list3.Add(new IPTCField(delimiterDefault, (ushort)IptcTag.ENVELOPE_RECORD_VERSION, 2, dataRecordedVersion, Encoding.ASCII));
            IptcList.Add(IptcTag.ENVELOPE_RECORD_VERSION, list3);

            //
        }

    
    

        /// <summary>
        /// Zerlegt APT13  IPTC segment in liste der Elemente
        /// </summary>
        public void parse()
        {
     
            byte delimiter;
            ushort  Size;
            byte[] data;
            IptcTag iTag;

            // localEncoding = null; // Solange nichts gefund wird == null

            localEncoding = Encoding.UTF8;

            kennung = Convert2.toInt(b[0], b[1]);  //APP13 = 0xFFED
          size = Convert2.toInt(b[2], b[3]);
          Datenkennung = Tools.subArray(b, 4, 14);    //Photoshop 3.0/0
          DataDescriptor = Tools.subArray(b, 18, 4);  //8BIM


            TypeFiled = Convert2.toInt(b[22], b[23]);
            Stringleng = b[24];

            Filler = b[25];

            sizeOfData = Convert2.toInt(b[26], b[27], b[28], b[29]);

            
            uint ptr = 30;
          //    while (ptr < size && b[ptr] != 0) // Fehlerhaft -benuzuz um die Bilder wieder zu reparieren
           while (ptr < size && b[ptr] == 0x1c) // DAS ist korrekt
            {
                delimiter = b[ptr];
                //    Console.WriteLine($"  {delimiter:X}    {ptr}");
                iTag = (IptcTag)Convert2.toInt(b[ptr + 1], b[ptr + 2]);    //EXCEPTION  TODO
                Size = Convert2.toInt(b[ptr + 3], b[ptr + 4]);
                data = Tools.subArray(b, (int) ptr + 5, Size);

                if (iTag == IptcTag.CODED_CHARACTER_SET)  //Tag für Encoding gefunden
                {
                    if ((data[0] == 0x1B) && (data[1] == 0x25) && (data[2] == 0x47)) { localEncoding = Encoding.UTF8; } // ESC % G = UTF-8
                    if ((data[0] == 0x1B) && (data[1] == 0x28) && (data[2] == 0x42)) { localEncoding = Encoding.ASCII; }// ESC ( B = ASCII
                    if ((data[0] == 0x1B) && (data[1] == 0x2E) && (data[2] == 0x41)) { localEncoding = Encoding.GetEncoding("iso-8859-1"); } // ESC . A = ISO 8859-1
                }
                // var localString =  localEncoding.GetString(data);
                //Console.WriteLine(localString);

                if (IptcList.ContainsKey(iTag) )
                {
                    List<IPTCField> list2 = IptcList[iTag];
                    list2.Add(new IPTCField(delimiter, iTag, Size, data, localEncoding));
                    IptcList.Remove(iTag);
                    IptcList.Add(iTag, list2);
                }
                else
                {
                    if (Enum.IsDefined(typeof(IptcTag), iTag))
                    {
                        List<IPTCField> list2 = new List<IPTCField>();
                        list2.Add(new IPTCField(delimiter, iTag, Size, data, localEncoding));
                        IptcList.Add(iTag, list2);
                    }
                    else
                    { Console.WriteLine("Weg");  }
                }
                ptr += Size ;
                ptr += 5;
            }
        }

        //Berechnett die Längen im APT 13 neu
        private ushort recalculate()
        {

           ushort  laenge = 0;
            foreach (List<IPTCField> list in IptcList.Values)
            {
                foreach (IPTCField field in list)
                {

                    if (field.data != null && field.data.Length >0)
                    {
                        //  Console.WriteLine(Convert2.dumpStr(field.data));
                        field.size = (ushort)field.data.Length;
                        laenge += (ushort)(field.size + 5);
                    }
                }
            }
            return laenge;
        }

        public void toFile(BinaryWriter wr)
        {
            // Wenn kein Encoding vorhandne ist, schreiben wir eins als UTF-8

            if (!IptcList.Keys.Contains( IptcTag.CODED_CHARACTER_SET))
            {
                byte[] data = new byte[3];
                data[0] = 0x1B;  
                data[1] = 0x25; 
                data[2] = 0x47;
                IPTCField encoding = new IPTCField(DELIMITER, IptcTag.CODED_CHARACTER_SET, 3, data, localEncoding);

                List<IPTCField> list = new List<IPTCField>();
                list.Add(encoding);
                IptcList.Add(IptcTag.CODED_CHARACTER_SET, list);

            }

                ushort diff = (ushort) (size - sizeOfData);
         //   Console.WriteLine($"Size of data {sizeOfData}   {size}  {diff} ");
            sizeOfData = recalculate();
            size = (ushort) (sizeOfData + 28);
            if (size % 2 == 1) { size++; } //scheint immer gerade sein zu muessen
          //  Console.WriteLine($"Size of data {sizeOfData}     {size}");

       

            wr.Write(Tools.toByteArray(kennung));
            wr.Write(Tools.toByteArray(size)); // neuberechnen

            wr.Write(Datenkennung);
            wr.Write(DataDescriptor);
            wr.Write(TypeFiled);
            wr.Write(Stringleng);
            wr.Write(Filler);
            wr.Write(Tools.toByteArray(sizeOfData)); //Neuberechnene

            IptcList = IptcList.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            foreach (List<IPTCField> list in IptcList.Values)
            {
                foreach ( IPTCField field in list)
                    {
                        if (field.data != null && field.data.Length > 0)
                        {
                            wr.Write(field.Delimiter);
                            wr.Write(Tools.toByteArray(field.iTag));
                            wr.Write(Tools.toByteArray(field.size));
                            wr.Write(field.data);
                        }
                    }
            }

            if ((sizeOfData + 28) % 2 == 1) //keien Ahnung warum, scheint aber so zu sein
            { wr.Write((byte) 0x0A); }
        }


        public IptcTag[] getTagsList()
        {
            return IptcList.Keys.ToArray<IptcTag>();
        }

     


        public String getElement (IptcTag iTag)
        {
            String s = "";
 
            if (IptcList.ContainsKey(iTag))
            {
                List<IPTCField> list2 = IptcList[iTag];
                
                foreach (IPTCField field in list2)
                {
                   s+= field.DataString+"*";
                }
            }

            return s.Length == 0  ? s:  s.Remove(s.Length-1);
        }


        public void setElement(IptcTag iTag, string s)
        {
           

            if (IptcList.ContainsKey(iTag))
            {
                List<IPTCField> list2 = IptcList[iTag];

                if (list2.Count == 1)
                {
                    list2[0].DataString = s;
                }

                   else  if (list2.Count > 1)
                {
                    foreach (IPTCField field in list2)
                    {
                        list2.Remove(field);
                    }
                }

                
            }

        }
    }
}
