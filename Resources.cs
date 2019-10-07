using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects
{
    static class Resources
    {
        const string ResourceStartName = "Ryken.Video.Effects.";
        static Assembly assembly;
        static Resources()
        {
            assembly = typeof(Resources).GetTypeInfo().Assembly;
        }
        public unsafe static Stream GetEmbeddedResourceStream(string fileName)
        {
            fixed (char* namePointer = fileName)
            {
                bool startsCorrectly = fileName.StartsWith(ResourceStartName);
                int length = fileName.Length;
                if (!startsCorrectly)
                    length += ResourceStartName.Length;
                char* newNamePointer = stackalloc char[length];
                char* fileNamePointer = newNamePointer;
                if (!startsCorrectly)
                {
                    for (int i = 0; i < ResourceStartName.Length; i++)
                    {
                        newNamePointer[i] = ResourceStartName[i];
                    }
                    fileNamePointer += ResourceStartName.Length;
                }
                // Swap out spaces and slashes with their appropriate values
                for (int i = 0; i < fileName.Length; i++)
                {
                    if (namePointer[i] == ' ')
                        fileNamePointer[i] = '_';
                    else if (namePointer[i] == '/' && namePointer[i] == '\\')
                        fileNamePointer[i] = '.';
                    else fileNamePointer[i] = namePointer[i];
                }
                var newFileName = new String(newNamePointer, 0, length);
                return assembly.GetManifestResourceStream(newFileName);
            }
        }
        public static byte[] GetEmbeddedResourceBytes(string fileName)
        {
            using (var stream = GetEmbeddedResourceStream(fileName))
            {
                stream.Seek(0, SeekOrigin.Begin);
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                return bytes;
            }
        }
    }
}
