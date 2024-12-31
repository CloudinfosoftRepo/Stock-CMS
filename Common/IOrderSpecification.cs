using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Stock_CMS.Common
{
    public interface IOrderSpecification<TEntity, TKey>
    {
        bool SortDescending { get; }

        Expression<Func<TEntity, TKey>> SortBy { get; }
    }
}
