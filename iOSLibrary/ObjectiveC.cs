using System;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using System.Collections.Generic;
using MonoTouch.UIKit;

namespace WM
{
	public static class ObjectiveC
	{
		public static IEnumerable<NSObject> ToEnumerable(this NSArray array)
		{
			return array.ToEnumerable<NSObject>();
		}

		public static IEnumerable<T> ToEnumerable<T>(this NSArray array) where T : NSObject
		{
			for (uint i = 0; i < array.Count; i++)
				yield return (T) Runtime.GetNSObject(array.ValueAt(i));
		}

		public static T[] ToArray<T>(this NSArray array) where T : NSObject
		{
			return NSArray.FromArray<T>(array);
		}

		public static NSObject[] ToArray(this NSArray array)
		{
			return NSArray.FromArray<NSObject>(array);
		}

		public static List<T> ToList<T>(this NSArray array) where T : NSObject
		{
			return array.ToEnumerable<T>().ToList();
		}

		public static List<NSObject> ToList(this NSArray array)
		{
			return array.ToList<NSObject>();
		}

		public static void ForEach(this NSArray array, Action<NSObject> action)
		{
			for (uint i = 0; i < array.Count; i++)
				action(Runtime.GetNSObject(array.ValueAt(i)));
		}

		public static int IndexOf(this NSArray array, NSObject obj)
		{
			for (uint i = 0; i < array.Count; i++)
				if (array.ValueAt(i) == obj.Handle)
					return (int) i;
			return -1;
		}

		public static void TrySelector(this NSObject obj, Selector sel, NSObject withObject)
		{
			if (obj.RespondsToSelector(sel))
				obj.PerformSelector(sel, withObject, 0);
		}


		// TODO: NEED TO CHECK IF THIS IS STILL AN ISSUE
		public static DateTime NsDateToDateTime(NSDate date)
		{
			return (new DateTime(2001, 1, 1, 0, 0, 0)).AddSeconds(date.SecondsSinceReferenceDate);
		}

		public static NSDate DateTimeToNsDate(DateTime date)
		{
			return NSDate.FromTimeIntervalSinceReferenceDate((date - (new DateTime(2001, 1, 1, 0, 0, 0))).TotalSeconds);
		}

		public static IEnumerable<T> ToEnumerable<T>(this NSSet set) where T : NSObject
		{
			var res = new List<T>();
			set.Enumerate(delegate(NSObject obj, ref bool stop)
			{
				var item = (T) (obj); // Executed only once, not 3 times
				if (item != null)
				{
					res.Add(item);
					stop = false; // inside the null check
				}
			});
			return res;
		}

		public static void ShowHideTabBar(UITabBarController view, bool visible)
		{
			view.View.Subviews[1].Hidden = !visible;
		}

		public static void AutoDismissKeyboard(UITextField textField)
		{
			textField.ShouldReturn = delegate
			{
				textField.ResignFirstResponder();
				return true;
			};
		}

		public static bool IsPhone()
		{
			return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
		}
	}
}

