using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using NdefLibrary.Ndef;
using Poz1.NFCForms.Abstract;
using System.Collections.ObjectModel;
using Android.App;
using Android.Content;
using NFCZavrsniMobile.Data;
using NFCZavrsniMobile.Models;
using NFCZavrsniMobile.Screens;
using NFCZavrsniMobile.Helpers;

namespace NFCZavrsniMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAttendance : ContentPage
    {
        private readonly INfcForms device;
        private StackLayout screenPanel;
        private Label screenLabel;

        public AddAttendance() {
            InitializeComponent();
            device = DependencyService.Get<INfcForms>();
            device.NewTag += HandleNewTag;
            screenLabel = new Label
            {
                Text = "Scan a tag" + System.Environment.NewLine + "to add new attendance!",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Blue,
                FontSize = 24,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            screenPanel = new StackLayout()
            {
                Children = { screenLabel },
                BackgroundColor = Color.White,
                IsVisible = true,
                Opacity = 0.7
            };
            Content = screenPanel;
        }

        void HandleNewTag(object sender, NfcFormsTag e)
        {
            var contentRead = Encoding.ASCII.GetString(e.NdefMessage[0].Payload);
            //welcomeLabel.Text = contentRead.ToString();
            var serialNumber = BitConverter.ToString(e.Id); 
            var r = new CrudApi(App.btoken);
            Uri restUri = new Uri(Constants.RestURLAddAttendance);
            AddAttendanceBody body = new AddAttendanceBody(serialNumber, contentRead);
            AddAttendanceResponseBody responseBody = null; 
            try
            {
                responseBody = Task.Run(async () =>
                { return await r.PostAsync<AddAttendanceBody, AddAttendanceResponseBody>(restUri, body); }).Result;
                    var spRecord = new NdefTextRecord
                    {
                        Payload = Encoding.ASCII.GetBytes(responseBody.NfcContentUploaded)
                    };
                    var msg = new NdefMessage { spRecord };
                    device.WriteTag(msg);
            }
            catch (Exception excp)
            {
                DisplayAlert("Error", "Failed!", "OK");

                    return;
            }
            restUri = new Uri(Constants.RestURLVerifyAttendance);
            bool proslo = Task.Run(async () => { return await r.PostAsync<AddAttendanceResponseBody>(restUri, responseBody); }).Result;
            DisplayAlert("Success!", "New attendance successfuly added!", "DONE");
            return;
        }

        public Command LogoutCommand
        {
            get
            {
                return new Command(() => { Settings.BearerToken = ""; });
            }
        }

        public void LogoutClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Login());
        }

    }
}