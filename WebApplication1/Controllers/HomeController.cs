using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    
    public class HomeController : Controller
    {  
        private readonly IHttpClientFactory _clientFactory;
        public IEnumerable<CarModel> carModel;
        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
         
            
            var request = new HttpRequestMessage(HttpMethod.Get, "http://order-api.caradis.fr/api/Order/GetAllMarkets");
           
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response =await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
               carModel= Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<CarModel>>(responseStream);
            }
            else
            {
                carModel = new List<CarModel>();
            }
            return View(carModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<User> Login()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://order-api.caradis.fr/api/User/Login");
            request.Content = new StreamContent(JsonSerializer.Serialize(new User()
            {
                UserName = "admin@ixxo.io",
                Password = "1234567"
            }));

            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(request);
            User user = null;
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                 user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(responseStream);
            }
            return user;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
