using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Data.Entities.Identity;

namespace Speurzoekers.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserRepository(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> CheckAuthenticateAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
            return result.Succeeded;
        }


        public async Task<UserInfo> GetUserInfoByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var rolesOfUser = await _userManager.GetRolesAsync(user);
            return new UserInfo
            {
                Id = user.Id,
                Roles = rolesOfUser.ToArray()
            };
        }

        public async Task<ActionResult> RegisterUserAsync(UserRegister userToRegister)
        {
            var user = _mapper.Map<ApplicationUser>(userToRegister);
            
            var result = await _userManager.CreateAsync(user, userToRegister.Password);
            
            return new ActionResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => new ActionResultError
                {
                    Code = e.Code,
                   Description =  e.Description
                }).ToArray()
            };

        }
    }
}
