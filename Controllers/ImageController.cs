using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToyStore2.Models;

namespace ToyStore2.Controllers;

public class ImageController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public ImageController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
    {
        _userManager = userManager;
        _env = env;
    }

    public async Task<IActionResult> GetAvatar()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null && user.AvatarImage != null)
        {
            return File(user.AvatarImage, "image/jpeg");
        }

        // Если картинки нет — отдаем заглушку из wwwroot/images/anonymous.png
        var avatarPath = "/images/anonymous.jpg";
        var fullPath = Path.Combine(_env.WebRootPath, avatarPath.TrimStart('/'));

        if (System.IO.File.Exists(fullPath))
        {
            return File(System.IO.File.ReadAllBytes(fullPath), "image/png");
        }

        return NotFound();
    }
}