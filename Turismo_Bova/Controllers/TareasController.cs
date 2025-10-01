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
    [CargoAuthorize("Administrador", "Coordinador de Mantenimiento", "Supervisor", "Mecanico")]
    public class TareasController : Controller
    {
        private readonly AppDBContext _context;

        public TareasController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            /*var tareas = await _context.tareas
                .Include(t => t.empleado)
                .Include(t => t.mantenimiento)
                .ToListAsync();

            return View(tareas);*/

            var cargoUsuario = User.Claims.FirstOrDefault(c => c.Type == "Cargo")?.Value;

            IQueryable<Tarea> query = _context.tareas
                .Include(t => t.empleado)
                .Include(t => t.mantenimiento);

            if (cargoUsuario == "Mecanico")
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                if (int.TryParse(userIdClaim, out int userId))
                {
                    var usuario = await _context.usuarios
                        .Include(u => u.Empleado)
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    if (usuario?.Empleado != null)
                    {
                        int empleadoId = usuario.Empleado.Id;

                        query = query.Where(t => t.EmpleadoId == empleadoId);
                    }
                    else
                    {
                        query = query.Where(t => false); // No mostrar nada si no hay empleado asociado
                    }
                }
                else
                {
                    query = query.Where(t => false); // No mostrar nada si no hay claim válido
                }
            }

            var tareas = await query.ToListAsync();
            return View(tareas);
        }

        public IActionResult Crear()
        {
            CargarCombos(new Tarea());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Tarea tarea)
        {
            try
            {
                _context.tareas.Add(tarea);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Tarea registrada correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al registrar la tarea: " + ex.Message;
                TempData["Tipo"] = "error";
                CargarCombos(tarea);
                return View(tarea);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var tarea = await _context.tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            CargarCombos(tarea);
            return View(tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Tarea tarea)
        {
            try
            {
                _context.tareas.Update(tarea);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Tarea actualizada correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al actualizar la tarea: " + ex.Message;
                TempData["Tipo"] = "error";
                CargarCombos(tarea);
                return View(tarea);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmar(int id)
        {
            var tarea = await _context.tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            _context.tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Tarea eliminada correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

        private void CargarCombos(Tarea t)
        {
            ViewBag.EmpleadoId = new SelectList(_context.empleados, "Id", "Nombre", t?.EmpleadoId);
            ViewBag.MantenimientoId = new SelectList(_context.mantenimientos, "Id", "TipoMantenimiento", t?.MantenimientoId);
        }

        public async Task<IActionResult> Reporte(int id)
        {
            var tarea = await _context.tareas
                .Include(t => t.empleado)
                .Include(t => t.mantenimiento)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tarea == null) return NotFound();

            return View("Reporte", tarea);
        }
    }
}
