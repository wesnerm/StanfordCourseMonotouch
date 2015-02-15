using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreData;
using MonoTouch.MapKit;

namespace Photomania
{
    [Register("PhotosByPhotographerMapViewController")]
    public class PhotosByPhotographerMapViewController : MapViewController
    {
        public PhotosByPhotographerMapViewController()
        {
        }

        public PhotosByPhotographerMapViewController(IntPtr ptr) : base(ptr)
        {
        }


        Photographer _photographer;
        public Photographer Photographer
        {
            get
            {
                return _photographer;
            }
            set
            {
                _photographer = value;
            }
        }

    }
}

