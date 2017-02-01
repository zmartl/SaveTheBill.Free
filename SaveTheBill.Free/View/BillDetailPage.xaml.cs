using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SaveTheBill.Free.Model;
using SaveTheBill.Free.ViewModel;
using Xamarin.Forms;

namespace SaveTheBill.Free.View
{
	public partial class BillDetailPage : ContentPage
	{
		private readonly DetailPageViewModel _viewModel;
		private readonly Bill _localBill;
        private MediaFile _file;

        public BillDetailPage(Bill bill = null)
		{
			InitializeComponent();
			_viewModel = new DetailPageViewModel();
			if (bill != null)
			{
				_localBill = bill;
				FillEntries();
			}
		}

		private void FillEntries() 
		{
			TitleEntry.Text = _localBill.Name;
			AmountEntry.Text = _localBill.Amount;
			LocationEntry.Text = _localBill.Location;
			//TODO Cannot convert datetime to datetime offset
			//GuaranteeDatePicker.Date = _localBill.GuaranteeExpireDate;
			//BuyDateEntry.Date = _localBill.ScanDate;
			DetailEntry.Text = _localBill.Additions;
		}

		private void GuaranteeSwitch_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (GuaranteeDatePicker != null)
			{
				var guaranteeSwitch = (Switch)sender;

				GuaranteeDatePicker.IsEnabled = guaranteeSwitch.IsToggled;
			}
		}

		private async void Save_OnClicked(object sender, EventArgs e)
		{
			if (!IsValid()) return;

			var bill = new Bill
			{
				Name = TitleEntry.Text,
				Amount = AmountEntry.Text,
				HasGuarantee = GuaranteeSwitch.IsToggled,
				GuaranteeExpireDate = GuaranteeDatePicker.Date,
				Location = LocationEntry.Text,
				ScanDate = BuyDateEntry.Date,
				Additions = DetailEntry.Text
			};
			_viewModel.Save_OnClicked(bill, _localBill != null);

			await Navigation.PopAsync(true);

		}

		private bool IsValid()
		{
			var valid = true;

			if (string.IsNullOrEmpty(TitleEntry.Text))
			{
				valid = false;
				TitleValid.IsVisible = true;
			}
			else
			{
				TitleValid.IsVisible = false;
			}

			if (!string.IsNullOrEmpty(AmountEntry.Text))
			{
				AmountVaild.IsVisible = false;
				if (!_viewModel.MatchAmmoundRegex(AmountEntry.Text))
				{
					valid = false;
					AmountVaild.IsVisible = true;
				}
				else
				{
					AmountVaild.IsVisible = false;
				}
			}
			else
			{
				valid = false;
				AmountVaild.IsVisible = true;
			}


			return valid;
		}

		private void Image_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var image = (Image) sender;
            image.IsVisible = image.Source != null;
        }

        private async void AddPhoto_OnClicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Fehler", "Kamera nicht verfügbar.", "OK");
                return;
            }
            var res = await ChooseSource();
            _file = await _viewModel.HandleChoosenSource(res);

            HandleImageStream(_file);
        }


	    private void HandleImageStream(MediaFile file)
	    {
            ImageEntry.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();

                return stream;
            });
        }
	 

	    private async Task<string> ChooseSource()
	    {
            var result =  await DisplayActionSheet("Wählen Sie eine Quelle", "Abbrechen", null, "Kamera", "Gallerie");
	        return result;
	    }


	}
}
