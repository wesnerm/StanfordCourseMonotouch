
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace KitchenSink
{
    public partial class AskerViewController : UIViewController
    {
	    string _question;
	    string _answer;
	    UILabel _questionLabel;


	    public AskerViewController() : base ("AskerViewController", null)
        {
        }

		public string Question
		{
			get { return _question; }
			set { _question = value;
				if (QuestionLabel != null)
					QuestionLabel.Text = _question;
			}
		}

	    public string Answer
	    {
		    get { return _answer; }
		    set { _answer = value;
			if (AnswerTextField != null)
				AnswerTextField.Text = _answer;
			}
	    }


	    [Outlet]
		UILabel QuestionLabel
	    {
		    get { return _questionLabel; }
		    set { _questionLabel = value; }
	    }

	    [Outlet]
		UITextField AnswerTextField { get; set; }


		class Del : UITextFieldDelegate
		{
			public AskerViewController self;
			public override void EditingEnded(UITextField textField)
			{
				base.EditingEnded(textField);
				    self.Answer = textField.Text;
				if (0==textField.Text.Length) {
					self.PresentingViewController.DismissModalViewControllerAnimated(true);
				} else {
					//self._delegate(self, self.Question, self.Answer);
				}
			}

			public override bool ShouldReturn(UITextField textField)
			{
				if (textField.Text.Length > 0)
				{
					textField.ResignFirstResponder();
					return true;
				}

				return false;
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			QuestionLabel.Text = Question;
			if (AnswerTextField != null)
			{
				AnswerTextField.Placeholder = Answer;
				AnswerTextField.Delegate = new Del() {self = this};
			}
		}

	    public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			QuestionLabel.Text = Question;
			AnswerTextField.Text = "";
			AnswerTextField.BecomeFirstResponder();
		}

		public override void ViewDidUnload()
		{
			Question = null;
			AnswerTextField = null;
			base.ViewDidUnload();
		}

		public override bool ShouldAutorotate()
		{
			return true;
		}
    }
}

