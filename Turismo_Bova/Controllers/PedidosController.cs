using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Data;
using Turismo_Bova.Filters;
using Turismo_Bova.Models;
using Turismo_Bova.ViewModels;

namespace Turismo_Bova.Controllers
{
    [Authorize(Roles = "Administrador")]
    [CargoAuthorize("Administrador")]
    public class PedidosController : Controller
    {
        private readonly AppDBContext _context;

        public PedidosController(AppDBContext appDBContext)
        {
            _context = appDBContext;
        }
        public async Task<IActionResult> Index()
        {
            var pedi = await _context.pedidos
            .Include(u => u.proveedor)
            .ToListAsync();
            return View(pedi);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Productos = _context.productos
            .Select(p => new ProductoSimple { Id = p.Id, Nombre = p.Nombre })
            .ToList();


            ViewBag.Proveedores = _context.proveedores.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Pedido pedido, List<int> productoIds, List<int> cantidades)
        {
            pedido.FechaPedido = DateOnly.FromDateTime(DateTime.Today);
            _context.pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            for (int i = 0; i < productoIds.Count; i++)
            {
                var producto = await _context.productos.FindAsync(productoIds[i]);
                if (producto != null)
                {
                    var detalle = new Pedido_Producto
                    {
                        PedidoId = pedido.Id,
                        ProductoId = producto.Id,
                        Cantidad = cantidades[i],
                        PrecioUnitario = producto.PrecioUnidad,
                        NombreProducto = producto.Nombre
                    };
                    _context.pedido_Productos.Add(detalle);
                }
            }

            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Pedido registrado exitosamente.";
            TempData["Tipo"] = "success";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Mostrar(int id)
        {
            var pedido = await _context.pedidos
                .Include(p => p.proveedor)
                .Include(p => p.pedido_producto)
                    .ThenInclude(pp => pp.producto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [HttpPost]
        public IActionResult EliminarConfirmar(int id)
        {
            var pedido = _context.pedidos
                .Include(p => p.pedido_producto)
                .FirstOrDefault(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            _context.pedidos.Remove(pedido);

            _context.SaveChanges();
            TempData["Mensaje"] = "Pedido eliminado correctamente.";
            TempData["Tipo"] = "success";

            return RedirectToAction(nameof(Index));
        }


    }
}
