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
    [CargoAuthorize("Administrador", "Inspector de Flota", "Supervisor", "Coordinador de Mantenimiento")]
    public class MantenimientosController : Controller
    {
        private readonly AppDBContext _context;

        public MantenimientosController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mantenimientos = await _context.mantenimientos
                .Include(m => m.vehiculo)
                .Include(m => m.empleado)
                .Include(m => m.proveedor)
                .ToListAsync();

            return View(mantenimientos);
        }

        public IActionResult Crear()
        {
            ViewBag.VehiculoId = new SelectList(_context.vehiculos, "Id", "Placa");
            ViewBag.EmpleadoId = new SelectList(_context.empleados, "Id", "Nombre");
            ViewBag.ProveedorId = new SelectList(_context.proveedores, "Id", "RazonSocial");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Mantenimiento mantenimiento)
        {
            try
            {
                if (mantenimiento.FechaFin < mantenimiento.FechaInicio)
                {
                    TempData["Mensaje"] = "La fecha de fin no puede ser anterior a la fecha de inicio.";
                    TempData["Tipo"] = "Error";
                    CargarCombos(mantenimiento);
                    return View(mantenimiento);
                }

                _context.mantenimientos.Add(mantenimiento);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Mantenimiento registrado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al guardar: " + ex.Message;
                TempData["Tipo"] = "Error";
                CargarCombos(mantenimiento);
                return View(mantenimiento);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var mantenimiento = await _context.mantenimientos.FindAsync(id);
            if (mantenimiento == null)
                return NotFound();

            CargarCombos(mantenimiento);
            return View(mantenimiento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Mantenimiento mantenimiento)
        {
            try
            {
                if (mantenimiento.FechaFin < mantenimiento.FechaInicio)
                {
                    TempData["Mensaje"] = "La fecha de fin no puede ser anterior a la fecha de inicio.";
                    TempData["Tipo"] = "Error";
                    CargarCombos(mantenimiento);
                    return View(mantenimiento);
                }

                _context.mantenimientos.Update(mantenimiento);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Mantenimiento actualizado correctamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al actualizar: " + ex.Message;
                TempData["Tipo"] = "Error";
                CargarCombos(mantenimiento);
                return View(mantenimiento);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmar(int id)
        {
            var mantenimiento = await _context.mantenimientos.FindAsync(id);
            if (mantenimiento == null)
                return NotFound();

            _context.mantenimientos.Remove(mantenimiento);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Mantenimiento eliminado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }

        private void CargarCombos(Mantenimiento m)
        {
            ViewBag.VehiculoId = new SelectList(_context.vehiculos, "Id", "Placa", m.VehiculoId);
            ViewBag.EmpleadoId = new SelectList(_context.empleados, "Id", "Nombres", m.EmpleadoId);
            ViewBag.ProveedorId = new SelectList(_context.proveedores, "Id", "RazonSocial", m.ProveedorId);
        }

        public async Task<IActionResult> Reporte(int id)
        {
            var mantenimiento = await _context.mantenimientos
                .Include(m => m.vehiculo)
                .Include(m => m.empleado)
                .Include(m => m.proveedor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mantenimiento == null) return NotFound();

            return View("Reporte", mantenimiento);
        }

    }
}
