using System;
using System.Runtime.InteropServices;
using MonoTouch.Foundation;

namespace iOSLibrary
{

	public class Logger
	{
		[DllImport(MonoTouch.Constants.FoundationLibrary)]
		public extern static void NSLog(IntPtr message);

		public static void Log(string msg, params object[] args)
		{
			using (var nss = new NSString(string.Format(msg, args)))
			{
				NSLog(nss.Handle);
			}
		}
	}
}
