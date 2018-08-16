using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Poz1.NFCForms.Droid;
using Android.Nfc;
using Poz1.NFCForms.Abstract;
using System.Runtime.Remoting.Contexts;
using Android.Content;
using System.Text;
using Android.Nfc.Tech;
using Android.Util;
using Android.Telephony;
using Xamarin.Forms;
using Android.Support.V4.Content;
using Android;

namespace NFCZavrsniMobile.Droid
{


    [Activity(Label = "NFCZavrsniMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { NfcAdapter.ActionTechDiscovered })]
    [MetaData(NfcAdapter.ActionTechDiscovered, Resource = "@xml/nfc_tech_filter")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public NfcAdapter NFCdevice;
        public NfcForms x;
        private NfcAdapter nfcAdapter;
        private TelephonyManager telephonyManager;
        private string IMEINumber;

        protected override void OnCreate(Bundle bundle)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            NfcManager NfcManager = (NfcManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.NfcService);
            NFCdevice = NfcManager.DefaultAdapter;

            Xamarin.Forms.DependencyService.Register<INfcForms, NfcForms>();
            x = Xamarin.Forms.DependencyService.Get<INfcForms>() as NfcForms;

            nfcAdapter = NfcAdapter.GetDefaultAdapter(this);
            if (nfcAdapter == null)
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
                if (nfcAdapter.IsEnabled)
                {
                    /*
                    var rawMessages = Intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
                    var msg = (NdefMessage)rawMessages[0];
                    var theRecord = msg.GetRecords()[0];
                    var tagName = Encoding.ASCII.GetString(theRecord.GetPayload());

                    var alert = new AlertDialog.Builder(this).Create();
                    alert.SetMessage(tagName);
                    alert.SetTitle("NFC is working on this device");
                    alert.Show();
                    /*
                    var alert = new AlertDialog.Builder(this).Create();
                    alert.SetMessage("NFC is available.");
                    alert.SetTitle("NFC is working on this device");
                    alert.Show();
                    */
                }
                else
                {
                    var alert = new AlertDialog.Builder(this).Create();
                    alert.SetMessage("NFC is currently off.");
                    alert.SetTitle("Please enable NFC to use this app.");
                    alert.Show();
                }
            }
            
            telephonyManager = (TelephonyManager)GetSystemService(TelephonyService);
            IMEINumber = telephonyManager.Imei;
            LoadApplication(new App(IMEINumber));
        }
        

        private void CancelButtonOnClick(object sender, EventArgs eventArgs)
        {
            this.Finish();
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (nfcAdapter != null)
            {
                
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
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (nfcAdapter != null)
            {
                NFCdevice.DisableForegroundDispatch(this);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            //da ne otvara dvaput
            //intent.AddFlags(ActivityFlags.ClearTop);
            base.OnNewIntent(intent);
            x.OnNewIntent(this, intent);
        }


    }

}