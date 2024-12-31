using System.Collections.Generic;


namespace ENPAY.Common
{
    public class DataEnvelope<T>
    {
        //public List<AggregateFunctionsGroup>? GroupedData { get; set; }

        public List<T> CurrentPageData { get; set; }

        public int TotalItemCount { get; set; }
    }
}
