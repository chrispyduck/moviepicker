using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MoviePicker.Magic
{
    public static class Extensions
    {
        static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        public static T GetRandom<T>(this IList<T> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            // load random bytes into buffer and interpret as int32
            var buffer = new byte[sizeof(int)];
            rng.GetBytes(buffer);
            var @uint = BitConverter.ToUInt32(buffer, 0);

            // reinterpret the int32 as a percentage and use it to compute a random index value
            var pct = (double)@uint / uint.MaxValue;
            var idx = (int)Math.Round(options.Count * pct);

            return options[idx];
        }

        public static void Split<T>(this IEnumerable<T> source, Func<T, bool> selector, IList<T> trueList, IList<T> falseList)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (trueList == null)
                throw new ArgumentNullException(nameof(trueList));
            if (falseList == null)
                throw new ArgumentNullException(nameof(falseList));

            foreach (var item in source)
            {
                if (selector(item))
                    trueList.Add(item);
                else
                    falseList.Add(item);
            }
        }

        public static T GetVisualChild<T>(this DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
