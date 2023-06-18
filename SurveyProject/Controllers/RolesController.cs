using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurveyProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace SurveyProject.Controllers
{


    public class RolesController : Controller
    {
        private readonly SurveyContext  _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public RolesController(SurveyContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return 
                        View(await _context.AspNetRoles.ToListAsync());
                       
        }




        public async Task<IActionResult> RemoveUserFromRoles(string id)
        {

            string? role = Response.HttpContext.Session.GetString("role");
            IdentityUser ui = await userManager.FindByIdAsync(id);
            var states = await userManager.RemoveFromRoleAsync(ui, role);
            if (states.Succeeded)
            {
                return RedirectToAction("Details", "Roles", new { id = role });
            }
            else
            {
                return RedirectToAction("Details", "Roles", new { id = role });
            }

        }

        public IActionResult AddUesrToRole(string id)
        {
            string? role = Response.HttpContext.Session.GetString("role");
            ViewBag.role = role;
            List<IdentityUser> identityUsers = userManager.Users.ToList();
            //List<AspNetUser> users = _context.AspNetUsers.ToList();
            return View(identityUsers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUesrToRole(IFormCollection from)
        {
            string? role = Response.HttpContext.Session.GetString("role");
            try
            {
                
                if (role != null)
                {
                    string selectItem = from["selecteItems"];
                    string[] userIds = selectItem.Split(",");
                    foreach (var item in userIds)
                    {
                        IdentityUser User = await userManager.FindByIdAsync(item);
                        await userManager.AddToRoleAsync(User, role);
                    }
                }
                TempData["alert-Type"] = "Success";
                return RedirectToAction("Details", "Roles", new { id = role });
            }
            catch (DbUpdateException )
            {
                TempData["alert-Type"] = "Success";
            }

            return View();
        }





        // GET: Roles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            Response.HttpContext.Session.SetString("role", id);
            if (id == null || _context.AspNetRoles == null)
            {
                return NotFound();
            }
            var users = await userManager.GetUsersInRoleAsync(id);
            ViewBag.userss = users;
            var aspNetRole = await _context.AspNetRoles
                .FirstOrDefaultAsync(m => m.Name == id);
            if (aspNetRole == null)
            {
                return NotFound();
            }

            return View(aspNetRole);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NormalizedName,ConcurrencyStamp")] AspNetRoles aspNetRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aspNetRole);
        }


        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AspNetRoles == null)
            {
                return NotFound();
            }

            var aspNetRole = await _context.AspNetRoles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetRole == null)
            {
                return NotFound();
            }

            return View(aspNetRole);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AspNetRoles == null)
            {
                return Problem("Entity set 'JobsDbContext.AspNetRoles'  is null.");
            }
            var aspNetRole = await _context.AspNetRoles.FindAsync(id);
            if (aspNetRole != null)
            {
                _context.AspNetRoles.Remove(aspNetRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetRoleExists(string id)
        {
            return (_context.AspNetRoles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
