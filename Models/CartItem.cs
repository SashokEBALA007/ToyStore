namespace ToyStore2.Models;

public class CartItem
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ToyId { get; set; }
    public int Quantity { get; set; } = 1;
}