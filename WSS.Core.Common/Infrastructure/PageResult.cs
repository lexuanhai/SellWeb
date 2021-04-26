using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Common.Infrastructure
{
    public class PageResult<T>:PageBase where T:class
    {
        public PageResult()
        {
            Results = new List<T>();
        }
       public IList<T> Results { get; set; }
    }
}
