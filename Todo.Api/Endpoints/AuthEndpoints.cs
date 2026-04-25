using Microsoft.AspNetCore.Identity;
using Todo.Api.Auth;
using Todo.DAL.DTO.Identity;
using Todo.DAL.Models.Identity;
using Todo.ManagerLibrary.Managers;
using Todo.ManagerLibrary.Managers.Interface;


namespace Todo.Api.Endpoints;

public static class AuthEndpoints
{
    public static void RegisterAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", async (
            RegisterRequest request,
            IAuthManager authManager) =>
        {
            var registration = await authManager.RegisterUser(request);

            if (!registration.result.Succeeded)
                return Results.BadRequest(registration.result.Errors);

            await authManager.AddUserToRole(registration.user, "User");

            return Results.Ok();
        });

        group.MapPost("/login", async (
            LoginRequest request,
            IAuthManager authManager,
            JwtTokenService tokenService) =>
        {
            var user = await authManager.FindByEmail(request.Email);
            if (user is null)
                return Results.Unauthorized();

            var valid = await authManager.CheckPassword(user, request.Password);
            if (!valid)
                return Results.Unauthorized();

            var token = await tokenService.CreateTokenAsync(user);
            return Results.Ok(new { accessToken = token });
        });
    }
}