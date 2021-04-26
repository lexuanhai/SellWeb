using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {

    }
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDataBaseContext _context;
        public ProductRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
