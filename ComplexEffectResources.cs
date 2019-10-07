using Microsoft.Graphics.Canvas;
using Ryken.Video.Effects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects
{
    public class ComplexEffectResources : IDisposable
    {
        public string InstanceID { get; internal set; }
        CanvasRenderTarget[] targets;
        internal IVideoEffectHandlerArgs HandlerArgs;
        internal ComplexEffectHandlerBase Handler;
        internal ComplexEffectResources(int renderTargetCount, ComplexEffectHandlerBase handler)
        {
            Handler = handler;
            targets = new CanvasRenderTarget[renderTargetCount];
        }
        public CanvasRenderTarget GetRenderTarget(int index)
        {
            var creationArgs = Handler.GetRenderTargetCreationArgs(index, HandlerArgs);
            if (targets[index] == null || targets[index].SizeInPixels.Width != creationArgs.Width || targets[index].SizeInPixels.Height != creationArgs.Height || targets[index].AlphaMode != creationArgs.AlphaMode || targets[index].Format != creationArgs.Format)
            {
                targets[index]?.Dispose();
                targets[index] = new CanvasRenderTarget(HandlerArgs.Device, creationArgs.Width, creationArgs.Height, 96, creationArgs.Format, creationArgs.AlphaMode);
            }
            return targets[index];
        }

        public void Dispose()
        {
            
        }
    }
}
