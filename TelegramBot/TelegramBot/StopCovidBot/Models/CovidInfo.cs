using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StopCovidBot.Models
{
   public class CovidInfo
    {
        public static string GetInfoCovid()
        {
            try
            {
                using (FileStream f = new FileStream("Resources/Info.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var sr = new StreamReader(f, Encoding.UTF8);
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return string.Format("Sorry, info not found");
            }
        }

        public static string GetSitesInfo()
        {
            try
            {
                using (FileStream f = new FileStream("Resources/otherInfo.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var sr = new StreamReader(f, Encoding.UTF8);
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return string.Format("Sorry, info not found");
            }
        }

    

        public static string GetSympCovid()
        {
            try
            {
                using (FileStream f = new FileStream("Resources/symptoms.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var sr = new StreamReader(f, Encoding.UTF8);
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return string.Format("Sorry, info not found");
            }
        }
        public static string GetProfCovid()
        {
            try
            {
                using (FileStream f = new FileStream("Resources/prof.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var sr = new StreamReader(f, Encoding.UTF8);
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return string.Format("Sorry, info not found");
            }
        }

        public static string GetStatCovidTotalToday()
        {
            try
            {
                var client = new RestClient("https://api.covid19api.com/summary");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                Total total = JsonConvert.DeserializeObject<Total>(response.Content);
                return DoMessage(total);
            }
            catch(Exception e)
            {
                return string.Format("Sorry, info not found");
            }
        }
        public static string GetStatCovidCountryToday(string code)
        {
            try
            {
                var client = new RestClient(string.Format("https://api.covid19api.com/live/country/"+code.ToLower()));
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                List<Country> lstStat = JsonConvert.DeserializeObject<List<Country>>(response.Content);
                lstStat.Reverse();
                return DoMessage(lstStat[0]);
            }
            catch (Exception e)
            {
                return string.Format("Sorry, info not found");
            }
        }

        public static string GetCountriesCodes()
        {
            try
            {
                var client = new RestClient(string.Format("https://api.covid19api.com/countries"));
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                var lstCountries= JsonConvert.DeserializeObject<List<CodesCountry>>(response.Content);
                string dic = string.Empty;
                 foreach(var item in lstCountries)
                {
                    dic += $"{item.Country} - {item.Code}; ";
                }
                return dic;
            }
            catch (Exception e)
            {
                return $"Error. Not found";
            }
        }

        private static string DoMessage <T>(T obj)
        {
            string message=string.Empty;
            string confirmed;
            string death;
            string recovered;
            if (obj is Total)
            {
                
                 var m = obj as Total;
                 confirmed = string.Format($"Cлучаев заражения: {m.Global.NewConfirmed}  Bceго:{m.Global.TotalConfirmed}\n");
                 death = string.Format($"Погибло: {m.Global.NewDeaths}  Всего:{m.Global.TotalDeaths} человек\n");
                 recovered = string.Format($"Вылечилось: {m.Global.NewRecovered}  Всего:{m.Global.TotalRecovered} человек\n");
                 message = string.Format($"Cо вчерашнего дня зафиксировано в мире:\n\n {confirmed}\n{death}\n{recovered}\n");
            }
            else if(obj is Country)
            {
                var m = obj as Country;
                confirmed = string.Format($"Cлучаев заражения: {m.Confirmed}\n");
                death = string.Format($"Погибло: {m.Deaths} человек\n");
                recovered = string.Format($"Вылечилось: {m.Recovered} человек\n");
                message = string.Format($"На дату {m.Date}  в {m.CountryName} :\n\n {confirmed}\n{death}\n{recovered}\n");
            }
            return message;
        }
    }
}
