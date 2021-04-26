using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Dto.SearchModel
{
    public class Search
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 40;
    }
}
