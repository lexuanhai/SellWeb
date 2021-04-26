using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSS.Core.Common.Infrastructure;
using WSS.Core.Domain.Entities;
using WSS.Core.Dto.AutoMapper;
using WSS.Core.Dto.DataModel;
using WSS.Core.Dto.SearchModel.RoleSearch;
using WSS.Core.Dto.SearchModel.UserSearch;
using WSS.Core.Repository;
using WSS.Core.Repository.Repositories;

namespace WSS.Service.ProductQuantityService
{
    public interface IProductImagesService : IDisposable
    {
        List<ProductImagesModel> GetImagesByProductId(int productId);
        bool SaveEntity(string[] productImages, int productId);

        List<ProductImagesModel> ProductImagesByProductId(int productId);
        void Save();
    }
    public class ProductImagesService : IProductImagesService
    {
        IProductImagesRepository _productImagesRepository;
        IUnitOfWork _unitOfWork;
        public ProductImagesService(
              IProductImagesRepository productImagesRepository,
            IUnitOfWork unitOfWork)
        {
            _productImagesRepository = productImagesRepository;
            _unitOfWork = unitOfWork;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public List<ProductImagesModel> GetImagesByProductId(int productId)
        {
            var model = new List<ProductImagesModel>();
            if (productId > 0)
            {
                var productImages = _productImagesRepository.FindAll().Where(q => q.ProductId == productId).Select(q => q.ToModel()).ToList();
                if (productImages != null && productImages.Count > 0)
                {
                    model = productImages;
                }
            }
            return model;
        }

        public bool SaveEntity(string[] productImages , int productId)
        {
            try
            {               
                if (productId > 0)
                {
                    var oldProductImages = _productImagesRepository.FindAll().Where(q => q.ProductId == productId).ToList();
                    if (oldProductImages != null && oldProductImages.Count > 0)
                    {
                        _productImagesRepository.RemoveMultiple(oldProductImages);
                    }
                    foreach (var item in productImages)
                    {
                        var objImg = new ProductImagesModel();
                        objImg.ProductId = productId;
                        objImg.Url = item;
                        _productImagesRepository.Add(objImg.ToProductImages());
                    }
                    Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            return false;
        }

        public List<ProductImagesModel> ProductImagesByProductId(int productId)
        {
            var productsImages = new List<ProductImagesModel>();
            if (productId > 0)
            {
                productsImages = _productImagesRepository.FindAll().Where(pm => pm.ProductId == productId).Select(pm => pm.ToModel()).ToList();
            }
            return productsImages;            
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
