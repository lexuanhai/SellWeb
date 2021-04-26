using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using WSS.Core.Dto.DataModel;
using WSS.Service.BillService;
using WSS.Service.ColorService;
using WSS.Service.SizeService;
namespace WebSell.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BillController : Controller
    {
        private readonly IBillService _billService;
        public BillController(
            IBillService billService)
        {
            _billService = billService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SaveEntity(BillModel billVm)
        {           
            if (billVm.Id == 0)
            {
                _billService.Create(billVm);
            }            
            _billService.Save();
            return new OkObjectResult(billVm);
        }        
    }
}
