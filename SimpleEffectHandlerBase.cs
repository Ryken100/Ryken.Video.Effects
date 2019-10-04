using Microsoft.Graphics.Canvas.Effects;
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
        protected abstract void SetEffectProperties(IVideoEffectHandlerArgs args, T effect);
    }
}
