using IPCalculator.Web.Data;
using IPCalculator.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace WebApplication1.Controllers
{
    public class InternetProtocolController : Controller
    {
        public IActionResult Starter()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Starter(string ipDec)
        {
            string message = InternetProtocolService.ErrorValidIP(ipDec);
            if(message == "true")
            {              
                return RedirectToAction("IP", ipDec);
            }
            else
            {
                ViewData.Add("InvalidIP", message);
            }
            return View();
        }

        [Route("{ipDec}/IP")]
        public IActionResult IP(string ipDec)
        {
            TempData.Clear();
            IPInfos ipInfos = InternetProtocolService.IPInfosCalculate(ipDec);     
            TempData["IPInfos"] = JsonConvert.SerializeObject(ipInfos);
            return View(ipInfos);
        }

        [HttpPost]
        [Route("{ipDec}/IP")]
        public IActionResult IP(int subnetCount)
        {
            IPInfos ipInfos = JsonConvert.DeserializeObject<IPInfos>(TempData["IPInfos"] as string);

            string message = InternetProtocolService.ErrorSubnetCountMessage(subnetCount, ipInfos.NetworkClass);
            if(message == "true")
            {
                ipInfos.SubnetCount = subnetCount;
                TempData["IPInfos"] = JsonConvert.SerializeObject(ipInfos);
                return RedirectToAction("IPSubnetsSM");
                
            }
            else
            {
                TempData["IPInfos"] = JsonConvert.SerializeObject(ipInfos);
                ViewData.Add("ErrorSubnetCountMessage", message);
            }           
            return View(ipInfos);
        }

        [Route("IPSubnetsSM")]
        public IActionResult IPSubnetsSM()
        {
            IPInfos ipInfos = JsonConvert.DeserializeObject<IPInfos>(TempData["IPInfos"] as string);

            ipInfos = InternetProtocolService.SMCalculate(ipInfos);
            TempData["IPInfos"] = JsonConvert.SerializeObject(ipInfos);
            return View(ipInfos);            
        }

        [HttpPost]
        [Route("IPSubnetsSM")]
        public IActionResult IPSubnetsSM(int subnetNumber)
        {
            IPInfos ipInfos = JsonConvert.DeserializeObject<IPInfos>(TempData["IPInfos"] as string);

            string message = InternetProtocolService.ErrorSubnetNumberMessage(ipInfos.SubnetCount, subnetNumber);
            if (message == "true")
            {
                ipInfos.CurrentSubnetNumber = subnetNumber;
                TempData["IPInfos"] = JsonConvert.SerializeObject(ipInfos);
                return RedirectToAction("IPSubnetDetails");
            }
            else
            {
                TempData["IPInfos"] = JsonConvert.SerializeObject(ipInfos);
                ViewData.Add("ErrorSubnetNumberMessage", message);
            }
            return View(ipInfos);
        }

        [Route("IPSubnetDetails")]
        public IActionResult IPSubnetDetails()
        {
            IPInfos ipInfos = JsonConvert.DeserializeObject<IPInfos>(TempData["IPInfos"] as string);
            ipInfos = InternetProtocolService.IPInfosSubnetCalculate(ipInfos);

            TempData["IPInfos"] = JsonConvert.SerializeObject(ipInfos);
            return View(ipInfos);
        }

        [HttpPost]
        [Route("IPSubnetDetails")]
        public IActionResult IPSubnetDetails(int subnetNumber)
        {
            IPInfos ipInfos = JsonConvert.DeserializeObject<IPInfos>(TempData["IPInfos"] as string);

            string message = InternetProtocolService.ErrorSubnetNumberMessage(ipInfos.SubnetCount, subnetNumber);
            if (message == "true")
            {
                ipInfos.CurrentSubnetNumber = subnetNumber;
                TempData["IPInfos"] = JsonConvert.SerializeObject(ipInfos);
                return RedirectToAction("IPSubnetDetails");
            }
            else
            {
                TempData["IPInfos"] = JsonConvert.SerializeObject(ipInfos);
                ViewData.Add("ErrorSubnetNumberMessage", message);
            }
            return View(ipInfos);
        }
    }
}
