using System;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleNoteSaver.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UsersRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            IdentityUser currentUser;
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
