using HermesBanking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HermesBanking.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var totalUsuarios = _userManager.Users.Count();
            var totalCuentas = _context.CuentasBancarias.Count();
            var totalTarjetas = _context.TarjetasCredito.Count();

            ViewData["TotalUsuarios"] = totalUsuarios;
            ViewData["TotalCuentas"] = totalCuentas;
            ViewData["TotalTarjetas"] = totalTarjetas;
            ViewData["TotalPrestamos"] = 0;


            return View();
        }

    }
}

