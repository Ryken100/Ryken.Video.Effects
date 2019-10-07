using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /*
        /// <summary>
        /// Get Win2D drawing arguments for these event args. Call VideoEffectHandlerCanvasArgs.Dispose() when done, or use a `using` block.
        /// </summary>
        /// <returns>Disposable VideoEffectHandlerCanvasArgs</returns>
        VideoEffectHandlerCanvasArgs GetDisposableCanvasArgs();*/
    }
}
