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


        // GET: BookController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new System.Uri("https://localhost:7172/");
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.GetAsync("api/Book/" + id);

                if (response.IsSuccessStatusCode)
                {
                    // Read the content of the response and deserialize it into a list of books
                    var content = await response.Content.ReadAsStringAsync();
                    var book = JsonConvert.DeserializeObject<Book>(content);
                    return View(book);
                }
                return View();
            }

        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Book book)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(BaseApi);
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Serialize the employee object to JSON
                    var jsonContent = JsonConvert.SerializeObject(book);
                    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                    // Send the POST request
                    var response = await httpClient.PostAsync("api/book/", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Extract the response content (the newly created book)
                        var responseData = await response.Content.ReadAsStringAsync();
                        var createdBook = JsonConvert.DeserializeObject<Book>(responseData);

                        // Redirect to the Index action or another suitable action
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Handle the error response here if needed
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception and return the view with error message
                ModelState.AddModelError(string.Empty, $"Exception: {ex.Message}");
            }

            // If we got this far, something failed, re-display form
            return View(book);

        }

        // GET: BookController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new System.Uri("https://localhost:7172/");
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.GetAsync("api/Book/" + id);

                if (response.IsSuccessStatusCode)
                {
                    // Read the content of the response and deserialize it into a list of employees
                    var content = await response.Content.ReadAsStringAsync();
                    var book = JsonConvert.DeserializeObject<Book>(content);
                    return View(book);
                }
                return View();
            }
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Book book)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(BaseApi);
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var jsonContent = JsonConvert.SerializeObject(book);
                    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync($"api/Book/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }
    }
}
