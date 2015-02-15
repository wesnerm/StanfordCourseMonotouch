
using System;
using System.Drawing;
using MonoTouch.CoreData;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using iOSLibrary;

namespace Photomania
{
	public partial class PhotosByPhotographerCDTVC : CoreDataTableViewController
	{
		Photographer _photographer;

		public PhotosByPhotographerCDTVC() : base("PhotosByPhotographerCDTVC", null)
		{
		}

		public PhotosByPhotographerCDTVC(IntPtr ptr) : base(ptr)
		{
			
		}


		public NSManagedObjectContext ManagedObjectContext { get; set; }

// When our Model is set here, we update our title to be the Photographer's name
//   and then set up our NSFetchedResultsController to make a request for Photos taken by that Photographer.

		[Export("Photographer")]
		public Photographer Photographer
		{
			get { return _photographer; }
			set
			{
				_photographer = value;
				Title = _photographer.Name;
				SetupFetchedResultsController();
			}
		}

// Creates an NSFetchRequest for Photos sorted by their title where the whoTook relationship = our Model.
// Note that we have the NSManagedObjectContext we need by asking our Model (the Photographer) for it.
// Uses that to build and set our NSFetchedResultsController property inherited from CoreDataTableViewController.

		void SetupFetchedResultsController()
		{
			if (Photographer.ManagedObjectContext != null)
			{
				var request = new NSFetchRequest("Photo")
				{
					SortDescriptors = new[] {new NSSortDescriptor("title", true, new Selector("localizedCaseInsensitiveCompare:")) },
					Predicate = NSPredicate.FromFormat("whoTook = %@", new NSObject[] { Photographer })
				};

				FetchedResultsController = new NSFetchedResultsController(request,
					Photographer.ManagedObjectContext, null, null);
			}
			else
			{
				FetchedResultsController = null;
			}
		}

// Loads up the cell for a given row by getting the associated Photo from our NSFetchedResultsController.

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var self = this;
			var cell = TableView.DequeueReusableCell("Photo");

			var photo = (Photo)self.FetchedResultsController.ObjectAt(indexPath);
			cell.TextLabel.Text = photo.Title;
			cell.DetailTextLabel.Text = photo.Subtitle;
			return cell;

		}


	}
}

