using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreData;

namespace Photomania
{
    [Register("PhotographerMapViewController")]
    public class PhotographerMapViewController : MapViewController
    {
        public NSManagedObjectContext managedObjectContext;

        public PhotographerMapViewController()
        {
        }

        public PhotographerMapViewController(IntPtr ptr) : base(ptr)
        {

		}

		#region Demo



		#endregion


	}
}

