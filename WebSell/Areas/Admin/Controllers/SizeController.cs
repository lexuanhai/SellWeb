using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using WSS.Core.Dto.DataModel;
using WSS.Core.Dto.SearchModel.RoleSearch;
using WSS.Service.SizeService;

namespace WebSell.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        private readonly ISizeService _sizeService;
        public SizeController(
            ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetPaging(RoleSearch search)
        {
            var model = _sizeService.GetSearchPaging(search);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(SizeModel colorVM)
        {
            var _message = "";
            try
            {
                if (colorVM.Id > 0)
                {
                    var status = _sizeService.SaveEntity(colorVM);
                    _message = status == true ? "Cập nhật thành công" : "Cập nhật không thành công";

                }
                else
                {
                    var status = _sizeService.SaveEntity(colorVM);
                    _message = status == true ? "Thêm mới thành công" : "Thêm mới không thành công";
                }
                _sizeService.Save();

                return new OkObjectResult(new { status = true, message = _message });
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new { status = false, message = "có lỗi xảy ra" });
            }
        }

        [HttpGet]
        public IActionResult GetColorById(int id)
        {
            if (id > 0)
            {
                var model = _sizeService.GetSizeById(id);

                return new OkObjectResult(new { data= model, status=true });
            }
            return new OkObjectResult(new {status = false });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                var status = _sizeService.Delete(id);
                _sizeService.Save();
                if (status)                
                    return new OkObjectResult(new { Status = status, Message = "Xóa màu thành công" });
                return new OkObjectResult(new { Status = status, Message = "Xóa màu không thành công" });

            }
            return new OkObjectResult(new { Status = false,Message="Màu không tồn tại" });
        }

       [HttpGet]
       public IActionResult GetAll()
        {
            var model = _sizeService.GetAll();
            if (model != null)
            {
                return new OkObjectResult(new { Status = true, Data = model });
            }
            return new OkObjectResult(new { Status = false });
        }
        
    }
}
