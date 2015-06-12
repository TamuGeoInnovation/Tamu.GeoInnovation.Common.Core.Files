using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using USC.GISResearchLab.Common.Core.Randomizers.RandomNumbers;
using USC.GISResearchLab.Common.Utils.Strings;

namespace USC.GISResearchLab.Common.Utils.Directories
{
	/// <summary>
	/// Summary description for DirectoryUtils.
	/// </summary>
	public class DirectoryUtils
	{
		public DirectoryUtils()
		{
		}
        
        public static bool testPath(string path, bool createIfNeeded, bool throwError)
        {
            bool ret = true;

            if (!DirectoryExists(path))
            {
                ret = false;
                if (throwError)
                {
                    throw new DirectoryNotFoundException("Directory not found: " + path);
                }
                else if (createIfNeeded)
                {
                    Directory.CreateDirectory(path);
                    ret = true;
                }
            }

            return ret;
        }

        public static string GetDirectoryParentPath(string path)
        {
            string ret = null;

            if (DirectoryExists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo parent = dir.Parent;
                ret = parent.FullName;
            }

            return ret;
        }

        public static string GetNextIncrementalDirectoryName(string directoryPath)
        {
            string ret = directoryPath;

            while (DirectoryExists(ret))
            {

                string parentPath = GetDirectoryParentPath(ret);
                string name = GetDirectoryName(ret);
                string incrementedName = StringUtils.GetNextIncrementalString(name);
                ret = Path.Combine(parentPath, incrementedName);

            }
            return ret;
        }


		public static bool DirectoryExists(string path)
		{
			bool ret = false;
			if (!String.IsNullOrEmpty(path))
			{
                ret = Directory.Exists(path);
			}
			return ret;
		}

        public static bool CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            return CopyDirectory(sourceDirectory, targetDirectory, false, null);
        }

        public static bool CopyDirectory(string sourceDirectory, string targetDirectory, bool ignoreError, BackgroundWorker backgroundWorker)
        {
            bool ret = false;
            try
            {
                DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
                DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

                ret = CopyAll(diSource, diTarget, ignoreError, backgroundWorker);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred Copying Directory", e);
            }
            return ret;
        }

        public static bool CopyAll(DirectoryInfo source, DirectoryInfo target, bool ignoreError, BackgroundWorker backgroundWorker)
        {
             bool ret = false;
             try
             {
                 if (backgroundWorker != null && backgroundWorker.CancellationPending)
                 {
                  
                     return false;
                 }
                 else
                 {
                     if (!Directory.Exists(target.FullName))
                     {
                         Directory.CreateDirectory(target.FullName);
                     }

                     foreach (FileInfo fi in source.GetFiles())
                     {
                         if (backgroundWorker != null && backgroundWorker.CancellationPending)
                         {
                          
                             return false;
                         }
                         else
                         {
                             
                             fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                         }
                     }

                     foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                     {
                         if (backgroundWorker != null && backgroundWorker.CancellationPending)
                         {
                           
                             return false;
                         }
                         else
                         {
                             DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                            
                         }
                     }
                 }
             }
             catch (Exception e)
             {
               
                 if (!ignoreError)
                 {
                     throw new Exception("An error occurred in CopyAll", e);
                 }
             }
            return ret;
        }

