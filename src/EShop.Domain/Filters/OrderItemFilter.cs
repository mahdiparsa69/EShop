namespace EShop.Domain.Filters;

public struct OrderItemFilter : IListFilter
{
    public int Offset { get; set; }

    public int Count { get; set; }
}