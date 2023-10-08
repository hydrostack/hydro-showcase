using Hydro;

namespace HydroShowcase.Pages.Products.Components;

public class ProductOrderValidation : HydroComponent
{
    public ProductOrderValidation()
    {
        Subscribe<ShowOrderValidationMessage>(data => Message = data.Message);
    }
    
    public string Message { get; set; }
}