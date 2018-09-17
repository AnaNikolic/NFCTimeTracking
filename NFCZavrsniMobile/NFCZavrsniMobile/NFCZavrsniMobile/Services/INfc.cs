using NdefLibrary.Ndef;
using System;
using NFCZavrsniMobile.Data;

namespace NFCZavrsniMobile.Services
{
    public interface INfc
    {
        /// <summary>
        /// Gets if the device is able to detect NFC tags
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Writes a Tag if available
        /// </summary>
        /// <param name="message">NDEF Message to write on the tag</param>
        void WriteTag(NdefMessage message);

        /// <summary>
        /// Event raised when a tag is discovered and scanned
        /// </summary>
        event EventHandler<NfcTag> NewTag;

        /// <summary>
        /// Event raised when a tag is discovered
        /// </summary>
        event EventHandler<NfcTag> TagConnected;

        /// <summary>
        /// Event raised when a tag is lost
        /// </summary>
        event EventHandler<NfcTag> TagDisconnected;
    }

}
