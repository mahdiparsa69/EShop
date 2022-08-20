namespace EShop.Domain.Filters;

public struct OrderFilter : IListFilter
{
    public int Offset { get; set; }
    public int Count { get; set; }

    public string? Name { get; set; }
}