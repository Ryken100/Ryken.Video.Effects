﻿using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects
{
    public abstract class SimpleEffectHandlerBase<T> : IVideoEffectHandler where T : ICanvasEffect, new()
    {
        T effect;
        void IVideoEffectHandler.CreateResources()
        {
            effect = new T();
        }

        void IVideoEffectHandler.DestroyResources()
        {
            effect.Dispose();
        }

        void IVideoEffectHandler.ProcessFrame(IVideoEffectHandlerArgs args)
        {
            SetEffectProperties(args, effect);
            using (var session = args.OutputFrame.CreateDrawingSession())
            {
                session.DrawImage(effect, args.OutputFrame.Bounds, args.InputFrame.Bounds);
            }
        }

        /// <summary>
        /// Set effect properties and source in this method
        /// </summary>
        /// <param name="args">The arguments for the current video frame</param>
        /// <param name="effect">The effect generated by SimpleEffectHandlerBase</param>
        protected abstract void SetEffectProperties(IVideoEffectHandlerArgs args, T effect);
    }
}
