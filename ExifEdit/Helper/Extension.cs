
using System.Text;

using System.Xml;
using System.Xml.XPath;

namespace ExifEdit.Helper
{
    public static class Extension
    {
        /// <summary>
        /// Pretty print for a xml String
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string PrettyXML(this String s)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(s);
                return doc.Pretty();
            }
            catch (Exception ex)
            { return s; }
        }

        /// <summary>
        /// Prety Print for a xmlDocument
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string Pretty(this XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = " ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
                OmitXmlDeclaration = true,
                Encoding = Encoding.GetEncoding("ISO-8859-1")
            };

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Remove the last found removeStr,
        /// is path = null --> return ""
        /// </summary>
        /// <param name="path"></param>
        /// <param name="removeStr"></param>
        /// <returns></returns>
        public static string RemoveLast(this string path, string removeStr)
        {
            if (path == null ) return "";
            if (removeStr == null) return path;
            int lastIndex = path.LastIndexOf(removeStr);
            if (lastIndex >= 0)
            {
                path = path.Remove(lastIndex);
            }
            return path;
        }


  




    }
}
