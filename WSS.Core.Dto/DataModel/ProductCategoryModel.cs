using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Common;

namespace WSS.Core.Dto.DataModel
{
  public class ProductCategoryModel
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string NameParent { get; set; }
        public int? ParentId { set; get; }
        public Status Status { set; get; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

    }
}
