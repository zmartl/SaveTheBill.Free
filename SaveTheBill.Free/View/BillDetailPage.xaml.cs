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
        private readonly Bill _localBill;
        private readonly DetailPageViewModel _viewModel;
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
            GuaranteeSwitch.IsToggled = _localBill.HasGuarantee;
            GuaranteeDatePicker.Date = _localBill.GuaranteeExpireDate.DateTime;
            BuyDateEntry.Date = _localBill.ScanDate.DateTime;
            DetailEntry.Text = _localBill.Additions;
			var imagePath = _localBill.ImageSource;
			if (imagePath != null)
			{
				ImageEntry.Source = ImageSource.FromFile(imagePath);
			}
        }

        private void GuaranteeSwitch_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (GuaranteeDatePicker != null)
            {
                var guaranteeSwitch = (Switch) sender;

                GuaranteeDatePicker.IsEnabled = guaranteeSwitch.IsToggled;
            }
        }

        private async void Save_OnClicked(object sender, EventArgs e)
        {
			try
			{
				if (!IsValid()) return;

				var bill = new Bill
				{
					Name = TitleEntry.Text,
					Amount = AmountEntry.Text,
					HasGuarantee = GuaranteeSwitch.IsToggled,
					GuaranteeExpireDate = GuaranteeDatePicker.Date.AddDays(1),
					Location = LocationEntry.Text,
					ScanDate = BuyDateEntry.Date.AddDays(1),
					Additions = DetailEntry.Text
				};

			    if (_file != null)
			    {
			        bill.ImageSource = _file.Path;
			    }
			    else if (!string.IsNullOrWhiteSpace(_localBill.ImageSource))
			    {
			        bill.ImageSource = _localBill.ImageSource;
			    }
			    else
			    {
			        bill.ImageSource = string.Empty;
			    }

                if (_localBill != null)
					bill.Id = _localBill.Id;
				
				_viewModel.Save_OnClicked(bill, _localBill != null);

				await Navigation.PopAsync(false);
			}
			catch (Exception ex)
			{
				await DisplayAlert(ex.Message, ex.ToString(), "Ok");
			}
            try
            {
                if (!IsValid()) return;

                var bill = new Bill {
                    Name = TitleEntry.Text,
                    Amount = AmountEntry.Text,
                    HasGuarantee = GuaranteeSwitch.IsToggled,
                    GuaranteeExpireDate = GuaranteeDatePicker.Date.AddDays(1),
                    Location = LocationEntry.Text,
                    ScanDate = BuyDateEntry.Date.AddDays(1),
                    Additions = DetailEntry.Text
                };

                if (_file != null)
                    bill.ImageSource = _file.Path;

                if (_localBill != null)
                    bill.Id = _localBill.Id;

                _viewModel.Save_OnClicked(bill, _localBill != null);

                await Navigation.PopAsync(false);
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, ex.ToString(), "Ok");
            }
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
			try
			{
				_file = await _viewModel.HandleChoosenSource(res);

				if (_file == null) throw new ArgumentNullException();

				HandleImageStream(_file);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Fehler", "Beim auswählen der Datei ist ein Fehler aufgetreten", "Ok");
			}
        }


        private void HandleImageStream(MediaFile file)
        {
            ImageEntry.Source = ImageSource.FromStream(() => {
                var stream = file.GetStream();
                return stream;
            });
        }


        private async Task<string> ChooseSource()
        {
            var result = await DisplayActionSheet("Wählen Sie eine Quelle", "Abbrechen", null, "Kamera", "Gallerie");
            return result;
        }
    }
}
