using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;

namespace Ryken.Video.Effects.Core
{
    public sealed class VideoEffect : IBasicVideoEffect
    {
        string id;
        CanvasDevice device;
        IPropertySet properties;
        public VideoEffect()
        {

        }

        public void SetEncodingProperties(VideoEncodingProperties encodingProperties, IDirect3DDevice device)
        {
            //this.device = CanvasDevice.GetSharedDevice();
            this.device = CanvasDevice.CreateFromDirect3D11Device(device);
        }

        public void ProcessFrame(ProcessVideoFrameContext context)
        {
            using (var output = CanvasRenderTarget.CreateFromDirect3D11Surface(device, context.OutputFrame.Direct3DSurface))
            using (var input = CanvasRenderTarget.CreateFromDirect3D11Surface(device, context.InputFrame.Direct3DSurface))
            {
                var args = new VideoEffectHandlerArgs()
                {
                    ID = id,
                    Device = device,
                    InputFrame = input,
                    OutputFrame = output,
                    Properties = properties
                };
                VideoEffectManager.ProcessFrame(args);
            }
        }

        public void Close(MediaEffectClosedReason reason)
        {
            
        }

        public void DiscardQueuedFrames()
        {
            
        }

        public bool IsReadOnly => true;

        public IReadOnlyList<VideoEncodingProperties> SupportedEncodingProperties => new List<VideoEncodingProperties>();

        public MediaMemoryTypes SupportedMemoryTypes => MediaMemoryTypes.Gpu;

        public bool TimeIndependent => true;

        public void SetProperties(IPropertySet configuration)
        {
            if (configuration.ContainsKey("ID"))
            {
                this.id = configuration["ID"] as string;
                properties = properties;
            }
            else
            {
                throw new InvalidOperationException("VideoEffect property 'ID' not set");
            }
        }
    }
}
