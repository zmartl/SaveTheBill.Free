using System;
using System.ComponentModel;

using SaveTheBill.Free.Model;
using SaveTheBill.Free.ViewModel;

using Xamarin.Forms;

namespace SaveTheBill.Free.View
{
    public partial class HomePage : ContentPage, INotifyPropertyChanged
    {
        private readonly HomePageViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HomePageViewModel();
            EntriesListView.Footer = string.Empty;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listView = (ListView) sender;
            var item = ((ListView) sender).SelectedItem;

            if (item == null) return;

            Navigation.PushAsync(new BillDetailPage((Bill) item));
            listView.SelectedItem = null;
        }

        private void Add_OnClicked(object sender, EventArgs e)
        {
            if (_viewModel.PremiumVersionRequired())
            {
                DisplayAlert("Upgrade erforderlich",
                    "Sie haben die Anzahl maximaler Belege in der kostenlosen Version erreicht. Bitte kaufen Sie die Vollversion.",
                    "Ok");
                return;
            }

            var result = (ToolbarItem) sender;
            if (result != null)
                Navigation.PushAsync(new BillDetailPage());
        }

        private void Delete_OnClicked(object sender, EventArgs e)
        {
            var mi = (MenuItem) sender;

            if (mi == null) return;

            var item = (Bill) mi.CommandParameter;

            _viewModel.DeleteEntry(item);
        }

        private void Share_OnClicked(object sender, EventArgs e)
        {
            var mi = (MenuItem) sender;

            if (mi == null) return;

            var item = (Bill) mi.CommandParameter;

            try
            {
                _viewModel.SendEmail(item);
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Email unavaiable")) DisplayAlert("Fehler", "Email nicht verfügbar", "Ok");
            }
        }
    }
}