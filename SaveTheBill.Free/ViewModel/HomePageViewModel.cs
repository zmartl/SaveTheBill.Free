using System.Collections.Generic;
using System.Windows.Input;
using Realms;
using Xamarin.Forms;

namespace SaveTheBill.Free
{
	public class HomePageViewModel
	{
		private const string AuthorName = "Me";

		private Realm _realm;

		public IEnumerable<Bill> Entries { get; set; }

		public ICommand AddEntryCommand { get; private set; }

		public ICommand DeleteEntryCommand { get; private set; }

		public INavigation Navigation { get; set; }

		public HomePageViewModel()
		{
			_realm = Realm.GetInstance();

			Entries = _realm.All<Bill>().AsRealmCollection();
		}


		private void DeleteEntry(Bill entry)
		{
			_realm.Write(() => _realm.Remove(entry));
		}
	}
}
