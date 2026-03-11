using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExifEdit.Segments
{
    class JpegSegment
    {

        public char[] kenn = null;
      
        public byte[] data = null; // enthaelt auch kennung und size

        public JpegSegment(byte[] data)
        {
            this.data = data;
        }
    }
}
