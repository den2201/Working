using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StopCovidBot.Models
{
   public class CodesCountry
    {
        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("ISO2")]
        public string Code { get; set; }
    }
}
