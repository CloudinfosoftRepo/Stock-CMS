using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Stock_CMS.Common
{
    public interface IAsyncRepository<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        Task<IEnumerable<TModel>> ListAsync<TKey>(ISpecification<TEntity> spec, PaginationSpecification pageSpec, DbContext context);
    }
}
