using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WSS.Core.Common;

namespace WSS.Core.Domain.Entities
{
   public class ProductCategory : SameEntities
    {
        [MaxLength(500)]
        public string Name { set; get; }
        public int? ParentId { set; get; }
        public Status? Status { set; get; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
