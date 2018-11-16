using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckPE;
using Common;

namespace CheckPackages
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入nuget packages路径：");
            var packagesPath = Console.ReadLine();
            Console.WriteLine("输入检查结果文件保存路径（包括文件名）：");
            var resultPath = Console.ReadLine();

            var packagesList =new List<string>();
            FileOperation.GetFiles(packagesPath, "*.nupkg", ref packagesList);

            foreach (var packageFile in packagesList)
            {
                var tempDir = Path.GetTempPath() + Path.GetRandomFileName();
                ZipFile.ExtractToDirectory(packageFile, tempDir);

                var files = new List<string>();
                FileOperation.GetFiles(tempDir, "*.dll", ref files);

                foreach (var file in files)
                {
                    if (!Check.IsAnycpuOrX642(file))
                    {
                        var x32File = packageFile + " " + file.Replace(tempDir, "");
                        Console.WriteLine(x32File);
                    }

//                    if (!Check.IsAnycpuOrX64(file))
//                    {
//                        var x32File = packageFile + " " + file.Replace(tempDir, "");
//                        Console.WriteLine(x32File);
//                    }
                }

                try
                {
                    DirectoryInfo di = new DirectoryInfo(tempDir);
                    di.Delete(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            Console.WriteLine("任意键退出");
            Console.ReadKey();
        }
    }
}
