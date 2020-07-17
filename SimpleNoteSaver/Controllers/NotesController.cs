using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleNoteSaver.Data;
using SimpleNoteSaver.Models;
using SimpleNoteSaver.Repositories;

namespace SimpleNoteSaver.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsersRepository _userServices;
        private readonly INotesRepository _noteServices;

        public NotesController(ApplicationDbContext context, IUsersRepository userServices, INotesRepository noteServices)
        {
            _context = context;
            _userServices = userServices;
            _noteServices = noteServices;
        }


        public async Task<IActionResult> Index()
        {
            var currentUser = await _userServices.GetCurrentUser(this.User);
            var notes = await _noteServices.GetNotes(currentUser.Id);
            return View(notes);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _noteServices.GetOneNote(id ?? default);

            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            var currentUser = await _userServices.GetCurrentUser(this.User);
            ViewData["UserId"] = currentUser.Id;
            ViewData["Mode"] = "create";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,UserId")] Notes note)
        {
            if (!(await _userServices.ValidateUserId(note.UserId, this.User)) ) return Unauthorized();

            if (ModelState.IsValid)
            {
                await _noteServices.CreateNote(note);
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                return View(note);
            }
            
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            var currentUser = await _userServices.GetCurrentUser(this.User);
            ViewData["UserId"] = currentUser.Id;
            ViewData["Mode"] = "edit";
            return View("Create", note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,UserId")] Notes note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (!(await _userServices.ValidateUserId(note.UserId, this.User))) return Unauthorized();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_noteServices.NoteExists(note.Id) == false)
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
            var currentUser = await _userServices.GetCurrentUser(this.User);
            ViewData["UserId"] = currentUser.Id;
            return View(note);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _noteServices.GetOneNote(id ?? default);

            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var note = await _context.Note.FindAsync(id);

            if (await _userServices.ValidateUserId(note.UserId, this.User)) 
            {
                await _noteServices.DeleteNote(note);
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                return Unauthorized();
            }
            
        }

    }
}
