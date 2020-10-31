using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryken.Video.Effects.Core;

namespace Ryken.Video.Effects
{
    public sealed class InterpolationEffectHandler : IVideoEffectHandler
    {
        /// <summary>
        /// The interpolation mode to use when screen resolution is lower than video resolution
        /// </summary>
        public CanvasImageInterpolation DownscaleInterpolationMode { get; set; } = CanvasImageInterpolation.Linear;

        /// <summary>
        /// The interpolation mode to use when screen resolution is higher than video resolution
        /// </summary>
        public CanvasImageInterpolation UpscaleInterpolationMode { get; set; } = CanvasImageInterpolation.Linear;
        public bool IsEnabled { get; set; } = true;

        bool IVideoEffectHandler.ProcessFrame(IVideoEffectHandlerArgs args)
        {
            using (var ds = args.OutputFrame.CreateDrawingSession())
            {
                ds.DrawImage(args.InputFrame, args.OutputFrame.Bounds, args.InputFrame.Bounds, 1, 
                    (args.OutputFrame.SizeInPixels.Width > args.InputFrame.SizeInPixels.Width || args.OutputFrame.SizeInPixels.Height > args.InputFrame.SizeInPixels.Height) ? UpscaleInterpolationMode : DownscaleInterpolationMode);
            }
            return true;
        }

        void IVideoEffectHandler.CreateResources()
        {
            
        }

        void IVideoEffectHandler.DestroyResources()
        {
            
        }

    }
}
