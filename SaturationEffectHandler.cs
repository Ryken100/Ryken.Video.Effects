using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects
{
    public sealed class SaturationEffectHandler : IVideoEffectHandler
    {
        SaturationEffect sat;

        public float Saturation { get; set; } = 1;

        void IVideoEffectHandler.ProcessFrame(IVideoEffectHandlerArgs args)
        {
            using (var ds = args.OutputFrame.CreateDrawingSession())
            {
                sat.Saturation = Saturation;
                sat.Source = args.InputFrame;
                ds.DrawImage(sat, args.OutputFrame.Bounds, args.InputFrame.Bounds);
            }
        }

        void IVideoEffectHandler.CreateResources()
        {
            sat = new SaturationEffect();
        }

        void IVideoEffectHandler.DestroyResources()
        {
            sat.Dispose();
        }
    }
}
