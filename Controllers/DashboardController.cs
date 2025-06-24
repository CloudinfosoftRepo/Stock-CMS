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
        private readonly IFileService _fileService;
        private readonly string _backupPath = @"C:\Program Files (x86)\Plesk\Databases\MSSQL\MSSQL16.MSSQLSERVER2022\MSSQL\Backup\STOCKS.bak"; // Update with your path

        public DashboardController(ICustomerService customerService, ILogger<DashboardController> logger, IStockService stockService, IPermissionService permissionService, IFileService fileService)
        {
            _customerService = customerService;
            _logger = logger;
            _stockService = stockService;
            _permissionService = permissionService;
            _fileService = fileService;
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

        //Database BackUp and Restore methods can be added here if needed in future
        [HttpGet]
        public IActionResult CreateAndDownloadBackup()
        {
            try
            {
                _fileService.CreateBackup(_backupPath);
                if (!System.IO.File.Exists(_backupPath))
                {
                    return NotFound();
                }
                byte[] fileBytes = System.IO.File.ReadAllBytes(_backupPath);
                string fileName = Path.GetFileName(_backupPath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Ok(ex.Message);
            }
        }
    }
}
