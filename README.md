# Ryken.Video.Effects
A .NET UWP library to make it a little easier for you to render videos with Win2D effects.

# Using this component
All code below this point assumes you've imported the `Ryken.Video.Effects` namespace.

## Step 1: Register your MediaPlayer/MediaElement
First you need to register your video playback method to receive effects, whether it be `MediaPlayer`, `MediaElement` or `MediaClip`.
You do this by calling either:

```csharp
VideoEffectManager.AddVideoEffect(MediaElement mediaClip, string id, IPropertySet properties)
```
```csharp
VideoEffectManager.AddVideoEffect(MediaClip mediaClip, string id, IPropertySet properties)
```
```csharp
VideoEffectManager.AddVideoEffect(MediaPlayer mediaPlayer, string id, IPropertySet properties)
```
```csharp
VideoEffectManager.AddFrameServerVideoEffect(MediaPlayer mediaPlayer, string id, IPropertySet properties, FrameworkElement container)
```

The parameter `id` can be whatever string you want, and any effects that are registered with this same `id` will be used to render your video (we'll get to this later).

Using `VideoEffectManager.AddFrameServerVideoEffect` puts the `MediaPlayer` into frame server mode, and renders it on top of the provided `container` element.
This mode may be useful to some, as the resolution of the video surface will be in screen resolution rather than video resolution, which allows for custom upscaling scenarios.

## Step 2: Register an effect
Next, you register an effect using:
```csharp
AddVideoHandler.RegisterVideoEffectHandler(string id, IVideoEffectHandler handler)
```
The parameter `id` must be the same used to register your playback method in the previous step.
`handler` is the object that will render the video using effects. You can either implement `IVideoEffectHandler` yourself to get the effects you want,
or you can use one of the handlers than come in the library such as `SaturationEffectHandler`.

**That's it!** Just call `VideoEffectManager.AddVideoEffect/AddFrameServerVideoEffect` and `AddVideoHandler.RegisterVideoEffectHandler` you're good to go!

# Examples
## Render a video in black and white
This small example shows how to render a video in black and white using `SaturationEffectHandler` with the `Saturation` property set to 0.
This example assumes you've already created a `MediaPlayer`, `MediaElement` or `MediaClip`.
```csharp
// First set your MediaPlayer up to receive effects. This can also be a MediaElement or MediaClip.
VideoEffectManager.AddVideoEffect(mediaPlayer, "Any old string works as an id", null);

// Create the saturation effect and set saturation to zero
var saturationEffect = new SaturationEffectHandler() { Saturation = 0 };

// Register the saturation effect with the same id string you used in .AddVideoEffect.
VideoEffectManager.RegisterVideoEffectHandler("Any old string works as an id", saturationEffect);
```
Now your video is in black and white!

## Implementing IVideoEffectHandler
If you want to create your own effect, you'll have to implement `IVideoEffectHandler`.
`IVideoEffectHandler` has three methods, `ProcessFrame`, `CreateResources` and `Destroy` resources.

`ProcessFrame(IVideoEffectHandlerArgs args)` is where you draw the video frames. It receives arguments that make it easy to start drawing with Win2D. This will be called automatically for each frame by `VideoEffectManager`.

`CreateResources()` is where you create the resources you'll need in `ProcessFrame`. You probably won't have to implement this method.

`DestroyResources()` is where you destroy the resources made in `CreateResources`, if any.

To render a video frame in `ProcessFrame(IVideoEffectHandlerArgs args)`, you use Win2D to draw `args.InputFrame` onto `args.OutputFrame`, by creating a `CanvasDrawingSession` on `args.OutputFrame`.
While this won't serve as a tutorial for Win2D, the example code below will show you how to implement `ProcessFrame` to render a video with a gaussian blur effect:
```csharp
void IVideoEffectHandler.ProcessFrame(IVideoEffectHandlerArgs args)
{
    using (var drawingSession = args.OutputFrame.CreateDrawingSession())
    {
        using (var blur = new GaussianBlurEffect() { Source = args.InputFrame, BlurAmount = 25 })
        {
            drawingSession.DrawImage(blur, args.OutputFrame.Bounds, args.InputFrame.Bounds);
        }
    }
}
```


