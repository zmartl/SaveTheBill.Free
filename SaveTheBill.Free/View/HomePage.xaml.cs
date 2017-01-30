using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SaveTheBill.Free
{
	public partial class HomePage : ContentPage
	{
		public HomePage()
		{
			InitializeComponent();
			BindingContext = new HomePageViewModel();
			EntriesListView.Footer = string.Empty;
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var listView = (ListView)sender;
			var item = ((ListView)sender).SelectedItem;

			if (item == null) return;

			Navigation.PushAsync(new BillDetailPage((Bill)item));
			listView.SelectedItem = null;
		}

		private void Add_OnClicked(object sender, EventArgs e)
		{
			var result = (ToolbarItem)sender;
			if (result != null)
				Navigation.PushAsync(new BillDetailPage());
		}
	}
}
