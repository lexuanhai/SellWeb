using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IProductImagesRepository : IRepository<ProductImages>
    {

    }
    public class ProductImagesRepository : Repository<ProductImages>, IProductImagesRepository
    {
        private readonly AppDataBaseContext _context;
        public ProductImagesRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
