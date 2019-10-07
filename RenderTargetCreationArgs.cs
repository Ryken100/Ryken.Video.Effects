using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics;
using Windows.Graphics.DirectX;
using Windows.Graphics.Imaging;

namespace Ryken.Video.Effects
{
    public struct RenderTargetCreationArgs
    {
        public int Width { get; }
        public int Height { get; }
        public DirectXPixelFormat Format { get; }
        public CanvasAlphaMode AlphaMode { get; }

        public RenderTargetCreationArgs(Size size) : this((int)size.Width, (int)size.Height) { }
        public RenderTargetCreationArgs(Size size, DirectXPixelFormat format) : this((int)size.Width, (int)size.Height, format) { }
        public RenderTargetCreationArgs(Size size, DirectXPixelFormat format, CanvasAlphaMode alphaMode) : this((int)size.Width, (int)size.Height, format, alphaMode) { }

        public RenderTargetCreationArgs(SizeInt32 size) : this(size.Width, size.Height) { }
        public RenderTargetCreationArgs(SizeInt32 size, DirectXPixelFormat format) : this(size.Width, size.Height, format) { }
        public RenderTargetCreationArgs(SizeInt32 size, DirectXPixelFormat format, CanvasAlphaMode alphaMode) : this(size.Width, size.Height, format, alphaMode) { }

        public RenderTargetCreationArgs(BitmapSize size) : this((int)size.Width, (int)size.Height) { }
        public RenderTargetCreationArgs(BitmapSize size, DirectXPixelFormat format) : this((int)size.Width, (int)size.Height, format) { }
        public RenderTargetCreationArgs(BitmapSize size, DirectXPixelFormat format, CanvasAlphaMode alphaMode) : this((int)size.Width, (int)size.Height, format, alphaMode) { }

        public RenderTargetCreationArgs(int width, int height) : this(width, height, DirectXPixelFormat.B8G8R8A8UIntNormalized) { }
        public RenderTargetCreationArgs(int width, int height, DirectXPixelFormat format) : this(width, height, format, CanvasAlphaMode.Premultiplied) { }
        public RenderTargetCreationArgs(int width, int height, DirectXPixelFormat format, CanvasAlphaMode alphaMode)
        {
            Width = width;
            Height = height;
            Format = format;
            AlphaMode = alphaMode;
        }
    }
}
