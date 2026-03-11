
using ExifEdit.Exif;
using ExifEdit.Helper;
using ExifEdit.Segments;
using ExifEdit.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iptc.MetaDataReader
{
    class MetaDataReaderJpeg
    {
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private void parsefile(Stream input)
        {
            IPTCSeg iptc;
            Dictionary<uint, ExifEntrie> exifEntryList = new Dictionary<uint, ExifEntrie>();
            byte[] b = ReadFully(input);



            SeperatorJpg seperatorJpg = new SeperatorJpg(b);
            seperatorJpg.createSegemnts();

            //  segListe = jpgsep.ListeJpegSegments;
            byte[] bx;

            for (int i = 0; i < seperatorJpg.ListeJpegSegments.Count; i++)
            {
                bx = seperatorJpg.ListeJpegSegments[i];
                ushort seg = Convert2.toInt(bx[0], bx[1]);
                //  listBox1.Items.Add("" + i + ": " + seperatorJpg.getSegmentTyp(seg));

            }


            if (seperatorJpg.app13 != 255)
            {
                iptc = new IPTCSeg(seperatorJpg.ListeJpegSegments[seperatorJpg.app13]);
                iptc.parse();

                IptcTag[] tags = iptc.getTagsList();
                for (uint i = 0; i < tags.Length; i++)
                {
                    //  listBox1.Items.Add(IptcTags.getDescription((IptcTag)tags[i]) + " " + IptcSeg.getElement(tags[i]));
                }
            }

            if (seperatorJpg.app1 != 255)
            {
                //            parseExif = new ParamterParser(filename);
                ExifParser exifParser = new ExifParser();
                exifEntryList = exifParser.analyse(seperatorJpg.ListeJpegSegments[seperatorJpg.app1]);
                foreach (ExifEntrie ef in exifEntryList.Values)
                {
                    //lb_exif.Items.Add($"{ef.tagtyp.ToString()} : {ef.value}");

                    //listBox1.Items.Add($"({ef.ifdTyp}:{ef.id:X4}) {ef.tagtyp.ToString()} : {ef.value} as {ef.typ.ToString()} {ef.dataCount} ");
                    // Console.WriteLine($"({ef.ifdTyp}:{ef.id:X4}) {ef.tagtyp.ToString()} as {ef.typ.ToString()} : {ef.value} ");
                }
                
            }

        }
    }
}
