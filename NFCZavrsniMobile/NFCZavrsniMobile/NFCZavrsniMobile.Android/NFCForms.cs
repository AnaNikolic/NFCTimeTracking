﻿using Android.Content;
using Android.Nfc;
using Android.Nfc.Tech;
using NFCZavrsniMobile.Data;
using NFCZavrsniMobile.Services;
using NFCZavrsniMobile.Droid;
using System;
using System.Collections.Generic;
using System.IO;

[assembly: Xamarin.Forms.Dependency (typeof (NfcForms))]
namespace NFCZavrsniMobile.Droid
{
    #region Android NFC Techs
    public sealed class NFCTechs
    {
        public const string IsoDep = "android.nfc.tech.IsoDep";
        public const string NfcA = "android.nfc.tech.NfcA";
        public const string NfcB = "android.nfc.tech.NfcB";
        public const string NfcF = "android.nfc.tech.NfcF";
        public const string NfcV = "android.nfc.tech.NfcV";
        public const string Ndef = "android.nfc.tech.Ndef";
        public const string NdefFormatable = "android.nfc.tech.NdefFormatable";
        public const string MifareClassic = "android.nfc.tech.MifareClassic";
        public const string MifareUltralight = "android.nfc.tech.MifareUltralight";
    }
    #endregion

    public class NfcForms : INfc
    {
        #region Private Variables

        private NfcAdapter nfcDevice;
        private NfcTag nfcTag;
        private Tag droidTag;

        #endregion

        #region Properties

        public bool IsAvailable
        {
            get
            {
                return nfcDevice.IsEnabled;
            }
        }

        #endregion

        #region Constructors

        public NfcForms()
        {
            NfcManager NfcManager = (NfcManager)Android.App.Application.Context.GetSystemService(Context.NfcService);
            nfcDevice = NfcManager.DefaultAdapter;
            nfcTag = new NfcTag();
        }

        #endregion

        #region Private Methods

        private Ndef GetNdef(Tag tag)
        {
            Ndef ndef = Ndef.Get(tag);
            if (ndef == null)
                return null;
            else
                return ndef;
        }

        private NdefLibrary.Ndef.NdefMessage ReadNdef(Ndef ndef)
        {
            if (ndef == null || ndef.CachedNdefMessage == null)
            {
                return null;
            }

            var bytes = ndef.CachedNdefMessage.ToByteArray();
            var message = NdefLibrary.Ndef.NdefMessage.FromByteArray(bytes);

            return message;
        }

        #endregion

        #region Public Methods

        public void OnNewIntent(object sender, Intent e)
        {
            droidTag = e.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;
            if (droidTag != null)
            {
                nfcTag.TechList = new List<string>(droidTag.GetTechList());
                nfcTag.Id = droidTag.GetId();

                if (GetNdef(droidTag) == null)
                {
                    nfcTag.IsNdefSupported = false;
                }
                else
                {
                    nfcTag.IsNdefSupported = true;
                    Ndef ndef = GetNdef(droidTag);
                    nfcTag.NdefMessage = ReadNdef(ndef);
                    nfcTag.IsWriteable = ndef.IsWritable;
                    nfcTag.MaxSize = ndef.MaxSize;
                }

                RaiseNewTag(nfcTag);
            }
        }

        public void WriteTag(NdefLibrary.Ndef.NdefMessage message)
        {
            if (droidTag == null)
            {
                throw new Exception("Tag Error: No Tag to write, register to NewTag event before calling WriteTag()");
            }

            Ndef ndef = GetNdef(droidTag);

            if (ndef == null)
            {
                throw new Exception("Tag Error: NDEF not supported");
            }


            try
            {
                ndef.Connect();
                RaiseTagConnected(nfcTag);
            }

            catch
            {
                throw new Exception("Tag Error: No Tag nearby");
            }

            if (!ndef.IsWritable)
            {
                ndef.Close();
                throw new Exception("Tag Error: Tag is write locked");
            }

            int size = message.ToByteArray().Length;

            if (ndef.MaxSize < size)
            {
                ndef.Close();
                throw new Exception("Tag Error: Tag is too small");
            }

            try
            {
                List<Android.Nfc.NdefRecord> records = new List<Android.Nfc.NdefRecord>();
                for (int i = 0; i < message.Count; i++)
                {
                    if (message[i].CheckIfValid())
                        records.Add(new Android.Nfc.NdefRecord(Android.Nfc.NdefRecord.TnfWellKnown, message[i].Type, message[i].Id, message[i].Payload));
                    else
                    {
                        throw new Exception("NDEFRecord number " + i + "is not valid");
                    }
                };
                Android.Nfc.NdefMessage msg = new Android.Nfc.NdefMessage(records.ToArray());
                ndef.WriteNdefMessage(msg);
            }

            catch (TagLostException tle)
            {
                throw new Exception("Tag Lost Error: " + tle.Message);
            }

            catch (IOException ioe)
            {
                throw new Exception("Tag IO Error: " + ioe.ToString());
            }

            catch (Android.Nfc.FormatException fe)
            {
                throw new Exception("Tag Format Error: " + fe.Message);
            }

            catch (Exception e)
            {
                throw new Exception("Tag Error: " + e.ToString());
            }

            finally
            {
                ndef.Close();
                RaiseTagTagDisconnected(nfcTag);
            }

        }

        #endregion

        #region Events

        public event EventHandler<NfcTag> TagConnected;

        public void RaiseTagConnected(NfcTag tag)
        {
            nfcTag.IsConnected = true;

            if (TagConnected != null)
            {
                TagConnected(this, tag);
            }
        }

        public event EventHandler<NfcTag> TagDisconnected;

        public void RaiseTagTagDisconnected(NfcTag tag)
        {
            nfcTag.IsConnected = false;

            if (TagDisconnected != null)
            {
                TagDisconnected(this, tag);
            }
        }


        public event EventHandler<NfcTag> NewTag;

        public void RaiseNewTag(NfcTag tag)
        {
            if (NewTag != null)
            {
                NewTag(this, tag);
            }
        }

        #endregion
    }
}
