using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WSS.Core.Domain.Entities
{
  public class ProductImages:SameEntities
    {
        public string Url { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
    }
}
