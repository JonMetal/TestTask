using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Extensions
{
    public static class CollectionExtension
    {
        public static void CopyFrom<T>(this ICollection<T> toCollection, ICollection<T> fromCollection)
        {
            toCollection.Clear();
            foreach (var item in fromCollection)
            {
                toCollection.Add(item);
            }
        }
    }
}
