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

namespace Ryken.Video.Effects.Core
{
    public interface IVideoEffectHandlerArgs
    {
        /// <summary>
        /// The texture containing the video frame from the media framework
        /// </summary>
        CanvasBitmap InputFrame { get; }

        /// <summary>
        /// The render target that InputFrame will be rendered onto
        /// </summary>
        CanvasRenderTarget OutputFrame { get; }

        /// <summary>
        /// The device used to create and draw InputFrame and OutputFrame
        /// </summary>
        CanvasDevice Device { get; }

        /// <summary>
        /// The ID of the video effect that generated these arguments
        /// </summary>
        string ID { get; }

        /// <summary>
        /// Properties used when creating the effect
        /// </summary>
        IPropertySet Properties { get; }

        /// <summary>
        /// A randomly generated ID used to identify different media players that have the same effect ID
        /// </summary>
        string InstanceID { get; }

        /// <summary>
        /// The time stamp of the current frame
        /// </summary>
        TimeSpan? Position { get; }

        /// <summary>
        /// The matrix transformation that should be applied to all drawing operations on the output frame
        /// </summary>
        Matrix3x2 Transform { get; }

        /// <summary>
        /// The effective size of the output frame after the transform has been applied
        /// </summary>
        Size OutputSize { get; }

        /// <summary>
        /// The effect bounds of the output frame after the transform has been applied
        /// </summary>
        Rect OutputBounds { get; }

        /// <summary>
        /// Creates a drawing session on the output frame
        /// </summary>
        /// <returns>CanvasDrawingSession</returns>
        CanvasDrawingSession CreateDrawingSession();
        /*
        /// <summary>
        /// Get Win2D drawing arguments for these event args. Call VideoEffectHandlerCanvasArgs.Dispose() when done, or use a `using` block.
        /// </summary>
        /// <returns>Disposable VideoEffectHandlerCanvasArgs</returns>
        VideoEffectHandlerCanvasArgs GetDisposableCanvasArgs();*/
    }
}
