using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace ExifEdit.Tags
{
    /// <summary>
    /// Zusammenstellung der einzelnen IPTCTags
    /// </summary>
    public struct IPTCInfo
    {
        public String Name;
        public String Description;
        public byte maxCount;
        public int maxSize;

    }
    class IPTCInfos
    {

        //Indexer für IPTcTAG
        public IPTCInfo this[IptcTag tag]
        {
            get
            {
                if (IptcInfo.ContainsKey(tag))
                { return IptcInfo[tag]; }
                else { return IptcInfo[IptcTag.NOTHING]; }
            }
        }

        /// <summary>
        ///  Indexer für UINT16
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public IPTCInfo? this[UInt16 tag]
        {
            get
            {
                if (IptcInfo.ContainsKey((IptcTag)tag))
                { return IptcInfo[(IptcTag)tag]; }
                else { return IptcInfo[IptcTag.NOTHING]; }
            }
        }

        //maxsize = 2*16 steht für nicht bekannte Info

        public Dictionary<IptcTag, IPTCInfo> IptcInfo = new Dictionary<IptcTag, IPTCInfo> {
                { IptcTag.NOTHING, new IPTCInfo{ Name = "", maxCount = 0, maxSize = 0, Description = ""}}, //Null Element
                { IptcTag.ENVELOPE_RECORD_VERSION, new IPTCInfo{ Name = "Enveloped Record Version", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.DESTINATION, new IPTCInfo{ Name = "Destination", maxCount = 1, maxSize =256, Description = ""}}, //*30
                { IptcTag.FILE_FORMAT, new IPTCInfo{ Name = "File Format", maxCount = 1, maxSize = 2, Description = ""}},
                { IptcTag.FILE_VERSION, new IPTCInfo{ Name = "File Version", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.SERVICE_ID, new IPTCInfo{ Name = "Service Identifier", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.ENVELOPE_NUMBER, new IPTCInfo{ Name = "Envelope Number", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.PRODUCT_ID, new IPTCInfo{ Name = "Product Identifier", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.ENVELOPE_PRIORITY, new IPTCInfo{ Name = "Envelope Priority", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.DATE_SENT, new IPTCInfo{ Name = "Date Sent", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.TIME_SENT, new IPTCInfo{ Name = "Time Sent", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.CODED_CHARACTER_SET, new IPTCInfo{ Name = "Coded Character Set", maxCount = 1, maxSize = 32, Description = ""}},
                { IptcTag.UNIQUE_OBJECT_NAME, new IPTCInfo{ Name = "Unique Object Name", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.ARM_IDENTIFIER, new IPTCInfo{ Name = "ARM Identifier", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.ARM_VERSION, new IPTCInfo{ Name = "ARM Version", maxCount = 1, maxSize = 256, Description = ""}},

                { IptcTag.APPLICATION_RECORD_VERSION, new IPTCInfo{ Name = "Application Record Version", maxCount = 1, maxSize = 256, Description = "Datensatzversion"}},
                { IptcTag.OBJECT_TYPE_REFERENCE, new IPTCInfo{ Name = "Object Type Reference", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.OBJECT_ATTRIBUTE_REFERENCE, new IPTCInfo{ Name = "Object Attribute Reference", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.OBJECT_NAME, new IPTCInfo{ Name = "Object Name", maxCount = 1, maxSize = 64, Description = "Objectname"}},
                { IptcTag.EDIT_STATUS, new IPTCInfo{ Name = "Edit Status", maxCount = 1, maxSize = 64, Description = "Bearbeitungsstand"}},
                { IptcTag.EDITORIAL_UPDATE, new IPTCInfo{ Name = "Editorial Update", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.URGENCY, new IPTCInfo{ Name = "Urgency", maxCount = 1, maxSize = 1, Description = "Dringlichkeit"}}, //1..8 1= hoechste
                { IptcTag.SUBJECT_REFERENCE, new IPTCInfo{ Name = "Subject Reference", maxCount = 1, maxSize = 256, Description = "andere Kategorien"}},
                { IptcTag.CATEGORY, new IPTCInfo{ Name = "Category", maxCount = 1, maxSize = 3, Description = "Kategorie"}},
                { IptcTag.SUPPLEMENTAL_CATEGORIES, new IPTCInfo{ Name = "Supplemental Category", maxCount = 255, maxSize = 32, Description = ""}},
                { IptcTag.FIXTURE_ID, new IPTCInfo{ Name = "Fixture Identifier", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.KEYWORDS, new IPTCInfo{ Name = "Keywords", maxCount = 255, maxSize = 64, Description = "Stichwörter"}},
                { IptcTag.CONTENT_LOCATION_CODE, new IPTCInfo{ Name = "Content Location Code", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.CONTENT_LOCATION_NAME, new IPTCInfo{ Name = "Content Location Name", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.RELEASE_DATE, new IPTCInfo{ Name = "Release Date", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.RELEASE_TIME, new IPTCInfo{ Name = "Release Time", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.EXPIRATION_DATE, new IPTCInfo{ Name = "Expiration Date", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.EXPIRATION_TIME, new IPTCInfo{ Name = "Expiration Time", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.SPECIAL_INSTRUCTIONS, new IPTCInfo{ Name = "Special Instructions", maxCount = 1, maxSize = 256, Description = "spezielle Anweisungen"}},
                { IptcTag.ACTION_ADVISED, new IPTCInfo{ Name = "Action Advised", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.REFERENCE_SERVICE, new IPTCInfo{ Name = "Reference Service", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.REFERENCE_DATE, new IPTCInfo{ Name = "Reference Date", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.REFERENCE_NUMBER, new IPTCInfo{ Name = "Reference Number", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.DATE_CREATED, new IPTCInfo{ Name = "Date Created", maxCount = 1, maxSize =8, Description = "Erstellungsdatum"}},
                { IptcTag.TIME_CREATED, new IPTCInfo{ Name = "Time Created", maxCount = 1, maxSize = 11, Description = "Erstellungsuhrzeit"}},
                { IptcTag.DIGITAL_DATE_CREATED, new IPTCInfo{ Name = "Digital Date Created", maxCount = 1, maxSize = 8, Description = "Digitaliserungsdatum"}},
                { IptcTag.DIGITAL_TIME_CREATED, new IPTCInfo{ Name = "Digital Time Created", maxCount = 1, maxSize = 11, Description = "Digitaliserungsuhrzeit"}},
                { IptcTag.ORIGINATING_PROGRAM, new IPTCInfo{ Name = "Originating Program", maxCount = 1, maxSize = 32, Description = "Ursprungs Programm"}},
                { IptcTag.PROGRAM_VERSION, new IPTCInfo{ Name = "Program Version", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.OBJECT_CYCLE, new IPTCInfo{ Name = "Object Cycle", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.BY_LINE, new IPTCInfo{ Name = "Author", maxCount = 1, maxSize = 32, Description = "Erstellername"}},
                { IptcTag.BY_LINE_TITLE, new IPTCInfo{ Name = "Author Title", maxCount = 1, maxSize = 32, Description = "ErstellerTitel"}},
                { IptcTag.CITY, new IPTCInfo{ Name = "City", maxCount = 1, maxSize = 32, Description = "Stadt"}},
                { IptcTag.SUB_LOCATION, new IPTCInfo{ Name = "Sub-location", maxCount = 1, maxSize = 32, Description = "Ortsteil/Straße"}},
                { IptcTag.PROVINCE_OR_STATE, new IPTCInfo{ Name = "Province/State", maxCount = 1, maxSize = 32, Description = "Bundesland"}},
                { IptcTag.COUNTRY_CODE, new IPTCInfo{ Name = "Country Code", maxCount = 1, maxSize = 3, Description = "ISO_Länderkürzel"}},
                { IptcTag.COUNTRY_NAME, new IPTCInfo{ Name = "Country", maxCount = 1, maxSize = 64, Description = "Land"}},
                { IptcTag.ORIGINAL_TRANSMISSION_REFERENCE, new IPTCInfo{ Name = "Original Transmission Reference", maxCount = 1, maxSize = 32, Description = "Ort der Originalübertragung"}},
                { IptcTag.HEADLINE, new IPTCInfo{ Name = "Headline", maxCount = 1, maxSize = 256 , Description = "Überschrift"}},
                { IptcTag.CREDIT, new IPTCInfo{ Name = "Credit", maxCount = 1, maxSize = 32, Description = "Anbieter"}},
                { IptcTag.SOURCE, new IPTCInfo{ Name = "Source", maxCount = 1, maxSize = 32, Description = "Quelle"}},
                { IptcTag.COPYRIGHT_NOTICE, new IPTCInfo{ Name = "Copyright Notice", maxCount = 1, maxSize = 128, Description = "Urheberrechtsvermerk"}},
                { IptcTag.CONTACT, new IPTCInfo{ Name = "Contact", maxCount = 1, maxSize = 128, Description = "Kontakt"}},
                { IptcTag.CAPTION, new IPTCInfo{ Name = "Caption/Abstract", maxCount = 1, maxSize = 2000, Description = "Beschreibung"}},
                { IptcTag.LOCAL_CAPTION, new IPTCInfo{ Name = "Local Caption", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.CAPTION_WRITER, new IPTCInfo{ Name = "Caption Writer/Editor", maxCount = 1, maxSize = 32, Description = "Redakteur"}},
                { IptcTag.RASTERIZED_CAPTION, new IPTCInfo{ Name = "Rasterized Caption", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.IMAGE_TYPE, new IPTCInfo{ Name = "Image Type", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.IMAGE_ORIENTATION, new IPTCInfo{ Name = "Image Orientation", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.LANGUAGE_IDENTIFIER, new IPTCInfo{ Name = "Language Identifier", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.AUDIO_TYPE, new IPTCInfo{ Name = "Audio Type", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.AUDIO_SAMPLING_RATE, new IPTCInfo{ Name = "Audio Sampling Rate", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.AUDIO_SAMPLING_RESOLUTION, new IPTCInfo{ Name = "Audio Sampling Resolution", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.AUDIO_DURATION, new IPTCInfo{ Name = "Audio Duration", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.AUDIO_OUTCUE, new IPTCInfo{ Name = "Audio Outcue", maxCount = 1, maxSize = 256, Description = ""}},

                { IptcTag.JOB_ID, new IPTCInfo{ Name = "Job Identifier", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.MASTER_DOCUMENT_ID, new IPTCInfo{ Name = "Master Document Identifier", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.SHORT_DOCUMENT_ID, new IPTCInfo{ Name = "Short Document Identifier", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.UNIQUE_DOCUMENT_ID, new IPTCInfo{ Name = "Unique Document Identifier", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.OWNER_ID, new IPTCInfo{ Name = "Owner Identifier", maxCount = 1, maxSize = 256, Description = ""}},

                { IptcTag.OBJECT_PREVIEW_FILE_FORMAT, new IPTCInfo{ Name = "Object Data Preview File Format", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.OBJECT_PREVIEW_FILE_FORMAT_VERSION, new IPTCInfo{ Name = "Object Data Preview File Format Version", maxCount = 1, maxSize = 256, Description = ""}},
                { IptcTag.OBJECT_PREVIEW_DATA, new IPTCInfo{ Name = "Object Data Preview Data", maxCount = 1, maxSize = 256, Description = ""}}
        };

    }
}

