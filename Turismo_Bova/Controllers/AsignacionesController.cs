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
    [CargoAuthorize("Coordinador de Flota", "Coordinador de Conductores", "Supervisor", "Conductor")]
    public class AsignacionesController : Controller
    {
        private readonly AppDBContext _context;

        public AsignacionesController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            /*var asignaciones = await _context.asignacion_Ruta_Vehiculo_Horarios
                .Include(a => a.vehiculo)
                .Include(a => a.ruta)
                .Include(a => a.horario)
                .Include(a => a.empleado)
                .ToListAsync();

            return View(asignaciones);*/

            var cargoUsuario = User.Claims.FirstOrDefault(c => c.Type == "Cargo")?.Value;

            IQueryable<Asignacion_Ruta_Vehiculo_Horario> query = _context.asignacion_Ruta_Vehiculo_Horarios
                .Include(a => a.vehiculo)
                .Include(a => a.ruta)
                .Include(a => a.horario)
                .Include(a => a.empleado);

            if (cargoUsuario == "Conductor")
            {
                // Obtener el UserId del claim
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                if (int.TryParse(userIdClaim, out int userId))
                {
                    // Buscar el empleado relacionado al usuario logueado
                    var usuario = await _context.usuarios
                        .Include(u => u.Empleado)
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    if (usuario?.Empleado != null)
                    {
                        int empleadoId = usuario.Empleado.Id;

                        // Filtrar asignaciones solo del conductor logueado
                        query = query.Where(a => a.EmpleadoId == empleadoId);
                    }
                    else
                    {
                        // Si no se encuentra empleado, mostrar vacío o retornar error
                        query = query.Where(a => false);
                    }
                }
                else
                {
                    // Si el UserId no es válido, mostrar vacío
                    query = query.Where(a => false);
                }
            }

            var asignaciones = await query.ToListAsync();
            return View(asignaciones);
        }

        public IActionResult Crear()
        {
            CargarCombos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Asignacion_Ruta_Vehiculo_Horario a)
        {
            try
            {
                _context.asignacion_Ruta_Vehiculo_Horarios.Add(a);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Asignación registrada correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al registrar: " + ex.Message;
                CargarCombos();
                return View(a);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var a = await _context.asignacion_Ruta_Vehiculo_Horarios.FindAsync(id);
            if (a == null) return NotFound();

            CargarCombos();
            return View(a);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Asignacion_Ruta_Vehiculo_Horario a)
        {
            try
            {
                _context.asignacion_Ruta_Vehiculo_Horarios.Update(a);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Asignación actualizada correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar: " + ex.Message;
                CargarCombos();
                return View(a);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmar(int id)
        {
            var a = await _context.asignacion_Ruta_Vehiculo_Horarios.FindAsync(id);
            if (a == null) return NotFound();

            _context.asignacion_Ruta_Vehiculo_Horarios.Remove(a);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Asignación eliminada correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

        private void CargarCombos()
        {
            ViewBag.VehiculoId = new SelectList(_context.vehiculos, "Id", "Placa");
            ViewBag.RutaId = new SelectList(_context.rutas, "Id", "Destino");
            ViewBag.HorarioId = new SelectList(_context.horarios, "Id", "HoraSalida");
            ViewBag.EmpleadoId = new SelectList(_context.empleados, "Id", "Nombre");
        }
    }
}
