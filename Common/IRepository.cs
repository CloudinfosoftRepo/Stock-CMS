using System.Collections.Generic;
using ENPAY.Common;
using Microsoft.EntityFrameworkCore;

namespace   Stock_CMS.Common
{
    public interface IRepository<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        IEnumerable<TModel> List<TKey>(ISpecification<TEntity> spec, PaginationSpecification pageSpec, DbContext context);
    }
}
