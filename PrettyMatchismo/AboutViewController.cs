
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace WM
{
    public partial class AboutViewController : UIViewController
    {
        public AboutViewController() : base ("AboutViewController", null)
        {
        }

        public AboutViewController(IntPtr handle) : base(handle)
        {
            NSBundle.MainBundle.LoadNib("AboutViewController", this, null);

            //if (this.View != null)
            //    NSBundle.MainBundle.LoadNib("SameView", this.View, null);
            //else
            //    Console.WriteLine("View is null");
        }
		
        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }
		
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}

