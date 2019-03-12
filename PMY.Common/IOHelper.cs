using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.Common
{
    public class IOHelper
    {
        public static DateTime GetFileLastWriteTimeOfFolder(string folderPath)
        {
            DateTime FileNewTime = DateTime.MinValue;
            bool flag = true;
            if (!Directory.Exists(folderPath))
            {
                return FileNewTime;
            }

            ComparerTime(folderPath, ref FileNewTime, ref flag);
            return FileNewTime;
        }

        private static void ComparerTime(string folderPath, ref DateTime fileNewTime, ref bool flag)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo info in fileInfos)
            {
                string fullName = info.FullName;
                string extension = info.Extension.ToLower();
                if (extension == ".cs")
                {
                    if (flag)
                    {
                        fileNewTime = info.LastWriteTime;
                        flag = false;
                    }
                    if (DateTime.Compare(info.LastWriteTime, fileNewTime) > 0)
                    {
                        fileNewTime = info.LastWriteTime;
                    }

                }
            }

            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            foreach (DirectoryInfo info in directoryInfos)
            {
                string path = info.FullName;
                ComparerTime(path, ref fileNewTime, ref flag);

            }
        }
    }
}
