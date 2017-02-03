using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Messaging;
using Realms;
using SaveTheBill.Free.Model;
using SaveTheBill.Free.Resources;

namespace SaveTheBill.Free.ViewModel
{
    public class HomePageViewModel
    {
        private readonly Realm _realm;

        public HomePageViewModel()
        {
            _realm = Realm.GetInstance();

            Entries = _realm.All<Bill>().AsRealmCollection();
        }

        public IEnumerable<Bill> Entries { get; set; }


        public void DeleteEntry(Bill entry)
        {
            _realm.Write(() => _realm.Remove(entry));
        }

        public bool PremiumVersionRequired()
        {
            var count = Entries.Count();
            var upgradeRequired = count >= 20;

            return upgradeRequired;
        }

        public void SendEmail(Bill bill)
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;
			if (emailMessenger.CanSendEmail)
			{
				var billText = "<b>Beschreibung: </b>" + bill.Name + "<br/>" +
							   "<b>Betrag: <b/> " + bill.Amount + ".-" +
							   "<br/><b>Garantie läuft ab: </b>" +
							   (bill.HasGuarantee
								   ? bill.GuaranteeExpireDate.ToString("dd.MM.yyyy")
								   : "Keine Garantie erfasst") +
							   "<br/><b>Kaufort: <b/> " + bill.Location +
							   "<br/><b>Eingescannt am: <b/> " + bill.ScanDate.ToString("dd.MM.yyyy");

				if (bill.ImageSource == null)
				{
					var email = new EmailMessageBuilder()
						.Subject("SaveTheBill Quittung Nummer #" + bill.Id)
						.BodyAsHtml(Email.EmailStart + "<br/><br/>" + Email.EmailText + "<br/><br/>" +
									billText + "<br/><br/>" +
									Email.EmailEnd + "<br/>" + Email.Signature)
						.Build();
					emailMessenger.SendEmail(email);
				}
				else
				{
					var email = new EmailMessageBuilder()
						.Subject("SaveTheBill Quittung Nummer #" + bill.Id)
						.BodyAsHtml(Email.EmailStart + "<br/><br/>" + Email.EmailText + "<br/><br/>" +
									billText + "<br/><br/>" +
									Email.EmailEnd + "<br/>" + Email.Signature)
						.WithAttachment(bill.ImageSource, "image/jpeg")
						.Build();
					emailMessenger.SendEmail(email);
				}
			}
			else
			{
				throw new Exception("Email unavaiable");
			}

        }
    }
}