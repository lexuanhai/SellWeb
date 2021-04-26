using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebSell.Areas.Admin.Models;
using WSS.Service.FunctionService;

namespace WebAdmin.Areas.Admin.Components.SileBar
{
    public class SileBar: ViewComponent
    {
        private readonly IFunctionService _functionService;

        public SileBar(IFunctionService functionService)
        {
            _functionService = functionService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var functions = _functionService.GetAll();
            var model = new FunctionViewModel();
            if (functions != null && functions.Count > 0)
            {
                model.Funcions = functions;
            }
            //var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            //var functions = new List<FunctionModel>();
            ////if (roles.Split(";").Contains(CommonConstants.AdminRole))
            ////{
            ////    functions = await _functionService.GetAll();
            ////}
            ////else
            ////{
            ////    functions = new List<FunctionModel>();
            ////}
            //functions = await _functionService.GetAll();
            return View(model);
        }
    }
}
