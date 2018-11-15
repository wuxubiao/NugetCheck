using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CheckPE
{
    public class Check
    {
        // pass the path to the file and check the return
        public static FilePEType GetFilePE(string path)
        {
            FilePEType pe = new FilePEType();
            pe = FilePEType.IMAGE_FILE_MACHINE_UNKNOWN;
            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] data = new byte[4096];
                    fs.Read(data, 0, 4096);
                    ushort result = BitConverter.ToUInt16(data, BitConverter.ToInt32(data, 60) + 4);
                    try
                    {
                        pe = (FilePEType)result;
                    }
                    catch (Exception)
                    {
                        pe = FilePEType.IMAGE_FILE_MACHINE_UNKNOWN;
                    }
                }
            }
            return pe;
        }

        public static MachineType GetMachineType(string fileName)
        {
            const int PE_POINTER_OFFSET = 60;
            const int MACHINE_OFFSET = 4;
            byte[] data = new byte[4096];
            using (Stream s = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                s.Read(data, 0, 4096);
            }
            // dos header is 64 bytes, last element, long (4 bytes) is the address of the PE header
            int PE_HEADER_ADDR = BitConverter.ToInt32(data, PE_POINTER_OFFSET);
            int machineUint = BitConverter.ToUInt16(data, PE_HEADER_ADDR + MACHINE_OFFSET);
            return (MachineType)machineUint;
        }

        public static bool IsAnycpuOrX64(string fileName)
        {

            Assembly assembly = Assembly.LoadFrom(fileName);
            assembly.ManifestModule.GetPEKind(out var peKind, out var machine);
            var result = peKind.HasFlag(PortableExecutableKinds.Required32Bit);
            assembly = null;
            return !result;
        }

        public static bool IsX32(string fileName)
        {
            Assembly assembly = Assembly.ReflectionOnlyLoadFrom(fileName);
            assembly.ManifestModule.GetPEKind(out var peKind, out var machine);
            var result = peKind.HasFlag(PortableExecutableKinds.Required32Bit);

            return result;
        }

        public static bool IsX64(string fileName)
        {
            Assembly assembly = Assembly.ReflectionOnlyLoadFrom(fileName);
            assembly.ManifestModule.GetPEKind(out var peKind, out var machine);
            var result = peKind.HasFlag(PortableExecutableKinds.Required32Bit);

            return result;
        }
    }
}
