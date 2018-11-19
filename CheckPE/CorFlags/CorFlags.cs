using System;
using System.Collections.Generic;
using System.Text;

namespace CheckPE
{
    public class CorFlags
    {
        public static bool IsAnycpuOrX64(string fileName)
        {
            try
            {
                var assemblyInfo = new AssemblyInfo();
                var modDef = assemblyInfo.OpenAssembly(fileName);
                var corFlags = assemblyInfo.ExtractInfo(modDef);
                modDef.Dispose();

                return !corFlags.x32bitreq;
            }
            catch (Exception e)
            {
                return Check.IsAnycpuOrX64(fileName);
            }
        }
    }
}
