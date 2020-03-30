using System;
using System.ComponentModel;
using System.Linq;

namespace obs_cli.Helpers.Extensions
{
    public static class BindingListExtensions
    {
        public static void RemoveAll<T>(this BindingList<T> list, Func<T, bool> predicate)
        {
            foreach (var item in list.Where(predicate).ToArray())
            {
                list.Remove(item);
            }
        }

        public static void Move<T>(this BindingList<T> list, int oldIndex, int newIndex)
        {
            var item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);
        }
    }
}
