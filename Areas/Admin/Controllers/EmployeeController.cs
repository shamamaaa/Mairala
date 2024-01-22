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

    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Include(x => x.Position).ToListAsync();
            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            CreateEmployeeVm employeeVm = new CreateEmployeeVm();
            GetList(ref employeeVm);
            return View(employeeVm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVm employeeVm)
        {
            if (!ModelState.IsValid)
            {
                GetList(ref employeeVm);
                return View(employeeVm);
            }

            bool result =  _context.Positions.Any(x=>x.Id==employeeVm.PositionId);
            if (!result)
            {
                ModelState.AddModelError("PositionId", "Position not found");
                GetList(ref employeeVm);
                return View(employeeVm);
            }
            if (employeeVm.Photo.CheckType())
            {
                ModelState.AddModelError("Photo", "Photo type is not valid");
                GetList(ref employeeVm);
                return View(employeeVm);
            }
            if (employeeVm.Photo.CheckSize())
            {
                ModelState.AddModelError("Photo", "Photo size is not valid");
                GetList(ref employeeVm);
                return View(employeeVm);
            }
            string filename = await employeeVm.Photo.CreateFile(_env.WebRootPath,"assets","images");
            Employee employee = new Employee
            {
                Name = employeeVm.Name,
                Surname = employeeVm.Surname,
                TwLink = employeeVm.TwLink,
                FbLink = employeeVm.FbLink,
                LinLink = employeeVm.LinLink,
                PositionId = employeeVm.PositionId,
                ImageUrl = filename
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee is null) return NotFound();

            UpdateEmployeeVm employeeVm = new UpdateEmployeeVm
            {
                Name = employee.Name,
                Surname = employee.Surname,
                TwLink = employee.TwLink,
                FbLink = employee.FbLink,
                LinLink = employee.LinLink,
                PositionId = employee.PositionId,
                ImageUrl = employee.ImageUrl
            };
            return View(employeeVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateEmployeeVm employeeVm)
        {
            if (!ModelState.IsValid)
            {
                GetList(ref employeeVm);
                return View(employeeVm);
            }
            Employee existed = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null) return NotFound();

            bool result = _context.Positions.Any(x => x.Id == employeeVm.PositionId);
            if (!result)
            {
                ModelState.AddModelError("PositionId", "Position not found");
                GetList(ref employeeVm);
                return View(employeeVm);
            }
            if (employeeVm.Photo is not null)
            {
                if (employeeVm.Photo.CheckType())
                {
                    ModelState.AddModelError("Photo", "Photo type is not valid");
                    GetList(ref employeeVm);
                    return View(employeeVm);
                }
                if (employeeVm.Photo.CheckSize())
                {
                    ModelState.AddModelError("Photo", "Photo size is not valid");
                    GetList(ref employeeVm);
                    return View(employeeVm);
                }
                string newimage = await employeeVm.Photo.CreateFile(_env.WebRootPath, "assets", "images");
                existed.ImageUrl.DeletFile(_env.WebRootPath, "assets", "images");
                existed.ImageUrl= newimage;
            }
            existed.Name = employeeVm.Name;
            existed.Surname= employeeVm.Surname;
            existed.TwLink = employeeVm.TwLink;
            existed.FbLink = employeeVm.FbLink;
            existed.LinLink = employeeVm.LinLink;
            existed.PositionId= employeeVm.PositionId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee is null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) return BadRequest();
            var employee = await _context.Employees.Include(x=>x.Position).FirstOrDefaultAsync(x=>x.Id==id);
            if (employee is null) return NotFound();

            return View(employee);
        }

        private void GetList(ref CreateEmployeeVm vm)
        {
            vm.Positions = new(_context.Positions, "Id", "Name");
        }
        private void GetList(ref UpdateEmployeeVm vm)
        {
            vm.Positions = new(_context.Positions, "Id", "Name");
        }
    }
}
