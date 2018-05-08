using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HermesMsgClient.Models;
using Newtonsoft.Json;

namespace HermesMsgClient.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient Client = new HttpClient();

            
        public IActionResult Index()
        {
            return View();
        }
            
        [HttpPost]
        public async Task<IActionResult> Index(Message message)
        {
          
            var fixedMessage = new MessageText()
            {
                Text = message.Text
            };
            var response = await Client.PostAsync("http://hermesmsgapl.azurewebsites.net/api/messages",
                new StringContent(JsonConvert.SerializeObject(fixedMessage), Encoding.UTF8, "application/json"));
            var jsonContent = response.Content.ReadAsStringAsync().Result;
            var dotnetContent = JsonConvert.DeserializeObject<MessageResponse>(jsonContent);           
            ModelState.Clear();
            if (!String.IsNullOrEmpty(dotnetContent.Id))
            {
                ViewBag.result = $"ID: {dotnetContent.Id}";
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Search(Message message)
        {
            var fixedMessage = new MessageId()
            {
                Id = message.Id
            };
            var response = await Client.GetAsync($"http://hermesmsgapl.azurewebsites.net/api/messages/{fixedMessage.Id}");
            var jsonContent = response.Content.ReadAsStringAsync().Result;
            var dotnetContent = JsonConvert.DeserializeObject<MessageResponse>(jsonContent);           
            return View(dotnetContent);

        }

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
