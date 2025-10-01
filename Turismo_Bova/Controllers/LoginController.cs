using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Turismo_Bova.Data;
using Turismo_Bova.ViewModels;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Models;
using Microsoft.AspNetCore.Identity;

namespace Turismo_Bova.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDBContext _appDBContext;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public LoginController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _appDBContext.usuarios
                .Include(u => u.rol)
                .Include(u => u.Empleado)
                .FirstOrDefaultAsync(user => user.Correo == model.Correo);

            if (usuario != null)
            {
                //var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Contraseña, model.Contraseña);

                if (usuario.Contraseña == model.Contraseña)//resultado == PasswordVerificationResult.Success)
                {
                    // Configurar sesión
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim("UserId", usuario.Id.ToString()),
                        new Claim(ClaimTypes.Name, usuario.Nombre),
                        new Claim(ClaimTypes.Email, usuario.Correo),
                        new Claim(ClaimTypes.Role, usuario.rol?.Nombre ?? "Usuario"),
                        new Claim("Cargo", usuario.Empleado?.Cargo?.Trim() ?? "")
                    };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity),
                        properties
                    );

                    TempData["Mensaje"] = $"Bienvenido, {usuario.Nombre}";
                    TempData["Tipo"] = "success";
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["Mensaje"] = "Correo o contraseña incorrectos.";
            TempData["Tipo"] = "error";
            return View(model);
        }

        [HttpGet]
        public IActionResult Registro()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("TurismoBovaAuth");
            return RedirectToAction("Login");
        }

        public IActionResult AccesoDenegado()
        {
            TempData["Mensaje"] = "Acceso denegado. No tienes permiso para acceder a esta página.";
            TempData["Tipo"] = "error";
            return RedirectToAction("Index", "Home");
        }
    }
}
