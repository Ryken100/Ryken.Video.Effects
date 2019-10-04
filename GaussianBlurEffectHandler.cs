using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects
{
    public sealed class GaussianBlurEffectHandler : IVideoEffectHandler
    {
        public float BlurAmount { get; set; } = 10;
        public EffectOptimization Optimization { get; set; } = EffectOptimization.Speed;

        void IVideoEffectHandler.CreateResources()
        {
            
        }

        void IVideoEffectHandler.DestroyResources()
        {
            
        }

        void IVideoEffectHandler.ProcessFrame(IVideoEffectHandlerArgs args)
        {
            using (var drawingSession = args.OutputFrame.CreateDrawingSession())
            {
                using (var blur = new GaussianBlurEffect() { Source = args.InputFrame, BlurAmount = BlurAmount, Optimization = Optimization })
                {
                    drawingSession.DrawImage(blur, args.OutputFrame.Bounds, args.InputFrame.Bounds);
                }
            }
        }
    }
}
