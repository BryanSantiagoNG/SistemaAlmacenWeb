using Microsoft.AspNetCore.Mvc;
using SistemaAlmacenWeb.Models;
using Microsoft.AspNetCore.Http; // Para usar Sesiones
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

        // GET: Muestra la pantalla de Login
        public IActionResult Login()
        {
            // Si ya está logueado, mandar al inicio
            if (HttpContext.Session.GetString("UsuarioId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Procesa el usuario y contraseña
        [HttpPost]
        public IActionResult Login(string usuario, string password)
        {
            // Buscamos en la base de datos (Nota: En producción usa hash para passwords, aquí texto plano por simplicidad)
            var user = _context.Usuarios
                .FirstOrDefault(u => u.UsuarioNombre == usuario && u.Contraseña == password);

            if (user != null)
            {
                // Guardamos datos en sesión
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

        // GET: Cerrar Sesión
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Borra todo
            return RedirectToAction("Login");
        }

        // GET: Perfil de Usuario
        public IActionResult Perfil()
        {
            // Verificar si hay sesión
            var idString = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(idString)) return RedirectToAction("Login");

            int id = int.Parse(idString);
            var user = _context.Usuarios.Find(id);

            return View(user);
        }
    }
}