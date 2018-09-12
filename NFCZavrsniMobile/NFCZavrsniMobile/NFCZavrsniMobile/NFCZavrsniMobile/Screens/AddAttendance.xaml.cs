using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using NdefLibrary.Ndef;
using NFCZavrsniMobile.Data;
using NFCZavrsniMobile.Models;
using NFCZavrsniMobile.Screens;
using NFCZavrsniMobile.Helpers;
using NFCZavrsniMobile.Services;

namespace NFCZavrsniMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAttendance : ContentPage
    {
        private readonly INfc device;

        public AddAttendance() {
            InitializeComponent();
            device = DependencyService.Get<INfc>();
            device.NewTag += HandleNewTag;
        }

        void HandleNewTag(object sender, NfcTag e)
        {
            var contentRead = Encoding.ASCII.GetString(e.NdefMessage[0].Payload);
            string serialNumber = BitConverter.ToString(e.Id); 
            var r = new CrudApi(App.btoken);
            Uri restUri = new Uri(Constants.RestURLAddAttendance);
            string NFCContentUploaded = Guid.NewGuid().ToString();
            AddAttendanceBody body = new AddAttendanceBody(serialNumber, contentRead, NFCContentUploaded);
            AddAttendanceResponseBody responseBody = null; 
            try
            {
                var spRecord = new NdefTextRecord
                {
                    Payload = Encoding.ASCII.GetBytes(NFCContentUploaded)
                };
                var msg = new NdefMessage { spRecord };
                device.WriteTag(msg);
                responseBody = Task.Run(async () =>
                { return await r.PostAsync<AddAttendanceBody, AddAttendanceResponseBody>(restUri, body); }).Result;
                string text = responseBody.EmployeeInfo + " succesfully added attendance no." + responseBody.ID + " on point " + System.Environment.NewLine + responseBody.TagInfo;
                ShowSuccess(text);
                

            }
            catch (Exception excp)
            {
                ShowFail("An error occurred.");
                DependencyService.Get<IAudio>().PlayMp3File("door.mp3");
                return;
            }
            Device.StartTimer(System.TimeSpan.FromSeconds(5), () => { ShowBasic(); return true; });
            return;
        }


        public bool ShowSuccess(string text)
        {
            screenText.Text = text;
            imgAdd.IsVisible = false;
            imgSuccess.IsVisible = true;
            labelMain.Text = "Success!";
            Content = Basic;
            return true;
        }

        public bool ShowFail(string text)
        {
            screenText.Text = text;
            imgAdd.IsVisible = false;
            imgFail.IsVisible = true;
            labelMain.Text = "Failed!";
            Content = Basic;
            return true;
        }

        public void ShowBasic()
        {
            labelMain.Text = "Add attendance";
            screenText.Text = "Approach NFC tag you want to record attendance for.";
            imgAdd.IsVisible = true;
            imgSuccess.IsVisible = false;
            return;
        }

        public void LogoutClicked(object sender, EventArgs e)
        {
            var r = new CrudApi(App.btoken);
            Uri restUri = new Uri(Constants.RestURLLogout);
            Task.Run(async () => { return await r.PostAsync(restUri); });
            Settings.BearerToken = "";
            App.btoken = null;
            Navigation.PushModalAsync(new NavigationPage(new Login()));
            
        }

    }
}