

using ExifEdit.Helper;
using ExifReader.Exif;


namespace ExifEdit.Exif
{
   

    class ExifParser
    {


        Dictionary<uint, ExifEntrie> exifList;
        byte[] b;
      
        int size;

        bool isIntel = true;

        List<Tuple<uint, IfdTyp>> additionalIfd = new List<Tuple<uint, IfdTyp>>();

        public Dictionary<uint, ExifEntrie> analyse(byte[] b)
        {

            this.b = b;
            this.size = b.Length;

            exifList = new Dictionary<uint,ExifEntrie>();

            ushort dataFormat = Convert2.ToIntMot(b[10], b[11]);
            switch (dataFormat)
            {
                case 0x4949:
                    isIntel = true;
                    break;
                case 0x4D4D:
                    isIntel = false;
                    break;
                default:
                    isIntel = true;
                    break;
            }

            analyseData();
            
            return exifList;

        }

         const int  offset = 10; //die ersten 10 Bytes gehören nicht zu den EXIF daten, sondern zu 

        private void analyseData()
        {
            // TreeNode treeNode;
            uint ptr = 4+ offset;
            byte ifdNr = 0;
            uint ifdPos = Convert2.toInt(isIntel, b[ptr], b[ptr + 1], b[ptr + 2], b[ptr + 3]);
            //   treeNodeList.Add(NodeData.create("Pointer to IFD0", offset + ptr, 4, "IfdPtr", "" + ifdPos));

            while (ifdPos != 0)
            { 
                ifdPos = analyseIfd(ifdPos+ offset, ifdNr == 0? IfdTyp.IFD0: IfdTyp.EXTIFD,ifdNr);
                ifdNr++;
            }
            foreach (Tuple<uint, IfdTyp> elm in additionalIfd)
            {
                    
                ifdPos =  elm.Item1;
                IfdTyp ifdtyp = elm.Item2;
                while (ifdPos != 0)
                {
                    ifdPos = analyseIfd(ifdPos+ offset, ifdtyp, ifdNr++);
                }
            }
            // return treeNodeList;
        }

    

        private uint analyseIfd(uint ifdPos, IfdTyp ifdTyp, byte ifdNr)
        {
            uint ifdEntriesCount = Convert2.toInt(isIntel, b[ifdPos], b[ifdPos + 1]);
         //   Console.WriteLine("ifdEntriesCount:" + ifdEntriesCount);
            for (uint i = 0; i < ifdEntriesCount; i++)
            {
                analyseEntrie(ifdPos, i, ifdTyp,ifdNr);
            }
            uint nextIfd = Convert2.toInt(isIntel, b[ifdPos + 2 + 12 * ifdEntriesCount], b[ifdPos + 2 + 12 * ifdEntriesCount + 1]);
            return nextIfd;
        }


        private void analyseEntrie(uint ifdPos, uint i, IfdTyp ifdTyp, ushort ifdNr)
        {
            uint pos = ifdPos + 2 + 12 * i;


            ushort tagId = Convert2.toInt(isIntel, b[pos], b[pos + 1]);
            TagType tagType = (TagType)tagId;
            ushort dataTypeInt = Convert2.toInt(isIntel, b[pos + 2], b[pos + 3]);
            DataType dataType = (DataType)dataTypeInt;

            uint dataSetCount = Convert2.toInt(isIntel, b[pos + 4], b[pos + 5], b[pos + 6], b[pos + 7]);
            string dataValue = "";

            uint effDataSize = TagConvert.getDataSize(dataType) * dataSetCount;
            if (effDataSize == 0) {
               effDataSize = dataSetCount; //TopDo problematisch weil zu groß
            } 
            byte[] data = new byte[effDataSize];


            uint dataOrPtr = Convert2.toInt(isIntel, b[pos + 8], b[pos + 9], b[pos + 10], b[pos + 11]);

            if (effDataSize > 4)
            {
                uint ptr2data = Convert2.toInt(isIntel, b[pos + 8], b[pos + 9], b[pos + 10], b[pos + 11]);
                //manchmal scheien dei werte nicht zu stimmen
                if (b.Length < ptr2data + offset + effDataSize) { 
                    effDataSize = (uint)(b.Length - (ptr2data + offset)); //TODO check this size -1? ??
                }

                // Console.WriteLine (effDataSize +"  "+ b.Length);
                   Array.Copy(b, ptr2data+offset, data, 0, effDataSize);  
                dataValue = TagConvert.getValue(isIntel, tagType, dataType, data, dataSetCount,ifdTyp);
             }
           else
           {
                Array.Copy(b, pos + 8, data, 0, effDataSize);
                dataValue = TagConvert.getValue(isIntel, tagType, dataType, data, dataSetCount,ifdTyp);
                //    nodeArray[3] = NodeData.create("Data", offset + pos + 8, effDataSize, "Data", dataValue);
                //    // nodeArray[3] = NodeData.create("Data", offset + pos + 8, 4, "Data", "");
            }

            if ( tagType == TagType.TAG_GPSInfo)
            {
                additionalIfd.Add(new Tuple<uint, IfdTyp>(Convert2.toInt(isIntel, b[pos + 8], b[pos + 9], b[pos + 10], b[pos + 11]), IfdTyp.GPSIFD));
            }
            else if (tagType == TagType.TAG_ExifOffset)
            {
                additionalIfd.Add(new Tuple<uint, IfdTyp>(Convert2.toInt(isIntel, b[pos + 8], b[pos + 9], b[pos + 10], b[pos + 11]), IfdTyp.SUBIFD));
            }
            else
            {
                //dataValue =getSpecialValue(tagId,)
                ExifEntrie exifentry = new ExifEntrie();
                exifentry.ifdTyp = ifdTyp;
                exifentry.id = tagId;
                exifentry.tagtyp = tagType;
                exifentry.value = dataValue;
                exifentry.typ = dataType;
                exifentry.dataCount = dataSetCount;
                exifentry.obj = data;
                if (!exifList.ContainsKey(tagId))
                { exifList.Add(tagId, exifentry); }
                else
                { // einige Tagid´s sind doppelt!
                    if (!exifList.ContainsKey(((uint)tagType) + tagId))
                    { exifList.Add(((uint)tagType) + tagId, exifentry); }
                }
            }

        }
        }
    
    }