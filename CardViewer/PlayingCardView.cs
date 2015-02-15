using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;
using System.Collections.Generic;

namespace WM
{
    [Register("PlayingCardView")]
    public class PlayingCardView : UIView
    {
        int _rank;
        string _suit;
        bool _faceUp;
        public float _faceCardScaleFactor = .90f;

        private static List<PlayingCardView> maintain = new List<PlayingCardView>();

        public PlayingCardView(IntPtr p) : base(p)
        {
        }

       
        public PlayingCardView(RectangleF frame) :base( frame )
        {
            maintain.Add(this);
        }

        public override void Draw(RectangleF rect)
        {
            base.Draw(rect);
            var rrect = UIBezierPath.FromRoundedRect(Bounds, 12f);
            rrect.AddClip();

            UIColor.White.SetFill();
            UIGraphics.RectFill(Bounds);

            UIColor.Black.SetStroke();
            rrect.Stroke();

			if (faceUp)
            {
	            var faceImage = UIImage.FromBundle("Support/"+rankAsString()+suit+".jpg");
	            if (faceImage != null)
	            {
                    var r = Bounds;
                    r = r.Inset(Bounds.Width * (1- faceCardScaleFactor),
                               Bounds.Height * (1-faceCardScaleFactor));
                               
	                faceImage.Draw(r);
	            }
	            else
	            {
	                drawPips();
	            }
                drawCorners();
            }
            else
            {
                UIImage.FromBundle("Support/cardback.png").Draw(Bounds);
            }

        }

        
        const float PIP_HOFFSET_PERCENTAGE = 0.165f;
        const float PIP_VOFFSET1_PERCENTAGE = 0.090f;
        const float PIP_VOFFSET2_PERCENTAGE = 0.175f;
        const float PIP_VOFFSET3_PERCENTAGE = 0.270f;
        const float PIP_FONT_SCALE_FACTOR = .2f;
        
        void drawPips()
        {
            if (rank==1||rank==5||rank==9||rank==3)
                drawPips2(0,0,false);
            if (rank==6||rank==7||rank==8)
                drawPips2(PIP_HOFFSET_PERCENTAGE,0,false);
            if (rank==2||rank==3||rank==7||rank==8||rank==10)
                drawPips2(0,PIP_VOFFSET2_PERCENTAGE,rank!=7);
            if (rank>=4 && rank<=10)
                drawPips2(PIP_HOFFSET_PERCENTAGE, PIP_VOFFSET3_PERCENTAGE,true);
            if (rank==9||rank==10)
                drawPips2(PIP_HOFFSET_PERCENTAGE, PIP_VOFFSET1_PERCENTAGE,true);
            
        }
        
        void drawPips(float hOffset, float vOffset, bool upsideDown)
        {
            if (upsideDown) pushContextAndRotateUpsideDown();
            var middle = new PointF(Bounds.Width/2, Bounds.Height/2);
            var pipFont = UIFont.SystemFontOfSize(Bounds.Width * PIP_FONT_SCALE_FACTOR);
            var attributedSuit = new NSAttributedString(suit, font:pipFont);
            var pipSize = attributedSuit.Size;
            var pipOrigin = new PointF(
                middle.X - pipSize.Width/2 - hOffset*Bounds.Width,
                middle.Y - pipSize.Height/2 - vOffset*Bounds.Height);
            attributedSuit.DrawString(pipOrigin);
            if (hOffset!=0)
            {
                pipOrigin.X += hOffset*2*Bounds.Width;
                attributedSuit.DrawString(pipOrigin);
                
            }
            if (upsideDown) popContext();
        }

        void drawPips2(float hOffset, float vOffset, bool mirrorVertically)
        {
            drawPips(hOffset, vOffset, false);
            if (mirrorVertically)
                drawPips(hOffset, vOffset, true);
        }
        
        
        void pushContextAndRotateUpsideDown()
        {
            var context = UIGraphics.GetCurrentContext();
            context.SaveState();
            context.TranslateCTM(Bounds.Width,Bounds.Height);
            context.RotateCTM((float)Math.PI);
        }

        void popContext()
        {
            var context = UIGraphics.GetCurrentContext();
            context.RestoreState();
        }



        private static string [] rankarray = new [] { "?", "A", "2", 
            "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" }; 

        public string rankAsString()
        {
            return rankarray[rank];
        }


        public void drawCorners()
        {
            var paraStyle = new NSMutableParagraphStyle();
            paraStyle.Alignment = UITextAlignment.Center;
            UIFont cornerFont = UIFont.SystemFontOfSize(Bounds.Width * .20f );
            var cornerText = new NSAttributedString(rankAsString() + "\n"+ suit, 
                                                    font : cornerFont,
                                                    paragraphStyle: paraStyle);
            var textBounds = new RectangleF( new PointF(2,2), cornerText.Size);
            cornerText.DrawString(textBounds);

            pushContextAndRotateUpsideDown();
            cornerText.DrawString(textBounds);
            popContext();
        }

        public int rank
        {
            get { return _rank; }
            set { 
                _rank = value;
                SetNeedsDisplay();
            }
        }

        public string suit
        {
            get { return _suit; }
            set { 
                _suit = value;
                SetNeedsDisplay();
            }
        }

        public bool faceUp
        {
            get { return _faceUp; }
            set { 
                _faceUp = value;
                SetNeedsDisplay();
            }
        }
        
        public float faceCardScaleFactor
        {
            get { return _faceCardScaleFactor; }
            set { 
                _faceCardScaleFactor = value;
                SetNeedsDisplay();
            }
        }
        
        [Export("pinch:")]
        public void pinch(UIPinchGestureRecognizer gesture)
        {
            if (gesture.State == UIGestureRecognizerState.Changed
                || gesture.State == UIGestureRecognizerState.Ended)
            {
                faceCardScaleFactor *= gesture.Scale;
                gesture.Scale = 1f;
            }
        }

//        public override void DrawRect(RectangleF area, UIViewPrintFormatter formatter)
//        {
//            base.DrawRect(area, formatter);
//        }

    }
}

