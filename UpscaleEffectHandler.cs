using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryken.Video.Effects.Core;

namespace Ryken.Video.Effects
{
    public class UpscaleEffectHandler : ComplexEffectHandlerBase
    {
        protected override void CreateResources()
        {
            
        }

        protected override void DestroyResources()
        {
            
        }

        protected override int GetRenderTargetCount()
        {
            return 2;
        }

        protected override void ProcessFrame(IVideoEffectHandlerArgs args, ComplexEffectResources resources)
        {
            
        }

        protected internal override RenderTargetCreationArgs GetRenderTargetCreationArgs(int renderTargetIndex, IVideoEffectHandlerArgs args)
        {
            return new RenderTargetCreationArgs(args.InputFrame.SizeInPixels);
        }
    }
}
