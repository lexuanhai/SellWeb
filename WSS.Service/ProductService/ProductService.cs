using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSS.Core.Common.Infrastructure;
using WSS.Core.Domain.Entities;
using WSS.Core.Dto.AutoMapper;
using WSS.Core.Dto.DataModel;
using WSS.Core.Dto.SearchModel.ProductSearch;
using WSS.Core.Repository;
using WSS.Core.Repository.Repositories;

namespace WSS.Service.ProductService
{
    public interface IProductService : IDisposable
    {
        int SaveEntity(ProductModel productModel);
        //ColorModel GetColorById(int id);
        //List<ColorModel> GetAll();
        PageResult<ProductModel> GetSearchPaging(ProductSearch search);

        ProductModel GetProductId(int productId);
        void Save();
        //bool Delete(int id);
    }
    public class ProductService : IProductService
    {
        IProductRepository _productRepository;
        IProductCategoryRepository _productCategoryRepository;
        IUnitOfWork _unitOfWork;
        public ProductService(
              IProductRepository productRepository,
              IProductCategoryRepository productCategoryRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
        }
        public ProductModel GetProductById(int id)
        {
            var product = new ProductModel();
            if (id > 0)
            {
                product = _productRepository.Queryable()
                  .Where(r => r.Id == id)
                  .AsNoTracking()
                  .FirstOrDefault()
                  .ToModel();
            }
            return product;
        }
        public int SaveEntity(ProductModel productModel)
         {

            try
            {
                var product = new Product();
                if (productModel.Id > 0)
                {
                     product = GetProductById(productModel.Id).ToProduct();
                    product.Id = productModel.Id;
                    product.Name = productModel.Name;
                    product.CategoryId = productModel.CategoryId;
                    product.Price = productModel.Price;
                    product.PromotionalPrice = productModel.PromotionalPrice;
                    product.ManufactureDate = productModel.ManufactureDate;
                    product.ExpirationDate = productModel.ExpirationDate;
                    product.Head = productModel.Head;
                    product.Description = productModel.Description;
                    product.UpdatedDate = DateTime.Now;
                    _productRepository.Update(product);

                    //return true;
                }
                else
                {
                    product.Name = productModel.Name;
                    product.CategoryId = productModel.CategoryId;
                    product.Price = productModel.Price;
                    product.PromotionalPrice = productModel.PromotionalPrice;
                    product.ManufactureDate = productModel.ManufactureDate;
                    product.ExpirationDate = productModel.ExpirationDate;
                    product.Head = productModel.Head;
                    product.Description = productModel.Description;
                    product.CreatedDate = DateTime.Now;
                    _productRepository.Add(product);
                   

                    int id = product.Id;


                }

                Save();

                return product.Id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public PageResult<ProductModel> GetSearchPaging(ProductSearch search)
        {
            var query = _productRepository.FindAll(p=>p.ProductCategory).
                        Select(p=>new ProductModel()
                        {
                            Id= p.Id,
                            CategoryId = p.CategoryId,
                            Head = p.Head,
                            Description = p.Description,
                            Price = p.Price,
                            PromotionalPrice = p.PromotionalPrice,
                            Views =p.Views,
                            ManufactureDate = p.ManufactureDate,
                            ExpirationDate = p.ExpirationDate,
                            IsDeteled = p.IsDeteled,
                            CreatedDate = p.CreatedDate,
                            UpdatedDate =p.UpdatedDate,
                            Name = p.Name,
                            ProductCategoryParent = _productCategoryRepository.FindAll().Where(pc =>pc.Id == p.ProductCategory.ParentId).FirstOrDefault().ToModel(),
                            ProductCategories = p.ProductCategory.ToModel()                            
                        });
            var Total = query.Count();

            var data = query
                //.OrderByDescending(U => U.CreatedDate)
                .Skip((search.PageIndex - 1) * search.PageSize)
                .Take(search.PageSize)
                .ToList();

            var result = new PageResult<ProductModel>()
            {
                Results = data,
                PageIndex = search.PageIndex,
                PageSize = search.PageSize,
                Total = Total,
            };
            return result;
        }

        public ProductModel GetProductId(int productId)
        {
            var product = new ProductModel();
            if (productId > 0)
            {
                product = _productRepository.FindAll().Where(p => p.Id == productId).FirstOrDefault().ToModel();
            }
            return product;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
