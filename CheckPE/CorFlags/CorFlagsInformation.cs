using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace CheckPE
{
    public class CorFlagsInformation
    {
        public string assemblyVersion;
        public string version;
        public TargetRuntime clr_header;
        public string pe;
        public int corflags;
        public bool ilonly;
        public bool x32bitreq;
        public bool x32bitpref;
        public bool signed;
    }
}
