using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreText;

namespace WM
{
    public partial class AttributedViewController : UIViewController
    {
        public AttributedViewController(IntPtr handle) : base (handle)
        {
        }
		
        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }
		
		#region View lifecycle
		
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            updateSelectedWord(null);
			
            // Perform any additional setup after loading the view, typically from a nib.
        }
		
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }
		
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }
		
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }
		
        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }
		
		#endregion
		
        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
        }


        public string[] wordList
        {
            get 
            { 
                var s = _label.AttributedText.Value.Split(' ');

                if (s.Length == 0)
                    return new [] { "" };
                //NSCharacterSet.WhitespaceAndNewlines;
                return s;
            }
        }

        public string selectedWord
        {
            get {
                return wordList[(int)_selectedWordStepper.Value];
            }
        }

        partial void updateSelectedWord(NSObject sender)
        {
            _selectedWordStepper.MaximumValue = wordList.Length -1;
            _selectedWordLabel.Text = selectedWord;

        }

        void addLabelAttributes(NSDictionary attr, NSRange range)
        {
            var i = _label.AttributedText.Value.IndexOf(selectedWord);
            if (i >= 0)
            {
                var mat = (NSMutableAttributedString)_label.AttributedText.MutableCopy();
                mat.AddAttributes( attr, range);
                _label.AttributedText = mat;
            }

        }

        void addSelectedWordAttributes(NSDictionary attr)
        {
            var i = _label.AttributedText.Value.IndexOf(selectedWord);
            if (i >= 0)
                addLabelAttributes(attr, new NSRange(i, selectedWord.Length));
        }

        partial void underline()
        {
            var ct = new CTStringAttributes();
            ct.UnderlineStyle = CTUnderlineStyle.Single;
            addSelectedWordAttributes(ct.Dictionary);
        }

        partial void ununderline()
        {
            var ct = new CTStringAttributes();
            ct.UnderlineStyle = CTUnderlineStyle.None;
            addSelectedWordAttributes(ct.Dictionary);
        }

        partial void outline()
        {
            var ct = new UIStringAttributes();
            ct.StrokeWidth = -5;
            ct.StrokeColor = UIColor.Black;
            addSelectedWordAttributes(ct.Dictionary);
        }
        
        partial void unoutline()
        {
            var ct = new UIStringAttributes();
            ct.StrokeWidth = 0;

            addSelectedWordAttributes(ct.Dictionary);
        }

        partial void changeColor(UIButton button)
        {
            var ct = new CTStringAttributes();
            ct.ForegroundColor = button.BackgroundColor.CGColor;


            var sa = new UIStringAttributes();
            sa.ForegroundColor = button.BackgroundColor;
            addSelectedWordAttributes(sa.Dictionary);
        }

        partial void changeFont(UIButton button)
        {
            float fontSize = UIFont.SystemFontSize;
            NSRange er;
            var attr = _label.AttributedText.GetCoreTextAttributes(0, out er);
            var existingFont = attr.Font;
            if (existingFont != null)
                fontSize = existingFont.Size;

            var sa = new UIStringAttributes();
            sa.Font = button.TitleLabel.Font.WithSize(fontSize);
            addSelectedWordAttributes(sa.Dictionary);
        }
    
    }
}

