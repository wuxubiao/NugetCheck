using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace CheckPE
{
    public class AssemblyInfo
    {
        readonly TextWriter output;

        public AssemblyInfo()
            : this(Console.Out)
        {
        }

        public AssemblyInfo(TextWriter messagesOutput)
        {
            this.output = messagesOutput;
        }

        bool IsMarked(ModuleDefinition targetModule)
        {
            return targetModule.Types.Any(t => t.Name == "IMarked");
        }

        public void AssemblyInfoOutput(CorFlagsInformation info)
        {
            output.WriteLine("Version   : {0}", info.version);
            output.WriteLine("CLR Header: {0}", (info.clr_header <= TargetRuntime.Net_1_1) ? "2.0" : "2.5");
            output.WriteLine("PE        : {0}", info.pe);
            output.WriteLine("CorFlags  : 0x{0:X}", info.corflags);
            output.WriteLine("ILONLY    : {0}", info.ilonly ? 1 : 0);
            output.WriteLine("32BITREQ  : {0}", info.x32bitreq ? 1 : 0);
            output.WriteLine("32BITPREF : {0}", info.x32bitpref ? 1 : 0);
            output.WriteLine("Signed    : {0}", info.signed ? 1 : 0);
            //	anycpu: PE = PE32  and  32BIT = 0
            //	   x86: PE = PE32  and  32BIT = 1
            //	64-bit: PE = PE32+ and  32BIT = 0
        }

        public CorFlagsInformation ExtractInfo(ModuleDefinition assembly)
        {
            var info = new CorFlagsInformation();

            // The user defined version of the assembly
            info.assemblyVersion = assembly.Assembly.Name.Version.ToString();

            //Version of the mscorlib.dll that was assembly was compiled with and now should run against
            info.version = assembly.RuntimeVersion;

            info.clr_header = assembly.Runtime;

            info.pe = (assembly.Architecture == TargetArchitecture.AMD64 || assembly.Architecture == TargetArchitecture.IA64) ? "PE32+" : "PE32";

            info.corflags = (int)assembly.Attributes;

            info.ilonly = (assembly.Attributes & ModuleAttributes.ILOnly) == ModuleAttributes.ILOnly;

            info.x32bitreq = (assembly.Attributes & ModuleAttributes.Required32Bit) == ModuleAttributes.Required32Bit;

            info.x32bitpref = (assembly.Attributes & ModuleAttributes.Preferred32Bit) == ModuleAttributes.Preferred32Bit;

            info.signed = (assembly.Attributes & ModuleAttributes.StrongNameSigned) == ModuleAttributes.StrongNameSigned;

            return info;
        }

        public ModuleDefinition OpenAssembly(string fileName)
        {
            var fullPath = Path.GetFullPath(fileName);
            if (!File.Exists(fullPath))
            {
#if DEBUG
                output.WriteLine(fullPath);
#endif
                throw new FileNotFoundException();
            }

            var targetModule = ModuleDefinition.ReadModule(fullPath);

            return targetModule;
        }
    }
}
