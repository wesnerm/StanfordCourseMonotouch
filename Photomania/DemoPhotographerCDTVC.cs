
using System;
using System.Drawing;
using System.Linq;
using MonoTouch.CoreData;
using MonoTouch.CoreFoundation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using WM;

namespace Photomania
{
    public partial class DemoPhotographerCDTVC : PhotographerCDTVC
    {
        public DemoPhotographerCDTVC() : base ("DemoPhotographerCDTVC", null)
        {


        }

        public DemoPhotographerCDTVC(IntPtr p) : base(p)
        {

        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			if (RefreshControl != null)
				RefreshControl.AddTarget((s,a) => { Refresh(); },
				UIControlEvent.ValueChanged);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			if (ManagedObjectContext == null)
				UseDemoDocument();

		}


		// Either creates, opens or just uses the demo document
//   (actually, it will never "just use" it since it just creates the UIManagedDocument instance here;
//    the "just uses" case is just shown that if someone hands you a UIManagedDocument, it might already
//    be open and so you can just use it if it's documentState is UIDocumentStateNormal).
//
// Creating and opening are asynchronous, so in the completion handler we set our Model (managedObjectContext).

		void UseDemoDocument()
		{
			var fileManager = NSFileManager.DefaultManager;
			var docUrl = fileManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User).First();
			var url = docUrl.Append(@"Demo Document", false);
			var document = new UIManagedDocument(url);

			//var modelPath = NSBundle.MainBundle.ResourceUrl;
			//var modelUrl = modelPath.Append("Photomania.mom", false);
			//var mom = new NSManagedObjectModel(modelUrl);
			//var store = new NSPersistentStoreCoordinator(mom);
			//NSError error;
			//store.AddPersistentStoreWithType((NSString)"SQLite", "", modelUrl, 
			//	new NSDictionary(), out error);

			//var moc = new NSManagedObjectContext {PersistentStoreCoordinator = store};
			//ManagedObjectContext = moc;

			//	document.ManagedObjectContext = moc;


			if (!fileManager.FileExists(url.Path))
			{
				document.Save(url, UIDocumentSaveOperation.ForCreating, success =>
				{
					if (success)
					{
						ManagedObjectContext = document.ManagedObjectContext;
						Refresh();
					}
				});
			}
			else if (document.DocumentState == UIDocumentState.Closed)
			{
				document.Open(success =>
				{
					if (success)
						ManagedObjectContext = document.ManagedObjectContext;
				});
			}
			else
			{
				ManagedObjectContext = document.ManagedObjectContext;
			}
		}

		// Fires off a block on a queue to fetch data from Flickr.
		// When the data comes back, it is loaded into Core Data by posting a block to do so on
		//   self.managedObjectContext's proper queue (using performBlock:).
		// Data is loaded into Core Data by calling photoWithFlickrInfo:inManagedObjectContext: category method.

		[Action("refresh")]
		void Refresh()
		{
			RefreshControl.BeginRefreshing();
			var fetchQ = new DispatchQueue("Flickr Fetch");
			fetchQ.DispatchAsync(() =>
			{
				var photos = FlickrFetcher.RecentGeoreferencedPhotos();
				// put the photos in Core Data
				ManagedObjectContext.Perform(delegate
				{
					foreach (NSDictionary photo in photos.ToEnumerable())
						Photo.PhotoWithFlickrInfo(photo, ManagedObjectContext);
					DispatchQueue.MainQueue.DispatchAsync(() => RefreshControl.EndRefreshing());
				});
			});
		
		}

		

    }
}

