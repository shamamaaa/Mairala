using Mairala.Areas.Admin.ViewModels;
using Mairala.DAL;
using Mairala.Models;
using Mairala.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mairala.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public ProjectController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects.ToListAsync();
            return View(projects);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectVm projectVm)
        {
            if (!ModelState.IsValid)
            {
                return View(projectVm);
            }

            if (projectVm.Photo.CheckType())
            {
                ModelState.AddModelError("Photo", "Photo type is not valid");
                return View(projectVm);
            }
            if (projectVm.Photo.CheckSize())
            {
                ModelState.AddModelError("Photo", "Photo size is not valid");
                return View(projectVm);
            }
            string filename = await projectVm.Photo.CreateFile(_env.WebRootPath, "assets", "images","project");
            Project project = new Project
            {
                Title = projectVm.Title,
                Subtitle = projectVm.Subtitle,
                ImageUrl = filename
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (project is null) return NotFound();

            UpdateProjectVm projectVm = new UpdateProjectVm
            {
                Title = project.Title,
                Subtitle=project.Subtitle,
                ImageUrl = project.ImageUrl
            };
            return View(projectVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProjectVm projectVm)
        {
            if (!ModelState.IsValid)
            {
                return View(projectVm);
            }
            var existed = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            if (projectVm.Photo is not null)
            {
                if (projectVm.Photo.CheckType())
                {
                    ModelState.AddModelError("Photo", "Photo type is not valid");
                    return View(projectVm);
                }
                if (projectVm.Photo.CheckSize())
                {
                    ModelState.AddModelError("Photo", "Photo size is not valid");
                    return View(projectVm);
                }
                string newimage = await projectVm.Photo.CreateFile(_env.WebRootPath, "assets", "images","project");
                existed.ImageUrl.DeletFile(_env.WebRootPath, "assets", "images","project");
                existed.ImageUrl = newimage;
            }
            existed.Title = projectVm.Title;
            existed.Subtitle = projectVm.Subtitle;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (project is null) return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) return BadRequest();
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (project is null) return NotFound();

            return View(project);
        }

    }
}
