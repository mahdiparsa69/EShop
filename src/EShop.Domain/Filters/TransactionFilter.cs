namespace EShop.Domain.Filters;

public struct TransactionFilter : IListFilter
{
    public int Offset { get; set; }
    public int Count { get; set; }

    public string? Name { get; set; }
}