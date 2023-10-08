using Hydro;
using HydroShowcase.Domain;
using HydroShowcase.Pages.Order.Components;
using Microsoft.AspNetCore.Mvc;

namespace HydroShowcase.Pages.Products.Components.ProductList;

public class ProductList : HydroComponent
{
    private readonly IProductsStorage _productsStorage;

    public ProductList(IProductsStorage productsStorage)
    {
        _productsStorage = productsStorage;
        Subscribe<ProductCreatedEvent>(_ => SearchPhrase = null);
        Subscribe<ShowAddProductDialog>(_ => ShowAddDialog = true);
        Subscribe<HideAddProductDialog>(_ => ShowAddDialog = false);
    }

    public HashSet<string> Selection { get; set; } = new();
    public bool ShowAddDialog { get; set; }
    public string SearchPhrase { get; set; }
    public (ProductSorting Column, bool Ascending) Sorting { get; set; }

    private Cache<Task<Product[]>> Products =>
        Cache(async () =>
        {
            var query = _productsStorage.Query();

            if (!string.IsNullOrWhiteSpace(SearchPhrase))
            {
                query = query.Where(p => p.Name.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase));
            }

            query = Sorting switch
            {
                (ProductSorting.Name, Ascending: false) => query.OrderBy(p => p.Name),
                (ProductSorting.Name, Ascending: true) => query.OrderByDescending(p => p.Name),
                (ProductSorting.Price, Ascending: false) => query.OrderBy(p => p.Price),
                (ProductSorting.Price, Ascending: true) => query.OrderByDescending(p => p.Price),
                (ProductSorting.Stock, Ascending: false) => query.OrderBy(p => p.Stock),
                (ProductSorting.Stock, Ascending: true) => query.OrderByDescending(p => p.Stock),
                _ => query
            };

            return query.ToArray();
        });

    public override async Task RenderAsync()
    {
        var products = await Products.Value;

        Selection.RemoveWhere(id => !products.Any(l => l.Id == id));

        ViewBag.Products = products;
        ViewBag.HasProducts = products.Length != 0;
        ViewBag.HasSearch = !string.IsNullOrEmpty(SearchPhrase);
        ViewBag.AllSelected = Selection.Count == products.Length;
    }

    public void Select(string id, bool value)
    {
        if (value)
        {
            Selection.Add(id);
        }
        else
        {
            Selection.Remove(id);
        }
    }

    public async Task SelectAll(bool value)
    {
        if (value)
        {
            var products = await Products.Value;
            Selection = products.Select(p => p.Id).ToHashSet();
        }
        else
        {
            Selection.Clear();
        }
    }

    public void Add() =>
        Dispatch(new ShowAddProductDialog(), Scope.Global);

    public void Remove(string id) =>
        _productsStorage.RemoveProduct(id);

    public void ClearSearch() =>
        SearchPhrase = null;

    public void Sort(ProductSorting value) =>
        Sorting = (
            Column: value,
            Ascending: Sorting.Column == value && !Sorting.Ascending
        );

    public void Order()
    {
        if (!Selection.Any())
        {
            return;
        }

        var outOfStockProduct = _productsStorage.Query()
            .FirstOrDefault(p => Selection.Contains(p.Id) && p.Stock <= 0);

        if (outOfStockProduct != null)
        {
            Dispatch(new ShowOrderValidationMessage($"{outOfStockProduct.Name} is out of stock, so cannot be ordered"), Scope.Global);
            return;
        }

        Location(Url.Page("/Order/Index"), new OrderFormPayload(Selection));
    }
}