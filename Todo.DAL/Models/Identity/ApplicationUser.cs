using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Todo.DAL.Models.Identity;

public class ApplicationUser : IdentityUser
{
    [StringLength(400)]
    public string? Token { get; set; }
}