using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects
{
    public sealed class SaturationEffectHandler : SimpleEffectHandlerBase<SaturationEffect>
    {
        /// <summary>
        /// Gets or sets saturation intensity. 0 = fully desaturated (greyscale), 1 = normal saturation.
        /// </summary>
        public float Saturation { get; set; } = 1;

        protected override void SetEffectProperties(IVideoEffectHandlerArgs args, SaturationEffect effect)
        {
            effect.Source = args.InputFrame;
            effect.Saturation = Saturation;
        }
    }
}
