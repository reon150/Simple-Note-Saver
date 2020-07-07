using System;
using Microsoft.AspNetCore.Identity;
using SimpleNoteSaver.Data;
using SimpleNoteSaver.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleNoteSaver.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public UsersServices(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IdentityUser> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var currentUser = new IdentityUser();
            try
            {
                currentUser = await _userManager.GetUserAsync(claimsPrincipal);
            }
            catch (Exception)
            {
                throw;
            }
            return currentUser;
        }

        public async Task<bool> ValidateUserId(string userId, ClaimsPrincipal User)
        {
            var currentUser = await GetCurrentUser(User);
            if (userId == currentUser.Id) return true;
            else return false;
        }
    }

}
