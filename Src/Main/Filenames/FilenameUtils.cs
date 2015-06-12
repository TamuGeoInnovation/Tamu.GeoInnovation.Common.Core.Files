namespace USC.GISResearchLab.Common.Utils.Filenames
{
    public class FilenameUtils
    {
        public static int MAX_DBF_NAME_LENGTH = 13;
        public static int MAX_SHAPEFILE_NAME_LENGTH = 20;
        public static int MAX_INFOTABLE_NAME_LENGTH = 13;

        public static string TrimName(string name)
        {
            return TrimName(name, MAX_SHAPEFILE_NAME_LENGTH, 0);
        }

        public static string TrimName(string name, int spacing)
        {
            return TrimName(name, MAX_SHAPEFILE_NAME_LENGTH, spacing);
        }
        public static string TrimName(string name, int maxLength, int spacing)
        {
            string ret = "";
            int length = (maxLength - spacing) - 1;

            if (name.Length > length)
            {
                ret = name.Substring(0, length);
            }
            else
            {
                ret = name;
            }

            return ret;
        }
    }
}
