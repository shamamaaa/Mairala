using Mairala.DAL;
using Mairala.Models;
using Mairala.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Mairala.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Include(x => x.Position).ToListAsync();
            var services = await _context.Services.ToListAsync();
            var projects = await _context.Projects.ToListAsync();
            HomeVm homeVm = new HomeVm
            {
                Employees = employees,
                Services = services,
                Projects = projects
            };

            return View(homeVm);
        }
    }
}