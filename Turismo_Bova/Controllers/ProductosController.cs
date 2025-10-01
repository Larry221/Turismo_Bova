using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Data;
using Turismo_Bova.Filters;
using Turismo_Bova.Models;

namespace Turismo_Bova.Controllers
{
    [Authorize(Roles = "Administrador")]
    [CargoAuthorize("Administrador")]
    public class ProductosController : Controller
    {
        private readonly AppDBContext _context;

        public ProductosController(AppDBContext appDBContext)
        {
            _context = appDBContext;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _context.productos
            .Include(u => u.proveedor)
            .ToListAsync();
            return View(user);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            var prove = _context.proveedores
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.RazonSocial })
                .ToList();
            ViewBag.Proveedores = prove;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Crear(Producto p)
        {
            var producto = new Producto
            {
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                CantidadStock = p.CantidadStock,
                PrecioUnidad = p.PrecioUnidad,
                ProveedorId = p.ProveedorId,
            };
            await _context.productos.AddAsync(producto);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Se agrego el producto.";
            TempData["Tipo"] = "success";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var us = _context.productos.FirstOrDefault(u => u.Id == id);

            if (us == null)
            {
                return NotFound();
            }

            var prov = _context.proveedores
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.RazonSocial
                }).ToList();

            ViewBag.Proveedores = prov;
            return View(us);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Producto u)
        {
            _context.productos.Update(u);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Se actualizo el producto.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult EliminarConfirmar(int id)
        {
            var use = _context.productos.Find(id);
            _context.productos.Remove(use);
            _context.SaveChanges();
            TempData["Mensaje"] = "Se elimino el producto.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }
    }
}
