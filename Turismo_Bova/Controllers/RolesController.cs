using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Data;
using Turismo_Bova.Filters;
using Turismo_Bova.Models;

namespace Turismo_Bova.Controllers
{
    [Authorize(Roles = "Administrador")]
    [CargoAuthorize("Administrador")]
    public class RolesController : Controller
    {
        private readonly AppDBContext _context;

        public RolesController(AppDBContext appDBContext)
        {
            _context = appDBContext;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _context.roles.ToListAsync(); 
            return View(roles);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Rol rol)
        {
            var existeRol = await _context.roles
                .FirstOrDefaultAsync(r => r.Nombre == rol.Nombre);

            if (existeRol != null)
            {
                TempData["Mensaje"] = "El rol ya esta registrado.";
                TempData["Tipo"] = "error";
                return View(rol);
            }
            else {
                var role = new Rol
                {
                    Nombre = rol.Nombre
                };

                await _context.roles.AddAsync(role);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "El rol se ha creado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public IActionResult Editar(int id) { 
            

            var role = _context.roles.Find(id);

            if (role == null) { 
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Rol rol)
        {
            _context.roles.Update(rol);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "El rol se ha actualizado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Eliminar(int id) 
        {
            var role = _context.roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        public IActionResult EliminarConfirmar(int id)
        {
            var role = _context.roles.Find(id);
            _context.roles.Remove(role);
            _context.SaveChanges();
            TempData["Mensaje"] = "El rol se ha eliminado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

    }
}


