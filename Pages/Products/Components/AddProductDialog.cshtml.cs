using System.ComponentModel.DataAnnotations;
using Hydro;
using HydroShowcase.Domain;

namespace HydroShowcase.Pages.Products.Components;

public class AddProductDialog : HydroComponent
{
    private readonly IProductsStorage _productsStorage;

    public AddProductDialog(IProductsStorage productsStorage)
    {
        _productsStorage = productsStorage;
    }

    [Required]
    public string Name { get; set; }
    
    [Required]
    [Range(0, double.PositiveInfinity)]
    public decimal Price { get; set; }
    
    [Required]
    public int Stock { get; set; }

    public void Save()
    {
        if (!ModelState.IsValid)
        {
            return;
        }
        
        _productsStorage.AddProduct(new(
            name: Name,
            price: Price,
            stock: Stock
        ));
        
        Dispatch(new ProductCreatedEvent(), Scope.Global);
        Close();
    }
    
    public void Close() =>
        Dispatch(new HideAddProductDialog(), Scope.Global);
}