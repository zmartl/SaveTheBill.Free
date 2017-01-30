using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Realms;

namespace SaveTheBill.Free
{
	public class DetailPageViewModel
	{
		private Realm _realm;
		public Bill Entry { get; set; }
		private IEnumerable<Bill> res { get; set; }

		public DetailPageViewModel()
		{
			_realm = Realm.GetInstance();
		}
		 
		public void Save_OnClicked(Bill bill, bool exist = false)
		{
			if (!exist)
			{
				var rest = _realm.All<Bill>().AsRealmCollection();

				var count = rest.Count;

				bill.Id = count + 1;

				_realm.Write(() =>
				{
					_realm.Add(bill);
				});
			}
			else
			{
				_realm.Write(() =>
				{
					_realm.Add(bill, update: true);
				});

			}
		}

		public bool MatchAmmoundRegex(string input)
		{
			var pattern = "(-?\\d{1,3}(,?\\d{3})*(\\.\\d{2}?))(\\D|$)";

			var reg = new Regex(pattern);

			return reg.Match(input).Success;
		}
	}
}