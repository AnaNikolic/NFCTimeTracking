using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.OS;
using Android.Telephony;
using NFCZavrsniMobile.Helpers;
using NFCZavrsniMobile.Services;
using System;
using Microsoft.WindowsAzure.MobileServices;

namespace NFCZavrsniMobile.Droid
{
    [Activity(Label = "NFCZavrsniMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, NoHistory = true)]
    [IntentFilter(new[] { NfcAdapter.ActionTechDiscovered })]
    [MetaData(NfcAdapter.ActionTechDiscovered, Resource = "@xml/nfc_tech_filter")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public NfcAdapter NFCdevice;
        public NfcForms x;
        private TelephonyManager telephonyManager;
        private string IMEINumber;

        public static MobileServiceClient MobileService = new MobileServiceClient("https://nfctimetrackingmobile.azurewebsites.net");

        public class TodoItem
        {
            public string Id { get; set; }
            public string Text { get; set; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            NfcManager NfcManager = (NfcManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.NfcService);
            NFCdevice = NfcManager.DefaultAdapter;
            Xamarin.Forms.DependencyService.Register<INfc, NfcForms>();
            x = Xamarin.Forms.DependencyService.Get<INfc>() as NfcForms;

            if (NFCdevice == null)
            {
                var alert = new AlertDialog.Builder(this).Create();
                alert.SetMessage("NFC is not supported on this device.");
                alert.SetTitle("NFC Unavailable");
                alert.SetButton("exit", CancelButtonOnClick);
                alert.SetCanceledOnTouchOutside(false);
                alert.Show();
            }
            else 
            {
                if (!NFCdevice.IsEnabled)
                {
                    var alert = new AlertDialog.Builder(this).Create();
                    alert.SetMessage("NFC is currently off.");
                    alert.SetTitle("Please enable NFC to use this app.");
                    alert.Show();
                }
            }
            
            telephonyManager = (TelephonyManager)GetSystemService(TelephonyService);
            IMEINumber = telephonyManager.Imei;
            Settings.IMEI = IMEINumber;
            LoadApplication(new App());


        }
        

        private void CancelButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.Finish();
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (NFCdevice != null)
            {
                var intent = new Intent(this, GetType()).AddFlags(ActivityFlags.SingleTop);
                NFCdevice.EnableForegroundDispatch
                (
                    this,
                    PendingIntent.GetActivity(this, 0, intent, 0),
                    new[] { new IntentFilter(NfcAdapter.ActionTechDiscovered) },
                    new String[][] {new string[] {
                        NFCTechs.Ndef,
                    },
                    new string[] {
                        NFCTechs.MifareClassic,
                    },
                    }
                );
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            NFCdevice.DisableForegroundDispatch(this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            x.OnNewIntent(this, intent);
        }

        public override void OnBackPressed()
        {
            if (Settings.BearerToken == "" || Settings.VerifyToken == "")
            {
                return;
                
            }
            base.OnBackPressed();
            return;
        }
    }
}