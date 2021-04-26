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
using WSS.Core.Dto.SearchModel.ProductSearch;
using WSS.Core.Dto.SearchModel.RoleSearch;
using WSS.Core.Dto.SearchModel.UserSearch;
using WSS.Service.FunctionService;
using WSS.Service.ProductCategoryService;
//using WSS.Service.RoleService;
using WSS.Service.UserService;

namespace WebSell.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoryController(
            IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetPaging(ProductSearch search)
        {
            var model = _productCategoryService.GetSearchPaging(search);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductCategoryModel productCategoryVM)
        {
            var _message = "";
            try
            {
                if (productCategoryVM.Id > 0)
                {
                    var status = _productCategoryService.SaveEntity(productCategoryVM);
                    _message = status == true ? "Cập nhật thành công" : "Cập nhật không thành công";

                }
                else
                {
                    var status = _productCategoryService.SaveEntity(productCategoryVM);
                    _message = status == true ? "Thêm mới thành công" : "Thêm mới không thành công";
                }
                _productCategoryService.Save();

                return new OkObjectResult(new { Status = true, Message = _message });
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new { Status = false, Message = "có lỗi xảy ra" });
            }
        }

        [HttpGet]
        public IActionResult GetProductCategoryById(int id)
        {
            if (id > 0)
            {
                var model = _productCategoryService.GetProductCategoryById(id);

                return new OkObjectResult(new { Data= model, Status=true });
            }
            return new OkObjectResult(new {Status = false });
        }

        [HttpGet]
        public IActionResult GetParentProductCategory()
        {
            var model = _productCategoryService.GetParentProductCategory();

            if (model != null && model.Count > 0)
            {
                return new OkObjectResult(new { Data = model, Status = true });
            }
            return new OkObjectResult(new { Status = false });
        }

        [HttpGet]
        public IActionResult GetAllCategory()
        {
            var model = _productCategoryService.GetAllCategory();

            if (model != null && model.Count > 0)
            {
                return new OkObjectResult(new { Data = model, Status = true });
            }
            return new OkObjectResult(new { Status = false });
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                var status = _productCategoryService.Delete(id);
                _productCategoryService.Save();
                if (status)                
                    return new OkObjectResult(new { Status = status, Message = "Xóa danh mục sản phẩm thành công" });
                return new OkObjectResult(new { Status = status, Message = "Xóa danh mục sản phẩm không thành công" });

            }
            return new OkObjectResult(new { Status = false,Message="Danh mục sản phẩm không tồn tại" });
        }
    }
}
