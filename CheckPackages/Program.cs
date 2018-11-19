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
            var arguments = CommandLineArgumentParser.Parse(args);

            if (arguments.Has("/?") || arguments.Has("/help"))
            {
                Usage();
                Environment.Exit(0);
            }

            var path = System.IO.Directory.GetCurrentDirectory();
            var extension = "*.dll";
            bool result = true;

            if (arguments.Has("-p"))
            {
                path = arguments.Get("-p").Next;
            }

            if (arguments.Has("-t"))
            {
                switch (arguments.Get("-t").Next.ToString().ToLower())
                {
                    case "dll":
                        extension = "*.dll";
                        break;
                    case "exe":
                        extension = "*.exe";
                        break;
                    case "nuget":
                        extension = "*.nupkg";
                        break;
                    default:
                        break;
                }
            }

            var filesList = new List<string>();
            FileOperation.GetFiles(path, extension, ref filesList);

            if (arguments.Has("-t"))
            {
                switch (arguments.Get("-t").Next.ToString().ToLower())
                {
                    case "dll":
                    case "exe":
                        result = CheckDllOrExe(filesList);
                        break;
                    case "nuget":
                        result = CheckNuget(filesList);
                        break;
                    default:
                        result = CheckDllOrExe(filesList);
                        break; ;
                }
            }
            else
            {
                result = CheckDllOrExe(filesList);
            }

            if (result)
                Console.WriteLine("CHECK OK");
            else
                Console.WriteLine("CHECK Failed");
        }

        static bool CheckNuget(List<string> filesList)
        {
            bool result = true;

            foreach (var packageFile in filesList)
            {
                var tempDir = Path.GetTempPath() + Path.GetRandomFileName();
                try
                {
                    ZipFile.ExtractToDirectory(packageFile, tempDir);

                    var files = new List<string>();
                    FileOperation.GetFiles(tempDir, "*.dll", ref files);

                    foreach (var file in files)
                    {
                        if (!CorFlags.IsAnycpuOrX64(file))
                        {
                            var x32File = packageFile + " " + file.Replace(tempDir, "");
                            Console.WriteLine(x32File);
                            result = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(packageFile + " " + e);
                    result = false;
                }

                try
                {
                    DirectoryInfo di = new DirectoryInfo(tempDir);
                    di.Delete(true);
                }
                catch (Exception e)
                {

                }
            }

            return result;
        }

        static bool CheckDllOrExe(List<string> filesList)
        {
            bool result = true;

            foreach (var file in filesList)
            {
                if (!CorFlags.IsAnycpuOrX64(file))
                {
                    Console.WriteLine(file);
                    result = false;
                }
            }

            return result;
        }

        static void Usage()
        {
            Console.WriteLine(
                "Check whether a .NET dll is built for Any CPU, x86, or x64\n" +
                "The result lists the 32-bit version of the files\n" +
                "\n" +
                "Windows Usage: CheckPackages.exe [-p path] [-t Options]\n" +
                "\n" +
                " 	 [-p path]        If the path is empty, the current Directory is check\n" +
                " 	 [-t Options]     If the Options are empty, the default check dll\n" +
                " 	     Options:  \n" +
                " 	        nuget     check nuget Packages\n" +
                " 	          dll     check dll files\n" +
                " 	          exe     check exe files\n",
                GetVersion());
        }

        static string GetVersion()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly.GetName().Version.ToString();
        }
    }
}
