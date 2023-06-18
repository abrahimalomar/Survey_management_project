using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SurveyProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SurveyProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SurveyContext _context;
        public HomeController(ILogger<HomeController> logger, SurveyContext context)
        {
            _logger = logger;
            this._context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Questionnaires .ToList());
        }

       
        // GET: Questionnaires/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
           // ViewBag.ID = id;
            Response.HttpContext.Session.SetString("ID" ,id.ToString());
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
        public IActionResult Apply()
        {
            ViewBag.QuestionnaireId = 
                Guid.Parse(Response.HttpContext.Session.GetString("ID"));
            var questions = _context.Questions
    .Include(q => q.Options)
    
    .ToList();

            return View(questions);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(IFormCollection from, Dictionary<Guid, Guid> selectId)
        {
            if (ModelState.IsValid)
            {
                string selectItem = from["selecteItems"];
                string[] answer = selectItem.Split(",");

                for (int i = 0; i < answer.Length; i++)
                {
                    string answerValue = answer[i]; // Get the answer value at index i
                    Guid questionId = selectId.ElementAt(i).Key; // Get the question Id at index i
                    Guid clubId = selectId.ElementAt(i).Value; // Get the club Id at index i

                    // Create a new instance of the "answerss" model
                    UserQuestionnaire data = new UserQuestionnaire
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now,
                        UserId = HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier),
                        Questionnaireid = Guid.Parse(Response.HttpContext.Session.GetString("ID")),
                        //Questionnaireid=null,
                        QutionId = questionId,
                        answer = answerValue,

                        //Convert.ToInt32(Response.HttpContext.Session.GetString("job"));


                };
                    /*
                    var answerss = new answerss
                    {
                        Id = Guid.NewGuid(), // Generate a new Id for the answer
                        QutionId = questionId, // Set the question Id
                                               //  ClubId = clubId, // Set the club Id
                        answer = answerValue // Set the answer value
                    };
                    */

                    // Add the new instance to the database and save changes
                    _context.Add(data);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
