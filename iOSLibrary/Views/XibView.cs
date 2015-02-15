using System;
using MonotouchHelper;

namespace MonotouchHelper
{
    using System;
    
    using MonoTouch.ObjCRuntime;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;
    
    namespace Member.iOS.Base.Views {
        
        /// <summary>
        /// XibView is a Xib-backed UIView subclass which enables easy customization and custom
        /// behavior addition.
        /// </summary>
        public class XibView : UIView {
            
            /// <summary>
            /// Exception thrown when a loaded XIB doesn't contain any views inside it.
            /// </summary>
            class EmptyXibException : Exception {
            }
            
            /// <summary>
            /// Initializes a new instance of the <see cref="Member.iOS.Base.Views.XibView"/> class.
            /// </summary>
            /// <param name='handle'>
            /// Handle.
            /// </param>
            public XibView(IntPtr handle) : base(handle) {
            }
            
            /// <summary>
            /// Upon loading from a containing XIB, takes care of replacing the current instance (which acts as a stab) with
            /// a real view loaded from its XIB.
            /// </summary>
            public override void AwakeFromNib() {
                base.AwakeFromNib();
                
                NSArray views = NSBundle.MainBundle.LoadNib(GetType().Name, this, new NSDictionary());
                
                if (views.Count == 0) {
                    throw new EmptyXibException();
                }
                
                UIView rootView = Runtime.GetNSObject(views.ValueAt(0)) as UIView;
                rootView.Frame = new System.Drawing.RectangleF(0, 0, Frame.Width, Frame.Height);
                AddSubview(rootView);
            }
            
        }
    }
}

