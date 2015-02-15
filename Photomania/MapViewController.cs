
using System;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.MapKit;

namespace Photomania
{
    public partial class MapViewController : UIViewController
    {
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public MapViewController()
			: base (UserInterfaceIdiomIsPhone ? "MapViewController_iPhone" : "MapViewController_iPad", null)
        {
        }
		
        public MapViewController(IntPtr ptr) : base(ptr)
        {

        }

        [Outlet]
        public MKMapView MapView { get; set; }
		
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			MapView.Delegate  = new Del();
	        _needUpdateRegion = true;
        }

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			if (_needUpdateRegion)
				UpdateRegion();
		}

	    bool _needUpdateRegion;
		void UpdateRegion()
		{
			_needUpdateRegion = false;
			var boundingRect = new RectangleF();
			bool started = false;
			foreach (MKAnnotation annotation in MapView.Annotations)
			{
				var annotationRect = new RectangleF((float)annotation.Coordinate.Latitude, (float)annotation.Coordinate.Longitude, 0, 0);
				if (!started)
				{
					started = true;
					boundingRect = annotationRect;
				}
				else
				{
					boundingRect = RectangleF.Union(boundingRect, annotationRect);
				}
			}
			if (started)
			{
				boundingRect = boundingRect.Inset(-0.2f, -0.2f);
				if ((boundingRect.Size.Width < 20) && (boundingRect.Size.Height < 20))
				{
					MKCoordinateRegion region;
					region.Center.Latitude = boundingRect.Location.X + boundingRect.Size.Width/2;
					region.Center.Longitude = boundingRect.Location.Y + boundingRect.Size.Height/2;
					region.Span.LatitudeDelta = boundingRect.Size.Width;
					region.Span.LongitudeDelta = boundingRect.Size.Height;
					MapView.SetRegion(region, true);
				}
			}
		}

	    class Del : MKMapViewDelegate
		{
			public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView view)
			{
				if (view.LeftCalloutAccessoryView is UIImageView )
				{
					var imageView = (UIImageView) (view.LeftCalloutAccessoryView);
					var selector = new Selector("thumbnail");
					if (view.Annotation.RespondsToSelector(selector))
					{
						// this should be done in a different thread!
						imageView.Image = (UIImage) view.Annotation.ValueForKey((NSString)"thumbnail");
					}
				}
			}

		    static readonly NSString _reuseId = (NSString) @"MapViewController";
			public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, NSObject annotation)
			{
				MKAnnotationView view = mapView.DequeueReusableAnnotation(_reuseId);
				if (view == null)
				{
					view = new MKPinAnnotationView(annotation, _reuseId)
					{
						CanShowCallout = true, 
						LeftCalloutAccessoryView = new UIImageView(new RectangleF(0, 0, 30, 30))
					};
					if (mapView.Delegate.RespondsToSelector(new Selector("mapView:annotationView:calloutAccessoryControlTapped:")))
						view.RightCalloutAccessoryView = new UIButton(UIButtonType.DetailDisclosure);
				}

				if (view.LeftCalloutAccessoryView is UIImageView)
				{
					var imageView = (UIImageView) (view.LeftCalloutAccessoryView);
					imageView.Image = null;
				}

				return view;
			}

		}
    }
}

