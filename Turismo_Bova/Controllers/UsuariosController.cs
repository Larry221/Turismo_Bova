using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Data;
using Turismo_Bova.Filters;
using Turismo_Bova.Models;
using Turismo_Bova.ViewModels;

namespace Turismo_Bova.Controllers
{
    [Authorize(Roles = "Administrador")]
    [CargoAuthorize("Administrador")]
    public class UsuariosController : Controller
    {
        private readonly AppDBContext _context;

        public UsuariosController(AppDBContext appDBContext)
        {
            _context = appDBContext;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _context.usuarios
            .Include(u => u.rol) 
            .ToListAsync();
            return View(user);

        }

        [HttpGet]
        public IActionResult Crear()
        {
            CargarViewBags();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Crear(Usuario u)
        {
            var correoEnUso = await _context.usuarios
                .AnyAsync(r => r.Correo == u.Correo);

            if (correoEnUso)
            {
                TempData["Mensaje"] = "El correo ya está registrado en otra cuenta.";
                TempData["Tipo"] = "error";
                CargarViewBags();
                return View(u);
            }

            if (u.EmpleadoId != null)
            {
                var empleado = await _context.empleados.FindAsync(u.EmpleadoId);
                if (empleado == null || empleado.Correo != u.Correo)
                {
                    TempData["Mensaje"] = "El correo ingresado no coincide con el del empleado seleccionado.";
                    TempData["Tipo"] = "error";
                    CargarViewBags();
                    return View(u);
                }
            }

            if (u.ClienteId != null)
            {
                var cliente = await _context.clientes.FindAsync(u.ClienteId);
                if (cliente == null || cliente.Correo != u.Correo)
                {
                    TempData["Mensaje"] = "El correo ingresado no coincide con el del cliente seleccionado.";
                    TempData["Tipo"] = "error";
                    CargarViewBags();
                    return View(u);
                }
            }

            var usuario = new Usuario
            {
                Nombre = u.Nombre,
                Correo = u.Correo,
                Contraseña = u.Contraseña,
                RolId = u.RolId,
                EmpleadoId = u.EmpleadoId,
                ClienteId = u.ClienteId
            };

            //usuario.Contraseña = _passwordHasher.HashPassword(usuario, u.Contraseña);
            await _context.usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "El usuario se ha creado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Editar(int id)
        {
            var us = _context.usuarios.FirstOrDefault(u => u.Id == id);

            if (us == null)
            {
                return NotFound();
            }

            var roles = _context.roles
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Nombre
                }).ToList();

            ViewBag.Roles = roles;
            return View(us);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Usuario u)
        {
            _context.usuarios.Update(u);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "El usuario se ha actualizado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }  

        [HttpPost]
        public IActionResult EliminarConfirmar(int id)
        {
            var use = _context.usuarios.Find(id);
            _context.usuarios.Remove(use);
            _context.SaveChanges();
            TempData["Mensaje"] = "El usuario se ha eliminado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

        private void CargarViewBags()
        {
            var roles = _context.roles
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Nombre })
                .ToList();

            var empleados = _context.empleados
                .Select(e => new { e.Id, e.Nombre, e.Correo })
                .ToList();

            var clientes = _context.clientes
                .Select(c => new { c.Id, c.RazonSocial, c.Correo })
                .ToList();

            ViewBag.Roles = roles;
            ViewBag.Empleados = empleados.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre }).ToList();
            ViewBag.Clientes = clientes.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.RazonSocial }).ToList();

            ViewBag.EmpleadosCorreo = System.Text.Json.JsonSerializer.Serialize(empleados);
            ViewBag.ClientesCorreo = System.Text.Json.JsonSerializer.Serialize(clientes);
        }

    }
}
