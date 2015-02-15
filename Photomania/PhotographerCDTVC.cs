using System;
using MonoTouch.ObjCRuntime;
using iOSLibrary;
using MonoTouch.Foundation;
using MonoTouch.CoreData;
using MonoTouch.UIKit;

namespace Photomania
{
	public class PhotographerCDTVC : CoreDataTableViewController
	{
		NSManagedObjectContext _managedObjectContext;

		public PhotographerCDTVC()
		{
		}

		public PhotographerCDTVC(string name, NSBundle bundle) : base(name, bundle)
		{
			
		}

		public PhotographerCDTVC(IntPtr handle) : base(handle)
		{
		}

		public NSManagedObjectContext ManagedObjectContext
		{
			get { return _managedObjectContext; }
			set
			{
				_managedObjectContext = value;
				if (value != null)
				{
					NSFetchRequest request = NSFetchRequest.FromEntityName("Photographer");
					request.SortDescriptors = new[] {new NSSortDescriptor("name", true, new Selector("localizedCaseInsensitiveCompare:"))};
					FetchedResultsController = new NSFetchedResultsController(request, _managedObjectContext, null, null);
				}
				else
				{
					FetchedResultsController = null;
				}
			}
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = TableView.DequeueReusableCell("Photographer");

			var photographer = (Photographer) FetchedResultsController.ObjectAt(indexPath);
			cell.TextLabel.Text = photographer.Name;
			cell.DetailTextLabel.Text = string.Format("{0} photos", photographer.Photos.Count);

			return cell;
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
				if (segue.Identifier == "setPhotographer:")
				{
					var photographer = (Photographer) FetchedResultsController.ObjectAt(indexPath);
					if (segue.DestinationViewController.RespondsToSelector(new Selector("setPhotographer:")))
						segue.DestinationViewController.PerformSelector(new Selector("setPhotographer:"), photographer, 0);
				}
			}

		}
	}
}

