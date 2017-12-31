using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Stratis.guru.Controllers
{
    public class APIController : Controller
    {
        public async System.Threading.Tasks.Task<ActionResult> Price()
        {
            using (var client = new HttpClient())
            {
                dynamic StratisTicket = JsonConvert.DeserializeObject(await client.GetStringAsync("https://api.coinmarketcap.com/v1/ticker/stratis/"));
                return Content(Convert.ToString(StratisTicket[0].price_usd));
            }
        }

        public double ConvertEUR_USD()
        {
            using (var client = new WebClient())
            {
                dynamic StratisTicket = JsonConvert.DeserializeObject(client.DownloadString("https://api.fixer.io/latest"));
                return Convert.ToDouble(StratisTicket.rates.USD);
            }
        }

        public ActionResult PriceUSD()
        {
            return Content("dsds");
        }

        public ActionResult PriceEUR()
        {
            return Content("dsds");
        }
        public async System.Threading.Tasks.Task<ActionResult> PercentChange(string delay)
        {
            using (var client = new HttpClient())
            {
                dynamic StratisTicket = JsonConvert.DeserializeObject(await client.GetStringAsync("https://api.coinmarketcap.com/v1/ticker/stratis/"));
                switch(delay)
                {
                    case "1h":
                        return Content(Convert.ToString(StratisTicket[0].percent_change_1h));
                    default:
                    case "24h":
                        return Content(Convert.ToString(StratisTicket[0].percent_change_24h));
                    case "d":
                        return Content(Convert.ToString(StratisTicket[0].percent_change_7d));
                }
            }
        }
    }
}