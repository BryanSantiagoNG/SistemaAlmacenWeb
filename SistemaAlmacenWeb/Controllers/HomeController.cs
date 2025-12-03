using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SistemaAlmacenWeb.Models;
using System.Net.Http; // Para conectar a internet
using System.Text.Json.Nodes; // Para leer el JSON

namespace SistemaAlmacenWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = "https://api.exchangerate-api.com/v4/latest/USD";
                    var response = await client.GetStringAsync(url);

                    var data = JsonNode.Parse(response);

                    double tipoCambio = (double)data["rates"]["MXN"];

                    ViewBag.Dolar = tipoCambio.ToString("N2"); 
                    ViewBag.ApiStatus = "Conectado";
                }
            }
            catch
            {
                ViewBag.Dolar = "--.--";
                ViewBag.ApiStatus = "Sin conexión";
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