using Ryken.Video.Effects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects
{
    public abstract class ComplexEffectHandlerBase : IVideoEffectHandler
    {
        Dictionary<string, ComplexEffectResources> resourceDict = new Dictionary<string, ComplexEffectResources>();
        int targetCount;

        public bool IsEnabled { get; set; } = true;

        protected ComplexEffectHandlerBase()
        {
            targetCount = GetRenderTargetCount();
        }
        void IVideoEffectHandler.CreateResources()
        {
            this.CreateResources();
        }

        void IVideoEffectHandler.DestroyResources()
        {
            foreach (var resKvp in resourceDict)
            {
                resKvp.Value.Dispose();
            }
            resourceDict.Clear();
            this.DestroyResources();
        }

        bool IVideoEffectHandler.ProcessFrame(IVideoEffectHandlerArgs args)
        {
            if (!resourceDict.TryGetValue(args.InstanceID, out var resources))
            {
                resources = new ComplexEffectResources(targetCount, this);
                resourceDict.Add(args.InstanceID, resources);
            }
            resources.HandlerArgs = args;
            return ProcessFrame(args, resources);
        }
        protected abstract void CreateResources();
        protected abstract void DestroyResources();
        protected abstract bool ProcessFrame(IVideoEffectHandlerArgs args, ComplexEffectResources resources);
        protected internal abstract RenderTargetCreationArgs GetRenderTargetCreationArgs(int renderTargetIndex, IVideoEffectHandlerArgs args);
        protected abstract int GetRenderTargetCount();
    }
}
