using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDataBaseContext _context;
        public UnitOfWork(AppDataBaseContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
