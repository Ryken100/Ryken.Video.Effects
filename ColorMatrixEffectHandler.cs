using Microsoft.Graphics.Canvas.Effects;
using Ryken.Video.Effects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects
{
    public class ColorMatrixEffectHandler : SimpleEffectHandlerBase<ColorMatrixEffect>
    {
        public const float PCRangeMinChannelValue = 0;
        public const float PCRangeMaxChannelValue = 1;
        public const float VideoRangeMinChannelValue = 16f / 255f;
        public const float VideoRangeMaxChannelValue = 235f / 255f;

        public static readonly Matrix5x4 PCRangeToVideoRangeMatrix = new Matrix5x4()
        {
            M11 = VideoRangeMaxChannelValue - VideoRangeMinChannelValue, M51 = VideoRangeMinChannelValue,
            M22 = VideoRangeMaxChannelValue - VideoRangeMinChannelValue,  M52 = VideoRangeMinChannelValue,
            M33 = VideoRangeMaxChannelValue - VideoRangeMinChannelValue, M53 = VideoRangeMinChannelValue,
            M44 = 1
        };

        public Matrix5x4 ColorMatrix { get; set; } = new Matrix5x4()
        {
            M11 = 1,
            M22 = 1,
            M33 = 1,
            M44 = 1
        };
        protected override void SetEffectProperties(IVideoEffectHandlerArgs args, ColorMatrixEffect effect)
        {
            effect.Source = args.InputFrame;
            effect.ColorMatrix = ColorMatrix;
        }
    }
}
