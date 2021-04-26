using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Areas.Admin.Models;
using WSS.Core.Dto.DataModel;
using WSS.Core.Dto.SearchModel.FunctionSearch;
using WSS.Core.Dto.SearchModel.RoleSearch;
using WSS.Core.Dto.SearchModel.UserSearch;
using WSS.Service.FunctionService;
using WSS.Service.ProductCategoryService;
using WSS.Service.ProductQuantityService;
//using WSS.Service.RoleService;
using WSS.Service.UserService;

namespace WebSell.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductQuantityController : Controller
    {
        private readonly IProductQuantityService _productQuantityService;
        public ProductQuantityController(
            IProductQuantityService productImagesService)
        {
            _productQuantityService = productImagesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveEntity(List<ProductQuantityModel> productQuantity)
        {
            try
            {
                bool status = false;

                if (productQuantity != null)
                {
                    status = _productQuantityService.SaveEntity(productQuantity);
                }

                return new OkObjectResult(new { Status = status});
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        [HttpGet]
        public IActionResult GetQuantitiesByProductId(int productId)
        {
            var productQuantities = new List<ProductQuantityModel>(); 
            if (productId > 0)
            {
                productQuantities = _productQuantityService.GetQuantityByProductId(productId);
              
            }

            bool status = productQuantities.Count > 0 ? true : false;

            return new OkObjectResult(new { Status = status , Data = productQuantities });
        }
        //[HttpGet]


        //public IActionResult GetProductImageById(int productId)
        //{
        //    if (productId > 0)
        //    {
        //        var model = _productQuantityService.GetImagesByProductId(productId);

        //        return new OkObjectResult(new { Data= model, Status=true });
        //    }

        //    return new OkObjectResult(new {Status = false });
        //}

    }
}
