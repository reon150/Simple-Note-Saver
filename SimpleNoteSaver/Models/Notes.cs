﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SimpleNoteSaver.Models
{
    public class Notes
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        internal IQueryable<Notes> ToListAsync()
        {
            throw new NotImplementedException();
        }
    }

}
