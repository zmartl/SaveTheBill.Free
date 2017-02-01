using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using HockeyApp.iOS;
using UIKit;

namespace SaveTheBill.Free.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("7d9ec1ea174646f9b16b68acf1d06e3d");
            manager.StartManager();

            LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
