using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
	public class EnquiryController : Controller
	{
		private readonly IStockService _StockService;
		private readonly ILogger<EnquiryController> _logger;

		public EnquiryController(ILogger<EnquiryController> logger, IStockService StockService)
		{
			_logger = logger;
			_StockService = StockService;
		}

		public IActionResult Index()
		{
			return View();
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
		public async Task<JsonResult> DeleteStock(long id)
		{
			// var product = await _context.TblProducts.FindAsync(id);
			//if (product == null) return Json(new { success = false });

			// _context.TblProducts.Remove(product);
			//await _context.SaveChangesAsync();
			return Json(new { success = true });
		}
	}
}
