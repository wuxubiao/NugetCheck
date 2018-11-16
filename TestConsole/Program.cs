using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CheckPE;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string file32 = @"C:\Work\NugetCheck\x32\bin\Debug\x32.dll";
            string file64 = @"C:\Work\NugetCheck\x64\bin\Debug\x64.dll";
            string fileanycpu = @"C:\Work\NugetCheck\anycpu\bin\Debug\anycpu.dll";

            var bbb=Check.GetMachineType(file32);
            var a1 = Check.GetCharacteristics(file32);
            var a2 = Check.GetCharacteristics(file64);
            var a3 = Check.GetCharacteristics(fileanycpu);

            var b1 = Check.IsAnycpuOrX64(file32);
            var b2 = Check.IsAnycpuOrX64(file64);
            var b3 = Check.IsAnycpuOrX64(fileanycpu);

            Console.ReadKey();
        }
    }
}
