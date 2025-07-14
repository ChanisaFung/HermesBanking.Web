using HermesBanking.Domain.Entidades;
using HermesBanking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HermesBanking.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CuentaBancariaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CuentaBancariaController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var cuentas = await _context.CuentasBancarias.ToListAsync();
            return View(cuentas);
        }

        public IActionResult Create()
        {
            ViewData["Usuarios"] = _userManager.Users
            .Select(u => new SelectListItem
            {
           Value = u.Id,
           Text = u.Email
            })
              .ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CuentaBancaria cuenta)
        {
            if (ModelState.IsValid)
            {
                cuenta.Id = Guid.NewGuid();
                cuenta.FechaCreacion = DateTime.UtcNow;
                _context.CuentasBancarias.Add(cuenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(cuenta);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var cuenta = await _context.CuentasBancarias.FindAsync(id);
            if (cuenta == null) return NotFound();

            ViewData["Usuarios"] = _userManager.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Email
                }).ToList();

            return View(cuenta);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CuentaBancaria cuenta)
        {
            if (ModelState.IsValid)
            {
                _context.CuentasBancarias.Update(cuenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(cuenta);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var cuenta = await _context.CuentasBancarias.FindAsync(id);
            if (cuenta == null) return NotFound();

            return View(cuenta);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cuenta = await _context.CuentasBancarias.FindAsync(id);
            if (cuenta != null)
            {
                _context.CuentasBancarias.Remove(cuenta);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
