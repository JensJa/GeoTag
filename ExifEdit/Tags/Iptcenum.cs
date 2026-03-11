using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExifEdit.Tags
{
    public enum IptcMode { Overwrite, OverwriteWhenClear, NotOverwrite, Add, Remove, Clear, OverwriteGPS , OverwriteGPSWhenClear }


    //@see https://de.wikipedia.org/wiki/IPTC-IIM-Standard

    //// Queries for the IPTC Address fields
    //// Note: Region is normally the State or County
    //String iptcCountry = "/app13/irb/8bimiptc/IptcSeg/Country/Primary Location Name";
    //String iptcRegion = "/app13/irb/8bimiptc/IptcSeg/Province\\/State";
    //String iptcCity = "/app13/irb/8bimiptc/IptcSeg/City";
    //String iptcSubLocation = "/app13/irb/8bimiptc/IptcSeg/Sub-location";
    //string iptcObjectName = @"/app13/irb/8bimiptc/IptcSeg/object name";
    //string iptcCaption = @"/app13/irb/8bimiptc/IptcSeg/Caption";
    //string iptcAuthor = @"/app13/irb/8bimiptc/IptcSeg/Writer\/Editor";
    //string iptcCopyright = @"/app13/irb/8bimiptc/IptcSeg/copyright notice";

    //private void InitViaBmp(string path)
    //{
    //    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
    //    {
    //        BitmapSource img = BitmapFrame.Create(stream);
    //        BitmapMetadata md = (BitmapMetadata)img.Metadata;
    //        Console.Out.WriteLine("+Caption=" + (string)md.GetQuery(iptcCaption));

    public enum IptcTag : ushort
    {
        NOTHING = 0, //Null element

        ENVELOPE_RECORD_VERSION = 0x0100, // 0 + 0x0100
        DESTINATION = 0x0105, // 5
        FILE_FORMAT = 0x0114, // 20
        FILE_VERSION = 0x0116, // 22
        SERVICE_ID = 0x011E, // 30
        ENVELOPE_NUMBER = 0x0128, // 40
        PRODUCT_ID = 0x0132, // 50
        ENVELOPE_PRIORITY = 0x013C, // 60
        DATE_SENT = 0x0146, // 70
        TIME_SENT = 0x0150, // 80
        CODED_CHARACTER_SET = 0x015A, // 90
        UNIQUE_OBJECT_NAME = 0x0164, // 100
        ARM_IDENTIFIER = 0x0178, // 120
        ARM_VERSION = 0x017a, // 122

        // IPTC ApplicationRecord Tags
        APPLICATION_RECORD_VERSION = 0x0200, // 0 + 0x0200
        OBJECT_TYPE_REFERENCE = 0x0203, // 3
        OBJECT_ATTRIBUTE_REFERENCE = 0x0204, // 4
        OBJECT_NAME = 0x0205, // 5
        EDIT_STATUS = 0x0207, // 7
        EDITORIAL_UPDATE = 0x0208, // 8
        URGENCY = 0X020A, // 10
        SUBJECT_REFERENCE = 0X020C, // 12
        CATEGORY = 0x020F, // 15
        SUPPLEMENTAL_CATEGORIES = 0x0214, // 20
        FIXTURE_ID = 0x0216, // 22
        KEYWORDS = 0x0219, // 25
        CONTENT_LOCATION_CODE = 0x021A, // 26
        CONTENT_LOCATION_NAME = 0x021B, // 27
        RELEASE_DATE = 0X021E, // 30
        RELEASE_TIME = 0x0223, // 35
        EXPIRATION_DATE = 0x0225, // 37
        EXPIRATION_TIME = 0x0226, // 38
        SPECIAL_INSTRUCTIONS = 0x0228, // 40
        ACTION_ADVISED = 0x022A, // 42
        REFERENCE_SERVICE = 0x022D, // 45
        REFERENCE_DATE = 0x022F, // 47
        REFERENCE_NUMBER = 0x0232, // 50
        DATE_CREATED = 0x0237, // 55
        TIME_CREATED = 0X023C, // 60
        DIGITAL_DATE_CREATED = 0x023E, // 62
        DIGITAL_TIME_CREATED = 0x023F, // 63
        ORIGINATING_PROGRAM = 0x0241, // 65
        PROGRAM_VERSION = 0x0246, // 70
        OBJECT_CYCLE = 0x024B, // 75
        BY_LINE = 0x0250, // 80
        BY_LINE_TITLE = 0x0255, // 85
        CITY = 0x025A, // 90
        SUB_LOCATION = 0x025C, // 92
        PROVINCE_OR_STATE = 0x025F, // 95
        COUNTRY_CODE = 0x0264, // 100
        COUNTRY_NAME = 0x0265, // 101
        ORIGINAL_TRANSMISSION_REFERENCE = 0x0267, // 103
        HEADLINE = 0x0269, // 105
        CREDIT = 0x026E, // 110
        SOURCE = 0x0273, // 115
        COPYRIGHT_NOTICE = 0x0274, // 116
        CONTACT = 0x0276, // 118
        CAPTION = 0x0278, // 120
        LOCAL_CAPTION = 0x0279, // 121
        CAPTION_WRITER = 0x027A, // 122
        RASTERIZED_CAPTION = 0x027D, // 125
        IMAGE_TYPE = 0x0282, // 130
        IMAGE_ORIENTATION = 0x0283, // 131
        LANGUAGE_IDENTIFIER = 0x0287, // 135
        AUDIO_TYPE = 0x0296, // 150
        AUDIO_SAMPLING_RATE = 0x0297, // 151
        AUDIO_SAMPLING_RESOLUTION = 0x0298, // 152
        AUDIO_DURATION = 0x0299, // 153
        AUDIO_OUTCUE = 0x029A, // 154

        JOB_ID = 0x02B8, // 184
        MASTER_DOCUMENT_ID = 0x02B9, // 185
        SHORT_DOCUMENT_ID = 0x02BA, // 186
        UNIQUE_DOCUMENT_ID = 0x02BB, // 187
        OWNER_ID = 0x02BC, // 188

        OBJECT_PREVIEW_FILE_FORMAT = 0x02C8, // 200
        OBJECT_PREVIEW_FILE_FORMAT_VERSION = 0x02C9, // 201
        OBJECT_PREVIEW_DATA = 0x02CA // 202
    }



    public static class IptcTags
    {

        public static UInt16 Value(this IptcTag foo) { return (UInt16)foo; }
        public static IptcTag Value(this UInt16 foo) { return (IptcTag)foo; }


    }
}
