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
        /// <summary>
        /// Gets or sets the amount of blur to be applied to the image.
        /// </summary>
        public float BlurAmount { get; set; } = 10;

        /// <summary>
        /// Level of performance optimization.
        /// </summary>
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
