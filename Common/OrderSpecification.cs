using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Stock_CMS.Common
{
    public class OrderSpecification<TEntity, TKey> : IOrderSpecification<TEntity, TKey>
    {
        public bool SortDescending { get; } = false;

        public Expression<Func<TEntity, TKey>> SortBy { get; }

        public OrderSpecification(Expression<Func<TEntity, TKey>> sortBy)
        {
            SortBy = sortBy;
        }

        public OrderSpecification(Expression<Func<TEntity, TKey>> sortBy, bool sortDescending)
        {
            SortBy = sortBy;
            SortDescending = sortDescending;
        }
    }
}
