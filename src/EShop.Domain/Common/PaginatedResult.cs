using EShop.Domain.Models;

namespace EShop.Domain.Common;

public class PaginatedResult<TModelBase>
where TModelBase : BaseModel
{
    public List<TModelBase> Items { get; set; }

    public int TotalCount { get; set; }
}