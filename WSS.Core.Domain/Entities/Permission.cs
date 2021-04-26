using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WSS.Core.Domain.Entities
{
    public class Permission:SameEntities
    {
        public Guid RoleId { get; set; }
        public int? FunctionId { set; get; }
        public bool? CanCreate { get; set; }
        public bool? CanRead { get; set; }
        public bool? CanUpdate { get; set; }
        public bool? CanDelete { get; set; }
        [ForeignKey("RoleId")]
        public virtual AppRole Roles { get; set; }
        [ForeignKey("FunctionId")]
        public virtual Function Functions { get; set; }
    }
}
