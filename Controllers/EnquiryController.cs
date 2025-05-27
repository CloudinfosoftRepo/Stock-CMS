using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
	public class EnquiryController : Controller
	{
		private readonly IStockService _StockService;
		private readonly ILogger<EnquiryController> _logger;
        private readonly ITrackingService _TrackingService;
        private readonly IPermissionService _permissionService;

		public EnquiryController(ILogger<EnquiryController> logger, IStockService StockService,ITrackingService trackingService,IPermissionService permissionService)
		{
			_logger = logger;
			_StockService = StockService;
            _TrackingService = trackingService;
            _permissionService = permissionService;
		}

		public async Task<IActionResult> Index()
		{
            var userId = int.Parse(Request.Cookies["UserId"]);
            var perm = await _permissionService.GetPermissionsByUserMenu(userId, 2);
            var actionlist = perm != null && perm.Any() && perm.FirstOrDefault().ActionList != null ? perm.FirstOrDefault().ActionList : null;
            if (actionlist != null && actionlist.Any(x => x.Action.ToUpper() == "VIEW" && x.IsEnabled == true))
            {
                //IEnumerable<ActionItem> ViewBag.ActionList = perm.FirstOrDefault().ActionList;
                return View(perm.FirstOrDefault().ActionList);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

		[HttpGet]
		public async Task<ActionResult> GetStocks(long id)
		{
			try
			{
				var Stocks = await _StockService.GetStockByClientId(id);
				return Json(Stocks);
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
		public async Task<ActionResult> GetStocksById(long id)
		{
			try
			{
				var Stocks = await _StockService.GetStockById(id);
				return Json(Stocks);
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
		public async Task<JsonResult> CreateStock([FromBody] StockDto Stock)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var userId = HttpContext.Request.Cookies["UserId"];

					Stock.IsActive = true;
					Stock.CreatedAt = DateTime.Now;
					Stock.CreatedBy = int.Parse(userId);

					var result = await _StockService.AddStock(Stock);
					Stock.Id = result;
					string message = result == -1 ? "Stock already exists." :
					 result == 0 ? "Failed to add Stock." :
					 $"Stock added successfully.";
					bool success = result > 0;
					return Json(new { success = success, message = message, Stock });
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
		public async Task<JsonResult> UpdateStock([FromBody] StockDto Stock)
		{
			if (!ModelState.IsValid)
			{
				return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
			}

			try
			{
				var userId = HttpContext.Request.Cookies["UserId"];

				Stock.UpdatedBy = int.Parse(userId);
				var result = await _StockService.UpdateStock(Stock);
				string message = result == -2 ? "No record Found." :
				   result == -1 ? "Stock already exists." :
				   result == 0 ? "Failed to update Stock." :
				   "Stock updated successfully.";
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
        public async Task<JsonResult> UpdateStockJson([FromBody]StockJsonUploadDto data)
        {
            if (data.Id < 1 && string.IsNullOrEmpty(data.JsonString))
            {
                return Json(new { success = false, message = "Values cannot be Empty" });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];
                var result = await _StockService.UpdateStockJson(data.Id, data.JsonString, int.Parse(userId));
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "Stock already exists." :
                   result == 0 ? "Failed to update Stock." :
                   "Stock updated successfully.";
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
        public async Task<JsonResult> DeleteStockbyColumn([FromBody] StockDto stock)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                stock.UpdatedBy = int.Parse(userId);
                stock.UpdatedAt = DateTime.Now;
                var result = await _StockService.DeleteStockbyColumn(stock);
                string message = result == -2 ? "No record Found." :
                  result == -1 ? "Client already exists." :
                  result == 0 ? "Failed to update Client." :
                  "Convert to Client successfully.";
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
		public async Task<JsonResult> DeleteStock(long id)
		{
			// var product = await _context.TblProducts.FindAsync(id);
			//if (product == null) return Json(new { success = false });

			// _context.TblProducts.Remove(product);
			//await _context.SaveChangesAsync();
			return Json(new { success = true });
		}

        [HttpGet]
        public async Task<ActionResult> GetHeader(long id)
        {
            try
            {
                var header = await _StockService.GetStockById(id);
                return Json(header);
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

        public async Task<IActionResult> Tracking()
        {
            var userId = int.Parse(Request.Cookies["UserId"]);
            var perm = await _permissionService.GetPermissionsByUserMenu(userId, 2);
            var actionlist = perm != null && perm.Any() && perm.FirstOrDefault().ActionList != null ? perm.FirstOrDefault().ActionList : null;
            if (actionlist != null && actionlist.Any(x => x.Action.ToUpper() == "VIEW" && x.IsEnabled == true))
            {
                //IEnumerable<ActionItem> ViewBag.ActionList = perm.FirstOrDefault().ActionList;
                return View(perm.FirstOrDefault().ActionList);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> GetTracking(long id)
        {
            try
            {
                var tracking = await _TrackingService.GetTracking();
                return Json(tracking);
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
        public async Task<ActionResult> GetTrackingById(long id)
        {
            try
            {
                var Tracking = await _TrackingService.GetTrackingbyStockId(id);
                return Json(Tracking);
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
        public async Task<JsonResult> CreateTracking(string Tracking, IFormFile? clientSend, IFormFile? clientResponse)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newdata = JsonConvert.DeserializeObject<TrackingDto>(Tracking);
                    var userId = HttpContext.Request.Cookies["UserId"];

                    newdata.IsActive = true;
                    newdata.SendFile = clientSend;
                    newdata.ResponseFile = clientResponse;
                    newdata.CreatedAt = DateTime.Now;
                    newdata.CreatedBy = int.Parse(userId);

                    var result = await _TrackingService.AddTracking(newdata);
                    newdata.Id = result;
                    string message = result == -1 ? "Tracking already exists." :
                     result == 0 ? "Failed to add Tracking." :
                     $"Tracking added successfully.";
                    bool success = result > 0;
                    return Json(new { success = success, message = message, newdata });
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
        public async Task<JsonResult> UpdateTracking(string Tracking, IFormFile? clientSend, IFormFile? clientResponse)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var newdata = JsonConvert.DeserializeObject<TrackingDto>(Tracking);
                var userId = HttpContext.Request.Cookies["UserId"];

                newdata.UpdatedBy = int.Parse(userId);
                newdata.SendFile = clientSend;
                newdata.ResponseFile = clientResponse;
                var result = await _TrackingService.UpdateTracking(newdata);
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "Tracking already exists." :
                   result == 0 ? "Failed to update Tracking." :
                   "Tracking updated successfully.";
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
        public async Task<JsonResult> UpdateTrackingbyColumn([FromBody] TrackingDto data)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                data.UpdatedBy = int.Parse(userId);
                data.UpdatedAt = DateTime.Now;
                var result = await _TrackingService.UpdateTrackingbyColumn(data);
                string message = result == -1 ? "No record Found." :
                  result == 0 ? "Failed to delete Form." :
                  "Form deleted successfully.";
                bool success = result > 0;
                return Json(new { success = success, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during delete");
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<JsonResult> UploadDoc([FromForm] UploadStockDto data)
        {
            if (ModelState.IsValid)
            {
                if (data.CustomerId == null || data.CustomerId == 0)
                {
                    return Json(new { success = false, message = "Please Go Through a Client to add Stocks", errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    var result = await _StockService.UploadStockExcel(data,Int32.Parse(userId ?? "0"));
                    //Stock.Id = result;
                    //string message = result == -1 ? "Stock already exists." :
                    // result == 0 ? "Failed to add Stock." :
                    // $"Stock added successfully.";
                    //bool success = result > 0;
                    return Json(new { success = true, message = result });
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
        public async Task<JsonResult> UpdateNomineeJson(long id, string jsonString)
        {
            if (id < 1 && string.IsNullOrEmpty(jsonString))
            {
                return Json(new { success = false, message = "Values cannot be Empty" });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];
                var result = await _StockService.UpdateNomineeJson(id, jsonString, int.Parse(userId));
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "Nominee already exists." :
                   result == 0 ? "Failed to update Nominees." :
                   "Nominees updated successfully.";
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
