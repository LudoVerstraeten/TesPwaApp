using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Common.Repositories;

namespace Speurzoekers.Data.Repositories
{
    public class FakeUserRepository : IUserRepository
    {
        public Task<bool> CheckAuthenticateAsync(string username, string password)
        {
            return Task.FromResult(true);
        }

        public Task<UserInfo> GetUserInfoByNameAsync(string username)
        {
            return Task.FromResult(new UserInfo() {Id = Guid.NewGuid(), Roles = new string[0]});
        }

        public Task<ActionResult> RegisterUserAsync(UserRegister userToRegister)
        {
            return Task.FromResult(new ActionResult{Succeeded = true});
        }
    }
}
