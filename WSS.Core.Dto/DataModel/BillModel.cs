using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Common;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Dto.DataModel
{
  public class BillModel
    {
        public int Id { get; set; }
        public BillStatus BillStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int? CustomerId { get; set; }
        public UserModel User { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public UserModel UserModel { get; set; }
        public List<BillDetail> BillDetails { set; get; }
    }
}
