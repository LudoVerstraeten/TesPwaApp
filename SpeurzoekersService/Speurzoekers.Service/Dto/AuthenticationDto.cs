using System.ComponentModel.DataAnnotations;

namespace Speurzoekers.Service.Dto
{
    public class AuthenticateDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Incorrect email address used for username")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
