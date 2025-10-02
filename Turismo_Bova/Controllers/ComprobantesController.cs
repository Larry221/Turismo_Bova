using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Data;
using Turismo_Bova.Models;
using Turismo_Bova.Filters;

namespace Turismo_Bova.Controllers
{
    [Authorize(Roles = "Administrador")]
    [CargoAuthorize("Administrador")]
    public class ComprobantesController : Controller
    {
        private readonly AppDBContext _context;

        public ComprobantesController(AppDBContext context)
        {
            _context = context;
        }

        // Listar comprobantes
        public async Task<IActionResult> Index()
        {
            var comprobantes = await _context.comprobantes
                .Include(c => c.Cliente)
                .ToListAsync();
            return View(comprobantes);
        }

        // Crear comprobante GET
        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Clientes = _context.clientes.ToList();
            return View();
        }

        // Crear comprobante POST
        [HttpPost]
        public async Task<IActionResult> Crear(Comprobante c)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = _context.clientes.ToList();
                return View(c);
            }

            await _context.comprobantes.AddAsync(c);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Comprobante creado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction("Index");
        }

        // Editar comprobante GET
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var comprobante = await _context.comprobantes.FindAsync(id);
            if (comprobante == null) return NotFound();

            ViewBag.Clientes = _context.clientes.ToList();
            return View(comprobante);
        }

        // Editar comprobante POST
        [HttpPost]
        public async Task<IActionResult> Editar(int id, Comprobante comprobante)
        {
            if (id != comprobante.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = _context.clientes.ToList();
                return View(comprobante);
            }

            _context.Update(comprobante);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Comprobante actualizado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction("Index");
        }

        // Eliminar comprobante
        [HttpPost]
        public async Task<IActionResult> EliminarConfirmar(int id)
        {
            var comprobante = await _context.comprobantes.FindAsync(id);
            if (comprobante == null) return NotFound();

            _context.comprobantes.Remove(comprobante);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Comprobante eliminado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }
    }
}
