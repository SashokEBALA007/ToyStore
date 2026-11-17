namespace ToyStore2.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Навигационное свойство
    public List<Toy> Toys { get; set; } = new();
}