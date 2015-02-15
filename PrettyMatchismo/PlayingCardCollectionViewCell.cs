
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace WM
{
    public partial class PlayingCardCollectionViewCell : UICollectionViewCell
    {
        public static readonly UINib Nib = UINib.FromName("PlayingCardCollectionViewCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("PlayingCardCollectionViewCell");
		
        public PlayingCardCollectionViewCell(IntPtr handle) : base (handle)
        {
        }
		
        public static PlayingCardCollectionViewCell Create()
        {
            return (PlayingCardCollectionViewCell)Nib.Instantiate(null, null)[0];
        }

		[Outlet]
		public PlayingCardView PlayingCardView { get; set; }
    }
}

