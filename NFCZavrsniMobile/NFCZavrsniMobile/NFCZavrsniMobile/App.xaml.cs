using NFCZavrsniMobile.Data;
using NFCZavrsniMobile.Screens;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace NFCZavrsniMobile
{
	public partial class App : Application
	{
        public static BearerToken btoken = null;
		public App (string imei)
		{
			InitializeComponent();
            
            MainPage = new NavigationPage(new Login(imei));
             //MainPage = new AddAttendance();
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
