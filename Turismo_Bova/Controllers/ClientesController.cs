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
    public class ClientesController : Controller
    {
        private readonly AppDBContext _context;

        public ClientesController(AppDBContext appDBContext)
        {
            _context = appDBContext;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _context.clientes
            .ToListAsync();
            return View(user);
        }

        [HttpGet]
        public IActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Cliente c)
        {
            var existeUser = await _context.clientes
                .FirstOrDefaultAsync(r => r.Ruc == c.Ruc && r.Correo == c.Correo);

            if (existeUser != null)
            {
                TempData["Mensaje"] = "El RUC o Correo ya está registrado.";
                TempData["Tipo"] = "error";
                return View(c);
            }

            var em = new Cliente
            {
                RazonSocial = c.RazonSocial,
                Ruc = c.Ruc,
                Correo = c.Correo,
                Telefono = c.Telefono,
                Direccion = c.Direccion
            };

            await _context.clientes.AddAsync(em);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "El cliente se ha creado correctamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var clie = _context.clientes.FirstOrDefault(u => u.Id == id);

            if (clie == null)
            {
                return NotFound();
            }

            return View(clie);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            // Obtener el cliente original, incluyendo el usuario
            var clieOriginal = await _context.clientes
                .Include(e => e.usuario)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (clieOriginal == null)
            {
                return NotFound();
            }

            // Actualizar campos del empleado
            clieOriginal.RazonSocial = cliente.RazonSocial;
            clieOriginal.Correo = cliente.Correo;
            clieOriginal.Telefono = cliente.Telefono;
            clieOriginal.Ruc = cliente.Ruc;
            clieOriginal.Direccion = cliente.Direccion;

            if (clieOriginal.usuario != null)
            {
                clieOriginal.usuario.Correo = cliente.Correo;
            }

            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Cliente actualizado correctamente.";
            TempData["Tipo"] = "success";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarConfirmar(int id)
        {
            var use = _context.clientes.Find(id);
            _context.clientes.Remove(use);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
