using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Speurzoekers.Common.Config;
using Speurzoekers.Common.Domain.User;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Service.Dto;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Speurzoekers.Service.Controllers
{
    /// <summary>
    /// Based on:
    /// https://jakeydocs.readthedocs.io/en/latest/security/authentication/identity.html
    ///
    /// This is a special controller, do not copy too much from this.
    /// </summary>

    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly AuthenticationOptions _authenticationSettings;

        public UserController(IOptions<AuthenticationOptions> authenticationSettings,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _authenticationSettings = authenticationSettings.Value;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateDto authenticateDto)
        {
            var result = await _userRepository.CheckAuthenticateAsync(authenticateDto.UserName, authenticateDto.Password);
            if (!result)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            var user = await _userRepository.GetUserInfoByNameAsync(authenticateDto.UserName);

            return Ok(new
            {
                UserId = user.Id,
                Token = CreateToken(user)
            });
        }


        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]AuthenticateDto authDto)
        {
            try
            {
                var user = _mapper.Map< UserRegister>(authDto);
                var result = await _userRepository.RegisterUserAsync(user);

                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong during registering.");
            }
        }

        private string CreateToken(UserInfo user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_authenticationSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            tokenDescriptor.Subject.AddClaims(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
