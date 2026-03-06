using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.AuthenticationRequest;
using IAuthenticationService = Mutqan.BLL.Services.Interface.IAuthenticationService;
using RegisterRequest = Mutqan.DAL.DTO.Request.AuthenticationRequest.RegisterRequest;

namespace Mutqan.PL.Area.Identity
{
    [Area("Identity")]
    [Route("api/[Area]/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest registerRequest)
        {
            var result = await _authenticationService.RegisterAsync(registerRequest, Request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfairmEmail([FromQuery] string token, [FromQuery] string userId)
        {
            var result = await _authenticationService.ConfirmEmailAsync(token, userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var result = await _authenticationService.ForgetPasswordAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPasswordAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authenticationService.RefreshTokenAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("login-google")]
        public IActionResult LoginGoogle([FromQuery] string returnUrl)
        {
            var properties = _authenticationService.LoginWithGoogle(returnUrl);
            return Challenge(properties, ["Google"]);
        }
        [HttpGet("LoginGoogleCallback")]
        public async Task<IActionResult> LoginGoogleCallback()
        {
            var auth = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!auth.Succeeded)
            {
                return Unauthorized();
            }
            var result = await _authenticationService.LoginWithGoogleCallBack(auth.Principal);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
