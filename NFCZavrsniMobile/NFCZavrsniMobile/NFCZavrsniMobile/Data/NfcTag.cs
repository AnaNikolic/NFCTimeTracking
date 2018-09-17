using NdefLibrary.Ndef;
using System.Collections;

namespace NFCZavrsniMobile.Data
{
    public class NfcTag
    {
            public NfcTag()
            {

            }

            public NdefMessage NdefMessage;
            public IList TechList;
            public bool IsNdefSupported;
            public bool IsWriteable;
            public bool IsConnected;
            public byte[] Id;
            public int MaxSize;
    }
}
