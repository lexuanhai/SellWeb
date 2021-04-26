using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WSS.Core.Domain.Entities
{
    public class AppUserRole : SameEntities
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual AppRole Roles { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser Users { get; set; }
    }
}
