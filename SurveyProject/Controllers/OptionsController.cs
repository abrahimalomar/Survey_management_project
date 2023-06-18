using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurveyProject.Models;

namespace SurveyProject.Controllers
{
    [Authorize]
    public class OptionsController : Controller
    {
        private readonly SurveyContext _context;

        public OptionsController(SurveyContext context)
        {
            _context = context;
        }

        // GET: Options
        public async Task<IActionResult> Index(Guid id)
        {
            Response.HttpContext.Session.SetString("IdDelete", id.ToString());
            var surveyContext = _context.Options.Include(o => o.Qution).Where(q=>q.QutionId==id);
            return View(await surveyContext.ToListAsync());
        }

        // GET: Options/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var options = await _context.Options
                .Include(o => o.Qution)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (options == null)
            {
                return NotFound();
            }

            return View(options);
        }

        // GET: Options/Create
        public IActionResult Create()
        {
            ViewData["QutionId"] = new SelectList(_context.Questions, "Id", "Id");
            return View();
        }

        // POST: Options/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Options options, Guid? id)
        {
            Response.HttpContext.Session.SetString("IDd", id.ToString());
            var QutionId = Guid.Parse(Response.HttpContext.Session.GetString("IDd"));
            /// var QutionId = Guid.Parse(Response.HttpContext.Session.GetString("IdDelete"));
            if (ModelState.IsValid)
            {
                options.Id = Guid.NewGuid();
                options.QutionId = id;
                _context.Add(options);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Questions", new { id = QutionId });
            }
            ViewData["QutionId"] = new SelectList(_context.Questions, "Id", "Id", options.QutionId);
            return View(options);
        }

        // GET: Options/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var options = await _context.Options.FindAsync(id);
            if (options == null)
            {
                return NotFound();
            }
            ViewData["QutionId"] = new SelectList(_context.Questions, "Id", "Id", options.QutionId);
            return View(options);
        }

        // POST: Options/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Options1,Type,QutionId")] Options options)
        {
            if (id != options.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(options);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OptionsExists(options.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["QutionId"] = new SelectList(_context.Questions, "Id", "Id", options.QutionId);
            return View(options);
        }

        // GET: Options/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var options = await _context.Options
                .Include(o => o.Qution)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (options == null)
            {
                return NotFound();
            }

            return View(options);
        }

        // POST: Options/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
      
            var OptionsId = Guid.Parse(Response.HttpContext.Session.GetString("IdDelete"));

            var options = await _context.Options.FindAsync(id);
            _context.Options.Remove(options);
            await _context.SaveChangesAsync();
            //     return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Options", new { id = OptionsId });
        }

        private bool OptionsExists(Guid id)
        {
            return _context.Options.Any(e => e.Id == id);
        }
    }
}
