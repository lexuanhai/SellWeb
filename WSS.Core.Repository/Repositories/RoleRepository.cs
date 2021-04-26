using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {

    }
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly AppDataBaseContext _context;
        public RoleRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
