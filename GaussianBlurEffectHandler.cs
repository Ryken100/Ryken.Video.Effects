using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryken.Video.Effects.Core;

namespace Ryken.Video.Effects
{
    public sealed class GaussianBlurEffectHandler : SimpleEffectHandlerBase<GaussianBlurEffect>
    {
        /// <summary>
        /// Gets or sets the amount of blur to be applied to the image.
        /// </summary>
        public float BlurAmount { get; set; } = 10;

        /// <summary>
        /// Level of performance optimization.
        /// </summary>
        public EffectOptimization Optimization { get; set; } = EffectOptimization.Speed;

        protected override void SetEffectProperties(IVideoEffectHandlerArgs args, GaussianBlurEffect effect)
        {
            effect.Source = args.InputFrame;
            effect.BlurAmount = BlurAmount;
            effect.Optimization = Optimization;
        }
    }
}
