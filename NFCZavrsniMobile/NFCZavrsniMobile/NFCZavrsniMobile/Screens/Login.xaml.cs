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
	public partial class Login : ContentPage
	{
        private string IMEI;
        public Login (string imei)
		{
            IMEI = imei;
			InitializeComponent ();
		}

        public void SendCode(object Sender, EventArgs e)
        {
            string number  = phoneNumber.ToString();
            string phoneNumberInput = "+385998308829";
            string phoneID = IMEI;//"355457287365196";
            var r = new CrudApi();
            Uri restUri = new Uri(Constants.RestURLInitiateLogin);
            InitiateLogInBody body = new InitiateLogInBody(phoneID, phoneNumberInput);

            string code = Task.Run(async () => { return await r.PostAsync<InitiateLogInBody, string>(restUri, body, true); }).Result;
            //Task<string> code = r.PostAsync<InitiateLogInBody, string>(restUri, body, true);
            //CrudApi api = new CrudApi(token);
            //Uri uri = new Uri("https://webservis20180613101006.azurewebsites.net/api/Kolegijs");
            //Kolegij k = new Kolegij(0,"Programsko inženjerstvo","PI");
            //bool res = await api.PostAsync<Kolegij>(uri,k);   
            if (code != null)
            {
                Navigation.PushModalAsync(new Verify(code, phoneID, phoneNumberInput));
            }
            else
            {
                DisplayAlert("Not allowed", "Device and number do not match. Try retypeing the number.", "OK");
            }
        }
	}
}