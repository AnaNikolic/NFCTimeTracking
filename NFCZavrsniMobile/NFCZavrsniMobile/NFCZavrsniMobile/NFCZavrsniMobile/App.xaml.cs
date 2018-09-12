using NFCZavrsniMobile.Data;
using NFCZavrsniMobile.Screens;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NFCZavrsniMobile.Helpers;
using Newtonsoft.Json;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace NFCZavrsniMobile
{
	public partial class App : Application
	{   
        public static BearerToken btoken = null;
		public App ()
		{
            ToolbarItem t = new ToolbarItem { Text = "novii" };
            InitializeComponent();
            if (string.IsNullOrEmpty(Settings.BearerToken))
            {
                btoken = JsonConvert.DeserializeObject<BearerToken>(Settings.BearerToken);
                if (btoken.Expires > DateTime.Now) {
                    MainPage = new NavigationPage(new AddAttendance());
                }
                else
                {
                    MainPage = new NavigationPage(new Login());
                }
            }
            else
            {
                MainPage = new NavigationPage(new Login());
            }
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
