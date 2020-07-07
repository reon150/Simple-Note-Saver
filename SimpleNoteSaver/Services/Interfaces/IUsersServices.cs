using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleNoteSaver.Services.Interfaces
{
    public interface IUsersServices
    {
        Task<IdentityUser> GetCurrentUser(ClaimsPrincipal claimsPrincipal);
        Task<bool> ValidateUserId(string userId, ClaimsPrincipal User);
    }
}
