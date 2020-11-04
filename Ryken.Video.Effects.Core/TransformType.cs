using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects.Core
{
    [Flags]
    public enum TransformType
    {
        None = 0,
        ScaleWidthToTarget = 0b1,
        ScaleHeightToTarget = 0b10,
        ScaleWidthHeightToTarget = ScaleWidthToTarget | ScaleHeightToTarget
    }
}
