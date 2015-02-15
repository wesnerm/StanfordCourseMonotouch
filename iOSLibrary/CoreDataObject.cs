#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MonoTouch.CoreData;
using MonoTouch.Foundation;

#endregion

namespace iOSLibrary
{
	public class CoreDataObject : NSManagedObject
	{
		public CoreDataObject(IntPtr p) : base(p)
		{
		}

		public NSManagedObjectModel CreateModel()
		{
			var entity = new NSEntityDescription {
				Name = "TestEntity",
				Properties = new NSPropertyDescription[] { 
					new NSAttributeDescription
					{
						AttributeType = NSAttributeType.Integer32, 
						Name = "SomeId", 
						Optional = false
					}
				}
			};

			// add the entity to the model, and then add a configuration that
			// contains the entities
			var model = new NSManagedObjectModel {Entities = new[] {entity}};
			model.SetEntities(model.Entities, String.Empty);
			return model;
		}

		public NSPersistentStoreCoordinator CreateStore()
		{
			var appdocsdir =
				NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User).First();

			// get a location for the store
			var url = appdocsdir.Append("test.sqlite", false);
			var store = new NSPersistentStoreCoordinator(CreateModel());

			NSError error;
			store.AddPersistentStoreWithType((NSString) "SQLite", String.Empty, url, new NSDictionary(), out error);
			// ... test the error and report...
			
			return store;
		}

		public NSManagedObjectContext GetContext()
		{
			var context = new NSManagedObjectContext
			{
				PersistentStoreCoordinator = CreateStore()
			};
			//return this.context;
			return context;
		}


		void InsertingObjectsIntoTheContext()
		{
			var context = ManagedObjectContext;

			/*
			 * At first I tried to copy the methods in Objective-C examples by using
			 */

			NSEntityDescription.InsertNewObjectForEntityForName("TestEntity", context);

			/*
			 * but there appears to be issues with the bindings for that method. 
			 * In MonoTouch, the return type for InsertNewObjectForEntityForName 
			 * returns an NSEntityDescription but this method creates an 
			 * NSManagedObject which results in invalid cast exceptions. 
			 * I had to create instances of NSManagedObject directly. 
			 * Creating an NSManagedObject requires the NSEntityDescription for 
			 * the entity you want created and the NSManagedContext into which the 
			 * object will be placed. I discovered that the model didn’t appear 
			 * to have anything in the EntitiesByName dictionary, so we can’t 
			 * use that. So I use something like
			 */

			var entityDescription =
				context.PersistentStoreCoordinator.ManagedObjectModel.Entities
				.First(x =>x.Name == "TestEntity");
			var newObject = new NSManagedObject(entityDescription, context);

			/*
			 * Setting attribute values the object is done by using the SetValue method
			 */

			IntPtr p = new NSNumber(1234).Handle;
			newObject.SetValue(p, "SomeId");
		}


		void GettingObjectsOut()
		{
			var context = ManagedObjectContext;

			/*
			 * To retrieve our objects from the SQLite database we use an NSFetchRequest.
			 */

			// set up the request, fetch 20 at a time
			var request = new NSFetchRequest
			{
				Entity = Entity,
				FetchLimit = 20,
				ReturnsObjectsAsFaults = false,
				// define what order you want them in
				SortDescriptors = new[] {new NSSortDescriptor("SomeId", true)}
			};

			NSError error;
			NSObject[] retrievedObjects = context.ExecuteFetchRequest(request, out error);
			// ... test error object ...         

			var anObject = (NSManagedObject) retrievedObjects[0];

			// get the value of the SomeId attribute
			var p = anObject.ValueForKey("SomeId");
			int attributeValue = new NSNumber(p).Int32Value;

			/*
			 * Now, there are some plenty of other things to learn before we can really 
			 * use this, NSPredicate for filtering the data; retrieving the second 
			 * batch of records, versioning and migration just to name a few that 
			 * spring to mind, but I hope that this might serve as a starting point 
			 * for anyone else who is new to iOS and MonoTouch like me.
			 */
		}
	}
}