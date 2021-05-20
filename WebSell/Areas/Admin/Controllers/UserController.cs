using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebAdmin.Areas.Admin.Models;
using WebSell.Authorization;
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
        private readonly IAuthorizationService _authorizationService;
        //private readonly IUserRoleService _userRoleService;
        public UserController(
            IUserService userService, IAuthorizationService authorizationService)
           // IUserRoleService userRoleService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            //_userRoleService = userRoleService;
        }
        public async Task<IActionResult> Index()
        {
            //var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);
            //if (result.Succeeded == false)
            //    return new RedirectResult("/Admin/Login/Index");
            return View();
        }        

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var model = await _userService.GetAllAsync();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(UserModel userVM)
        {
            //User.Identity.Name
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
