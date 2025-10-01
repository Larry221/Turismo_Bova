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
    public class PagosController : Controller
    {
        private readonly AppDBContext _context;

        public PagosController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pagos = await _context.pagos
                .Include(p => p.contrato)
                    .ThenInclude(c => c.cliente)
                .ToListAsync();

            return View(pagos);
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
        public async Task<IActionResult> Crear(Pago pago)
        {
            try
            {
                _context.pagos.Add(pago);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Pago registrado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al registrar el pago: " + ex.Message;
                TempData["Tipo"] = "error";
                ViewBag.ContratoId = new SelectList(_context.contratos, "Id", "Id", pago.ContratoId);
                return View(pago);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var pago = await _context.pagos.FindAsync(id);
            if (pago == null) return NotFound();

            ViewBag.ContratoId = new SelectList(_context.contratos, "Id", "Id", pago.ContratoId);
            return View(pago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Pago pago)
        {
            try
            {
                _context.pagos.Update(pago);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Pago actualizado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al actualizar el pago: " + ex.Message;
                TempData["Tipo"] = "error";
                ViewBag.ContratoId = new SelectList(_context.contratos, "Id", "Id", pago.ContratoId);
                return View(pago);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmar(int id)
        {
            var pago = await _context.pagos.FindAsync(id);
            if (pago == null) return NotFound();

            _context.pagos.Remove(pago);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Pago eliminado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PagosPorContrato(int contratoId)
        {
            var contrato = await _context.contratos
                .Include(c => c.cliente)
                .Include(c => c.servicio)
                .FirstOrDefaultAsync(c => c.Id == contratoId);

            if (contrato == null) return NotFound();

            var pagos = await _context.pagos
                .Where(p => p.ContratoId == contratoId)
                .ToListAsync();

            ViewBag.Contrato = contrato;
            ViewBag.TotalPagado = pagos.Sum(p => p.MontoPagado);
            ViewBag.Restante = contrato.PrecioTotal - ViewBag.TotalPagado;

            return View("PagosPorContrato", pagos);
        }

    }
}
