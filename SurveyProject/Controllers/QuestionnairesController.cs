using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurveyProject.Models;

namespace SurveyProject.Controllers
{
    [Authorize]
    public class QuestionnairesController : Controller
    {
        private readonly SurveyContext _context;
        private readonly IHostingEnvironment webHost;
     
        public QuestionnairesController(SurveyContext context, IHostingEnvironment webHost)
        {
            _context = context;
          this.webHost = webHost;
        }

        public IActionResult Data()
        {
            return View();
        }
        // GET: Questionnaires
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var surveyContext = _context.Questionnaires.Include(q => q.User).Where(q=>q.User.Id==userId);
            return View(await surveyContext.ToListAsync());
        }

        // GET: Questionnaires/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionnaires = await _context.Questionnaires
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionnaires == null)
            {
                return NotFound();
            }

            return View(questionnaires);
        }

        // GET: Questionnaires/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Questionnaires/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titel,Description,ImgUrl,UserId")] Questionnaires questionnaires)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    if (questionnaires.File != null)
                    {
                        string uploads = Path.Combine(webHost.WebRootPath, "img/QuestionnairesImg");
                        string fullPath = Path.Combine(uploads, questionnaires.File.FileName);
                        questionnaires.File.CopyTo(new FileStream(fullPath, FileMode.Create));

                        // set the file path to an object property

                        questionnaires.ImgUrl = fullPath;
                    }

                    /*
                    var uploadDirecotory = "QuestionnairesImg/";
                    var uploadPath = Path.Combine(webHost.WebRootPath, uploadDirecotory);
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                   var fileName = Guid.NewGuid() + Path.GetExtension(questionnaires.File.FileName);
                    var filePath = Path.Combine(uploadPath,fileName);
                    using (var strem = System.IO.File.Create(filePath))
                    {
                        questionnaires.File.CopyTo(strem);
                    }
                    questionnaires.ImgUrl = "QuestionnairesImg/"+fileName;

                    */
                    questionnaires.Id = Guid.NewGuid();
                    questionnaires.UserId = HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                   
                    _context.Add(questionnaires);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }catch(Exception ex)
            {
                ViewBag.exceptions = ex;
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", questionnaires.UserId);
            return View(questionnaires);
        }

        // GET: Questionnaires/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionnaires = await _context.Questionnaires.FindAsync(id);
            if (questionnaires == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", questionnaires.UserId);
            return View(questionnaires);
        }

        // POST: Questionnaires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Titel,Description,ImgUrl,UserId")] Questionnaires questionnaires)
        {
            if (id != questionnaires.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionnaires);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionnairesExists(questionnaires.Id))
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
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", questionnaires.UserId);
            return View(questionnaires);
        }

        // GET: Questionnaires/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionnaires = await _context.Questionnaires
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionnaires == null)
            {
                return NotFound();
            }

            return View(questionnaires);
        }

        // POST: Questionnaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var questionnaires = await _context.Questionnaires.FindAsync(id);
            _context.Questionnaires.Remove(questionnaires);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionnairesExists(Guid id)
        {
            return _context.Questionnaires.Any(e => e.Id == id);
        }
    }
}
