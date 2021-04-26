using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {

    }
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private readonly AppDataBaseContext _context;
        public PermissionRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
