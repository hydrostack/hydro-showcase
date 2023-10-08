using Hydro;
using HydroShowcase.Domain;

namespace HydroShowcase.Pages.Order.Components;

public class OrderForm : HydroComponent
{
    private readonly IProductsStorage _productsStorage;

    public OrderForm(IProductsStorage productsStorage)
    {
        _productsStorage = productsStorage;
    }

    public OrderFormPayload Payload { get; set; }

    public override void Mount()
    {
        Payload = GetPayload<OrderFormPayload>();
    }

    public override void Render()
    {
        if (Payload == null)
        {
            return;
        }

        var products = _productsStorage.Query()
            .Where(p => Payload.Ids.Contains(p.Id))
            .ToList();

        ViewBag.Products = products;
    }
}