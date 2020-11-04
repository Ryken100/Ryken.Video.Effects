using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects.Core
{
    readonly struct TransformParams
    {
        public readonly TransformType Type;
        public readonly Vector2 TargetSize;
        public TransformParams(TransformType type, Vector2 targetSize)
        {
            Type = type;
            TargetSize = targetSize;
        }

        public void GetMatrixTransform(Vector2 pixelSize, out Matrix3x2 matrix, out Vector2 effectiveSize)
        {
            matrix = Matrix3x2.Identity;
            effectiveSize = pixelSize;
            if (Type.HasFlag(TransformType.ScaleWidthToTarget) || Type.HasFlag(TransformType.ScaleHeightToTarget))
            {
                var scale = TargetSize / pixelSize;
                if (Type == TransformType.ScaleWidthToTarget)
                {
                    matrix = Matrix3x2.CreateScale(1f / scale.X);
                    effectiveSize = pixelSize * scale.X;
                }
                else if (Type == TransformType.ScaleHeightToTarget)
                {
                    matrix = Matrix3x2.CreateScale(1f / scale.Y);
                    effectiveSize = pixelSize * scale.Y;
                }
                else if (Type == TransformType.ScaleWidthHeightToTarget)
                {
                    var scaleVal = Math.Max(scale.X, scale.Y);
                    matrix = Matrix3x2.CreateScale(1f / scaleVal);
                    effectiveSize = pixelSize * scaleVal;
                }
            }
            
        }
    }
}
