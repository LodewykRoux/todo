using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace Todo.DAL.Models.Identity;

[Description("Security Claims for a role")]
public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
}