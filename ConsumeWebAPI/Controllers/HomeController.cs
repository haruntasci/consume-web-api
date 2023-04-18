using ConsumeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace ConsumeWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        string baseURL = "http://localhost:8080/Users/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            //Calling the web API and populating the data in view using DataTable
            DataTable dt = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("GetUsers");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    dt = JsonConvert.DeserializeObject<DataTable>(results);
                }
                else
                {
                    Console.WriteLine("Error calling web API");
                }

                ViewData.Model = dt;

            }


            return View();
        }
        public async Task<IActionResult> Index2()
        {

            //Calling the web API and populating the data in view using Entity Model Class
            
            IList<UserEntity> user = new List<UserEntity>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("GetUsers");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<List<UserEntity>>(results);
                }
                else
                {
                    Console.WriteLine("Error calling web API");
                }

                ViewData.Model = user;

            }


            return View();
        }
        public async Task<ActionResult<String>> PostUser(UserEntity user)
        {
            UserEntity obj = new UserEntity()
            {
                Username = user.Username,
                Password = user.Password,
                Emailaddress = user.Emailaddress,
                Mobilenumber = user.Mobilenumber,

            };

            if(user.Username != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync<UserEntity>("PostUser",obj);

                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        Console.WriteLine("Error calling web API");
                    }

                    ViewData.Model = user;

                }
            }

            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}