using Microsoft.AspNetCore.Mvc;
using WebProApiCons.Models;

namespace WebProApiCons.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        public async Task<IActionResult> Index()
        {
            try 
            {
                var client = _httpClientFactory.CreateClient("WebProApi");
                var response = await client.GetAsync("api/products");

                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                    return View(products);
                }
                return View(new List<Product>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error : {ex.Message}";
                return View(new List<Product>());
            }

        }

        // Modo Edición (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if(id <= 0)
            {
                return View(new Product()); // Modo creación;
            }

            var client = _httpClientFactory.CreateClient("WebProApi");
            var response = await client.GetAsync($"api/products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadFromJsonAsync<Product>();
                return View(product);
            }

            TempData["Error"] = "Producto no encontrado";
            return RedirectToAction("Index");
        }

        // Modo Edición (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            var client = _httpClientFactory.CreateClient("WebProApi");
            HttpResponseMessage response;

            if (product.Id==0) 
            {
                response = await client.PostAsJsonAsync("api/products", product);
            }
            else
            {
                response = await client.PutAsJsonAsync($"api/products/{product.Id}", product);
            }

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = $"Producto {(product.Id==0? "Created": "Updated")}";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Failed to save product");
            return View(product);
        }

        // Modo Edición (DELETE)
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var response = await client.DeleteAsync($"/api/products/{id}");

            TempData[response.IsSuccessStatusCode ? "Success" : "Error"] =
                response.IsSuccessStatusCode ? "Product removed" : "Error to delete";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("WebProApi");
                var response = await client.PostAsJsonAsync("api/products", product);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to create product");
            return View(product);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTime()
        {
            DateTime time = DateTime.Now;
            string response = time.ToString("yyyy-MM-dd HH:mm:ss");
            return Json(response);
        }
    }
}
