using Microsoft.Graphics.Canvas;
using Ryken.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Media.Editing;
using Windows.Media.Effects;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Ryken.Video.Effects.Core
{
    public static class VideoEffectManager
    {
        static object listLock = new object();
        const string IDKey = "ID";

        static List<FrameServerHandler> frameServerHandlers = new List<FrameServerHandler>();
        static Dictionary<string, List<WeakReference<IVideoEffectHandler>>> handlers = new Dictionary<string, List<WeakReference<IVideoEffectHandler>>>();

        /// <summary>
        /// Adds a video effect to the MediaPlayer that uses MediaPlayer.IsVideoFrameServerEnabled instead of IBasicVideoEffect
        /// </summary>
        /// <param name="mediaPlayer"></param>
        /// <param name="id"></param>
        /// <param name="properties"></param>
        /// <param name="container"></param>
        public static void AddFrameServerVideoEffect(MediaPlayer mediaPlayer, string id, IPropertySet properties, FrameworkElement container)
        {
            if (mediaPlayer == null)
                throw new ArgumentNullException(nameof(mediaPlayer));
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Effect ID not set", nameof(id));
            if (properties == null)
            {
                properties = new PropertySet();
            }
            if (!properties.ContainsKey(IDKey))
                properties.Add(IDKey, id);
            if (frameServerHandlers.Count(h => h.Player == mediaPlayer && h.ID == id) > 0)
                throw new InvalidOperationException("This player has already been registered with this ID in frame server mode");
            var handler = new FrameServerHandler(mediaPlayer, container, id, properties);
            frameServerHandlers.Add(handler);
        }
        public static void RemoveFrameServerVideoEffect(MediaPlayer mediaPlayer)
        {
            frameServerHandlers.RemoveWhere(h =>
            {
                if (h.Player == mediaPlayer)
                {
                    h.Dispose();
                    return true;
                }
                return false;
            });
        }
        public static void RemoveFrameServerVideoEffect(MediaPlayer mediaPlayer, string id)
        {
            frameServerHandlers.RemoveWhere(h =>
            {
                if (h.Player == mediaPlayer && h.ID == id)
                {
                    h.Dispose();
                    return true;
                }
                return false;
                
            });
        }
        /// <summary>
        /// Add a Ryken.Video.Effects.VideoEffect to the the MediaElement
        /// </summary>
        /// <param name="mediaElement">The MediaElement to add the effect to</param>
        /// <param name="id">The ID to use for this effect to frames can be processed by IVideoHandler</param>
        /// <param name="properties">Additional properties that will be passed to IVideoEffectHandler implementations registered with the provided ID. Can be null.</param>
        public static void AddVideoEffect(MediaElement mediaElement, string id, IPropertySet properties)
        {
            if (mediaElement == null)
                throw new ArgumentNullException(nameof(mediaElement));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Effect ID not set", nameof(id));
            if (properties == null)
            {
                properties = new PropertySet();
            }
            if (!properties.ContainsKey(IDKey))
                properties.Add(IDKey, id);
            mediaElement.AddVideoEffect(typeof(VideoEffect).FullName, false, properties);
        }

        /// <summary>
        /// Add a Ryken.Video.Effects.VideoEffect to the the MediaPlayer
        /// </summary>
        /// <param name="mediaPlayer">The MediaPlayer to add the effect to</param>
        /// <param name="id">The ID to use for this effect to frames can be processed by IVideoHandler</param>
        /// <param name="properties">Additional properties that will be passed to IVideoEffectHandler implementations registered with the provided ID. Can be null.</param>
        [DefaultOverload]
        public static void AddVideoEffect(MediaPlayer mediaPlayer, string id, IPropertySet properties)
        {
            if (mediaPlayer == null)
                throw new ArgumentNullException(nameof(mediaPlayer));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Effect ID not set", nameof(id));
            if (properties == null)
            {
                properties = new PropertySet();
            }
            if (!properties.ContainsKey(IDKey))
                properties.Add(IDKey, id);
            mediaPlayer.AddVideoEffect(typeof(VideoEffect).FullName, false, properties);
        }

        /// <summary>
        /// Add a Ryken.Video.Effects.VideoEffect to the the MediaClip
        /// </summary>
        /// <param name="mediaClip">The MediaClip to add the effect to</param>
        /// <param name="id">The ID to use for this effect to frames can be processed by IVideoHandler</param>
        /// <param name="properties">Additional properties that will be passed to IVideoEffectHandler implementations registered with the provided ID. Can be null.</param>
        public static void AddVideoEffect(MediaClip mediaClip, string id, IPropertySet properties)
        {
            if (mediaClip == null)
                throw new ArgumentNullException(nameof(mediaClip));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Effect ID not set", nameof(id));
            if (properties == null)
            {
                properties = new PropertySet();
            }
            if (!properties.ContainsKey(IDKey))
                properties.Add(IDKey, id);
            mediaClip.VideoEffectDefinitions.Add(new VideoEffectDefinition(typeof(VideoEffect).FullName, properties));
        }

        /// <summary>
        /// Register an IVideoEffectHandler so it may begin receiving frames from video effects registered with the provided ID
        /// </summary>
        /// <param name="id">The ID of effects the handler should receive frames from</param>
        /// <param name="handler">The handler to be registered</param>
        public static void RegisterVideoEffectHandler(string id, IVideoEffectHandler handler)
        {
            bool exists = false;
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Effect ID not set", nameof(id));
            List<WeakReference<IVideoEffectHandler>> list = null;
            foreach (var kvp in handlers)
            {
                if (kvp.Key == id)
                    list = kvp.Value;
                foreach (var weak in kvp.Value)
                {
                    if (weak.TryGetTarget(out var existingHandler))
                    {
                        if (existingHandler == handler)
                        {
                            exists = true;
                            if (list != null)
                                break;
                        }
                    }
                }
                if (exists && list != null)
                    break;
            }
            if (list == null)
            {
                // Create list and add it to the dictionary if it doesn't exist
                list = new List<WeakReference<IVideoEffectHandler>>();
                handlers.Add(id, list);
            }
            lock (listLock)
            {
                foreach (var weakRef in list)
                {
                    if (weakRef.TryGetTarget(out var existingHandler) && existingHandler == handler)
                    {
                        // Return if the handler has already been added via this ID
                        return;
                    }
                }
                if (!exists)
                    handler.CreateResources();
                list.Add(new WeakReference<IVideoEffectHandler>(handler));
            }
            CleanHandlerList();
        }

        /// <summary>
        /// Unregister all registered instances of the provided handler from all IDs
        /// </summary>
        /// <param name="handler">The handler to unregister</param>
        public static void UnregisterVideoEffectHandler(IVideoEffectHandler handler)
        {
            lock (listLock)
            {
                foreach (var kvp in handlers)
                {
                    if (kvp.Value != null)
                    {
                        for (int i = 0; i < kvp.Value.Count; i++)
                        {
                            if (i >= 0)
                            {
                                if (kvp.Value[i].TryGetTarget(out var existingHandler) && existingHandler == handler)
                                {
                                    kvp.Value.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                }
                handler.DestroyResources();
                CleanHandlerList();
            }
        }

        /// <summary>
        /// Unregister the instance of the provided handler that has been registered with the provided ID
        /// </summary>
        /// <param name="id">The ID the handler was registered with</param>
        /// <param name="handler">The handler to unregister</param>
        public static void UnregisterVideoEffectHandler(string id, IVideoEffectHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Effect ID not set", nameof(id));
            lock (listLock)
            {
                bool exists = false;
                foreach (var kvp in handlers)
                {
                    if (kvp.Value != null)
                    {
                        for (int i = 0; i < kvp.Value.Count; i++)
                        {
                            if (i >= 0)
                            {
                                if (kvp.Value[i].TryGetTarget(out var existingHandler) && existingHandler == handler)
                                {
                                    if (kvp.Key == id)
                                    {
                                        kvp.Value.RemoveAt(i);
                                        i--;
                                    }
                                    else
                                    {
                                        exists = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if (exists)
                    handler.DestroyResources();
            }
        }
        static void CleanHandlerList()
        {
            lock (listLock)
            {
                foreach (var kvp in handlers)
                {
                    if (kvp.Value != null)
                    {
                        for (int i = 0; i < kvp.Value.Count; i++)
                        {
                            if (i >= 0)
                            {
                                // Remove the weak reference if the object is no longer referenced
                                if (!kvp.Value[i].TryGetTarget(out var existingHandler))
                                {
                                    kvp.Value.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static bool ProcessFrame(IVideoEffectHandlerArgs args)
        {
            bool returnVal = false;
            if (!string.IsNullOrWhiteSpace(args.ID) && handlers.TryGetValue(args.ID, out var list))
            {
                lock (listLock)
                {
                    bool firstComplete = false;
                    foreach (var weakRef in list)
                    {
                        if (weakRef.TryGetTarget(out var handler))
                        {
                            if (firstComplete)
                            {
                                // Draw the output frame back onto the input frame after the first effect
                                // This way, the output from previous effects is used as input for upcoming effects
                                if (args.InputFrame is CanvasRenderTarget input)
                                {
                                    using (var ds = input.CreateDrawingSession())
                                    {
                                        ds.DrawImage(args.OutputFrame, input.Bounds, args.OutputFrame.Bounds);
                                    }
                                }
                            }
                            handler.ProcessFrame(args);
                            firstComplete = true;
                            returnVal = true;
                        }
                    }
                }
            }
            return returnVal;
        }
    }
}
