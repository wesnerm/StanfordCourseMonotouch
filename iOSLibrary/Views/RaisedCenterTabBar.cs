//
//  RaisedCenterTabBarAppDelegate.m
//  RaisedCenterTabBar
//
//  Created by Peter Boctor on 12/15/10.
//
// Copyright (c) 2011 Peter Boctor
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE
//
// https://github.com/boctor/idev-recipes/tree/master/RaisedCenterTabBar/Classes

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;

namespace WM.Views
{
	public class RaisedCenterTabBar : UITabBarController
	{
		public static void WillShowViewController(UINavigationController navController, UIViewController viewController)
		{
			var selector = new Selector("willAppearIn:");
			if (viewController.RespondsToSelector(selector))
				viewController.PerformSelector(selector, navController, 0);
		}

		public void AddCenterButtonWithImage(UIImage buttonImage, UIImage highlightImage)
		{
			var button = new UIButton(UIButtonType.Custom)
			{
				AutoresizingMask =
					UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleLeftMargin |
					UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin,
				Frame = new RectangleF(new PointF(), buttonImage.Size)
			};
			button.SetBackgroundImage(buttonImage, UIControlState.Normal);
			button.SetBackgroundImage(highlightImage, UIControlState.Highlighted);

			var heightDifference = buttonImage.Size.Height - TabBar.Frame.Size.Height;
			var center = TabBar.Center;
			if (heightDifference > 0)
				center.Y -= heightDifference/2.0f;
			button.Center = center;
			View.AddSubview(button);
		}
	}
}
