
using System;
using System.Collections.Generic;
using System.Text;

namespace NFCZavrsniMobile.Services
{
    public interface IAudio
    {
        bool PlayMp3File(string fileName);
        bool PlayWavFile(string fileName);
    }
}