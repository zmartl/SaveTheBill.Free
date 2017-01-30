using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace SaveTheBill.Free
{
	public partial class BillDetailPage : ContentPage
	{
		private DetailPageViewModel viewModel;
		private Bill _localBill;

		public BillDetailPage(Bill bill = null)
		{
			InitializeComponent();
			viewModel = new DetailPageViewModel();
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
			viewModel.Save_OnClicked(bill, _localBill != null);

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
				if (!viewModel.MatchAmmoundRegex(AmountEntry.Text))
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

        private void AddPhoto_OnClicked(object sender, EventArgs e)
        {
              
        }


	}
}
