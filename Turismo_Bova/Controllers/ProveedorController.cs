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
    public class ProveedorController : Controller
    {
        private readonly AppDBContext _context;

        public ProveedorController(AppDBContext appDBContext)
        {
            _context = appDBContext;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _context.proveedores
            .ToListAsync();
            return View(user);
        }

        [HttpGet]
        public IActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Proveedor c)
        {
            var existeUser = await _context.proveedores
                .FirstOrDefaultAsync(r => r.Ruc == c.Ruc || r.Correo == c.Correo);

            if (existeUser != null)
            {
                TempData["Mensaje"] = "El RUC o Correo ya está registrado.";
                TempData["Tipo"] = "error";
                return View(c);
            }

            var em = new Proveedor
            {
                RazonSocial = c.RazonSocial,
                Ruc = c.Ruc,
                Correo = c.Correo,
                Telefono = c.Telefono,
                Direccion = c.Direccion
            };

            await _context.proveedores.AddAsync(em);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "El proveedor se ha creado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var prov = _context.proveedores.Find(id);

            if (prov == null)
            {
                return NotFound();
            }
            return View(prov);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Proveedor p)
        {
            _context.proveedores.Update(p);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "El proveedor se ha actualizado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult EliminarConfirmar(int id)
        {
            var use = _context.proveedores.Find(id);
            _context.proveedores.Remove(use);
            _context.SaveChanges();
            TempData["Mensaje"] = "El proveedor se ha eliminado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }


    }
}
