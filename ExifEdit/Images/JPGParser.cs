using ExifEdit.Exif;
using ExifEdit.Segments;
using ExifEdit.Segments;
using ExifEdit.Tags;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExifEdit.Images
{
    internal class JpgParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SpecialTags));
        public Dictionary<uint, ExifEntrie> ExifEntryList { get; set; }
        public Dictionary<IptcTag, List<IPTCField>> IptcList { get; set; }

        public IPTCSeg IptcSeg { get; set; } //ToDo private machen und über SaveTagsAndWrite2File übergeben, damit die Struktur auch bei nicht vorhandener IPTC Sektion angelegt wird
        public JpegSplitter jpegSplitter { get; set; } //todo private machen und über SaveTagsAndWrite2File übergeben, damit die Struktur auch bei nicht vorhandener IPTC Sektion angelegt wird

        ExifParser exifParser;

        /// <summary>
        /// call the Splitter fpr the JPEG File in to Segments and call the ExifParser for the Exif Segment and the IPTCSeg for the IPTC Segment, if they are available. The results are stored in the ExifEntryList and IptcList
        /// </summary>
        /// <param name="filename"></param>
        public void Parsefile(byte[] b)
        {
            
            ExifEntryList = new Dictionary<uint, ExifEntrie>();
            IptcList = new Dictionary<IptcTag, List<IPTCField>>();

            jpegSplitter = new JpegSplitter(b);
            jpegSplitter.ReadSegemnts();

            if (jpegSplitter.app1 != 255)
            {
                //            parseExif = new ParamterParser(filename);
                ExifParser exifParser = new ExifParser();
                ExifEntryList = exifParser.analyse(jpegSplitter.ListeJpegSegments[jpegSplitter.app1]);

            }

            if (jpegSplitter.app13 != 255)
            {
                IptcSeg = new IPTCSeg(jpegSplitter.ListeJpegSegments[jpegSplitter.app13]);
                IptcSeg.parse();
                IptcList = IptcSeg.IptcList;
            }

        }

        public void SaveTagsAndWrite2File(String Filename, Dictionary<uint, ExifEntrie> exifEntryList, Dictionary<IptcTag, List<IPTCField>> iptcList)
        {
            log.Debug("+saveTagsAndWrite2File");
            

            log.Debug("-saveTagsAndWrite2File");
        }

    }
}
