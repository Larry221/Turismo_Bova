using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Data;
using Turismo_Bova.Filters;
using Turismo_Bova.Models;

namespace Turismo_Bova.Controllers
{
    [Authorize(Roles = "Administrador, Empleado")]
    [CargoAuthorize("Administrador", "Ejecutivo de Contratos", "Supervisor")]
    public class FacturasController : Controller
    {
        private readonly AppDBContext _context;

        public FacturasController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var facturas = await _context.facturas
                .Include(f => f.contrato)
                    .ThenInclude(c => c.cliente)
                .ToListAsync();

            return View(facturas);
        }

        public IActionResult Crear()
        {
            ViewBag.ContratoId = new SelectList(
                _context.contratos.Include(c => c.cliente),
                "Id", "Id");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Factura factura)
        {
            try
            {
                // Obtener pagos del contrato
                var pagos = await _context.pagos
                    .Where(p => p.ContratoId == factura.ContratoId)
                    .ToListAsync();

                if (!pagos.Any())
                {
                    TempData["Mensaje"] = "No se puede generar factura sin pagos registrados.";
                    TempData["Tipo"] = "error";
                    ViewBag.ContratoId = new SelectList(_context.contratos, "Id", "Id", factura.ContratoId);
                    return View(factura);
                }

                double subtotal = pagos.Sum(p => p.MontoPagado);
                factura.Igv = Math.Round(subtotal * 0.18, 2);
                factura.Total = Math.Round(subtotal + factura.Igv, 2);
                factura.FechaEmision = DateOnly.FromDateTime(DateTime.Today);

                _context.facturas.Add(factura);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Factura generada correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al generar la factura: " + ex.Message;
                TempData["Tipo"] = "error";
                ViewBag.ContratoId = new SelectList(_context.contratos, "Id", "Id", factura.ContratoId);
                return View(factura);
            }
        }

        public async Task<IActionResult> Reporte(int id)
        {
            var factura = await _context.facturas
                .Include(f => f.contrato)
                    .ThenInclude(c => c.cliente)
                .Include(f => f.contrato)
                    .ThenInclude(c => c.servicio)
                .Include(f => f.contrato)
                    .ThenInclude(c => c.pago)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null) return NotFound();

            return View("Reporte", factura);
        }
    }
}
