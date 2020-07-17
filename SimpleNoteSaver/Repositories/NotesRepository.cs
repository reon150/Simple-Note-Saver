using Microsoft.EntityFrameworkCore;
using SimpleNoteSaver.Data;
using SimpleNoteSaver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleNoteSaver.Repositories
{
    public class NotesRepository : INotesRepository
    {

        private readonly ApplicationDbContext _context;

        public NotesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateNote(Notes note)
        {
            _context.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNote(Notes note)
        {

            try
            {
                _context.Note.Remove(note);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                e.ToString();
            }

        }

        public async Task<List<Notes>> GetNotes(string userId)
        {

            try
            {
                var notes = _context.Note.Where(n => n.UserId == userId);
                return await notes.ToListAsync();
            }
            catch (Exception)
            {
                return new List<Notes>();
            }

        }

        public async Task<Notes> GetOneNote(int noteId)
        {
            try
            {
                var notes = await _context.Note
                    .Include(n => n.User)
                    .FirstOrDefaultAsync(m => m.Id == noteId);
                return notes;
            }
            catch (Exception)
            {
                return new Notes();
            }

        }

        public bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }

    }
}
