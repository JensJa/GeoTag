using ExifReader.Exif;
using System;


namespace ExifEdit.Exif
{ 
   

    public struct ExifEntrie
    {
        public IfdTyp ifdTyp;
        public uint id;
        public TagType tagtyp;
        public String value;
        public DataType typ;
        public uint dataCount;
        public byte[] obj;
    }



}
