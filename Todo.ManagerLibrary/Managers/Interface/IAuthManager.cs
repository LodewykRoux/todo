using Microsoft.AspNetCore.Identity;
using Todo.DAL.DTO.Identity;
using Todo.DAL.Models.Identity;

namespace Todo.ManagerLibrary.Managers.Interface;

public interface IAuthManager
{
    Task<(IdentityResult result, ApplicationUser user)> RegisterUser(RegisterRequest request);
    Task<ApplicationUser?> FindByEmail(string email);
    Task<bool> CheckPassword(ApplicationUser user, string password);
    Task AddUserToRole(ApplicationUser user, string role);
    Task<bool> LogoutUser(ApplicationUser user);
}