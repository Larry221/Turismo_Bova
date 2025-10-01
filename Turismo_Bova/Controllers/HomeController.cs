using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Turismo_Bova.Models;
using Microsoft.AspNetCore.Authorization;
using Turismo_Bova.Data;
using Microsoft.EntityFrameworkCore;

namespace Turismo_Bova.Controllers
{   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _context;

        public HomeController(ILogger<HomeController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalUsuarios = await _context.usuarios.CountAsync();
            var totalVehiculos = await _context.vehiculos.CountAsync();
            var totalServicios = await _context.servicios.CountAsync();
            var totalContratos = await _context.contratos.CountAsync();
            var totalEmpleados = await _context.empleados.CountAsync();
            var totalRutas = await _context.rutas.CountAsync();
            var totalHorarios = await _context.horarios.CountAsync();
            var totalMantenimientos = await _context.mantenimientos.CountAsync();
            var totalFacturas = await _context.facturas.CountAsync();
            var totalPedidos = await _context.pedidos.CountAsync();
            var totalProductos = await _context.productos.CountAsync();
            var totalRoles = await _context.roles.CountAsync();

            ViewData["TotalUsuarios"] = totalUsuarios;
            ViewData["TotalRoles"] = totalRoles;
            ViewData["TotalEmpleados"] = totalEmpleados;
            ViewData["TotalContratos"] = totalContratos;
            ViewData["TotalVehiculos"] = totalVehiculos;
            ViewData["TotalMantenimientos"] = totalMantenimientos;
            ViewData["TotalRutas"] = totalRutas;
            ViewData["TotalHorarios"] = totalHorarios;
            ViewData["TotalProductos"] = totalProductos;
            ViewData["TotalPedidos"] = totalPedidos;
            ViewData["TotalServicios"] = totalServicios;
            ViewData["TotalFacturas"] = totalFacturas;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
