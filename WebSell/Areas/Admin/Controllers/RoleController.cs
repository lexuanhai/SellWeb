//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using WebAdmin.Areas.Admin.Models;
//using WSS.Core.Common.Extensions;
//using WSS.Core.Dto.DataModel;
//using WSS.Core.Dto.SearchModel.RoleSearch;
//using WSS.Core.Dto.SearchModel.UserSearch;
//using WSS.Service.FunctionService;
//using WSS.Service.RoleService;
//using WSS.Service.UserService;

//namespace WebSell.Areas.Admin.Controllers
//{    
//    public class RoleController : BaseController
//    {
//        private readonly IRoleService _roleService;
//        private readonly IFunctionService _functionService;
//        public RoleController(
//            IRoleService roleService,
//            IFunctionService functionService)
//        {
//            _roleService = roleService;
//            _functionService = functionService;
//        }
//        public IActionResult Index()
//        {            
//            return View();
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var model = await _roleService.GetAllAsync();
//            return new OkObjectResult(model);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetRoleById(Guid id)
//        {
//            var model = await _roleService.GetRoleById(id);
//            return new OkObjectResult(model);
//        }
//        [HttpPost]
//        public async Task<IActionResult> SaveEntity(RoleModel roleVM)
//        {
//            var _message = "";
//            bool status = false;
//            bool isExist = false;
//            try
//            {
//                if (!roleVM.Id.HasValue)
//                {
//                    isExist = await _roleService.IsExist(roleVM.Name);
//                    if (!isExist)
//                    {
//                        status = await _roleService.AddAsync(roleVM);
//                        _message = status ? "Thêm mới Role thành công" : "Thêm mới Role không thành công";
//                    }
//                    else
//                    {
//                        _message = "Tên Role đã tồn tại trong hệ thống";
//                    }                                     
//                }
//                else
//                {
//                    status = await _roleService.UpdateAsync(roleVM);

//                   _message = status ? "Cập nhật Role thành công" : "Cập nhật Role không thành công";
//                }                              

//                return new OkObjectResult(new { Status = status, Message = _message });
//            }
//            catch (Exception ex)
//            {
//                return new OkObjectResult(new { status = false, message = "có lỗi xảy ra" });
//            }
//        }

//       [HttpPost]
//        public async Task<IActionResult> Delete(Guid id)
//        {
//            var result = await _roleService.DeleteAsync(id);
//            if (result)
//            {
//                return new OkObjectResult(new {Status = true, Message ="Xóa Role thành công" });
//            }
//            return new OkObjectResult(new { Status = true, Message = "Xóa Role không thành công" });
//        }
//        //[HttpGet]
//        //public IActionResult GetRoleById(int id)
//        //{
//        //    if (id > 0)
//        //    {
//        //        var model = _roleService.GetRoleById(id);

//        //        return new OkObjectResult(new { data = model, status = true });
//        //    }
//        //    return new OkObjectResult(new { status = false });
//        //}

//        //[HttpPost]
//        //public IActionResult Delete(int id)
//        //{
//        //    if (id > 0)
//        //    {
//        //        var status = _roleService.Delete(id);
//        //        _roleService.Save();
//        //        if (status)
//        //            return new OkObjectResult(new { Status = status, Message = "Xóa quyền thành công" });
//        //        return new OkObjectResult(new { Status = status, Message = "Xóa quyền không thành công" });

//        //    }
//        //    return new OkObjectResult(new { Status = false, Message = "Quyền không tồn tại" });
//        //}

//        [HttpGet]
//        public IActionResult ListFunctionByRole(Guid roleId)
//        {
//            var functions = _roleService.GetListFunctionWithRole(roleId);
//            return new OkObjectResult(functions);
//        }
//        //[HttpGet]
//        //public IActionResult GetAllRole()
//        //{
//        //    var roles = _roleService.GetAll();
//        //    if (roles.Count > 0)
//        //    {
//        //        return new OkObjectResult(new { Data = roles, Status = true });
//        //    }
//        //    return new OkObjectResult(new { Status = false });
//        //}

//        [HttpGet]
//        public IActionResult ListAllFunction()
//        {
//            var functions = _functionService.GetAll();
//            return new OkObjectResult(new { Data = functions });
//        }

//        [HttpPost]
//        public IActionResult SavePermission(List<PermissionModel> listPermmission, Guid roleId)
//        {
//           var status = _roleService.SavePermission(listPermmission, roleId);
//            return new OkObjectResult(new { Status = status});
//        }

//    }
//}
