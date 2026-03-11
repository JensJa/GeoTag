

using ExifEdit.Exif;
using ExifEdit.Images;
using ExifEdit.Segments;
using ExifEdit.Tags;

using log4net;

namespace ExifEdit
{
    public class ReadExif
    {


        private static readonly ILog log = LogManager.GetLogger(typeof(ReadExif));

        SeperatorJpg seperatorJpg;
        public Dictionary<IptcTag, List<IPTCField>> iptcList;
        public Dictionary<uint, ExifEntrie> ExifEntryList { get; set; }

        JpgParser jpgParser = new JpgParser();

        /// <summary>
        /// parse a file for Information exif IptcSeg
        /// </summary>
        /// <param name="filename"></param>
        public void parsefile(String filename)
        {
           
            if (File.Exists(filename))
            {
                jpgParser.parsefile(filename, null,true);
                seperatorJpg = jpgParser.SeperatorJpg;
                ExifEntryList = jpgParser.ExifEntryList;
               
                iptcList = jpgParser.IptcList;


              


                //foreach (ExifEntrie ef in exifEntryList.Values)
                //{
                //    if (ef.ifdTyp == IfdTyp.GPSIFD)
                //    { lb_exif.Items.Add($"{((TagTypeGPS)ef.tagtyp).ToString()} [ {ef.tagtyp:X}]: {ef.value}"); }
                //    else
                //    { lb_exif.Items.Add($"{ef.tagtyp.ToString()} [ {ef.tagtyp:X}]: {ef.value}"); }
                //}
    
            }
            log.Debug("-parsefile");


        }

    }
}
