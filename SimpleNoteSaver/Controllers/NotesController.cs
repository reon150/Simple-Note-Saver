using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleNoteSaver.Data;
using SimpleNoteSaver.Models;

namespace SimpleNoteSaver.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext context;

        public NotesController(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task<IActionResult> Index()
        {
            var applicationDbContext = context.Note.Where(n => n.UserId == getCurrentUserId());
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await context.Note
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["UserId"] = getCurrentUserId();
            ViewData["Mode"] = "create";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,UserId")] Notes note)
        {
            if (!validateUserId(note.UserId)) return Unauthorized();

            if (ModelState.IsValid)
            {
                context.Add(note);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(note);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = getCurrentUserId();
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

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(note);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
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
            ViewData["UserId"] = getCurrentUserId();
            return View(note);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await context.Note
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var note = await context.Note.FindAsync(id);
            context.Note.Remove(note);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return context.Note.Any(e => e.Id == id);
        }

        private string getCurrentUserId()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserId = currentUserID;
            return UserId;
        }

        private bool validateUserId(string userId)
        {
            if (userId == getCurrentUserId()) return true;
            else return false;
        }
    }
}
