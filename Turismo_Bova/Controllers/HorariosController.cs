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
    [CargoAuthorize("Administrador", "Planificador de Rutas", "Supervisor")]
    public class HorariosController : Controller
    {
        private readonly AppDBContext _context;

        public HorariosController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var horarios = await _context.horarios
                .Include(h => h.ruta)
                .ToListAsync();
            return View(horarios);
        }

        public IActionResult Crear()
        {
            ViewBag.RutaId = new SelectList(_context.rutas, "Id", "Destino");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Horario horario)
        {
            try
            {
                if (horario.HoraSalida >= horario.HoraLlegada)
                {
                    ViewBag.RutaId = new SelectList(_context.rutas, "Id", "Destino", horario.RutaId);
                    TempData["Mensaje"] = "La hora de salida debe ser menor a la hora de llegada.";
                    TempData["Tipo"] = "Error";
                    return View(horario);
                }

                _context.horarios.Add(horario);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Horario registrado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.RutaId = new SelectList(_context.rutas, "Id", "Destino", horario.RutaId);
                TempData["Mensaje"] = "Error al guardar: " + ex.Message;
                TempData["Tipo"] = "Error";
                return View(horario);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var horario = await _context.horarios.FindAsync(id);
            if (horario == null)
                return NotFound();

            ViewBag.RutaId = new SelectList(_context.rutas, "Id", "Destino", horario.RutaId);
            return View(horario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Horario horario)
        {
            try
            {
                if (horario.HoraSalida >= horario.HoraLlegada)
                {
                    ViewBag.RutaId = new SelectList(_context.rutas, "Id", "Destino", horario.RutaId);
                    TempData["Mensaje"] = "La hora de salida debe ser menor a la hora de llegada.";
                    TempData["Tipo"] = "Error";
                    return View(horario);
                }

                _context.horarios.Update(horario);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Horario actualizado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.RutaId = new SelectList(_context.rutas, "Id", "Destino", horario.RutaId);
                TempData["Mensaje"] = "Error al actualizar: " + ex.Message;
                TempData["Tipo"] = "Error";
                return View(horario);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmar(int id)
        {
            var horario = await _context.horarios.FindAsync(id);
            if (horario == null)
                return NotFound();

            _context.horarios.Remove(horario);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Horario eliminado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }
    }
}
