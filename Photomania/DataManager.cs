using System;
using MonoTouch.Foundation;
using MonoTouch.CoreData;
using MonoTouch.UIKit;
using iOSLibrary;

namespace Photomania
{
    public class DataManager
    {
        public DataManager()
        {
        }

        public UIManagedDocument document;
        public NSManagedObjectContext context;

        public void Init(NSUrl url)
        {
            var document = new UIManagedDocument(url);

            var fileManager = NSFileManager.DefaultManager;
            if (fileManager.FileExists(url.Path))
            {
               document.Open((success)=>
                {
                    if (success)
                    {
                        documentIsReady();
                    }
                    else
                    {
                        Logger.Log("couldn't open document at " + url);
                    }

                });
            }
            else
            {
                document.Save(url, UIDocumentSaveOperation.ForCreating,
                    (bool success)=> // completion handler
                {
                    if (success)
                    {
                        documentIsReady();
                    }
                    else
                    {
                        Logger.Log("couldn't create document at " + url);
                    }

                });
            }
        }

        void documentIsReady()
        {
            if (document.DocumentState == UIDocumentState.Normal)
            {
                var context = document.ManagedObjectContext;
                // work with context
            }
        }


        void Notifications()
        {
            var center = NSNotificationCenter.DefaultCenter;
            var observer = center.AddObserver(UIDocument.ChangeNewKey, //UIDocumentStateChangeNotification,
                               (NSNotification Notifications)=> {},
            document);

            center.RemoveObserver(observer);
        }
    }
}

