using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WSS.Core.Domain.Entities
{
  public  class ProductColor:SameEntities
    {
        public int? ProductId { get; set; }
        public int? ColorId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
        [ForeignKey("ColorId")]
        public virtual Color Colors { get; set; }
    }
}
