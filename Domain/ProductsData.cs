namespace HydroShowcase.Domain;

public static class ProductsData
{
    public static readonly List<Product> DemoProducts = new()
    {
        new("LG OLED TV 65\"", price: 2155, stock: 5), 
        new("iPhone 15 Pro", price: 999, stock: 30), 
        new("Airpods Pro", price: 129, stock: 14), 
        new("Sonos One", price: 299, stock: 0), 
        new("Table", price: 3900, stock: 0), 
        new("Samsung S10", price: 899, stock: 23),
    };
}