using NFCZavrsniMobile.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
           // string number = phoneNumber.ToString();
            // var lozinkaForma = lozinka.Text;
            //number = "+385998308829";
            //string PhoneID = "355457287365196";
            var r = new RestService();
            LogInBody tijelo = new LogInBody(PhoneID, PhoneNumber, Token);
            Uri restUri = new Uri(Constants.RestURLBearerToken);
            App.btoken = Task.Run(async () => { return await r.AuthentificateAsync(restUri, tijelo); }).Result;
            //Task<BearerToken> token = r.AuthentificateAsync(restUri, tijelo);
            CrudApi api = new CrudApi(App.btoken);
            if (App.btoken.Access_token != "")
            {
                Navigation.PushModalAsync(new AddAttendance());
            }
            else
            {
                DisplayAlert("Verification failed", "You entered invalid code.", "OK");

            }

        }
    }
}