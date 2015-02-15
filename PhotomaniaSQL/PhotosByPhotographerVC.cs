
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.CoreFoundation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using iOSLibrary;

namespace Photomania
{
	[Register("PhotosByPhotographerVC")]
	public partial class PhotosByPhotographerVC : UITableViewController
	{
		Photographer _photographer;
		List<Photo> _photos;
		Database _database;

		public PhotosByPhotographerVC() 
		{
		}

		public PhotosByPhotographerVC(IntPtr ptr) : base(ptr)
		{
			
		}


// When our Model is set here, we update our title to be the Photographer's name
//   and then set up our NSFetchedResultsController to make a request for Photos taken by that Photographer.

		public List<Photo> Photos
		{
			get { return _photos; }
			set { 
				_photos = value;
				DispatchQueue.MainQueue.DispatchAsync(() => TableView.ReloadData());
			}
		}

		public Database Database
		{
			get { return _database; }
			set { 
				_database = value;
				Reload();
			}
		}

		public Photographer Photographer
		{
			get { return _photographer; }
			set
			{
				_photographer = value;
				Title = _photographer.Name;
				Reload();
			}
		}

// Creates an NSFetchRequest for Photos sorted by their title where the whoTook relationship = our Model.
// Note that we have the NSManagedObjectContext we need by asking our Model (the Photographer) for it.
// Uses that to build and set our NSFetchedResultsController property inherited from CoreDataTableViewController.

		void Reload()
		{
			if (Database != null && Photographer != null)
			{
				var photos = Database.Photos.Where(p => _photographer.Id == p.PhotographerId)
					.OrderBy(x=>x.Title).ToList();
				Photos = photos;
			}
		}

// Loads up the cell for a given row by getting the associated Photo from our NSFetchedResultsController.

		class Src : UITableViewDataSource
		{
			public PhotosByPhotographerVC This;

			public override int RowsInSection(UITableView tableView, int section)
			{
				return This.Photos ==null ? 0 : This.Photos.Count;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = This.TableView.DequeueReusableCell("Photo");

				var photo = This.Photos[indexPath.Item];
				cell.TextLabel.Text = photo.Title;
				cell.DetailTextLabel.Text = photo.Subtitle;
				
				return cell;
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.DataSource = new Src {This = this};
		}

	}
}

