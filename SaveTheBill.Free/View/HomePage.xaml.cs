using System;
using SaveTheBill.Free.Model;
using SaveTheBill.Free.ViewModel;
using Xamarin.Forms;

namespace SaveTheBill.Free.View
{
	public partial class HomePage : ContentPage
	{
	    private readonly HomePageViewModel _viewModel;
		public HomePage()
		{
			InitializeComponent();
			BindingContext = _viewModel = new HomePageViewModel();
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

        private void Delete_OnClicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;

            if (mi == null) return;

            var item = (Bill)mi.CommandParameter;

            _viewModel.DeleteEntry(item);
        }

        private void Share_OnClicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;

            if (mi == null) return;

            var item = (Bill)mi.CommandParameter;

            _viewModel.SendEmail(item);
        }
    }
}
