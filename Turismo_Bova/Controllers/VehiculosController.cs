using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Data;
using Turismo_Bova.Filters;
using Turismo_Bova.Models;

namespace Turismo_Bova.Controllers
{
    [Authorize(Roles = "Administrador, Empleado")]
    [CargoAuthorize("Administrador", "Inspector de Flota", "Supervisor")]
    public class VehiculosController : Controller
    {
        private readonly AppDBContext _context;

        public VehiculosController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string buscar)
        {
            var vehiculos = _context.vehiculos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var termino = buscar.Trim().ToLower();

                vehiculos = vehiculos.Where(v =>
                    v.Marca.ToLower().Contains(termino) ||
                    v.Modelo.ToLower().Contains(termino) ||
                    v.Placa.ToLower().Contains(termino) ||
                    v.Estado.ToLower().Contains(termino) ||
                    v.FechaAdquisicion.Year.ToString().Contains(termino));
            }

            return View(await vehiculos.ToListAsync());
        }


        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Vehiculo form)
        {
            var vehiculo = new Vehiculo
            {
                Marca = form.Marca,
                Modelo = form.Modelo,
                Placa = form.Placa,
                Capacidad = form.Capacidad,
                Estado = form.Estado,
                FechaAdquisicion = form.FechaAdquisicion
            };

            await _context.vehiculos.AddAsync(vehiculo);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Vehículo registrado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var vehiculo = await _context.vehiculos.FindAsync(id);
            if (vehiculo == null)
                return NotFound();

            return View(vehiculo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Vehiculo form)
        {
            var original = await _context.vehiculos.FindAsync(id);
            if (original == null)
                return NotFound();

            original.Marca = form.Marca;
            original.Modelo = form.Modelo;
            original.Placa = form.Placa;
            original.Estado = form.Estado;
            original.Capacidad = form.Capacidad;
            original.FechaAdquisicion = form.FechaAdquisicion;

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Vehículo actualizado correctamente.";
            TempData["Tipo"] = "success";

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> EliminarConfirmar(int id)
        {
            var vehiculo = await _context.vehiculos.FindAsync(id);
            if (vehiculo == null)
                return NotFound();

            _context.vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Vehículo eliminado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction(nameof(Index));
        }
    }
}
