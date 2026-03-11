
namespace ExifEdit
{
    static class Tools
    {
        /**
           Copys a part of the array B
           return a Subarray of b 
           starts with from, 
           copies for maximal size bytes
           if size = 0 copie to the end;
       */
        public static byte[] subArray(byte[] b, int start, int size)
        {
            int l = b.Length;
            if (size == 0 || start + size > l) { size = l - start; }
            byte[] bx = new byte[size];                           // datenbereich kopieren
            Array.Copy(b, start, bx, 0, (long)size);
            return bx;
        }


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


        public static String dumpStr(byte[] b)
        {
            String s = "";

            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] >= 0x20 && b[i] < 0xFF)
                { s += (char)b[i]; }
                else
                { s += "."; }

            }
            return s;

        }


        public static byte[] toByteArray(ushort u)
        {
            byte[] b = new byte[2];
            b[1] = (byte)(u & 255);
            b[0] = (byte)(u >> 8);

            return b;
        }

        public static byte[] toByteArray(uint u)
        {
            byte[] b = new byte[4];
            b[3] = (byte)(u & 255);
            b[2] = (byte)((u >> 8) & 255);
            b[1] = (byte)((u >> 16) & 255);
            b[0] = (byte)((u >> 24) & 255);
            return b;
        }


        /// <summary>
        /// Sucht eine  Datei an diversen stellen
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static  String checkfilePath(String path)
        {
            ////configData.SpeciesConfigFile


            //ConfigData configData = ConfigData.getInstance();

            ////direkt
            //if (File.Exists(path)) return path;


            ////an dArbeitsverzeichnis
            //String workPath = Directory.GetCurrentDirectory() + "\\" + path;
            //if (File.Exists(workPath)) return workPath;


            ////userdata
            //String userdataPath = Application.UserAppDataPath + "\\" + path;
            //if (File.Exists(userdataPath)) return userdataPath;

            ////localuserdata
            //String localuserdataPath = Application.LocalUserAppDataPath + "\\" + path;
            //if (File.Exists(localuserdataPath)) return localuserdataPath;


            ////an dArbeitsverzeichnis
            //String appPath = Application.CommonAppDataPath + "\\" + path;
            //if (File.Exists(appPath)) return appPath;


            ////programmverzeichnis
            //String progPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" + path;
            //if (File.Exists(progPath)) return progPath;

            return null;
        }

    }
}
