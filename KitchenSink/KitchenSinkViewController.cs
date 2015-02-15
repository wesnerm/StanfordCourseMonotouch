using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using iOSLibrary;

namespace KitchenSink
{
    public partial class KitchenSinkViewController : UIViewController
    {
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public KitchenSinkViewController(IntPtr handle) : base (handle)
        {
        }

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "Ask")
			{
				var asker = (AskerViewController) segue.DestinationViewController;
				asker.Question = "What food do you want in the sink?";
			}
		}

        [Action("CancelAsking")]
		public void CancelAsking()
		{
			
		}

        [Action("DoneAsking")]
		public void DoneAsking()
		{
			//var asker = (AskerViewController) segue.SourceViewController;

		}


		
		#region View lifecycle
		
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            // Perform any additional setup after loading the view, typically from a nib.
        }
		
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }
		
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }
		
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }
		
        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }
		
		#endregion
		
        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            if (UserInterfaceIdiomIsPhone)
            {
                return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
            } else
            {
                return true;
            }
        }
    }
}

