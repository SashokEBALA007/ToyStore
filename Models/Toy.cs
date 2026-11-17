namespace ToyStore2.Models;

public class Toy
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string? ImageUrl { get; set; }

    // Связь с категорией
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}