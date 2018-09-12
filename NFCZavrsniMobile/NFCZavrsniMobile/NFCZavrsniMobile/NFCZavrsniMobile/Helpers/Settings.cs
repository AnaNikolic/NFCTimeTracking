using NFCZavrsniMobile.Data;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFCZavrsniMobile.Helpers
{
  public static class Settings
        {
        
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static string BearerToken
        {
            get
            {
                return AppSettings.GetValueOrDefault("BearerToken", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("BearerToken", value);
            }
        }

        public static string IMEI
        {
            get
            {
                return AppSettings.GetValueOrDefault("IMEI", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("IMEI", value);
            }
        }
    }
}
