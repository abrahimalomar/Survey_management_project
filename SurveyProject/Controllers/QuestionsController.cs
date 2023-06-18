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
    public class QuestionsController : Controller
    {
        private readonly SurveyContext _context;

        public QuestionsController(SurveyContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index(Guid id)
        {
            // var userId = HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
           

            var surveyContext = _context.Questions.Include(q => q.Questionnaire).Where(q=>q.Id==id);
          
                List<Options> _options= _context.Options.Where(q => q.QutionId == id).ToList();
            ViewBag.options = _options;
            return View(await surveyContext.ToListAsync());
        }


        public async Task<IActionResult> ViewQuestions(Guid id)
        {
            Response.HttpContext.Session.SetString("IDdelete", id.ToString());
            var surveyContext = _context.Questions.Include(q => q.Questionnaire).Where(q=>q.QuestionnaireId==id);
            return View(await surveyContext.ToListAsync());
        }
        // GET: Questions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions
                .Include(q => q.Questionnaire)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questions == null)
            {
                return NotFound();
            }

            return View(questions);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewData["QuestionnaireId"] = new SelectList(_context.Questionnaires, "Id", "Id");
            return View();
        }

        // POST: Questions/Create
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionTitile,Type,IsRequired,QuestionnaireId")] Questions questions,Guid id)
        {
            Response.HttpContext.Session.SetString("ID", id.ToString());
            var QuestionId = Guid.Parse(Response.HttpContext.Session.GetString("ID"));
            if (ModelState.IsValid)
            {
                questions.Id = Guid.NewGuid();
                questions.QuestionnaireId = id;
                _context.Add(questions);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ViewQuestions", "Questions", new { id = QuestionId });
            }
            ViewData["QuestionnaireId"] = new SelectList(_context.Questionnaires, "Id", "Id", questions.QuestionnaireId);
            return View(questions);
        }

        private IActionResult ViewQuestions()
        {
            throw new NotImplementedException();
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions.FindAsync(id);
            if (questions == null)
            {
                return NotFound();
            }
            ViewData["QuestionnaireId"] = new SelectList(_context.Questionnaires, "Id", "Id", questions.QuestionnaireId);
            return View(questions);
        }

        // POST: Questions/Edit/5
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,QuestionTitile,Type,IsRequired,QuestionnaireId")] Questions questions)
        {
            if (id != questions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionsExists(questions.Id))
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
            ViewData["QuestionnaireId"] = new SelectList(_context.Questionnaires, "Id", "Id", questions.QuestionnaireId);
            return View(questions);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions
                .Include(q => q.Questionnaire)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questions == null)
            {
                return NotFound();
            }

            return View(questions);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            
            var QuestionId = Guid.Parse(Response.HttpContext.Session.GetString("IDdelete"));

            var questions = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(questions);
            await _context.SaveChangesAsync();
            // return RedirectToAction(nameof(Index));
            return RedirectToAction("ViewQuestions", "Questions", new { id = QuestionId });
        }

        private bool QuestionsExists(Guid id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
