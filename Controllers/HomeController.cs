using InventoryManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Nancy;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InventoryManagementApp.Controllers;
using Nancy.Json;

namespace InventoryManagementApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ValidateLogin(LoginModel login)
        {
            if (login.username == "Admin" && login.password == "admin")
            {
                return RedirectToAction("ListItem");
            }
            else
            {
                return View("ErrorLogin");
            }
        }

        public async Task<IActionResult> ListItem()
        {
            List<Item> itemList = new List<Item>();
            using (var httpClient = new HttpClient())
            {
                using (var responce = await httpClient.GetAsync("https://localhost:44358/api/inventory"))
                {
                    string apiResponse = await responce.Content.ReadAsStringAsync();
                    itemList = JsonConvert.DeserializeObject<List<Item>>(apiResponse);
                }
            }
            return View(itemList);
        }

        public ViewResult NewItem() => View();

        [HttpPost]
        public async Task<IActionResult> NewItem(Item item)
        {
            
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:44358/api/Inventory", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    JsonConvert.DeserializeObject<Item>(apiResponse);
                }
            }
            return RedirectToAction("ListItem");
        }

        [HttpGet]
        public async Task<IActionResult> EditItem(int id)
        {
            Item item = new Item();
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44358/api/inventory/{id}");
            var response = await httpClient.SendAsync(request);
            string apiResponse = await response.Content.ReadAsStringAsync();
            item = JsonConvert.DeserializeObject<Item>(apiResponse);

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem(Item item)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:44358/api/inventory/{item.Id}")
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(item), Encoding.UTF8, "application/json")
            };
            await httpClient.SendAsync(request);
            return RedirectToAction("ListItem");
        }

        [HttpGet]
        public async Task<IActionResult> GetConfirmationToDelete(int id)
        {
            Item item = new Item();
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44358/api/inventory/{id}");
            var response = await httpClient.SendAsync(request);
            string apiResponse = await response.Content.ReadAsStringAsync();
            item = JsonConvert.DeserializeObject<Item>(apiResponse);

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(Item item)
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:44358/api/inventory/{item.Id}")
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(item), Encoding.UTF8, "application/json")
            };
            await httpClient.SendAsync(request);
            
            return RedirectToAction("ListItem");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}