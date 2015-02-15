using System;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using System.Collections.Generic;

namespace WM
{
    public static class ObjectiveC
    {
        public static IEnumerable<NSObject> ToEnumerable(this NSArray array)
        {
            for(uint i=0; i<array.Count; i++)
                yield return Runtime.GetNSObject(array.ValueAt(i));

        }

        public static IEnumerable<T> ToEnumerable<T>(this NSArray array)
            where T : NSObject
        {
            for(uint i=0; i<array.Count; i++)
                yield return (T) Runtime.GetNSObject(array.ValueAt(i));
            
        }

        public static void ForEach(this NSArray array, Action<NSObject> action)
        {
            for(uint i=0; i<array.Count; i++)
                action(Runtime.GetNSObject(array.ValueAt(i)));
        }

        public static int IndexOf(this NSArray array, NSObject obj)
        {
            for(uint i=0; i<array.Count; i++)
                if (array.ValueAt(i)==obj.Handle)
                    return (int)i;
            return -1;
        }

    
    }
}

