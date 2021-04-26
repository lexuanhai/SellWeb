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
//using WSS.Service.RoleService;
using WSS.Service.UserService;

namespace WebSell.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FunctionController : Controller
    {
        private readonly IFunctionService _functionService;
        public FunctionController(
            IFunctionService functionService)
        {
            _functionService = functionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetPaging(FunctionSearch search)
        {
            var model = _functionService.GetSearchPaging(search);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(FunctionModel functionVM)
        {
            var _message = "";
            try
            {
                if (functionVM.Id > 0)
                {
                    var status = _functionService.SaveEntity(functionVM);
                    _message = status == true ? "Cập nhật thành công" : "Cập nhật không thành công";

                }
                else
                {
                    var status = _functionService.SaveEntity(functionVM);
                    _message = status == true ? "Thêm mới thành công" : "Thêm mới không thành công";
                }
                _functionService.Save();

                return new OkObjectResult(new { status = true, message = _message });
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new { status = false, message = "có lỗi xảy ra" });
            }
        }

        [HttpGet]
        public IActionResult GetFunctionById(int id)
        {
            if (id > 0)
            {
                var model = _functionService.GetFunctioneById(id);

                return new OkObjectResult(new { data= model, status=true });
            }
            return new OkObjectResult(new {status = false });
        }

        [HttpGet]
        public IActionResult GetParentFunction()
        {
            var model = _functionService.GetParentFunction();

            if (model != null && model.Count > 0)
            {
                return new OkObjectResult(new { data = model, Status = true });
            }
            return new OkObjectResult(new { Status = false });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                var status = _functionService.Delete(id);
                _functionService.Save();
                if (status)                
                    return new OkObjectResult(new { Status = status, Message = "Xóa danh mục thành công" });
                return new OkObjectResult(new { Status = status, Message = "Xóa danh mục không thành công" });

            }
            return new OkObjectResult(new { Status = false,Message="Danh mục không tồn tại" });
        }
    }
}
