using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCFConverter
{
    public class FileManager
    {
        public void CreateFolder(string folderpath)
        {
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
        }
        public bool CopyFile(List<string> copyList, string folderpath)
        {
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(folderpath);
            DirectoryInfo[] drinfo = directoryInfo.GetDirectories("*.*", System.IO.SearchOption.AllDirectories);  // Directory Info                      

            if (FolderCheck(folderpath))
            {
                foreach (string folder in copyList)
                {
                    if (folder != "Merge")  // Folder Name
                    {
                        int ver = RecentRevisionFileCheck(folder, folderpath +folder);
                        if(ver == -1)
                        {
                            return false;
                        }
                        else
                        {
                            string version = ver.ToString();

                            string targetfilename = folder + "_rev" + version + ".xlsx";

                            string target_path = System.IO.Path.Combine(folderpath, "Merge\\" + targetfilename);
                            System.IO.File.Copy(folderpath + folder + "\\" + targetfilename, target_path, true);
                            
                        }                        
                    }
                }
            }
            return true;
        }
        public bool FolderCheck(string folderpath)
        {
            if (System.IO.Directory.Exists(folderpath))
            {
                return true;
            }
            return false;
        }
        public int RecentRevisionFileCheck(string band, string folderpath)
        {
            DirectoryInfo di = new DirectoryInfo(folderpath);
            FileInfo[] fi = di.GetFiles();
            List<int> numList = new List<int>();
            foreach (var x in fi)
            {
                FileAttributes attributes = File.GetAttributes(x.FullName);
                if((attributes  == FileAttributes.Archive))
                {
                    try
                    {
                        string tmp = x.ToString().Replace(band + "_rev", "");
                        string number = tmp.Replace(".xlsx", "");
                        numList.Add(Convert.ToInt16(number));
                    }
                    catch (Exception e)
                    {
                        return -1;
                    }
                }                
            }
            return numList.Max();
        }


    }
}
