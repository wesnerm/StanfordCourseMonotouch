using System;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.CoreData;

//
//  CoreDataTableViewController.h
//
//  Created for Stanford CS193p Winter 2013.
//  Copyright 2013 Stanford University. All rights reserved.
//
// This class mostly just copies the code from NSFetchedResultsController's documentation page
//   into a subclass of UITableViewController.
//
// Just subclass this and set the fetchedResultsController.
// The only UITableViewDataSource method you'll HAVE to implement is tableView:cellForRowAtIndexPath:.
// And you can use the NSFetchedResultsController method objectAtIndexPath: to do it.
//
// Remember that once you create an NSFetchedResultsController, you CANNOT modify its @propertys.
// If you want new fetch parameters (predicate, sorting, etc.),
//  create a NEW NSFetchedResultsController and set this class's fetchedResultsController @property again.
//

namespace iOSLibrary
{
	[Register("CoreDataTableViewController")]
	public class CoreDataTableViewController : UITableViewController
	{
		// The controller (this class fetches nothing if this is not set).
		NSFetchedResultsController _fetchedResultsController;
		bool _beganUpdates;
		bool _suspendAutomaticTrackingOfChangesInManagedObjectContext;

		public CoreDataTableViewController()
		{
		}

		public CoreDataTableViewController(string name, NSBundle bundle) : base(name, bundle)
		{
		}

		public CoreDataTableViewController(IntPtr p) : base(p)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.Source = new SourceDelegate { This = this };
		}

		// Causes the fetchedResultsController to refetch the data.
		// You almost certainly never need to call this.
		// The NSFetchedResultsController class observes the context
		//  (so if the objects in the context change, you do not need to call performFetch
		//   since the NSFetchedResultsController will notice and update the table automatically).
		// This will also automatically be called if you change the fetchedResultsController @property.
		public void PerformFetch()
		{
			if (FetchedResultsController != null)
			{
				if (FetchedResultsController.FetchRequest.Predicate != null)
				{
					if (Debug) Logger.Log("[%@ PerformFetch] fetching %@ with predicate: %@", GetType(),
					FetchedResultsController.FetchRequest.EntityName,
					FetchedResultsController.FetchRequest.Predicate)
					;
				}
				else
				{
					if (Debug) Logger.Log("[%@ PerformFetch] fetching all %@ (i.e., no predicate)", GetType(),
					FetchedResultsController.FetchRequest.EntityName)
					;
				}
				NSError error;
				FetchedResultsController.PerformFetch(out error);
				if (error != null)
					Logger.Log(@"[%@ PerformFetch] %@", GetType(), error.LocalizedDescription);
			}
			else
			{
				if (Debug) Logger.Log(@"[%@ PerformFetch] no NSFetchedResultsController (yet?)", GetType())
				;
			}
			TableView.ReloadData();
		}

		public NSFetchedResultsController FetchedResultsController
		{
			get { return _fetchedResultsController; }
			set
			{
				var newfrc = value;
				var oldfrc = _fetchedResultsController;
				if (newfrc != oldfrc)
				{
					_fetchedResultsController = newfrc;

					if (newfrc != null)
						newfrc.Delegate = new FetchedResultsDelegate {This = this};
					
					if ((Title == null 
						|| oldfrc != null && Title == oldfrc.FetchRequest.Entity.Name) &&
					    (NavigationController == null || NavigationItem.Title == null))
					{

						Title = newfrc==null ? null : newfrc.FetchRequest.Entity.Name;
					}

					if (newfrc != null)
					{
						if (Debug)
							Logger.Log(@"[%@ FetchedResultsController] %@", GetType(), oldfrc != null? "updated" : "set");
						PerformFetch();
					}
					else
					{
						if (Debug) Logger.Log(@"[%@ FetchedResultsController] reset to nil", GetType());
						TableView.ReloadData();
					}
				}
			}
		}

		// Turn this on before making any changes in the managed object context that
		//  are a one-for-one result of the user manipulating rows directly in the table view.
		// Such changes cause the context to report them (after a brief delay),
		//  and normally our fetchedResultsController would then try to update the table,
		//  but that is unnecessary because the changes were made in the table already (by the user)
		//  so the fetchedResultsController has nothing to do and needs to ignore those reports.
		// Turn this back off after the user has finished the change.
		// Note that the effect of setting this to NO actually gets delayed slightly
		//  so as to ignore previously-posted, but not-yet-processed context-changed notifications,
		//  therefore it is fine to set this to YES at the beginning of, e.g., tableView:moveRowAtIndexPath:toIndexPath:,
		//  and then set it back to NO at the end of your implementation of that method.
		// It is not necessary (in fact, not desirable) to set this during row deletion or insertion
		//  (but definitely for row moves).
		public bool SuspendAutomaticTrackingOfChangesInManagedObjectContext
		{
			get { return _suspendAutomaticTrackingOfChangesInManagedObjectContext; }

			set
			{
				if (value)
				{
					_suspendAutomaticTrackingOfChangesInManagedObjectContext = true;
				}
				else
				{
					var sel = new Selector("endSuspensionOfUpdatesDueToContextChanges");
					PerformSelector(sel, null, 0);
				}
			}
		}

		[Export("endSuspensionOfUpdatesDueToContextChanges")]
		public void EndSuspensionOfUpdatesDueToContextChanges()
		{
			_suspendAutomaticTrackingOfChangesInManagedObjectContext = false;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			return null;
		}

