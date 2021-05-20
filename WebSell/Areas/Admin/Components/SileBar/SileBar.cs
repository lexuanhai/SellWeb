using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebSell.Areas.Admin.Models;
using WSS.Core.Common.Extensions;
using WSS.Core.Common.Utilities;
using WSS.Core.Dto.DataModel;
using WSS.Service.FunctionService;
using WSS.Service.RoleService;

namespace WebAdmin.Areas.Admin.Components.SileBar
{
    public class SileBar: ViewComponent
    {
        private readonly IFunctionService _functionService;
        private readonly IRoleService _roleService;
        public SileBar(IFunctionService functionService,
            IRoleService roleService)
        {
            _functionService = functionService;
            _roleService = roleService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {          
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            var rolesId = new List<string>();            
            List<FunctionModel> functions;
            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions =  _functionService.GetAll();
            }
            else
            {

                // functions = _functionService.GetFunctionByRoleId()
                //if (roles != null && roles.Count() > 0)
                //{
                //    var listRoleToModel = new List<RoleModel>();
                //    foreach (var item in roles)
                //    {
                //        var roleEntity = _roleService.GetRoleByName(item.ToString());
                //        if (roleEntity != null)
                //        {
                //            var roleModel = new RoleModel();
                //            roleModel.Id = roleEnti;
                //            listRoleToModel.Add(roleModel.T);
                //        }
                //    }
                //}
                functions = _functionService.GetAll();//("73D8E119-0AAE-4786-F916-08D907B6FBDC");
            }
            return View(functions);
        }
    }
}
