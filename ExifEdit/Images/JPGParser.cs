using ExifEdit.Exif;


using ExifEdit.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExifEdit.Segments;
using ExifEdit.Segments;

namespace ExifEdit.Images
{
    internal class JpgParser
    {

        public Dictionary<uint, ExifEntrie> ExifEntryList { get; set; }
        public Dictionary<IptcTag, List<IPTCField>> IptcList { get; set; }

        public IPTCSeg IptcSeg { get; set; }
        public SeperatorJpg SeperatorJpg { get; set; }
     
    
        public void parsefile(String filename, String provider, bool cache)
        {
            byte[] b;
             ExifEntryList = new Dictionary<uint, ExifEntrie>();

            using (var stream = File.Open(filename, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        b= ms.ToArray();
                    }
                }
            }

            
            //byte[] b = File.ReadAllBytes(filename);

        
            IptcList = new Dictionary<IptcTag, List<IPTCField>>();

            SeperatorJpg = new SeperatorJpg(b);
            SeperatorJpg.createSegemnts();

            //  segListe = jpgsep.ListeJpegSegments;
            byte[] bx;
    

            //for (int i = 0; i < SeperatorJpg.ListeJpegSegments.Count; i++)
            //{
            //    bx = SeperatorJpg.ListeJpegSegments[i];
            //    ushort seg = Helper.Convert2.toInt(bx[0], bx[1]);
            //    //  listBox1.Items.Add("" + i + ": " + seperatorJpg.getSegmentTyp(seg));

            //}

            if (SeperatorJpg.app1 != 255)
            {
                //            parseExif = new ParamterParser(filename);
                ExifParser exifParser = new ExifParser();
                ExifEntryList = exifParser.analyse(SeperatorJpg.ListeJpegSegments[SeperatorJpg.app1]);
                foreach (ExifEntrie ef in ExifEntryList.Values)
                {
                    //lb_exif.Items.Add($"{ef.tagtyp.ToString()} : {ef.value}");

                    //listBox1.Items.Add($"({ef.ifdTyp}:{ef.id:X4}) {ef.tagtyp.ToString()} : {ef.value} as {ef.typ.ToString()} {ef.dataCount} ");

                    // Console.WriteLine($"({ef.ifdTyp}:{ef.id:X4}) {ef.tagtyp.ToString()} as {ef.typ.ToString()} : {ef.value} ");
                }
               
            }

            if (SeperatorJpg.app13 != 255)
            {
                IptcSeg = new IPTCSeg(SeperatorJpg.ListeJpegSegments[SeperatorJpg.app13]);
                IptcSeg.parse();
                IptcList = IptcSeg.IptcList;
              

                //IptcTag[] tags = IptcSeg.getTagsList();
                //for (uint i = 0; i < tags.Length; i++)
                //{
                //  //  listBox1.Items.Add(IptcTags.getDescription((IptcTag)tags[i]) + " " + IptcSeg.getElement(tags[i]));
                //}

            }

           

            //ResolveGPS(tsw_permananetGps.Checked);
            //fillControlDaten();
            //fillControlIPTC();
            //fillControlIPTCList();

        }

    }
}
