using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using HockeyApp.Android;
using Plugin.Permissions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace SaveTheBill.Free.Droid
{
    [Activity(Label = "SaveTheBill.Free.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

			MobileAds.Initialize(ApplicationContext, "ca-app-pub-2913878865891583~2827092756");

            base.OnCreate(bundle);

            Forms.Init(this, bundle);

            CrashManager.Register(this);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}