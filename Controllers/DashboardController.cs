using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
    public class DashboardController : Controller
    {

        private readonly ICustomerService _customerService;
        private readonly ILogger<DashboardController> _logger;
        private readonly IStockService _stockService;
        private readonly IPermissionService _permissionService;

        public DashboardController(ICustomerService customerService, ILogger<DashboardController> logger, IStockService stockService, IPermissionService permissionService)
        {
            _customerService = customerService;
            _logger = logger;
            _stockService = stockService;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = int.Parse(Request.Cookies["UserId"]);
            var perm = await _permissionService.GetPermissionsByUserMenu(userId, 1);
            var actionlist = perm != null && perm.Any() && perm.FirstOrDefault().ActionList != null ? perm.FirstOrDefault().ActionList : null;
            if (actionlist != null && actionlist.Any(x => x.Action.ToUpper() == "VIEW" && x.IsEnabled == true))
            {
                //IEnumerable<ActionItem> ViewBag.ActionList = perm.FirstOrDefault().ActionList;
                return View(perm.FirstOrDefault().ActionList);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> GetEnqiryCustomersCount()
        {
            try
            {
                var customers = await _customerService.GetEnqiryCustomersCount();
                return Json(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during fetch");
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetStocksStatusCount()
        {
            try
            {
                var customers = await _stockService.GetStocksStatusCount();
                return Json(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during fetch");
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetStocksByStatus()
        {
            try
            {
                var customers = await _stockService.GetStocksByStatus();
                return Json(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during fetch");
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }

        }


        [HttpGet]
        public async Task<ActionResult> GetStocksByStatusName(string val)
        {
            try
            {
                var customers = await _stockService.GetStocksByStatusName(val);
                return Json(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during fetch");
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetUnpaidAmountByClient()
        {
            try
            {
                var customers = await _stockService.GetUnpaidAmountByClient();
                return Json(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during fetch");
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
