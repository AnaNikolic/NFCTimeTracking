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

namespace NFCZavrsniMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAttendance : ContentPage
    {

            private readonly INfcForms device;
            private StackLayout welcomePanel;
            private Label welcomeLabel;
      

            public AddAttendance() { 
            device = DependencyService.Get<INfcForms>();
            device.NewTag += HandleNewTag;/*
            device.TagConnected += device_TagConnected;
            device.TagDisconnected += device_TagDisconnected;*/

            welcomeLabel = new Label
            {
                Text = "Add attendance!" + System.Environment.NewLine + "Scan a tag!",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            welcomePanel = new StackLayout()
            {
                Children = { welcomeLabel },
                BackgroundColor = Color.White
            };

            Content = welcomePanel;
        }
        

        void HandleNewTag(object sender, NfcFormsTag e)
        {
            var contentRead = Encoding.ASCII.GetString(e.NdefMessage[0].Payload);
            welcomeLabel.Text = contentRead.ToString();
            
            var serialNumber = BitConverter.ToString(e.Id); 
            var r = new CrudApi(App.btoken);
            Uri restUri = new Uri(Constants.RestURLAddAttendance);
            AddAttendanceBody body = new AddAttendanceBody(serialNumber, contentRead);
            AddAttendanceResponseBody responseBody = null;/*
            responseBody = Task.Run(async () =>
                { return await r.PostAsync<AddAttendanceBody, AddAttendanceResponseBody>(restUri, body); }).Result;
            var spRecord = new NdefTextRecord
            {
                //Payload = Encoding.ASCII.GetBytes(response.Result.NfcContentUploaded)
                Payload = Encoding.ASCII.GetBytes(responseBody.NfcContentUploaded)
                //Payload = Encoding.ASCII.GetBytes("text")
            };
            var msg = new NdefMessage { spRecord };

            device.WriteTag(msg);
            */
        try
        {
            responseBody = Task.Run(async () =>
            { return await r.PostAsync<AddAttendanceBody, AddAttendanceResponseBody>(restUri, body); }).Result;
           //var response = r.PostAsync<AddAttendanceBody, AddAttendanceResponseBody>(restUri, body);
            var spRecord = new NdefTextRecord
        {
                //Payload = Encoding.ASCII.GetBytes(response.Result.NfcContentUploaded)
            Payload = Encoding.ASCII.GetBytes(responseBody.NfcContentUploaded)
                //Payload = Encoding.ASCII.GetBytes("text")
            };
        var msg = new NdefMessage { spRecord };

            device.WriteTag(msg);

        }
        catch (Exception excp)
        {
            DisplayAlert("Error", excp.Message, "OK");
            Navigation.PushModalAsync(new Fail());
        }
            restUri = new Uri(Constants.RestURLVerifyAttendance);
            bool proslo = Task.Run(async () => { return await r.PostAsync<AddAttendanceResponseBody>(restUri, responseBody); }).Result;
            Navigation.PushModalAsync(new Successful());
        }
    }
}