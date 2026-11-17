using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyStore2.Data;
using ToyStore2.Models;

namespace ToyStore2.Controllers;

public class ToysController : Controller
{
    private readonly ApplicationDbContext _context;

    public ToysController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        ViewBag.Title = "Наш магазин игрушек";
        ViewBag.Subtitle = "Лучшие игрушки по доступным ценам!";

        // Include нужен, чтобы подтянуть данные о категориях
        var toys = await _context.Toys.Include(t => t.Category).ToListAsync();
        return View(toys);
    }

    [Authorize]
    public async Task<IActionResult> AddToCart(int id)
    {
        var toy = await _context.Toys.FindAsync(id);
        if (toy == null) return NotFound();
        return Content($"Игрушка \"{toy.Name}\" добавлена в корзину (демо.)");
    }
}