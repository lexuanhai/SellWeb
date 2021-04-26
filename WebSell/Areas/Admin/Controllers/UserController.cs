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
using WSS.Core.Dto.SearchModel.UserSearch;
//using WSS.Service.UserRoleService;
using WSS.Service.UserService;

namespace WebSell.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        //private readonly IUserRoleService _userRoleService;
        public UserController(
            IUserService userService)
           // IUserRoleService userRoleService)
        {
            _userService = userService;
            //_userRoleService = userRoleService;
        }
        public IActionResult Index()
        {
            return View();
        }        

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _userService.GetAllAsync();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(UserModel userVM)
        {
            var _message = "";
            bool status = false;
            try
            {
                if (!userVM.Id.HasValue)
                {
                    status = await _userService.AddAsync(userVM);
                    _message = status ? "Thêm mới User thành công" : "Thêm mới User không thành công";
                }
                else
                {
                    status = await _userService.UpdateAsync(userVM);
                    _message = status ? "Cập nhật User thành công" : "Cập nhật User không thành công";
                }
                return new OkObjectResult(new { Status = status, Message = _message });
            }
            catch (Exception ex)
            {
                return new OkObjectResult(new { status = false, message = "có lỗi xảy ra" });
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.DeleteAsync(id);
            if (result)
            {
                return new OkObjectResult(new { Status = true, Message = "Xóa User thành công" });
            }
            return new OkObjectResult(new { Status = true, Message = "Xóa User không thành công" });
        }

    }
}
