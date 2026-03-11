using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExifEdit.Helper
{
    static class Convert2
    {
        /// <summary>
        /// if set, the internal converter works with this method - used for tiff, tiff can have intel or Motorola like format
        /// </summary>
        public static bool isIntelGlobal { get; 
            set; }


        /// <summary>
        /// convert a byte to bin-String
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static String toBin(byte x)
        {
            String s = Convert.ToString(x, 2);
            while (s.Length < 8)
            { s = "0" + s; }
            return s.Substring(0, 4) + " " + s.Substring(4, 4);
        }

        /// <summary>
        /// convert to binarry , bits like 7654_3210
        /// </summary>
        /// <param name="x"></param>
        /// <param name="fromBit"></param>
        /// <param name="toBit"></param>
        /// <returns></returns>
        public static String toBin(byte x, byte fromBit, byte toBit)
        {
            //  7 6 5 4 3 2 1 0
            //  1 2 3 4 5 6 7 8
            //  
            byte from = (byte)(8 - toBit - 1);
            byte to = (byte)(8 - fromBit - 1);
            String s = Convert.ToString(x, 2);
            while (s.Length < 8)
            { s = "0" + s; }
            String ss = s.Substring(0, 4) + " " + s.Substring(4, 4);
            if (to >= 4) to++;
            if (from >= 4) from++;
            ss = ss.Insert(to + 1, ">");
            ss = ss.Insert(from, "<");
            return ss;
        }


        public static ushort toInt(bool isIntel, byte b0, byte b1)
        {

            return !isIntel ? ToIntMot(b0, b1) : toIntIntel(b0, b1);
        }

        public static ushort toInt(byte b0, byte b1)
        {

            return !isIntelGlobal ? ToIntMot(b0, b1) : toIntIntel(b0, b1);
        }


        // 2 Byte
        public static ushort ToIntMot(byte b0, byte b1)
        {
            ushort r = 0;
            r = (ushort)_toIntMotorola(b0, b1);
            return r;
        }

        public static ushort toIntIntel(byte b0, byte b1)
        {
            ushort r = 0;
            r = (ushort)_toIntIntel(b0, b1);
            return r;
        }


        // 4 Byte

        public static uint toInt(bool isIntel, byte b0, byte b1, byte b2, byte b3)
        {

            return !isIntel ? ToIntMot(b0, b1, b2, b3) : toIntIntel(b0, b1, b2, b3);
        }

        public static uint toInt(byte b0, byte b1, byte b2, byte b3)
        {

            return !isIntelGlobal ? ToIntMot(b0, b1, b2, b3) : toIntIntel(b0, b1, b2, b3);
        }


        public static uint ToIntMot(byte b0, byte b1, byte b2, byte b3)
        {
            uint r = 0;
            r = (uint)_toIntMotorola(b0, b1, b2, b3);
            return r;
        }

        public static uint toIntIntel(byte b0, byte b1, byte b2, byte b3)
        {
            uint r = 0;
            r = (uint)_toIntIntel(b0, b1, b2, b3);
            return r;
        }

        // 8 Byte

        public static ulong toInt(bool isIntel, byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7)
        {
            return !isIntel ? ToIntMot(b0, b1, b2, b3, b4, b5, b6, b7) : toIntIntel(b0, b1, b2, b3, b4, b5, b6, b7); ;
        }

        public static ulong toInt(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7)
        {
            return !isIntelGlobal ? ToIntMot(b0, b1, b2, b3, b4, b5, b6, b7) : toIntIntel(b0, b1, b2, b3, b4, b5, b6, b7); ;
        }


        public static ulong ToIntMot(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7)
        {
            ulong r = 0;
            r = (ulong)_toIntMotorola(b0, b1, b2, b3, b4, b5, b6, b7);
            return r;
        }

        public static ulong toIntIntel(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7)
        {
            ulong r = 0;
            r = (ulong)_toIntIntel(b0, b1, b2, b3, b4, b5, b6, b7);
            return r;
        }
        /**
 * Converts 1..4 Bytes to a Integer ( the Bytes are in LSB Format)
 * @param b
 * @return
 *
         * Bytes
         * 1 --> Byte
         * 2 --> ushort
         * 3,4 --> uint
         * 5-8 --> ulong
         * 
 */
        private static ulong _toIntMotorola(params byte[] b)
        {
            ulong r = 0;
            for (int i = 0; i < b.Length; i++)
            {
                r = r << 8;
                r = r | b[i];
            }
            if (b.Length > 4) { return r; }
            else if (b.Length > 2) { return (uint)r; }
            else if (b.Length == 2) { return (ushort)r; }
            else { return (byte)r; }

        }




        /**
         * Converts 1..4 Bytes to a Integer ( the Bytes are in MSB Format)
         * @param b
         * @return
         * 
         * 	public  <T> T getValue(short colNr, int rowNr, T t  )
         */
        private static ulong _toIntIntel(params byte[] b)
        {
            ulong r = 0;

            for (int i = b.Length - 1; i >= 0; i--)
            {
                r = r << 8;
                r = r | b[i];
            }

            if (b.Length > 4) { return r; }
            else if (b.Length > 2) { return (uint)r; }
            else if (b.Length == 2) { return (ushort)r; }
            else { return (byte)r; }
        }

        public static String ToString(params byte[] b)
        {

            String ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret = ret + " " + b[i].ToString("X2");
            }


            return ret;
        }

        /**
         * Dump as as Hex String 
         * example: "40,41,42,0A,31,32" 
         * @param b
         * @param start
         * @param size
         * @return
         */
        public static String dump(byte[] b, uint start, uint size)
        {
            String s = "";

            uint end = start + size;
            if (end > b.Length - 1) { end = (uint)b.Length - 1; }
            for (uint i = start; i < end; i++)

            {
                String y = ((Byte)b[i]).ToString("X2");

                s += y;
                if (i < end) s += ",";
            }
            return s;

        }

        /**
         * Duma as a char String like dump
         * example " A  B  C  .  1  2 "
         * @param b
         * @param start
         * @param size
         * @return
         */
        public static String dumpChar(byte[] b, uint start, uint size)
        {
            String s = "";
            for (uint i = start; i < start + size; i++)

            {
                if (b[i] >= 0x20 && b[i] < 0xFF)
                { s += " " + (char)b[i] + " "; }
                else
                { s += " . "; }

            }
            return s;

        }


        public static char toChar(byte b)
        {
            char c;
            if (b >= 0x20 && b < 0xFF & b != 127)
            { c = (char)b; }
            else
            { c = '.'; }
            return c;

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


        /**
      * Dump as a String
      * example "ABC.12"
      * @param b
      * @param start
      * @param size
      * @return
      */
        public static String dumpStrZero(byte[] b, ulong start, ulong size)
        {
            String s = "";
            for (ulong i = start; i < start + size; i++)
            {
                if (b[i] >= 0x20 && b[i] < 0xFF)
                { s += (char)b[i]; }
                else if (b[i] == 0)
                { break; }
                else
                { s += "."; }

            }
            return s;

        }

    }



}
