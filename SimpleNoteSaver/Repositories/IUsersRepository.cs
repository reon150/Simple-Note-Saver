using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleNoteSaver.Repositories
{
    public interface IUsersRepository
    {
        Task<IdentityUser> GetCurrentUser(ClaimsPrincipal claimsPrincipal);
        Task<bool> ValidateUserId(string userId, ClaimsPrincipal User);
    }
}
