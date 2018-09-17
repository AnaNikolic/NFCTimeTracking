
using System;
using Xamarin.Forms;
using NFCZavrsniMobile.Services;
using Android.Media;
using NFCZavrsniMobile.Droid.Services;

[assembly: Dependency(typeof(AudioService))]
namespace NFCZavrsniMobile.Droid.Services
{
    public class AudioService : IAudio
    {
        public AudioService() { }

        private MediaPlayer _mediaPlayer;

        public bool PlayMp3File(string fileName)
        {
            _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.door);
            _mediaPlayer.Start();

            return true;
        }
        
        public bool PlayWavFile(string fileName)
        {
           _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.door);
           _mediaPlayer.Start();

           return true;
        }
    }
}
