using Mairala.Areas.Admin.ViewModels;
using Mairala.DAL;
using Mairala.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mairala.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public ServiceController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVm serviceVm)
        {
            if (!ModelState.IsValid)
            {
                return View(serviceVm);
            }

            Service service = new Service
            {
                Name = serviceVm.Name,
                Description = serviceVm.Description,
                Icon = serviceVm.Icon
            };

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service is null) return NotFound();

            UpdateServiceVm serviceVm = new UpdateServiceVm
            {
                Name = service.Name,
                Description= service.Description,
                Icon= service.Icon
            };
            return View(serviceVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateServiceVm serviceVm)
        {
            if (!ModelState.IsValid)
            {
                return View(serviceVm);
            }
            var existed = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            existed.Name = serviceVm.Name;
            existed.Description = serviceVm.Description;
            existed.Icon = serviceVm.Icon;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service is null) return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) return BadRequest();
            var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service is null) return NotFound();

            return View(service);
        }
    }
}
