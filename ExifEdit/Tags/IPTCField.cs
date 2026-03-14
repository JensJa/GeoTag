using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExifEdit.Segments;
using ExifReader.Exif;

namespace ExifEdit.Tags
{
    /// <summary>
    /// Collect the IPTC data from a file 
    /// </summary>

    public class IPTCField
    {
       
        public Encoding encoding { get; set; }
        public byte Delimiter { get; private set; }
        public ushort iTag { get; private set; }
        public ushort size { get; set; }
        public byte[] data { get; set; }

        public IptcTag Tag { get; private set; }
        public String Description  { get; set;}

        public DataType Type { get; set; }
        public bool modifyed { get; set; } = false;



        //public IPTCField( byte delimiter, ushort iTag, ushort size, byte[] data, Encoding encoding)
        //{
        //    Delimiter = delimiter;
        //    this.IFD = IFD;
        //    this.iTag = iTag;
        //    this.size = size;
        //    this.data = data;
        //    this.encoding = encoding;
        //    Tag = (IptcTag)iTag;
        //    this.Type = Type;


        //}

        public IPTCField(  byte delimiter, IptcTag tag, ushort size, byte[] data, Encoding encoding)
        {
            Delimiter = delimiter;
       
            Tag = tag;
            this.size = size;
            this.data = data;
            iTag = (ushort)tag;
            this.encoding = encoding;

        }
        
         public String DataString
        {
            get { 
            //   string sa = Encoding.ASCII.GetString(data);
            //    string su = Encoding.UTF8.GetString(data);

           //   Console.WriteLine($"   {sa} {su}      {Tools.dumpStr(data)}");
                return encoding == null ? Tools.dumpStr(data) : encoding.GetString(data);
            }
            set
            {
                if (value != null && value.Length > 0)
                { data = encoding.GetBytes(value); }
                // Encoding.UTF8.GetBytes(value)}
                else
                { size = 0; data = null; }
            }
        }
    }


}
