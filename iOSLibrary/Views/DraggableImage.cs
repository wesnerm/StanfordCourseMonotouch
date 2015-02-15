
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreAnimation;
using System.Text.RegularExpressions;
using System.Text;
using MonoTouch.AudioToolbox;
using MonoTouch.ObjCRuntime;
using System.Threading;

namespace WM
{

	public class DraggableImage : UIImageView
	{
		public DraggableImage(RectangleF frame) : base(frame)
		{
		}
		
		public PointF StartLocation { get; set; }
		
		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			var touch = touches.AnyObject as UITouch;
			if (touch == null) return;
			var pt = touch.LocationInView(this);
			StartLocation = pt;
			this.Superview.BringSubviewToFront(this);
		}
		
		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			// Move relative to the original touch point 
			var touch = touches.AnyObject as UITouch;
			if (touch == null) return;
			var pt = touch.LocationInView(this);
			var frame = Frame;
			frame.X += pt.X - StartLocation.X;
			frame.Y += pt.Y - StartLocation.Y;
			this.Frame = frame;
		}
		
		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			// Move relative to the original touch point 
			var touch = touches.AnyObject as UITouch;
			if (touch == null) return;
			var pt = touch.LocationInView(this);
			var frame = Frame;
			frame.X += pt.X - StartLocation.X;
			frame.Y += pt.Y - StartLocation.Y;
			Frame = frame;
		}


	}
}
