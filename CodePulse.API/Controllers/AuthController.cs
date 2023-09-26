using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            //Check email
            var identityuser = await _userManager.FindByEmailAsync(request.Email);

            if (identityuser != null)
            {
                //Check password
                bool checkPasswordRequest = await _userManager.CheckPasswordAsync(identityuser, request.Password);

                if (checkPasswordRequest)
                {
                    //Create a token and response
                    var response = new LoginResponseDto();
                    response.Email = request.Email;
                    response.Roles = (await _userManager.GetRolesAsync(identityuser)).ToList();
                    response.Token = _tokenRepository.CreateJwtToken(identityuser, response.Roles);
                    return Ok(response);
                }
            }

            ModelState.AddModelError("", "Email or password is incorrect");
            return ValidationProblem(ModelState);
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            //Create IdentityUser object
            var user = new IdentityUser
            {
                UserName = request.Email.Trim(),
                Email = request.Email.Trim()
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);
            
            if (identityResult.Succeeded)
            {
                //Add Role to user (Reader)
                var roleResult = await _userManager.AddToRoleAsync(user, "Reader");

                if (roleResult.Succeeded)
                {
                    return Ok();
                }

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}
