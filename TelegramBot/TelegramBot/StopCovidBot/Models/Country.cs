using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StopCovidBot.Models
{
  public  class Country
    {
        [JsonProperty("Country")]
        public string CountryName { get; set; }
        [JsonProperty("CoutryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("Province")]
        public string Province { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("Confirmed")]
        public long Confirmed { get; set; }

        [JsonProperty("Deaths")]
        public long Deaths { get; set; }

        
        [JsonProperty("Recovered")]
        public long Recovered { get; set; }

        [JsonProperty("Active")]
        public long Active { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }






    }
}
