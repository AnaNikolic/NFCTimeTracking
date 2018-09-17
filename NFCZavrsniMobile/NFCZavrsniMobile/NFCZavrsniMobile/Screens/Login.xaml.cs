using NFCZavrsniMobile.Data;
using NFCZavrsniMobile.Helpers;
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
	public partial class Login : ContentPage
	{
        public Login ()
		{
			InitializeComponent ();
		}

        public void SendCode(object Sender, EventArgs e)
        {
            string phoneNumber = phoneNumberInput.Text;//"+385998308829";
            string phoneID = Settings.IMEI;//"355457287365196";
            var r = new CrudApi();
            Uri restUri = new Uri(Constants.RestURLInitiateLogin);
            InitiateLogInBody body = new InitiateLogInBody(phoneID, phoneNumber);

            string code = Task.Run(async () => { return await r.PostAsync<InitiateLogInBody, string>(restUri, body, true); }).Result;
            Settings.VerifyToken = code;
            Settings.VerifyPhoneNumber = phoneNumber;
            if (code != null)
            {
                Navigation.PushAsync(new NavigationPage( new Verify(code, phoneID, phoneNumber)));
            }
            else
            {
                DisplayAlert("Not allowed", "Device and number do not match. Try retypeing the number.", "OK");
            }
        }


	}
}