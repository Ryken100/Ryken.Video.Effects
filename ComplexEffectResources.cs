using Microsoft.Graphics.Canvas;
using Ryken.Video.Effects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        Dictionary<string, ResourceObj> resources;
        internal ComplexEffectResources(int renderTargetCount, ComplexEffectHandlerBase handler)
        {
            Handler = handler;
            targets = new CanvasRenderTarget[renderTargetCount];
        }
        public CanvasRenderTarget GetRenderTarget(int index)
        {
            var creationArgs = Handler.GetRenderTargetCreationArgs(index, HandlerArgs);
            if (targets[index] == null || targets[index].Device != HandlerArgs.Device || targets[index].SizeInPixels.Width != creationArgs.Width || targets[index].SizeInPixels.Height != creationArgs.Height || targets[index].AlphaMode != creationArgs.AlphaMode || targets[index].Format != creationArgs.Format)
            {
                targets[index]?.Dispose();
                targets[index] = new CanvasRenderTarget(HandlerArgs.Device, creationArgs.Width, creationArgs.Height, 96, creationArgs.Format, creationArgs.AlphaMode);
            }
            return targets[index];
        }

        public ref T GetResource<T>(string key)
        {
            if (resources == null)
                resources = new Dictionary<string, ResourceObj>();
            if (!resources.TryGetValue(key, out ResourceObj val))
            {
                val = new ResourceObj<T>();
                resources.Add(key, val);
            }
            return ref val.GetValue<T>();
        }

        public ref T GetOrCreateResource<T>(string key) where T : class, new()
        {
            ref T val = ref GetResource<T>(key);
            if (val == null)
            {
                val = new T();
            }
            return ref val;
        }

        public void Dispose()
        {
            
        }

        abstract class ResourceObj
        {
            public abstract ref TValue GetValue<TValue>();
        }
        class ResourceObj<T> : ResourceObj
        {
            T value;

            public override ref TValue GetValue<TValue>()
            {
                return ref Unsafe.As<T, TValue>(ref value);
            }
        }
    }
}
