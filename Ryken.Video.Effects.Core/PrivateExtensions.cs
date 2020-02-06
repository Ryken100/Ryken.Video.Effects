using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Ryken.Video.Effects.Core
{
    static class PrivateExtensions
    {
        public static void RemoveWhere<T>(this ICollection<T> list, Func<T, bool> whereFunc)
        {
            while (list.Count(whereFunc) > 0)
            {
                var val = list.FirstOrDefault(whereFunc);
                if (list.Contains(val))
                    list.Remove(val);
            }
        }
        static bool shouldRunUIThreadRequest(UIElement element) => element.Dispatcher == null || element.Dispatcher.HasThreadAccess;
        public static async Task RunOnUIThread(this UIElement element, Action action)
        {
            if (shouldRunUIThreadRequest(element))
            {
                action();
            }
            else
            {
                await element.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => action());
            }
        }

        public static async Task<T> RunOnUIThread<T>(this UIElement element, Func<T> action)
        {
            if (shouldRunUIThreadRequest(element))
            {
                return action();
            }
            else
            {
                T val = default(T);
                await element.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => val = action());
                return val;
            }
        }

        public static async Task<T> RunOnUIThread<T>(this UIElement element, Func<Task<T>> action)
        {
            if (shouldRunUIThreadRequest(element))
            {
                return await action();
            }
            else
            {
                TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
                await element.Dispatcher.RunAsync(CoreDispatcherPriority.High, async () => tcs.TrySetResult(await action()));
                return await tcs.Task;
            }
        }

        public static double Round(this double d)
        {
            return Round(d, 1, RoundDirection.Both);
        }
        public static double Round(this double d, RoundDirection dir)
        {
            return Round(d, 1, dir);
        }
        public static double Round(this double d, double roundTo)
        {
            return Round(d, roundTo, RoundDirection.Both);
        }
        public static double Round(this double d, double roundTo, RoundDirection dir)
        {
            var remainder = d % roundTo;
            if (remainder == 0)
                return d;
            switch (dir)
            {
                case RoundDirection.Up:
                    d += roundTo - remainder;
                    break;
                case RoundDirection.Down:
                    d -= remainder;
                    break;
                case RoundDirection.Both:
                default:
                    if (remainder > roundTo * 0.5)
                    {
                        d += roundTo - remainder;
                    }
                    else d -= remainder;
                    break;
            }
            return d;
        }

        public static float Round(this float d)
        {
            return Round(d, 1, RoundDirection.Both);
        }
        public static float Round(this float d, RoundDirection dir)
        {
            return Round(d, 1, dir);
        }
        public static float Round(this float d, float roundTo)
        {
            return Round(d, roundTo, RoundDirection.Both);
        }
        public static float Round(this float d, float roundTo, RoundDirection dir)
        {
            var remainder = d % roundTo;
            if (remainder == 0)
                return d;
            switch (dir)
            {
                case RoundDirection.Up:
                    d += roundTo - remainder;
                    break;
                case RoundDirection.Down:
                    d -= remainder;
                    break;
                case RoundDirection.Both:
                default:
                    if (remainder > roundTo * 0.5)
                    {
                        d += roundTo - remainder;
                    }
                    else d -= remainder;
                    break;
            }
            return d;
        }
        public static Vector2 Round(this Vector2 d)
        {
            return Round(d, 1, RoundDirection.Both);
        }
        public static Vector2 Round(this Vector2 d, float roundTo)
        {
            return Round(d, roundTo, RoundDirection.Both);
        }
        public static Vector2 Round(this Vector2 d, Vector2 roundTo)
        {
            return Round(d, roundTo, RoundDirection.Both);
        }
        public static Vector2 Round(this Vector2 d, float roundTo, RoundDirection dir)
        {
            return Round(d, new Vector2(roundTo), dir);
        }
        public static Vector2 Round(this Vector2 d, Vector2 roundTo, RoundDirection dir)
        {
            d.X = Round(d.X, roundTo.X, dir);
            d.Y = Round(d.Y, roundTo.Y, dir);
            return d;
        }
        public static Vector3 Round(this Vector3 d)
        {
            return Round(d, 1, RoundDirection.Both);
        }
        public static Vector3 Round(this Vector3 d, float roundTo)
        {
            return Round(d, roundTo, RoundDirection.Both);
        }
        public static Vector3 Round(this Vector3 d, Vector3 roundTo)
        {
            return Round(d, roundTo, RoundDirection.Both);
        }
        public static Vector3 Round(this Vector3 d, float roundTo, RoundDirection dir)
        {
            return Round(d, new Vector3(roundTo), dir);
        }
        public static Vector3 Round(this Vector3 d, Vector3 roundTo, RoundDirection dir)
        {
            d.X = Round(d.X, roundTo.X, dir);
            d.Y = Round(d.Y, roundTo.Y, dir);
            d.Z = Round(d.Z, roundTo.Z, dir);
            return d;
        }
    }

    public enum RoundDirection { Up, Down, Both }
}
