using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WSS.Core.Domain.Entities
{
  public class ProductQuantity:SameEntities
    {
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public int? ProductId { get; set; }
        public int TotalImport { get; set; }
        public int TotalSell { get; set; }
        public int TotalStock { get; set; }

        [ForeignKey("ColorId")]
        public virtual Color Colors { get; set; }

        [ForeignKey("SizeId")]
        public virtual Size Sizes { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
    }
}
