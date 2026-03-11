using ExifEdit.Helper;
using ExifReader.Exif;
using System;


namespace ExifEdit.Exif
{
    class TagConvert
    {
        public static String getValue(bool isIntel, TagType tag, DataType dataType, byte[] data, uint count, IfdTyp ifdTyp)
        {
            Tuple<String, object[]> dataValue = null;

            dataValue = TagConvert.getValueObj(isIntel, data, dataType,count);

            Object[] o = dataValue.Item2;
            String s = dataValue.Item1;

            String s1 = SpecialTags.GetValueByTag(tag, o, ifdTyp);
            return s1 != null ? s1 : s;
        }

        /// <summary>
        /// Gibt ein Tupel zurück, das auf 1 einen String mit dem Datenwert enthaelt und auf 2 die Daten als Object
        /// </summary>
        /// <param name="isIntel"></param>
        /// <param name="data"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static Tuple<String,object[]> getValueObj(bool isIntel, byte[] data, DataType dataType, uint dataCount)
        {
            String s = "";
            Object[] o = null;
            // Todo werte können mehrfach vorkommen
            o = new Object[dataCount];
            //   for ( int i =0; i<dataCount; i++)

            for (int i = 0; i < dataCount; i++)
            {
                switch (dataType)
                {
                    case DataType.UnsignedByte:      // 8-Bit-Wert ohne Vorzeichen (Unsigned Byte)
                        o[i] = data[i];
                        s += "" + (byte)o[i] + " ";
                        break;
                    case DataType.AscciStr:         // ASCII String Länge + 1 String ist nullterminiert
                        o[i] = (Char)data[i];
                        if (data[i] != 0) s += o[i]; 
                        break;
                    case DataType.UnsignedShort: // 16-Bit-Wert ohne Vorzeichen
                        o[i] = (ushort)Convert2.toInt(isIntel, data[i * 2], data[i * 2+1]);
                        s = "" + (ushort)o[i] + " ";
                        break;
                    case DataType.UnsignedInt:       // 32-Bit-Wert ohne Vorzeichen(Unsigned Long)
                        o[i] = (uint)Convert2.toInt(isIntel, data[i * 4], data[i * 4+1], data[i * 4+2], data[i * 4+3]);
                        s += "" + (uint)o[i] + " ";
                        break;
                    case DataType.UnsignedRational: // Bruch ohne Vorzeichen(Unsigned Rational)  4 Bytes Nenner, 4 Bytes Zähler
                        Tuple<uint,uint> tu =  Tuple.Create(Convert2.toInt(isIntel, data[i * 8], data[i *8 + 1], data[i * 8 + 2], data[i *8 + 3]), Convert2.toInt(isIntel, data[i * 8+4], data[i * 8 + 5], data[i * 8 + 6], data[i * 8 + 7]));
                        o[i] = tu;
                        s = "" + tu.Item1 + "/" + tu.Item2;
                        break;
                    case DataType.SignedByte:         // 8-Bit-Wert mit Vorzeichen(Signed Byte)
                        o[i] = (sbyte)data[i];
                        s += "" + (sbyte)o[i];
                        break;
                    case DataType.Binaer:           // Binär Beliebige Folge von Bytes
                        o[i] = data;
                        s += "" + (string)Convert2.dump(data, (uint) (i * data.Length), (uint)data.Length);
                        break;
                    case DataType.SignedShort:       // 16-Bit-Wert mit Vorzeichen
                        o[i] = (short)Convert2.toInt(isIntel, data[i * 2], data[i * 2 + 1]);
                        s += "" + (short)o[i] + " ";
                        break;
                    case DataType.SignedInt:        // 32-Bit-Wert mit Vorzeichen (Signed Long)
                        o[i] =Convert2.toInt(isIntel, data[i * 4], data[i * 4 + 1], data[i * 4 + 2], data[i * 4 + 3]);
                        if (i == 0) s = "" + (uint)o[i]; else s += "/" + (uint)o[i];
                        break;
                    case DataType.SignedRational: // Bruch mit Vorzeichen (Signed Rational   4 Bytes Nenner, 4 Bytes Zähler
                        Tuple<int, int> ti = Tuple.Create((int) Convert2.toInt(isIntel, data[i *8], data[i * 8 + 1], data[i * 8 + 2], data[i * 8 + 3]), (int) Convert2.toInt(isIntel, data[i * 8 + 4], data[i * 8 + 5], data[i * 8 + 6], data[i * 8 + 7]));
                        o[i] = ti;
                        s = "" + ti.Item1 + "/" + ti.Item2;
                        break;
                    case DataType.SignedFloat:    // Gleitkommazahl einfach (Single Float) 4 Byte
                        o[i] = (float)Convert2.toInt(isIntel, data[i * 4], data[i * 4 + 1], data[i *4 + 2], data[i * 4 + 3]);
                        s = "" + (float)o[i] + " ";
                        break;
                    case DataType.DoubleFloat:  // Gleitkommazahl doppelt(Double Float) 8 Byte
                        o[i] = (double)Convert2.toInt(isIntel, data[i * 8], data[i * 8 + 1], data[i * 8 + 2], data[i * 8+ 3], data[i * 8 + 4], data[i * 8 + 5], data[i * 8 + 6], data[i * 8 + 7]);
                        s += "" + (double)o[i] + " ";
                        break;
                }
            }
            Tuple<String, object[]> retTuple = Tuple.Create(s, o);
            return retTuple;
        }


       

        public static byte getDataSize(DataType dataType)
        {
            byte size = 0;
            switch (dataType)
            {
                case DataType.UnsignedByte:      // 8-Bit-Wert ohne Vorzeichen (Unsigned Byte)
                    size = 1;
                    break;
                case DataType.AscciStr:         // ASCII String Länge + 1 String ist nullterminiert
                    size = 0;
                    break;
                case DataType.UnsignedShort: // 16-Bit-Wert ohne Vorzeichen
                    size = 2;
                    break;
                case DataType.UnsignedInt:       // 32-Bit-Wert ohne Vorzeichen(Unsigned Long)
                    size = 4;
                    break;
                case DataType.UnsignedRational: // Bruch ohne Vorzeichen(Unsigned Rational)  4 Bytes Nenner, 4 Bytes Zähler
                    size = 8;
                    break;
                case DataType.SignedByte:         // 8-Bit-Wert mit Vorzeichen(Signed Byte)
                    size = 1;
                    break;
                case DataType.Binaer:           // Binär Beliebige Folge von Bytes
                    size = 0;
                    break;
                case DataType.SignedShort:       // 16-Bit-Wert mit Vorzeichen
                    size = 2;
                    break;
                case DataType.SignedInt:        // 32-Bit-Wert mit Vorzeichen (Signed Long)
                    size = 4;
                    break;
                case DataType.SignedRational: // Bruch mit Vorzeichen (Signed Rational   4 Bytes Nenner, 4 Bytes Zähler
                    size = 8;
                    break;
                case DataType.SignedFloat:    // Gleitkommazahl einfach (Single Float) 4 Byte
                    size = 4;
                    break;
                case DataType.DoubleFloat:  // Gleitkommazahl doppelt(Double Float) 8 Byte
                    size = 8;
                    break;
            }
            return size;
        }


    }
}