        public static void CreateDirectory(string path)
        {
            try
            {
                if (!DirectoryExists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                throw new Exception("An error occured creating directory: " + path, e);
            }
        }

        public static void DeleteFiles(string path)
        {
            ArrayList files = getFileList(path);
            if (files != null)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    string filePath = (string)files[i];
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
        }

        public static void DeleteSubDirectories(string path)
        {
            DeleteSubDirectories(path, false);
        }
        public static void DeleteSubDirectories(string path, bool recursive)
        {
            ArrayList subDirectories = GetSubDirectories(path);
            if (subDirectories != null)
            {
                for (int i = 0; i < subDirectories.Count; i++)
                {
                    string subDirectory = (string)subDirectories[i];
                    if (Directory.Exists(subDirectory))
                    {
                        Directory.Delete(subDirectory, recursive);
                    }
                }
            }
        }

        public static void DeleteDirectory(string path)
        {
            if (DirectoryExists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static void ReCreateDirectory(string path)
        {
            if (DirectoryExists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }

        public static string GetFirstSubDirectoryPath(string path)
        {
            string ret = null;

            string sub = GetFirstSubDirectory(path);
            if (sub != null)
            {
                ret = sub + "\\";
            }

            return ret;
        }

        public static string GetFirstSubDirectory(string path)
        {
            string ret = null;

            ArrayList subdirectories = GetSubDirectories(path);
            if (subdirectories != null && subdirectories.Count > 0)
            {
                ret = (string)subdirectories[0];
            }

            return ret;
        }

        public static ArrayList GetSubDirectories(string path)
        {
            return GetSubDirectories(path, false, null);
        }

        public static ArrayList GetSubDirectories(string path, bool recursive, ArrayList nameMask)
		{
			ArrayList ret = null;

            SearchOption searchOption;

            if (recursive)
            {
                searchOption = SearchOption.AllDirectories;
            }
            else
            {
                searchOption = SearchOption.TopDirectoryOnly;
            }

			if (DirectoryExists(path))
			{
                if (nameMask == null)
                {
                    string[] subDirs = Directory.GetDirectories(path, "*", searchOption);
                    if (subDirs != null && subDirs.Length > 0)
                    {
                        ret = new ArrayList(subDirs);
                    }
                }
                else
                {
                    for (int i = 0; i < nameMask.Count; i++)
                    {
                        string[] subDirs = Directory.GetDirectories(path, (string)nameMask[i], searchOption);

                        if (subDirs != null && subDirs.Length > 0)
                        {
                            if (ret == null)
                            {
                                ret = new ArrayList(subDirs);
                            }
                            else
                            {
                                ret.AddRange(subDirs);
                            }
                        }           
                    }
                }
			}
			return ret;
		}

        public static List<string> GetSubDirectoriesAsStringList(string path)
        {
            return GetSubDirectoriesAsStringList(path, false, null);
        }

        public static List<string> GetSubDirectoriesAsStringList(string path, bool recursive, ArrayList nameMask)
        {
            List<string> ret = new List<string>();

            SearchOption searchOption;

            if (recursive)
            {
                searchOption = SearchOption.AllDirectories;
            }
            else
            {
                searchOption = SearchOption.TopDirectoryOnly;
            }

            if (DirectoryExists(path))
            {
                if (nameMask == null)
                {
                    string[] subDirs = Directory.GetDirectories(path, "*", searchOption);
                    if (subDirs != null && subDirs.Length > 0)
                    {
                        ret = new List<string>(subDirs);
                    }
                }
                else
                {
                    for (int i = 0; i < nameMask.Count; i++)
                    {
                        string[] subDirs = Directory.GetDirectories(path, (string)nameMask[i], searchOption);

                        if (subDirs != null && subDirs.Length > 0)
                        {
                            if (ret == null)
                            {
                                ret = new List<string>(subDirs);
                            }
                            else
                            {
                                ret.AddRange(subDirs);
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public static string GetDrive(string filePath)
        {
            string ret = "";
            if (filePath != null)
            {
                if (!filePath.Equals(""))
                {
                    ret = (Directory.GetDirectoryRoot(filePath));
                }
                else
                {
                    throw new Exception("DirectoryUtils.GetDrive() error: filePath empty");
                }
            }
            else
            {
                throw new Exception("DirectoryUtils.GetDrive() error: filePath is null");
            }
            return ret;
        }

        public static string GetDirectoryName(string path)
        {
            string ret = null;

            if (DirectoryExists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                ret = dir.Name;
            }

            return ret;
        }

        public static long GetDirectorySize(string path, bool includeSubdirectories)
        {
            return GetDirectorySize(new DirectoryInfo(path), includeSubdirectories);
        }

        // from http://msdn.microsoft.com/en-us/library/system.io.directory.aspx
        public static long GetDirectorySize(DirectoryInfo d, bool includeSubdirectories)
        {
            long Size = 0;
            
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                Size += fi.Length;
            }

            if (includeSubdirectories)
            {
                // Add subdirectory sizes.
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    Size += GetDirectorySize(di, includeSubdirectories);
                }
            }
            return (Size);
        }


		public static ArrayList getFileList(string path)
		{
			return getFileList(path, null, false);
		}

        public static ArrayList getFileList(string path, string typeFilter, bool useTypeFilter)
        {
            return getFileList(path, typeFilter, useTypeFilter, SearchOption.TopDirectoryOnly);
        }

        public static ArrayList getFileList(string path, string typeFilter, bool useTypeFilter, SearchOption SearchOption)
		{
			ArrayList ret = new ArrayList();

            if (DirectoryExists(path))
            {
                string[] files = null;
                if (useTypeFilter)
                {
                    files = Directory.GetFiles(path, typeFilter, SearchOption);
                }
                else
                {
                    files = Directory.GetFiles(path);
                }

                if (files != null && files.Length > 0)
                {
                    ret = new ArrayList(files);
                }
            }
			return ret;
		}

        public static List<string> GetFileListAsStringList(string path)
        {
            return GetFileListAsStringList(path, null, false);
        }

        public static List<string> GetFileListAsStringList(string path, string typeFilter, bool useTypeFilter)
        {
            return GetFileListAsStringList(path, typeFilter, useTypeFilter, SearchOption.TopDirectoryOnly);
        }

        public static List<string> GetFileListAsStringList(string path, string typeFilter, bool useTypeFilter, SearchOption SearchOption)
        {
            List<string> ret = new List<string>();

            if (DirectoryExists(path))
            {
                string[] files = null;
                if (useTypeFilter)
                {
                    files = Directory.GetFiles(path, typeFilter, SearchOption);
                }
                else
                {
                    files = Directory.GetFiles(path);
                }

                if (files != null && files.Length > 0)
                {
                    ret = new List<string>(files);
                }
            }
            return ret;
        }

        

        public static string GetRandomFile(string path, string typeFilter)
        {
            return GetRandomFile(path, new string[] { typeFilter });
        }

        public static string GetRandomFile(string path, string [] typeFilters)
        {
            string ret = "";

            if (DirectoryExists(path))
            {
                List<string> files = new List<string>();
                if (typeFilters != null)
                {
                    foreach (string s in typeFilters)
                    {
                        string[] list = Directory.GetFiles(path, s);
                        if (list != null)
                        {
                            files.AddRange(list);
                        }
                    }
                }

                if (files != null)
                {
                    int seed = RandomNumberGenerator.GetRandomNumber();
                    Random random = new Random(seed);
                    int index = random.Next(0, files.Count - 1);

                    ret = files[index];
                }

            }
            return ret;
        }

        /*
         * Special Folders
         * 
        0	Desktop			C:\Documents and Settings\Charlie\Desktop
        2	Programs		C:\Documents and Settings\Charlie\Start Menu\Programs
        5	Personal		D:\documents
        6	Favorites		C:\Documents and Settings\Charlie\Favorites
        8	Recent			C:\Documents and Settings\Charlie\Recent
        9	SendTo			C:\Documents and Settings\Charlie\SendTo
        11	StartMenu		C:\Documents and Settings\Charlie\Start Menu
        13	MyMusic			D:\documents\My Music
        16	DesktopDirectory	C:\Documents and Settings\Charlie\Desktop
        17	MyComputer			 
        26	ApplicationData		C:\Documents and Settings\Charlie\Application Data
        28	LocalApplicationData	C:\Documents and Settings\Charlie\Local Settings\Application Data
        32	InternetCache		C:\Documents and Settings\Charlie\Local Settings\Temporary Internet Files
        33	Cookies			C:\Documents and Settings\Charlie\Cookies
        34	History			C:\Documents and Settings\Charlie\Local Settings\History
        35	CommonApplicationData	C:\Documents and Settings\All Users\Application Data
        37	System			C:\WINDOWS\System32
        38	ProgramFiles		C:\Program Files
        39	MyPictures		D:\documents\My Pictures
        43	CommonProgramFiles	C:\Program FilesCommon Files
         * */

        public static String GetHomeDir()
        {
            string ret = null;
            ret = Environment.GetEnvironmentVariable("USERPROFILE");
            return ret;
        }

        public static String GetLocalApplicationDataDir()
        {
            string ret = null;
            ret = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return ret;
        }

        public static String GetLocalSettingsDir()
        {
            string ret = null;
            ret = DirectoryUtils.GetDirectoryParentPath(GetLocalApplicationDataDir());
            return ret;
        }

        public static String GetLocalSettingsLocalApplicationTempDir()
        {
            return Path.Combine(GetLocalSettingsDir(), "temp");
        }

        public static String GetMyDocumentsDir()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }


	}


}
