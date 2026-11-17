using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToyStore2.Data;
using ToyStore2.Models;

var builder = WebApplication.CreateBuilder(args);

// База данных
var connectionString = "Server=(localdb)\\mssqllocaldb;Database=ToyStore2;Trusted_Connection=True;MultipleActiveResultSets=true";
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Identity (с упрощенными паролями)
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Создание БД, тестовых данных и пользователя
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // 1. Создаем базу данных, если её нет
    context.Database.EnsureCreated();

    // 2. Добавляем игрушки и категории, если их нет
    if (!context.Categories.Any())
    {
        var catSoft = new ToyStore2.Models.Category { Name = "Мягкие игрушки" };
        var catCars = new ToyStore2.Models.Category { Name = "Машинки" };
        context.Categories.AddRange(catSoft, catCars);
        context.SaveChanges();

        context.Toys.AddRange(
            new ToyStore2.Models.Toy { Name = "Мягкий мишка", Description = "Большой и уютный", Price = 1500, ImageUrl = "/images/bear.jpg", CategoryId = catSoft.Id },
            new ToyStore2.Models.Toy { Name = "Гоночная машинка", Description = "Красная, быстрая", Price = 750, ImageUrl = "/images/car.jpg", CategoryId = catCars.Id }
        );
        context.SaveChanges();
    }

    // 3. ПРИНУДИТЕЛЬНО СОЗДАЕМ ПОЛЬЗОВАТЕЛЯ (Обход ошибки 400)
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    if (await userManager.FindByEmailAsync("admin@test.ru") == null)
    {
        var testUser = new ApplicationUser { UserName = "admin@test.ru", Email = "admin@test.ru" };
        await userManager.CreateAsync(testUser, "1234");
    }
}

// УБРАЛИ СТРОГИЕ ПРОВЕРКИ HTTPS (чтобы не было конфликтов)
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Toys}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();