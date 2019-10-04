using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryken.Video.Effects.Core
{
    public sealed class VideoEffectHandlerCanvasArgs : IDisposable
    {
        internal bool ShouldDisposeOfInput, ShouldDisposeOfOutput;
        public CanvasDrawingSession DrawingSession { get; internal set; }
        public CanvasBitmap InputFrame { get; internal set; }
        internal CanvasRenderTarget OutputFrame { get; set; }
        public void Dispose()
        {
            if (ShouldDisposeOfInput)
                InputFrame.Dispose();
            if (ShouldDisposeOfOutput)
                OutputFrame.Dispose();
            DrawingSession.Dispose();
        }
    }
}
