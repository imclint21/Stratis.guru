using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Globalization;
using NBitcoin;

namespace Stratis.guru.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Active = "Index";
            return View();
        }
        
        [Route("generate-address", Name = "Generator")]
        public ActionResult Generator()
        {
            ViewBag.Active = "Generator";
            Key privateKey = new Key();
            ViewBag.Pri = privateKey.GetWif(Network.StratisMain).ToString();
            ViewBag.Pub = privateKey.PubKey.GetAddress(Network.StratisMain).ToString();
            return View();
        }

        [ChildActionOnly]
        /*[OutputCache(Duration = 60, VaryByParam = "none")]*/
        public ActionResult HomePrice()
        {
            string CurrencySymbol = new RegionInfo(System.Threading.Thread.CurrentThread.CurrentCulture.LCID).ISOCurrencySymbol;
            using (var client = new WebClient())
            {
                dynamic StratisTicket = JsonConvert.DeserializeObject(client.DownloadString("https://api.coinmarketcap.com/v1/ticker/stratis/"));
                double StratisPriceUSD = Convert.ToDouble(StratisTicket[0].price_usd);
                ViewBag.PercentChange = Convert.ToDouble(StratisTicket[0].percent_change_24h);
                ViewBag.PercentChangeColor = ViewBag.PercentChange < 0 ? "red" : "green";

                switch (CurrencySymbol)
                {
                    default:
                    case "EUR":
                        ViewBag.Price = (StratisPriceUSD / ConvertEUR_USD()).ToString("C");
                        break;

                    case "USD":
                        ViewBag.Price = StratisPriceUSD.ToString("C");
                        break;
                }
            }
            return PartialView();
        }

        public double ConvertEUR_USD()
        {
            using (var client = new WebClient())
            {
                dynamic StratisTicket = JsonConvert.DeserializeObject(client.DownloadString("https://api.fixer.io/latest"));
                return 1.19985;// Convert.ToDouble(StratisTicket.rates.USD);
            }
        }
    }
}