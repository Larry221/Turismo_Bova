using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Turismo_Bova.Filters
{
    public class CargoAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _cargosPermitidos;

        public CargoAuthorizeAttribute(params string[] cargosPermitidos)
        {
            _cargosPermitidos = cargosPermitidos;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
                return;
            }

            var rolUsuario = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
            var cargoUsuario = user.Claims.FirstOrDefault(c => c.Type == "Cargo")?.Value ?? "";

            if (rolUsuario == "Administrador")
            {
                // El Administrador siempre tiene acceso
                return;
            }

            if (rolUsuario == "Empleado" && _cargosPermitidos.Contains(cargoUsuario, StringComparer.OrdinalIgnoreCase))
            {
                // El empleado tiene un cargo permitido
                return;
            }

            context.Result = new RedirectToActionResult("AccesoDenegado", "Login", null);
        }
    }
}