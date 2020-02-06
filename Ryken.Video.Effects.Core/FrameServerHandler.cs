using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.Playback;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Composition;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.Graphics;
using Windows.Graphics.DirectX;

namespace Ryken.Video.Effects.Core
{
    class FrameServerHandler : IDisposable
    {
        object ResourceLock = new object();
        public MediaPlayer Player { get; }
        public string ID { get; internal set; }
        public string InstanceID { get; internal set; }
        public FrameworkElement Container { get; }
        public IPropertySet Properties { get; }
        CanvasRenderTarget destinationTarget, sourceTarget;
        public CanvasDevice CanvasDevice { get; }
        public CompositionGraphicsDevice CompositionDevice { get; }
        public CompositionDrawingSurface DrawingSurface { get; private set; }
        Visual ContainerVisual { get; }
        Compositor Compositor { get; }
        SpriteVisual SpriteVisual { get; }
        CompositionSurfaceBrush SurfaceBrush { get; }
        public FrameServerHandler(MediaPlayer player, FrameworkElement container, string id, string instanceId, IPropertySet properties)
        {
            CanvasDevice = new CanvasDevice();
            Container = container;
            ID = id;
            InstanceID = instanceId;
            Player = player;
            ContainerVisual = ElementCompositionPreview.GetElementVisual(container);
            Compositor = ContainerVisual.Compositor;
            CompositionDevice = CanvasComposition.CreateCompositionGraphicsDevice(Compositor, CanvasDevice);
            SpriteVisual = Compositor.CreateSpriteVisual();
            SurfaceBrush = Compositor.CreateSurfaceBrush();

            SpriteVisual.Brush = SurfaceBrush;
            SurfaceBrush.Stretch = CompositionStretch.Uniform;
            ElementCompositionPreview.SetElementChildVisual(container, SpriteVisual);
            var sizeAni = Compositor.CreateExpressionAnimation("Container.Size");
            sizeAni.SetReferenceParameter("Container", ContainerVisual);
            SpriteVisual.StartAnimation("Size", sizeAni);

            CanvasDevice.DeviceLost += CanvasDevice_DeviceLost;

            Container.SizeChanged += Container_SizeChanged;

            player.IsVideoFrameServerEnabled = true;
            player.VideoFrameAvailable += Player_VideoFrameAvailable;
            player.MediaOpened += Player_MediaOpened;
            //createDestinationTarget();
        }

        private void CanvasDevice_DeviceLost(CanvasDevice sender, object args)
        {

        }

        private void Container_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            createDestinationTarget();
        }

        private void Player_VideoFrameAvailable(MediaPlayer sender, object args)
        {
            createSourceTarget();
            sender.CopyFrameToVideoSurface(sourceTarget);
            RenderFrame();
        }

        private void Player_MediaOpened(MediaPlayer sender, object args)
        {
            Container.RunOnUIThread(createDestinationTarget);
        }

        public void Dispose()
        {
            Container.SizeChanged -= Container_SizeChanged;
            Player.VideoFrameAvailable -= Player_VideoFrameAvailable;
        }

        void RenderFrame()
        {
            if (destinationTarget != null)
            {
                lock (ResourceLock)
                {
                    //using (CanvasDevice.Lock())
                    {
                        var args = new VideoEffectHandlerArgs()
                        {
                            InputFrame = sourceTarget,
                            OutputFrame = destinationTarget,
                            ID = ID,
                            InstanceID = InstanceID,
                            Properties = Properties,
                            Device = CanvasDevice
                        };
                        bool effectsAdded = VideoEffectManager.ProcessFrame(args);
                        using (var ds = CanvasComposition.CreateDrawingSession(DrawingSurface))
                        {
                            ds.DrawImage(destinationTarget);
                        }
                    }
                }
            }
            else
            {
                Container.RunOnUIThread(createDestinationTarget);
            }
        }

        #region Render target creation
        void createSourceTarget()
        {
            if (sourceTarget == null || sourceTarget.Description.Width != Player.PlaybackSession.NaturalVideoWidth || sourceTarget.Description.Height != Player.PlaybackSession.NaturalVideoHeight)
            {
                sourceTarget?.Dispose();
                sourceTarget = new CanvasRenderTarget(CanvasDevice, Player.PlaybackSession.NaturalVideoWidth, Player.PlaybackSession.NaturalVideoHeight, 96);
            }
        }

        // Can only be run on the UI thread
        void createDestinationTarget()
        {
            // Don't run if not on the UI thread
            if (!Container.Dispatcher.HasThreadAccess)
                return;
            // Don't run if no video is loaded
            if (Player.PlaybackSession.NaturalVideoWidth == 0)
                return;
            double containerRatio = Container.ActualWidth / Container.ActualHeight, videoRatio = (double)Player.PlaybackSession.NaturalVideoWidth / Player.PlaybackSession.NaturalVideoHeight;

            double dW, dH;
            if (containerRatio >= videoRatio)
            {
                dH = Container.ActualHeight;
                dW = dH * videoRatio;
            }
            else
            {
                dW = Container.ActualWidth;
                dH = dW / videoRatio;
            }
            // Multiply the width and height of the UI container by the device's scale factor
            var display = DisplayInformation.GetForCurrentView();
            int width = (int)PrivateExtensions.Round(dW* display.RawPixelsPerViewPixel), height = (int)PrivateExtensions.Round(dH * display.RawPixelsPerViewPixel);

            if (width < 1 || height < 1)
                return;

            lock (ResourceLock)
            {
                if (destinationTarget == null || destinationTarget.Description.Width != width || destinationTarget.Description.Height != height)
                {
                    destinationTarget?.Dispose();
                    destinationTarget = new CanvasRenderTarget(CanvasDevice, width, height, 96);
                    if (DrawingSurface == null)
                    {
                        DrawingSurface = CompositionDevice.CreateDrawingSurface2(new SizeInt32() { Width = width, Height = height }, DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
                        SurfaceBrush.Surface = DrawingSurface;
                    }
                    else DrawingSurface.Resize(new SizeInt32() { Width = width, Height = height });
                }
            }
        }
        #endregion
    }
}
