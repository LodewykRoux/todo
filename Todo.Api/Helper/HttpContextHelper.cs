using System.Security.Claims;
using Todo.DAL;
using Todo.DAL.Models.Identity;

namespace Todo.Api.Helper;

public class HttpContextHelper(HttpContext httpContext, DataContext _dataContext)
{
    public ApplicationUser? GetUser()
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return null;
        }

        return _dataContext.Users.Find(userId);
    }
}