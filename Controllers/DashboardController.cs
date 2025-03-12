using Microsoft.AspNetCore.Mvc;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
    public class DashboardController : Controller
    {

        private readonly ICustomerService _customerService;
        private readonly ILogger<DashboardController> _logger;
        private readonly IStockService _stockService;

        public DashboardController(ICustomerService customerService, ILogger<DashboardController> logger, IStockService stockService)
        {
            _customerService = customerService;
            _logger = logger;
            _stockService = stockService;
        }

        public IActionResult Dashboard()
        {
            return View();
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
    }
}
