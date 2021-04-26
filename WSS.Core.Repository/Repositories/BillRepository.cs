using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IBillRepository : IRepository<Bill>
    {

    }
    public class BillRepository : Repository<Bill>, IBillRepository
    {
        private readonly AppDataBaseContext _context;
        public BillRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
