using Microsoft.AspNetCore.Identity;

namespace ToyStore2.Models;

public class ApplicationUser : IdentityUser
{
    // Поле для хранения аватарки в виде массива байтов
    public byte[]? AvatarImage { get; set; }
}