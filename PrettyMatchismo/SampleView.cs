using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;
using System.Collections.Generic;

namespace WM
{
    [Register("SampleView")]
    public class SampleView : UIView
    {
        public SampleView(IntPtr p) : base(p)
        {
            BackgroundColor = UIColor.Black;
            //var array = NSBundle.MainBundle.LoadNib("SampleView", this, null);
            var array = Nib.Instantiate(this, null);
            AddSubview((UIView) array[0]);
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public static readonly UINib Nib = UINib.FromName("SampleView", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("SampleView");

        public static SampleView Create()
        {
            return (SampleView)Nib.Instantiate(null, null)[0];
        }

        UIView _view;

        [Outlet] 
        UIView view
        {
            get { return _view; }
            set { 
                _view = value; 
                AddSubview(value);
            }
        }


    }
}

