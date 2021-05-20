using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WSS.Core.Common.Extensions;
using WSS.Core.Domain.Entities;
using WSS.Core.Dto.DataModel;

namespace WebSell.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;
        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            User.GetSpecificClaim("Email");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userName, password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                   
                    return new OkObjectResult(new { Status = 1, Message = "Đăng nhập thành công" });
                }
                if (result.IsLockedOut)
                {
                    return new ObjectResult(new { Status = 2, Message = "Tài khoản của bạn đã bị khóa" });
                }
                else
                {
                    return new ObjectResult(new { Status = 3, Message = "Đăng nhập không thành công" });
                }
            }
            return new ObjectResult(new { });
        }

    }
}
