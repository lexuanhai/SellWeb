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
    public class ProductImagesController : Controller
    {
        private readonly IProductImagesService _productImagesService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ProductImagesController(
            IProductImagesService productImagesService,
            IHostingEnvironment hostingEnvironment)
        {
            _productImagesService = productImagesService;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveEntity(string[] productImages, int productId)
        {
            try
            {
                bool status = false;
                if (productImages != null && productImages.Length > 0 && productId > 0)
                {
                    status = _productImagesService.SaveEntity(productImages, productId);
                }

                return new OkObjectResult(new { Status = status});
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        public List<string> UploadImage()
        {
            DateTime now = DateTime.Now;
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                var lstImages = new List<string>();

                var folderImg = $@"\images\product\{now.ToString("dd-MM-yyyy")}";
                var folder = _hostingEnvironment.WebRootPath + folderImg;
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                foreach (var item in files)
                {
                    var filename = ContentDispositionHeaderValue
                                    .Parse(item.ContentDisposition)
                                    .FileName
                                    .Trim('"');

                    string filePath = Path.Combine(folder, filename);

                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        item.CopyTo(fs);
                        fs.Flush();
                    }

                    var path = Path.Combine(folderImg, filename).Replace(@"\", @"/");

                    lstImages.Add(path);
                }

                return lstImages;                
            }
            return new List<string>();
        }
        [HttpGet]
        public IActionResult GetProductImagesById(int productId)
        {
            var productImages = new List<ProductImagesModel>();
            if (productId > 0)
            {
                productImages = _productImagesService.GetImagesByProductId(productId);                
            }
            bool status = productImages.Count() > 0 ? true : false;
            return new OkObjectResult(new {Status = status,Data = productImages });
        }
        
    }
}
