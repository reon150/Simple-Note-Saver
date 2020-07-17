using SimpleNoteSaver.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleNoteSaver.Repositories
{
    public interface INotesRepository
    {
        Task<List<Notes>> GetNotes(string userId);
        Task<Notes> GetOneNote(int noteId);
        Task CreateNote(Notes note);
        Task DeleteNote(Notes note);
        bool NoteExists(int id);
    }
}
