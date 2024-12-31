using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_CMS.Common
{
    public class PageSpecification : PaginationSpecification
    {
        private const int _maxResults = int.MaxValue;

        public PageSpecification(int requestedNumberOfResults, int requestedPageNumber)
            : base(_maxResults, requestedNumberOfResults, requestedPageNumber, true)
        {
        }
    }
}
