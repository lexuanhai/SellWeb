using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using WSS.Core.Dto.DataModel;
using WSS.Core.Dto.SearchModel.RoleSearch;
using WSS.Service.ColorService;
using WSS.Service.SizeService;
namespace WebSell.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;
        public ColorController(
            IColorService colorService)
        {
            _colorService = colorService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetPaging(RoleSearch search)
        {
            var model = _colorService.GetSearchPaging(search);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ColorModel colorVM)
        {
            var _message = "";
            try
            {
                if (colorVM.Id > 0)
                {
                    var status = _colorService.SaveEntity(colorVM);
                    _message = status == true ? "Cập nhật thành công" : "Cập nhật không thành công";

                }
                else
                {
                    var status = _colorService.SaveEntity(colorVM);
                    _message = status == true ? "Thêm mới thành công" : "Thêm mới không thành công";
                }
                _colorService.Save();

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
                var model = _colorService.GetColorById(id);

                return new OkObjectResult(new { data= model, status=true });
            }
            return new OkObjectResult(new {status = false });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                var status = _colorService.Delete(id);
                _colorService.Save();
                if (status)                
                    return new OkObjectResult(new { Status = status, Message = "Xóa màu thành công" });
                return new OkObjectResult(new { Status = status, Message = "Xóa màu không thành công" });

            }
            return new OkObjectResult(new { Status = false,Message="Màu không tồn tại" });
        }

       [HttpGet]
       public IActionResult GetAll()
        {
            var model = _colorService.GetAll();
            if (model != null)
            {
                return new OkObjectResult(new { Status = true, Data = model });
            }
            return new OkObjectResult(new { Status = false });
        }
        
    }
}
