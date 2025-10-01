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
    [CargoAuthorize("Ejecutivo de Contratos", "Supervisor")]
    public class ContratosController : Controller
    {
        private readonly AppDBContext _context;

        public ContratosController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contratos = await _context.contratos
                .Include(c => c.cliente)
                .Include(c => c.servicio)
                .ToListAsync();

            return View(contratos);
        }

        public IActionResult Crear()
        {
            CargarCombos(new Contrato());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Contrato contrato)
        {
            try
            {
                if (contrato.FechaFin < contrato.FechaInicio)
                {
                    TempData["Mensaje"] = "La fecha de fin no puede ser anterior a la de inicio.";
                    TempData["Tipo"] = "error";
                    CargarCombos(contrato);
                    return View(contrato);
                }

                _context.contratos.Add(contrato);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Contrato registrado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al registrar el contrato: " + ex.Message;
                TempData["Tipo"] = "error";
                CargarCombos(contrato);
                return View(contrato);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var contrato = await _context.contratos.FindAsync(id);
            if (contrato == null) return NotFound();

            CargarCombos(contrato);
            return View(contrato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Contrato contrato)
        {
            try
            {
                if (contrato.FechaFin < contrato.FechaInicio)
                {
                    TempData["Mensaje"] = "La fecha de fin no puede ser anterior a la de inicio.";
                    TempData["Tipo"] = "error";
                    CargarCombos(contrato);
                    return View(contrato);
                }

                _context.contratos.Update(contrato);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Contrato actualizado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al actualizar el contrato: " + ex.Message;
                TempData["Tipo"] = "error";
                CargarCombos(contrato);
                return View(contrato);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmar(int id)
        {
            var contrato = await _context.contratos.FindAsync(id);
            if (contrato == null) return NotFound();

            _context.contratos.Remove(contrato);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Contrato eliminado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

        private void CargarCombos(Contrato c)
        {
            ViewBag.ClienteId = new SelectList(_context.clientes, "Id", "RazonSocial", c?.ClienteId);
            ViewBag.ServicioId = new SelectList(_context.servicios, "Id", "Nombre", c?.ServicioId);
        }

        public async Task<IActionResult> Reporte(int id)
        {
            var contrato = await _context.contratos
                .Include(c => c.cliente)
                .Include(c => c.servicio)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contrato == null) return NotFound();

            return View("Reporte", contrato);
        }
    }
}