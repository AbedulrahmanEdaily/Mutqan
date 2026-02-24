using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Mutqan.DAL.DTO.Request.AuthenticationRequest;
using Mutqan.DAL.DTO.Response;
using Mutqan.DAL.DTO.Response.AuthenicationResponse;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Mutqan.BLL.Services.Interface
{
    public interface IAuthenticationService : IScopedService
    {
        Task<BaseResponse> RegisterAsync(RegisterRequest registerRequest, HttpRequest request);
        Task<BaseResponse> ConfirmEmailAsync(string token , string userId);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        AuthenticationProperties LoginWithGoogle(string returnUrl);
        Task<LoginResponse> LoginWithGoogleCallBack(ClaimsPrincipal claimsPrincipal);
        Task<BaseResponse> ForgetPasswordAsync(ForgetPasswordRequest request);
        Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
