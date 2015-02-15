using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MonoTouch.CoreFoundation;
using MonoTouch.ObjCRuntime;
using WM;
using iOSLibrary;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SQLite;

namespace Photomania
{
	[Register("PhotographerCDTVC")]
	public class PhotographerCDTVC : UITableViewController
	{
		readonly List<Photographer> _photographers = new List<Photographer>();
		Database _database;

		public PhotographerCDTVC()
		{
		}

		public PhotographerCDTVC(string name, NSBundle bundle) : base(name, bundle)
		{
			
		}

		public PhotographerCDTVC(IntPtr handle) : base(handle)
		{
		}


		public List<Photographer> Photographers
		{
			get
			{
				return _photographers;
			}
			set
			{
				if (_photographers == value)
					return;
				_photographers.Clear();
				_photographers.AddRange(value);
				DispatchQueue.MainQueue.DispatchAsync(()=>TableView.ReloadData());
			}
		}

		public Database Database
		{
			get { return _database; }
			set
			{
				_database = value;
				Reload();
			}
		}

		class Src : UITableViewDataSource
		{
			public PhotographerCDTVC This;

			public override int RowsInSection(UITableView tableView, int section)
			{
				return This.Photographers.Count;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = This.TableView.DequeueReusableCell("Photographer");

				var photographer = This.Photographers[indexPath.Item];
				cell.TextLabel.Text = photographer.Name;
				cell.DetailTextLabel.Text = string.Format("{0} photos", 1);
				return cell;
			}
		}

		public void Reload()
		{
			if (_database == null) return;
			//Database.Queue.DispatchAsync(() =>
			{
				Photographers = _database.Photographers.OrderBy(p => p.Name).ToList();
			}//);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			if (Database == null)
				Database = Database.ConstructDatabase();
			TableView.DataSource = new Src() { This = this };
			if (RefreshControl != null)
				RefreshControl.AddTarget((s, a) => { Refresh(); },
				UIControlEvent.ValueChanged);
		}


		// Gets the NSIndexPath of the UITableViewCell which is sender.
		// Then uses that NSIndexPath to find the Photographer in question using NSFetchedResultsController.
		// Prepares a destination view controller through the "setPhotographer:" segue by sending that to it.

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			NSIndexPath indexPath = null;

			if (sender is UITableViewCell)
				indexPath = TableView.IndexPathForCell((UITableViewCell)sender);

			if (indexPath != null)
			{
				var c = new Class(typeof (NSObject));
				var c1 = new Class(typeof (PhotographerCDTVC));
				var c2 = new Class(typeof (PhotosByPhotographerVC));


				if (segue.Identifier == "setPhotographer:")
				{
					var photographer = Photographers[indexPath.Item];
					((PhotosByPhotographerVC) segue.DestinationViewController).Photographer = photographer;
				}
			}
		}

		// Either creates, opens or just uses the demo document
		//   (actually, it will never "just use" it since it just creates the UIManagedDocument instance here;
		//    the "just uses" case is just shown that if someone hands you a UIManagedDocument, it might already
		//    be open and so you can just use it if it's documentState is UIDocumentStateNormal).
		//
		// Creating and opening are asynchronous, so in the completion handler we set our Model (managedObjectContext).

		// Fires off a block on a queue to fetch data from Flickr.
		// When the data comes back, it is loaded into Core Data by posting a block to do so on
		//   self.managedObjectContext's proper queue (using performBlock:).
		// Data is loaded into Core Data by calling photoWithFlickrInfo:inManagedObjectContext: category method.

		[Action("refresh")]
		void Refresh()
		{
			RefreshControl.BeginRefreshing();
//			Database.Queue.DispatchAsync(() =>
			{
				var photos = FlickrFetcher.RecentGeoreferencedPhotos();
				// put the photos in Core Data
				foreach (NSDictionary photo in photos.ToEnumerable())
					Photo.PhotoWithFlickrInfo(photo, Database);
				Reload();
				DispatchQueue.MainQueue.DispatchAsync(() => RefreshControl.EndRefreshing());
			}//);

		}

	}
}

