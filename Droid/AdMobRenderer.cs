using Android.Widget;
using Android.Gms.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MeetupManager.Controls.AdView), typeof(MeetupManager.Droid.PlatformSpecific.AdViewRenderer))]
namespace MeetupManager.Droid.PlatformSpecific
{
	public class AdViewRenderer : ViewRenderer<Controls.AdView, AdView>
	{
		string adUnitId = string.Empty;
		AdSize adSize = AdSize.SmartBanner;
		AdView adView;
		AdView CreateNativeControl()
		{
			if (adView != null)
				return adView;

			adUnitId = Forms.Context.Resources.GetString(Resource.String.banner_ad_unit_id);
			adView = new AdView(Forms.Context);
			adView.AdSize = adSize;
			adView.AdUnitId = adUnitId;

			var adParams = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

			adView.LayoutParameters = adParams;

			adView.LoadAd(new AdRequest
							.Builder()
							.Build());
			return adView;
		}

		protected override void OnElementChanged(ElementChangedEventArgs e)
		{
			base.OnElementChanged(e);
			if (Control == null)
			{
				CreateNativeControl();
				SetNativeControl(adView);
			}
		}
	}
}