using Layer_Entities.ModelsDTO;
using Layer_Entities.Wrappers;
using Layer_Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [SwaggerTag("User Functionalities")]
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        [SwaggerOperation(
        Summary = "Login User",
        Description = "Login with a user after of registered.")]
        [HttpPost("authenticate-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            try
            {
                if (request == null)
                {
                    return NoContent();
                }

                return Ok(await _userService.LoginAsync(request));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [SwaggerOperation(
        Summary = "Register User",
        Description = "Register a user with a mail with the next standar: @bhd.com. Put the password with the regular expression")]
        [HttpPost("register-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResponse<RegisterResponseUser>))]
        public async Task<IActionResult> RegisterUserAsync(RegisterRequest request)
        {
            var result = await _userService.RegisterUsers(request);

            if (result.Errors.Any())
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
    }
}
