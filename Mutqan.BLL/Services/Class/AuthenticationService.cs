using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Mutqan.BLL.Services.Interface;
using Mutqan.DAL.DTO.Request.AuthenticationRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.AuthenicationResponse;
using Mutqan.DAL.Models;
using Mutqan.DAL.Repository.Interface;
using System.Security.Claims;

namespace Mutqan.BLL.Services.Class
{
    public class AuthenticationService : Interface.IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            , IEmailSender emailSender,ITokenService tokenService
            ,IRefreshTokenRepository refreshTokenRepository
            ,LinkGenerator linkGenerator
            , IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse> RegisterAsync(DAL.DTO.Request.AuthenticationRequest.RegisterRequest registerRequest, HttpRequest request)
        {
            var user = registerRequest.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }
            await _userManager.AddToRoleAsync(user, "User");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);
            var url = $"{request.Scheme}://{request.Host}/api/Identity/account/ConfirmEmail?token={token}&userId={user.Id}"; ;
            await _emailSender.SendEmailAsync(user.Email, "Welcome to Mutqan", $"<p>Thank you for registering {user.FullName} Please Confirm Your Email by</p>" + $"<a href='{url}'>Click Here</a>");
            return new BaseResponse
            {
                Success = true,
                Message = "User registered successfully"
            };
        }
        public async Task<LoginResponse> LoginAsync(DAL.DTO.Request.AuthenticationRequest.LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }
            else if (await _userManager.IsLockedOutAsync(user))
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "User is locked out,try again later"
                };
            }
            var password = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (password.IsLockedOut)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "User is locked out,try again later"
                };
            }
            else if (password.IsNotAllowed)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Email is not confirmed"
                };
            }
            else if (!password.Succeeded)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid Password"
                };
            }
            var AccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                Token = newRefreshToken
            };
            await _refreshTokenRepository.AddAsync(refreshToken);
            return new LoginResponse
            {
                Success = true,
                Message = "Login Successfully",
                AccessToken = AccessToken,
                RefreshToken = newRefreshToken
            };
        }
        public  AuthenticationProperties LoginWithGoogle(string returnUrl)
        {
            var ctx = _httpContextAccessor.HttpContext;
            var callbackUrl = _linkGenerator.GetUriByAction(
                httpContext: ctx,
                action: "LoginGoogleCallback",
                controller: "Account",
                values: null,
                scheme: ctx.Request.Scheme,
                host: ctx.Request.Host
            );
            returnUrl = Uri.EscapeDataString(returnUrl);

            return _signInManager.ConfigureExternalAuthenticationProperties(
                "Google",
                $"{callbackUrl}?returnUrl={returnUrl}"
            );
        }
        public async Task<LoginResponse> LoginWithGoogleCallBack(ClaimsPrincipal claimsPrincipal)
        {
            if(claimsPrincipal is null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Google login failed"
                };
            }

            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            var fullName = claimsPrincipal.FindFirstValue(ClaimTypes.Name);

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FullName = fullName!,
                    EmailConfirmed = true,
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "User");
            }
            var providerKey = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? claimsPrincipal.FindFirstValue("sub");

            var info = new UserLoginInfo("Google", providerKey!, "Google");
            await _userManager.AddLoginAsync(user, info);

            var accessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
            };

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new LoginResponse
            {
                Success = true,
                Message = "Login Successfully",
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }
        public async Task<BaseResponse> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User does not exist"
                };
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return new BaseResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            return new BaseResponse
            {
                Success = true,
                Message = "Email Confirmed"
            }; ;
        }
        public async Task<BaseResponse> ForgetPasswordAsync(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }
            var code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            user.CodeResetPassword = code;
            user.CodeResetPasswordExpiration = DateTime.UtcNow.AddMinutes(5);

            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(user.Email, "Reset Password Code", $"<p>Your password reset code is: <strong>{code}</strong></p><p>This code will expire in 5 minutes.</p>");
            return new BaseResponse
            {
                Success = true,
                Message = "Reset password code sent to email"
            };
        }
        public async Task<BaseResponse> ResetPasswordAsync(DAL.DTO.Request.AuthenticationRequest.ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }
            if (user.CodeResetPassword != request.ResetCode)
                return new BaseResponse { Success = false, Message = "Invalid reset code" };

            if (user.CodeResetPasswordExpiration < DateTime.UtcNow)
                return new BaseResponse { Success = false, Message = "Reset code has expired" };
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Password reset failed: " + string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }
            user.CodeResetPassword = null;
            user.CodeResetPasswordExpiration = null;
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(user.Email, "Password Changed", "<p>Your password is Changed</p>");
            return new BaseResponse
            {
                Success = true,
                Message = "Password changed successfully"
            };
        }
        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {

            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity.Name;

            var user = _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefault(u => u.UserName == username);

            if (user is null)
                return new LoginResponse { Success = false, Message = "User not found" };

            var refreshTokenInDb = user.RefreshTokens
                .FirstOrDefault(t => t.Token == request.RefreshToken);

            if (refreshTokenInDb is null)
                return new LoginResponse { Success = false, Message = "Invalid Refresh Token" };

            if (refreshTokenInDb.IsRevoked)
                return new LoginResponse { Success = false, Message = "Token has been revoked" };

            if (refreshTokenInDb.ExpiryDate <= DateTime.UtcNow)
                return new LoginResponse { Success = false, Message = "Token has expired" };
            refreshTokenInDb.IsRevoked = true;
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);
            return new LoginResponse
            {
                Success = true,
                Message = "Token Refreshed Successfully",
                AccessToken = await _tokenService.GenerateAccessToken(user),
                RefreshToken = newRefreshToken
            };
        }
    }
}
