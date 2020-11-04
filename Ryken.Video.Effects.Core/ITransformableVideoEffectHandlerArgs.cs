using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects.Core
{
    public interface ITransformableVideoEffectHandlerArgs
    {
        void SetTransform(Matrix3x2 transform, Vector2 outputSize);
    }
}
