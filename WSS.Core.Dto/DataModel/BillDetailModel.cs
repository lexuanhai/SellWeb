using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Common;

namespace WSS.Core.Dto.DataModel
{
  public class BillDetailModel
    {       
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }
        public int? BillId { get; set; }
        public Decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public SizeModel SizeModel { get; set; }
        public ColorModel ColorModel { get; set; }
        public BillModel BillModel { get; set; }
    }
}
