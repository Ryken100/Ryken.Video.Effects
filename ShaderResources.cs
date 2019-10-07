using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects
{
    public static class ShaderResources
    {
        public static byte[] GetShaderResourceBytes(string shaderName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ShaderBin.");
            sb.Append(shaderName);
            sb.Append(".bin");
            return Resources.GetEmbeddedResourceBytes(sb.ToString());
        }
    }
}
