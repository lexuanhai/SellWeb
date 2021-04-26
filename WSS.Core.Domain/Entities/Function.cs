using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Common;

namespace WSS.Core.Domain.Entities
{
   public class Function:SameEntities
    {
        public string Name { set; get; }
        public string URL { set; get; }
        public int? ParentId { set; get; }
        public string IconCss { get; set; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
    }
}
