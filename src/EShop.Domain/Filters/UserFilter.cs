namespace EShop.Domain.Filters;

public struct UserFilter : IListFilter
{
    public int Offset { get; set; }

    public int Count { get; set; }

    public string? Username { get; set; }
}