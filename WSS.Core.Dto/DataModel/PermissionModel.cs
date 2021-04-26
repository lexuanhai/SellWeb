using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Dto.DataModel
{
   public class PermissionModel
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? FunctionId { set; get; }
        public bool? CanCreate { get; set; }
        public bool? CanRead { get; set; }
        public bool? CanUpdate { get; set; }
        public bool? CanDelete { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
