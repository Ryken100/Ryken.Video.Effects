﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects.Core
{
    /// <summary>
    /// Implement this interface and call VideoEffectManager.AddVideoEffectHandler to add effects to videos.
    /// </summary>
    public interface IVideoEffectHandler
    {
        bool IsEnabled { get; set; }
        bool ProcessFrame(IVideoEffectHandlerArgs args);
        void CreateResources();
        void DestroyResources();
    }
}
