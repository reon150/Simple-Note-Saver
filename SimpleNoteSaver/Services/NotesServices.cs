using Microsoft.AspNetCore.Mvc;
using SimpleNoteSaver.Data;
using SimpleNoteSaver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleNoteSaver.Services
{
    public class NotesServices
    {

        private readonly ApplicationDbContext _context;

        public NotesServices(ApplicationDbContext context)
        {
            _context = context;
        }
        /*public async Task<List<Notes>> GetNotes()
        {
            var applicationDbContext = _context.Note.Where(n => n.UserId == getCurrentUserId());
            return await applicationDbContext.ToListAsync();
        }*/


    }
}
