using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Data;
using Turismo_Bova.Filters;
using Turismo_Bova.Models;

namespace Turismo_Bova.Controllers
{
    [Authorize(Roles = "Administrador, Empleado")]
    [CargoAuthorize("Administrador", "Ejecutivo de Contratos", "Supervisor")]
    public class ServiciosController : Controller
    {
        private readonly AppDBContext _context;

        public ServiciosController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string buscar)
        {
            var servicios = _context.servicios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var termino = buscar.Trim().ToLower();
                servicios = servicios.Where(s =>
                    s.Nombre.ToLower().Contains(termino) ||
                    s.Descripcion.ToLower().Contains(termino));
            }

            return View(await servicios.ToListAsync());
        }

        public IActionResult Crear() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Servicio servicio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(servicio.Nombre))
                    servicio.Nombre = "Sin nombre";

                if (string.IsNullOrWhiteSpace(servicio.Descripcion))
                    servicio.Descripcion = "Sin descripción";

                if (servicio.PrecioBase <= 0)
                    servicio.PrecioBase = 1;

                _context.servicios.Add(servicio);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Servicio registrado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al guardar: {ex.Message}";
                TempData["Tipo"] = "error";
                return View(servicio);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var servicio = await _context.servicios.FindAsync(id);
            if (servicio == null)
                return NotFound();

            return View(servicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Servicio servicio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(servicio.Nombre))
                    servicio.Nombre = "Sin nombre";

                if (string.IsNullOrWhiteSpace(servicio.Descripcion))
                    servicio.Descripcion = "Sin descripción";

                if (servicio.PrecioBase <= 0)
                    servicio.PrecioBase = 1;

                _context.servicios.Update(servicio);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Servicio actualizado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al actualizar: {ex.Message}";
                TempData["Tipo"] = "error";
                return View(servicio);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmar(int id)
        {
            var servicio = await _context.servicios.FindAsync(id);
            if (servicio == null)
                return NotFound();

            _context.servicios.Remove(servicio);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Servicio eliminado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }
    }
}
