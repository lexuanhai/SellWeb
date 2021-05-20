using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IAppRoleRepository : IRepository<AppRole>
    {

    }
    public class AppRoleRepository : Repository<AppRole>, IAppRoleRepository
    {
        private readonly AppDataBaseContext _context;
        public AppRoleRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
