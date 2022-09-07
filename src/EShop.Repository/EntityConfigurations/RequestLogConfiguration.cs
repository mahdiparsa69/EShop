using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Repository.EntityConfigurations
{
    public class RequestLogConfiguration : BaseModelConfiguration<RequestLog>
    {
        public override void ConfigureDerived(EntityTypeBuilder<RequestLog> builder)
        {

        }
    }
}
