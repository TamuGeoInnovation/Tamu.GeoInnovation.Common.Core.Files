using System.IO;

namespace USC.GISResearchLab.Common.Core.IOs.Paths
{
    public class PathUtils
    {
        public static string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public static string Combine(string path1, string path2, string path3)
        {
            return Path.Combine(Path.Combine(path1, path2), path3);
        }

        public static string Combine(string path1, string path2, string path3, string path4)
        {
            return Path.Combine(Path.Combine(Path.Combine(path1, path2), path3), path4);
        }

        public static string Combine(string path1, string path2, string path3, string path4, string path5)
        {
            return Path.Combine(Path.Combine(Path.Combine(Path.Combine(path1, path2), path3), path4), path5);
        }

        public static string RemoveInvalidFileNameChars(string fileName)
        {
            char[] invalidFileChars = Path.GetInvalidFileNameChars();

            foreach (char invalidFChar in invalidFileChars)
            {
                fileName = fileName.Replace(invalidFChar.ToString(), "");
            }
            return fileName;

        }

        public static bool ContainsInvalidFileNameChars(string fileName)
        {
            bool ret = false;
            char[] invalidFileChars = Path.GetInvalidFileNameChars();

            foreach (char invalidFChar in invalidFileChars)
            {
                if (fileName.Contains(invalidFChar.ToString()))
                {
                    if (invalidFChar.ToString().CompareTo("/") != 0)
                    {
                        ret = true;
                        break;
                    }
                }

            }
            return ret;

        }
    }
}
