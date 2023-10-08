namespace HydroShowcase.Domain;

public class Product
{
    public Product(string name, decimal price, int stock)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Price = price;
        Stock = stock;
    }

    public string Id { get; private set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}