using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Common.Infrastructure
{
   public abstract class PageBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPage
        {
            get
            {
                var totalPage = (double)Total / PageSize;
                return (int)Math.Ceiling(totalPage);
            }
        }
        public int Total { get; set; }
        public int FirstNextPage
        {
            get
            {
                return (PageIndex - 1) * PageSize + 1;
            }
        }
        public int LastNextPage
        {
            get
            {
                return Math.Min(PageIndex*PageSize,Total);
            }
        }
    }
}
