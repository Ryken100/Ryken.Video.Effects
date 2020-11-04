using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Graphics.Imaging;
using Windows.Media;

namespace Ryken.Video.Effects.Core
{
    public sealed class VideoEffectHandlerArgs : IVideoEffectHandlerArgs, ITransformableVideoEffectHandlerArgs
    {
        public CanvasBitmap InputFrame { get; internal set; }
        
        public CanvasRenderTarget OutputFrame { get; internal set; }
        
        public CanvasDevice Device { get; internal set; }
        
        public string ID { get; internal set; }
        
        public IPropertySet Properties { get; internal set; }

        public string InstanceID { get; internal set; }

        public TimeSpan? Position { get; internal set; }

        public Matrix3x2 Transform { get; internal set; } = Matrix3x2.Identity;

        public Size OutputSize { get; internal set; }

        public Rect OutputBounds { get; internal set; }

        internal VideoEffectHandlerArgs() { }

        public VideoEffectHandlerArgs(CanvasDevice device, CanvasBitmap inputFrame, CanvasRenderTarget outputFrame, string id, string instanceId, IPropertySet properties, TimeSpan? position)
        {
            Device = device;
            InputFrame = inputFrame;
            OutputFrame = outputFrame;
            ID = id;
            InstanceID = instanceId;
            Properties = properties;
            Position = position;
        }

        void ITransformableVideoEffectHandlerArgs.SetTransform(Matrix3x2 transform, Vector2 outputSize)
        {
            Transform = transform;
            OutputSize = outputSize.ToSize();
            OutputBounds = new Rect(new Point(), OutputSize);
        }

        public CanvasDrawingSession CreateDrawingSession()
        {
            var ds = OutputFrame.CreateDrawingSession();
            ds.Transform = Transform;
            return ds;
        }

        /*public virtual VideoEffectHandlerCanvasArgs GetDisposableCanvasArgs()
        {
            var args = new VideoEffectHandlerCanvasArgs()
            {
                ShouldDisposeOfInput = false,
                ShouldDisposeOfOutput = false
            };
            args.InputFrame = InputFrame;
            args.OutputFrame = OutputFrame;
            args.DrawingSession = args.OutputFrame.CreateDrawingSession();
            return args;
        }*/
    }
}
