using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Data;
using Turismo_Bova.Filters;
using Turismo_Bova.Models;

namespace Turismo_Bova.Controllers
{
    [Authorize(Roles = "Administrador")]
    [CargoAuthorize("Administrador")]
    public class EmpleadoController : Controller
    {
        private readonly AppDBContext _context;

        public EmpleadoController(AppDBContext appDBContext)
        {
            _context = appDBContext;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _context.empleados
            .Include(u => u.usuario)
            .ToListAsync();
            return View(user);
        }

        [HttpGet]
        public IActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Empleado e)
        {
            var existeUser = await _context.empleados
                .FirstOrDefaultAsync(r => r.Dni == e.Dni && r.Correo == e.Correo);

            if (existeUser != null)
            {
                TempData["Mensaje"] = "El DNI o Correo ya está registrado.";
                TempData["Tipo"] = "error";
                return View(e);
            }

            var em = new Empleado
            {
                Nombre = e.Nombre,
                Dni = e.Dni,
                Correo = e.Correo,
                Telefono = e.Telefono,
                Cargo = e.Cargo
            };

            await _context.empleados.AddAsync(em);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "El empleado se ha creado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var em = _context.empleados.FirstOrDefault(u => u.Id == id);

            if (em == null)
            {
                return NotFound();
            }

            return View(em);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            // Obtener el empleado original, incluyendo el usuario
            var emplOriginal = await _context.empleados
                .Include(e => e.usuario)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (emplOriginal == null)
            {
                return NotFound();
            }

            // Actualizar campos del empleado
            emplOriginal.Nombre = empleado.Nombre;
            emplOriginal.Correo = empleado.Correo;
            emplOriginal.Telefono = empleado.Telefono;
            emplOriginal.Dni = empleado.Dni;
            emplOriginal.Cargo = empleado.Cargo;

            if (emplOriginal.usuario != null)
            {
                emplOriginal.usuario.Correo = empleado.Correo;
            }

            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Empleado actualizado correctamente.";
            TempData["Tipo"] = "success";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarConfirmar(int id)
        {
            var use = _context.empleados.Find(id);
            _context.empleados.Remove(use);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