		// Set to YES to get some debugging output in the console.
		public bool Debug { get; set; }


		public class SourceDelegate : UITableViewSource
		{
			public CoreDataTableViewController This;

			public bool Empty
			{
				get
				{
					if (This._fetchedResultsController == null)
						return true;

					if (This._fetchedResultsController.FetchedObjects.Length == 0)
						return true;

					return false;
				}
			}


			public override int NumberOfSections(UITableView tableView)
			{
				if (Empty)
					return 1;

				return (int) SectionsArray().Count;
			}

			public override int RowsInSection(UITableView tableView, int section)
			{
				if (Empty)
					return 0;
				return This._fetchedResultsController.FetchedObjects.Length;
				//return Sections(section).Count;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				if (Empty)
					return null;
				return This.GetCell(tableView, indexPath);
			}

			public override string TitleForHeader(UITableView tableView, int section)
			{
				//if (Empty)
				//	return null;
				
				//var s = Sections(section);
				//return s.Name;
				return null;
			}

			public override int SectionFor(UITableView tableView, string title, int atIndex)
			{
				//if (Empty)
				//	return 0;
				//return Self._fetchedResultsController.SectionFor(title, atIndex);
				return 0;
			}

			public override string[] SectionIndexTitles(UITableView tableView)
			{
				//if (Empty)
				//	return null;
				//return Array.ConvertAll(Sections(), x=>x.IndexTitle);
				return null;
			}

			readonly IntPtr _sectionsSelector = Selector.GetHandle("sections");
			IntPtr SectionsCall()
			{
				var f = This._fetchedResultsController;
				return Messaging.intptr_objc_msgSend(f.Handle, _sectionsSelector);
			}

			public NSFetchedResultsSectionInfo Sections(int index)
			{
				var h = SectionsCall();
				if (h == IntPtr.Zero)
					return null;

				var array = new NSArray(h);
				var n = array.ValueAt((uint) index);
				if (n == IntPtr.Zero)
					return null;

				return new MyNSFetchedResultsSectionInfo(n);

			}

			public NSArray SectionsArray()
			{
				var h = SectionsCall();
				if (h == IntPtr.Zero)
					return null;

				var array = new NSArray(h);
				return array;
			}


			public NSFetchedResultsSectionInfo[] Sections()
			{
				var array = SectionsArray();
				if (array == null)
					return null;

				var result = new NSFetchedResultsSectionInfo[array.Count];
				for (int i = 0; i < result.Length; i++ )
				{
					var elem = new MyNSFetchedResultsSectionInfo(array.ValueAt((uint)i));
					result[i] = elem;
				}
				return result;
			}
		}

		public class FetchedResultsDelegate : NSFetchedResultsControllerDelegate
		{
			public CoreDataTableViewController This;

			public override void DidChangeContent(NSFetchedResultsController controller)
			{
				if (This._beganUpdates)
					This.TableView.EndUpdates();
			}

			public override void DidChangeObject(NSFetchedResultsController controller, NSObject anObject, NSIndexPath indexPath,
			                                     NSFetchedResultsChangeType type, NSIndexPath newIndexPath)
			{
				if (!This.SuspendAutomaticTrackingOfChangesInManagedObjectContext)
				{
					switch (type)
					{
					case NSFetchedResultsChangeType.Insert:
						This.TableView.InsertRows(new[] {newIndexPath}, UITableViewRowAnimation.Fade);
						break;

					case NSFetchedResultsChangeType.Delete:
						This.TableView.DeleteRows(new[] {indexPath}, UITableViewRowAnimation.Fade);
						break;

					case NSFetchedResultsChangeType.Update:
						This.TableView.ReloadRows(new[] {indexPath}, UITableViewRowAnimation.Fade);
						break;

					case NSFetchedResultsChangeType.Move:
						This.TableView.DeleteRows(new[] {indexPath}, UITableViewRowAnimation.Fade);
						This.TableView.InsertRows(new[] {newIndexPath}, UITableViewRowAnimation.Fade);
						break;
					}
				}
			}

			public override void DidChangeSection(NSFetchedResultsController controller, INSFetchedResultsSectionInfo sectionInfo, uint sectionIndex,
				NSFetchedResultsChangeType type)
			{
				if (!This.SuspendAutomaticTrackingOfChangesInManagedObjectContext)
				{
					switch (type)
					{
					case NSFetchedResultsChangeType.Insert:
						This.TableView.InsertSections(new NSIndexSet(sectionIndex), UITableViewRowAnimation.Fade);
						break;

					case NSFetchedResultsChangeType.Delete:
						This.TableView.DeleteSections(new NSIndexSet(sectionIndex), UITableViewRowAnimation.Fade);
						break;
					}
				}

				base.DidChangeSection(controller, sectionInfo, sectionIndex, type);
			}

			public override void WillChangeContent(NSFetchedResultsController controller)
			{
				if (!This.SuspendAutomaticTrackingOfChangesInManagedObjectContext)
				{
					This.TableView.BeginUpdates();
					This._beganUpdates = true;
				}
			}

		}

      
        public class MyNSFetchedResultsSectionInfo : NSFetchedResultsSectionInfo
        {
            public MyNSFetchedResultsSectionInfo(IntPtr n) : base(n)
            {
            }

            public override int Count => 0;
             
            public override string Name => "";

            public override string IndexTitle => "";

            public override NSObject[] Objects => null;

        }

	}
}

