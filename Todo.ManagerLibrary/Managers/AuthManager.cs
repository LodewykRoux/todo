using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Todo.DAL.DTO.Identity;
using Todo.DAL.Models.Identity;
using Todo.ManagerLibrary.Managers.Interface;

namespace Todo.ManagerLibrary.Managers;

public class AuthManager(UserManager<ApplicationUser> userManager) : IAuthManager
{
    public async Task<(IdentityResult result, ApplicationUser user)> RegisterUser(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };
        var result = await userManager.CreateAsync(user, request.Password);
        return (result, user);
    }

    public async Task<ApplicationUser?> FindByEmail(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user;
    }

    public async Task<bool> CheckPassword(ApplicationUser user, string password)
    {
        var valid = await userManager.CheckPasswordAsync(user, password);
        return valid;
    }
    
    public async Task AddUserToRole(ApplicationUser user, string role)
    {
        await userManager.AddToRoleAsync(user, role);
    }
    
    public async Task<bool> LogoutUser(ApplicationUser user)
    {
        try
        {
            user.Token = null;
            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        catch
        {
            return false;
        }
    }
}