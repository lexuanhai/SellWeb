using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface ISizeRepository : IRepository<Size>
    {

    }
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        private readonly AppDataBaseContext _context;
        public SizeRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
