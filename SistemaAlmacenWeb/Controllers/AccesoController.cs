using Microsoft.AspNetCore.Mvc;
using SistemaAlmacenWeb.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SistemaAlmacenWeb.Controllers
{
    public class AccesoController : Controller
    {
        private readonly SistemaAlmacenContext _context;

        public AccesoController(SistemaAlmacenContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UsuarioId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario, string password)
        {
            var user = _context.Usuarios
                .FirstOrDefault(u => u.UsuarioNombre == usuario && u.Contraseña == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UsuarioId", user.IdUsuario.ToString());
                HttpContext.Session.SetString("UsuarioNombre", user.UsuarioNombre);
                HttpContext.Session.SetString("Rol", user.Rol ?? "Empleado");

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login");
        }

        public IActionResult Perfil()
        {
            var idString = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(idString)) return RedirectToAction("Login");

            int id = int.Parse(idString);
            var user = _context.Usuarios.Find(id);

            return View(user);
        }
    }
}