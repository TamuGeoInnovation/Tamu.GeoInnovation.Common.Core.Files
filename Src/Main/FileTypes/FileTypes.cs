using System;

namespace USC.GISResearchLab.Common.Utils.FileTypes
{
    /// <summary>
    /// Summary description for FileTypeUtils.
    /// </summary>
    public class FileTypes
    {

        public const int FILE_TYPE_LOG = 5;
        public const int FILE_TYPE_SHAPEFILES = 6;
        public const int FILE_TYPE_DATABASES = 7;
        public const int FILE_TYPE_CONFIGURATION = 8;

        public static string[] LogFileTypes = { "Text Files:txt", "Log Files:log" };
        public static string[] ShapefileFileTypes = { "Shapefiles:shp" };
        public static string[] DatabaseFileTypes = { "Access Databases:mdb" };
        public static string[] ConfigurationFileTypes = { "Coinfiguration Files:xml" };

        public FileTypes()
        {

        }

        public static string getFileTypeExtension(string fileType)
        {
            string ret = "";

            if (fileType != null)
            {
                string[] parts = fileType.Split('(');
                string name = parts[0];
                string ext = parts[1];
                ret = ext.Substring(0, ext.Length - 1);
            }
            else
            {
                throw new Exception("Error getting extension for filetype: " + fileType);
            }
            return ret;
        }

        public static string getFileTypeList(int fileType)
        {
            string ret = "";

            switch (fileType)
            {
                case FILE_TYPE_LOG:
                    ret = getFileTypesArrayList(LogFileTypes);
                    break;
                case FILE_TYPE_SHAPEFILES:
                    ret = getFileTypesArrayList(ShapefileFileTypes);
                    break;
                case FILE_TYPE_DATABASES:
                    ret = getFileTypesArrayList(DatabaseFileTypes);
                    break;
                case FILE_TYPE_CONFIGURATION:
                    ret = getFileTypesArrayList(ConfigurationFileTypes);
                    break;

                default:
                    ret = "";
                    break;
            }

            return ret;

        }

        private static string getFileTypesArrayList(string[] values)
        {
            string ret = "";

            //"nmea files (*.nmea)|*.nmea|All files (*.*)|*.*";

            for (int i = 0; i < values.Length; i++)
            {
                if (i > 0)
                {
                    ret += "|";
                }

                string current = values[i];
                string[] parts = current.Split(':');
                string type = parts[0];
                string ext = parts[1];
                string display = type + " (*." + ext + ")" + " |*." + ext;
                ret += display;
            }

            //ret += "|All files (*.*)|*.*";
            return ret;
        }
    }
}
