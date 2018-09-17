using NFCZavrsniMobile.Data;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NFCZavrsniMobile.Helpers;
using Newtonsoft.Json;

namespace NFCZavrsniMobile.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Verify : ContentPage
	{
        string Token;
        string PhoneID;
        string PhoneNumber;

		public Verify (string token, string phoneID, string phoneNumber)
		{
            Token = token;
            PhoneID = phoneID;
            PhoneNumber = phoneNumber;
			InitializeComponent();
		}

        public void LoginUser(object Sender, EventArgs e)
        {
            string code = verificationCodeInput.Text;
            code = Settings.VerifyToken;
            if (code == Token)
            {
                var r = new RestService();
                LogInBody tijelo = new LogInBody(PhoneID, PhoneNumber, code);
                Uri restUri = new Uri(Constants.RestURLBearerToken);
                App.btoken = Task.Run(async () => { return await r.AuthentificateAsync(restUri, tijelo); }).Result;
                Settings.BearerToken = JsonConvert.SerializeObject(App.btoken);
                CrudApi api = new CrudApi(App.btoken);
                if (App.btoken.Access_token != "")
                {
                    Settings.VerifyToken = "";
                    Navigation.PushModalAsync(new NavigationPage(new AddAttendance()));
                }
            }
            else
            {
                DisplayAlert("Verification failed", "You entered invalid code.", "OK");
            }
        }

    }
}