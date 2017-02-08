using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Plugin.Messaging;

using Realms;

using SaveTheBill.Free.Model;
using SaveTheBill.Free.Resources;

namespace SaveTheBill.Free.ViewModel
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private readonly Realm _realm;
        private IDisposable _monitor;

        private IEnumerable<Bill> Entries { get; }

        private bool IsEmpty => !Entries.Any();

        public HomePageViewModel()
        {
            _realm = Realm.GetInstance();

            Entries = _realm.All<Bill>().AsRealmCollection();
            _monitor =
                _realm.All<Bill>()
                    .SubscribeForNotifications((sender, changes, error) => { OnPropertyChanged(nameof(IsEmpty)); });
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


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
                var billText = "<b>Beschreibung: </b>" + bill.Name + "<br/>" + "<b>Betrag: <b/> " + bill.Amount + ".-" +
                               "<br/><b>Garantie läuft ab: </b>" +
                               (bill.HasGuarantee
                                   ? bill.GuaranteeExpireDate.ToString("dd.MM.yyyy")
                                   : "Keine Garantie erfasst") + "<br/><b>Kaufort: <b/> " + bill.Location +
                               "<br/><b>Eingescannt am: <b/> " + bill.ScanDate.ToString("dd.MM.yyyy");

                if (bill.ImageSource == null)
                {
                    var email =
                        new EmailMessageBuilder().Subject("SaveTheBill Quittung Nummer #" + bill.Id)
                            .BodyAsHtml(Email.EmailStart + "<br/><br/>" + Email.EmailText + "<br/><br/>" + billText +
                                        "<br/><br/>" + Email.EmailEnd + "<br/>" + Email.Signature)
                            .Build();
                    emailMessenger.SendEmail(email);
                }
                else
                {
                    var email =
                        new EmailMessageBuilder().Subject("SaveTheBill Quittung Nummer #" + bill.Id)
                            .BodyAsHtml(Email.EmailStart + "<br/><br/>" + Email.EmailText + "<br/><br/>" + billText +
                                        "<br/><br/>" + Email.EmailEnd + "<br/>" + Email.Signature)
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