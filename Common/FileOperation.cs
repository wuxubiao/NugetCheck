using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    public class FileOperation
    {
        public static void GetFiles(string path, string searchPattern, ref List<string> files)
        {
            if (files == null)
            {
                files = new List<string>();
            }

            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles(searchPattern);
            DirectoryInfo[] dii = dir.GetDirectories();

            foreach (FileInfo f in fil)
            {
                files.Add(f.FullName);//添加文件的路径到列表
            }

            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo d in dii)
            {
                GetFiles(d.FullName, searchPattern, ref files);
            }
        }
    }
}
