using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NFCZavrsniMobile.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BearerToken
    {
        [JsonProperty]
        public string Access_token { get; set; }
        [JsonProperty]
        public string Token_type { get; set; }
        [JsonProperty]
        public long Expires_in { get; set; }
        [JsonProperty]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = ".issued")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Issued { get; set; }

        [JsonProperty(PropertyName = ".expires")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Expires { get; set; }

    }
}
