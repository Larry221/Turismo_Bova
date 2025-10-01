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
    public class RutasController : Controller
    {
        private readonly AppDBContext _context;

        public RutasController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string buscar)
        {
            var rutas = _context.rutas.Include(r => r.servicio).AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var termino = buscar.Trim().ToLower();
                rutas = rutas.Where(r =>
                    r.Origen.ToLower().Contains(termino) ||
                    r.Destino.ToLower().Contains(termino) ||
                    r.DuracionEstimada.ToLower().Contains(termino) ||
                    r.servicio.Nombre.ToLower().Contains(termino));
            }

            return View(await rutas.ToListAsync());
        }

        public IActionResult Crear()
        {
            ViewBag.ServicioId = new SelectList(_context.servicios, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Ruta ruta)
        {
            try
            {
                // Validaciones mínimas para evitar errores en la base de datos
                if (string.IsNullOrWhiteSpace(ruta.Origen))
                    ruta.Origen = "Sin origen";

                if (string.IsNullOrWhiteSpace(ruta.Destino))
                    ruta.Destino = "Sin destino";

                if (string.IsNullOrWhiteSpace(ruta.DuracionEstimada))
                    ruta.DuracionEstimada = "0 min";

                if (ruta.DistanciaKm <= 0)
                    ruta.DistanciaKm = 1;

                if (ruta.ServicioId <= 0)
                {
                    ViewBag.ServicioId = new SelectList(_context.servicios, "Id", "Nombre");
                    TempData["Mensaje"] = "Debes seleccionar un servicio válido.";
                    TempData["Tipo"] = "Error";
                    return View(ruta);
                }

                _context.rutas.Add(ruta);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Ruta registrada correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al guardar la ruta: " + ex.Message;
                TempData["Tipo"] = "Error";
                ViewBag.ServicioId = new SelectList(_context.servicios, "Id", "Nombre", ruta.ServicioId);
                return View(ruta);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var ruta = await _context.rutas.FindAsync(id);
            if (ruta == null)
                return NotFound();

            ViewBag.ServicioId = new SelectList(_context.servicios, "Id", "Nombre", ruta.ServicioId);
            return View(ruta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Ruta ruta)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ruta.Origen))
                    ruta.Origen = "Sin origen";

                if (string.IsNullOrWhiteSpace(ruta.Destino))
                    ruta.Destino = "Sin destino";

                if (string.IsNullOrWhiteSpace(ruta.DuracionEstimada))
                    ruta.DuracionEstimada = "0 min";

                if (ruta.DistanciaKm <= 0)
                    ruta.DistanciaKm = 1;

                if (ruta.ServicioId <= 0)
                {
                    ViewBag.ServicioId = new SelectList(_context.servicios, "Id", "Nombre", ruta.ServicioId);
                    TempData["Mensaje"] = "Debes seleccionar un servicio válido.";
                    TempData["Tipo"] = "Error";
                    return View(ruta);
                }

                _context.rutas.Update(ruta);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Ruta actualizada correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al actualizar la ruta: " + ex.Message;
                TempData["Tipo"] = "Error";
                ViewBag.ServicioId = new SelectList(_context.servicios, "Id", "Nombre", ruta.ServicioId);
                return View(ruta);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmar(int id)
        {
            var ruta = await _context.rutas.FindAsync(id);
            if (ruta == null)
                return NotFound();

            _context.rutas.Remove(ruta);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Ruta eliminada correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult HorariosPorRuta(int rutaId)
        {
            var horarios = _context.horarios
                .Where(h => h.RutaId == rutaId)
                .Select(h => new {
                    id = h.Id,
                    texto = h.HoraSalida.ToShortTimeString() + " - " + h.HoraLlegada.ToShortTimeString()
                }).ToList();

            return Json(horarios);
        }

    }
}
