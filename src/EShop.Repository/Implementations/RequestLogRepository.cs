using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;

namespace EShop.Repository.Implementations
{
    public class RequestLogRepository : BaseRepository<RequestLog, RequestLogFilter>, IRequestLogRepository
    {
        public RequestLogRepository(EShopDbContext dbContext) : base(dbContext)
        {
        }

        public override IQueryable<RequestLog> ApplyFilter(IQueryable<RequestLog> query, RequestLogFilter filter)
        {
            return query;
        }

        public override IQueryable<RequestLog> ConfigureInclude(IQueryable<RequestLog> query)
        {
            return query;
        }

        public override IQueryable<RequestLog> ConfigureListInclude(IQueryable<RequestLog> query)
        {
            return query;
        }
    }
}
