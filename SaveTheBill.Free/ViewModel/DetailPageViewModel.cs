﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;

using Realms;

using SaveTheBill.Free.Model;

namespace SaveTheBill.Free.ViewModel
{
    public class DetailPageViewModel
    {
        private readonly Realm _realm;

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
                var last = _realm.All<Bill>().OrderByDescending(entity => entity.Id).FirstOrDefault();

                var id = last == null ? 1 : last.Id + 1;

                bill.Id = id;

                _realm.Write(() => { _realm.Add(bill); });
            }
            else
            {
                _realm.Write(() => { _realm.Add(bill, true); });
            }
        }

        public bool MatchAmmoundRegex(string input)
        {
            var pattern = "(-?\\d{1,3}(,?\\d{3})*(\\.\\d{2}?))(\\D|$)";

            var reg = new Regex(pattern);

            return reg.Match(input).Success;
        }

        public async Task<MediaFile> HandleChoosenSource(string input)
        {
            MediaFile file;
			if (input.Equals("Gallerie"))
			{
				file = await CrossMedia.Current.PickPhotoAsync();

				if (file == null)
					return null;
			}
			else if (input.Equals("Kamera"))
			{
				file =
					await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
					{
						Directory = "LocalData",
						Name = "bill_" + DateTime.Now + ".jpg"
					});

				if (file == null)
					return null;
			}
			else 
			{
				file = null;
			}

            return file;
        }
    }
}