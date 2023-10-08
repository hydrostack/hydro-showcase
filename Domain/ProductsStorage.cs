namespace HydroShowcase.Domain;

public interface IProductsStorage
{
    IQueryable<Product> Query();
    void AddProduct(Product product);
    void RemoveProduct(string id);
}

public class ProductsStorage : IProductsStorage
{
    private static readonly List<Product> Products = ProductsData.DemoProducts;

    public IQueryable<Product> Query() =>
        Products.AsQueryable();

    public void AddProduct(Product product) =>
        Products.Add(product);
    
    public void RemoveProduct(string id) =>
        Products.RemoveAll(p => p.Id == id);
}