using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
    public class MasterController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<MasterController> _logger;

        public MasterController(ILogger<MasterController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }
        
        //Customer
        public IActionResult Client()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            try
            {
                var customers = await _customerService.GetCustomer();
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

        [HttpPost]
        public async Task<JsonResult> CreateCustomer([FromBody] CustomerDto customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    customer.IsActive = true;
                    customer.CreatedAt = DateTime.Now;
                    customer.CreatedBy = int.Parse(userId);

                    var result = await _customerService.AddCustomer(customer);
                    customer.Id = result;
                    string message = result == -1 ? "Client already exists." :
                     result == 0 ? "Failed to add Client." :
                     "Client added successfully.";
                    bool success = result > 0;
                    return Json(new { success = success, message = message, customer });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during add");
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Invalid data.", errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateCustomer([FromBody] CustomerDto customer)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                customer.UpdatedBy = int.Parse(userId);
                var result = await _customerService.UpdateCustomer(customer);
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "Client already exists." :
                   result == 0 ? "Failed to update Client." :
				   "Client updated successfully.";
                bool success = result > 0;
                return Json(new { success = success, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Update");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteCustomer(long id)
        {
            // var product = await _context.TblProducts.FindAsync(id);
            //if (product == null) return Json(new { success = false });

            // _context.TblProducts.Remove(product);
            //await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

		[HttpPost]
		public async Task<JsonResult> UpdateCustomerbyColumn([FromBody] CustomerDto customer)
		{
			if (!ModelState.IsValid)
			{
				return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
			}

			try
			{
				var userId = HttpContext.Request.Cookies["UserId"];

				customer.UpdatedBy = int.Parse(userId);
                customer.UpdatedAt = DateTime.Now;
				var result = await _customerService.UpdateCustomerbyColumn(customer);
                string message = result == -2 ? "No record Found." :
                  result == -1 ? "Client already exists." :
                  result == 0 ? "Failed to update Client." :
                  "Client updated successfully.";
                bool success = result > 0;
                return Json(new { success = success, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Update");
                return Json(new { success = false, message = ex.Message });
            }
        }
	}
}
