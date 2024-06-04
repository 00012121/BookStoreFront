using BookStoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BookStoreFront.Controllers
{
    public class BookController : Controller
    {
        private const string BaseApi = "https://localhost:7172/";
        
        // GET: BookController
        public async Task<ActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new System.Uri(BaseApi);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.GetAsync("api/Book");

                if (response.IsSuccessStatusCode)
                {
                    // Read the content of the response and deserialize it into a list of Books
                    var content = await response.Content.ReadAsStringAsync();
                    var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(content);
                    return View(books);
                }
                return View();
            }

        }
    }
}
