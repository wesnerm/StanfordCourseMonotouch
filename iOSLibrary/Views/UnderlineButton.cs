using System.Drawing;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;

namespace WM
{
	public class UnderlineButton : UIButton
	{
		public override void Draw (RectangleF rect)
		{
			base.Draw (rect);
			// Red underline
			CGContext context = UIGraphics.GetCurrentContext();
			context.SetRGBFillColor(0xFF,0xFF,0,1);
			context.SetLineWidth(1);
			context.MoveTo(0,rect.Height -5);
			context.AddLineToPoint(rect.Width,rect.Height -5);
			context.StrokePath();
		}
	}
}