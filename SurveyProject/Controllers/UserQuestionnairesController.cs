using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
  
    public class UserQuestionnaireController : Controller
    {
        private readonly SurveyContext _context;

        public UserQuestionnaireController(SurveyContext context)
        {
            _context = context;
        }
        public IActionResult Test()
        {
            return View();
        }
        // GET: UserQuestionnaires
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var surveyContext = _context.UserQuestionnaire.Include(u => u.Questionnaire).Include(u => u.User).Include(u =>u.Questions).Where(q=>q.User.Id== userId);
            return View(await surveyContext.ToListAsync());
        }

        public async Task<IActionResult> ViewResponses(Guid id)
        {
            Response.HttpContext.Session.SetString("IDdelete", id.ToString());
  
              var surveyContext = _context.UserQuestionnaire.Include(u =>  u.Questionnaire).Include(u => u.User).Include(u=>u.Questions) .Where(q=>q.Questionnaireid==id);
               return View(await surveyContext.ToListAsync()); 
        }

        // GET: UserQuestionnaires/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userQuestionnaire = await _context.UserQuestionnaire
                .Include(u => u.Questionnaire)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userQuestionnaire == null)
            {
                return NotFound();
            }

            return View(userQuestionnaire);
        }

        // GET: UserQuestionnaires/Create
        public IActionResult Create()
        {
            ViewData["Questionnaireid"] = new SelectList(_context.Questionnaires, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: UserQuestionnaires/Create
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,answer,UserId,Questionnaireid,QutionId")] UserQuestionnaire userQuestionnaire)
        {
            if (ModelState.IsValid)
            {
                userQuestionnaire.Id = Guid.NewGuid();
                _context.Add(userQuestionnaire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Questionnaireid"] = new SelectList(_context.Questionnaires, "Id", "Id", userQuestionnaire.Questionnaireid);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", userQuestionnaire.UserId);
            return View(userQuestionnaire);
        }

        // GET: UserQuestionnaires/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userQuestionnaire = await _context.UserQuestionnaire.FindAsync(id);
            if (userQuestionnaire == null)
            {
                return NotFound();
            }
            ViewData["Questionnaireid"] = new SelectList(_context.Questionnaires, "Id", "Id", userQuestionnaire.Questionnaireid);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", userQuestionnaire.UserId);
            return View(userQuestionnaire);
        }

        // POST: UserQuestionnaires/Edit/5
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Date,answer,UserId,Questionnaireid,QutionId")] UserQuestionnaire userQuestionnaire)
        {
            if (id != userQuestionnaire.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userQuestionnaire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserQuestionnaireExists(userQuestionnaire.Id))
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
            ViewData["Questionnaireid"] = new SelectList(_context.Questionnaires, "Id", "Id", userQuestionnaire.Questionnaireid);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", userQuestionnaire.UserId);
            return View(userQuestionnaire);
        }

        // GET: UserQuestionnaires/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userQuestionnaire = await _context.UserQuestionnaire
                .Include(u => u.Questionnaire)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userQuestionnaire == null)
            {
                return NotFound();
            }

            return View(userQuestionnaire);
        }

        // POST: UserQuestionnaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var Questionnaireid = Guid.Parse(Response.HttpContext.Session.GetString("IDdelete"));
            var userQuestionnaire = await _context.UserQuestionnaire.FindAsync(id);
            _context.UserQuestionnaire.Remove(userQuestionnaire);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewResponses", "UserQuestionnaire", new { id = Questionnaireid });
           // return RedirectToAction("Index", "Options", new { id = OptionsId });
        }

        private bool UserQuestionnaireExists(Guid id)
        {
            return _context.UserQuestionnaire.Any(e => e.Id == id);
        }
    }
}
