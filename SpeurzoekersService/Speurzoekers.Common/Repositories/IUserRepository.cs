using System.Threading.Tasks;
using Speurzoekers.Common.Domain;
using Speurzoekers.Common.Domain.User;

namespace Speurzoekers.Common.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CheckAuthenticateAsync(string username, string password);
        Task<UserInfo> GetUserInfoByNameAsync(string username);
        Task<ActionResult> RegisterUserAsync(UserRegister userToRegister);
    }
}
