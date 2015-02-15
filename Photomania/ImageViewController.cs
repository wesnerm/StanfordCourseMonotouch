
using System;
using System.Drawing;
using MonoTouch.CoreFoundation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace WM
{
    public partial class ImageViewController : UIViewController
    {
        NSUrl _imageUrl;
        UIImageView _imageView;
        UIPopoverController _popover;



        public ImageViewController() : base ("ImageViewController", null)
        {
        }

        public ImageViewController(IntPtr ptr) : base(ptr)
        {

            //NSBundle.MainBundle.LoadNib("ImageViewController", this, null);

        }

	    public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            // Perform any additional setup after loading the view, typically from a nib.
      
            scrollView.AddSubview(ImageView);
            scrollView.MinimumZoomScale = .2f;
            scrollView.MaximumZoomScale = 5.0f;
            scrollView.Delegate = new ScrollViewDelegate { Parent = this };

            if (titleBarButtonItem != null && Title != null)
                titleBarButtonItem.Title = Title;

            ResetImage();
        }

        public class ScrollViewDelegate : UIScrollViewDelegate
        {
            public ImageViewController Parent;

            public override UIView ViewForZoomingInScrollView(UIScrollView scrollView)
            {
                return Parent.ImageView;
            }
        }

        public UIImageView ImageView
        {
            get { return _imageView ?? (_imageView = new UIImageView(RectangleF.Empty)); }
        }


        public NSUrl ImageUrl
        {
            get { return _imageUrl; }
            set { 

                _imageUrl = value;
                ResetImage();
            }
        }

        public override string Title
        {
            get { return base.Title; }
            set 
            {
                base.Title = value;
                if (titleBarButtonItem != null && value != null)
                    titleBarButtonItem.Title = value;
            }
        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if (segueIdentifier=="Show URL")
                return ImageUrl != null && (_popover == null || !_popover.PopoverVisible);
            return true;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "Show URL")
            {
                var avc = (AttributedStringViewController) segue.DestinationViewController;
                avc.Text = new NSAttributedString(ImageUrl.Description);
                _popover = ((UIStoryboardPopoverSegue)segue).PopoverController;
            }
        }

        public void ResetImage()
        {
            if (scrollView==null)
                return;

            if (ImageView.Superview == null)
                scrollView.AddSubview(ImageView);

            scrollView.ContentSize = SizeF.Empty;
            scrollView.ZoomScale = 1f;
            ImageView.Image = null;

	        var imageUrl = ImageUrl;
	        var imageFetchQ = new DispatchQueue("image fetcher");
            if (spinner != null) spinner.StartAnimating();
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
            imageFetchQ.DispatchAsync( ()=>
			{
				NSThread.SleepFor(2.0);
				var imageData = ImageUrl == null ? null : NSData.FromUrl(ImageUrl);
				var image = imageData == null ? null : UIImage.LoadFromData(imageData);
				if (ImageUrl == imageUrl)
				{
					DispatchQueue.MainQueue.DispatchAsync(() =>
					{
                        if (spinner!=null) spinner.StopAnimating();
                        UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
                        if (image != null)
                        {
                            scrollView.ContentSize = image.Size;
    						ImageView.Image = image;
    						ImageView.Frame = new RectangleF(PointF.Empty, image.Size);
                        }
					});
				}
			});
        }

    }
}

