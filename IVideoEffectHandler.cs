using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects
{
    /// <summary>
    /// Implement this interface and call VideoEffectManager.AddVideoEffectHandler to add effects to videos.
    /// </summary>
    public interface IVideoEffectHandler
    {
        void ProcessFrame(IVideoEffectHandlerArgs args);
        void CreateResources();
        void DestroyResources();
    }
}
