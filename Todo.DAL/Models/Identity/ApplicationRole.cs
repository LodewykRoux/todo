using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Todo.DAL.Models.Identity;

[Description("Security Roles in the System")]
public class ApplicationRole : IdentityRole
{
    [MaxLength(255)]
    public string Description { get; set; }
}