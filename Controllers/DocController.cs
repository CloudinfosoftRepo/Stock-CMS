using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
	public class DocController : Controller
	{
		private readonly IDocService _docService;
		private readonly ILogger<MasterController> _logger;

		public DocController(ILogger<MasterController> logger, IDocService DocService)
		{
			_logger = logger;
			_docService = DocService;
		}
		public IActionResult Doc()
		{
			return View();
		}

		[HttpGet]
	  public async Task<ActionResult> GetDocs(long id)
		{
			try
			{
				var Docs = await _docService.GetDocByClientId(id);
				return Json(Docs);
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
		public async Task<IActionResult> create([FromForm] DocDto doc)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var userId = HttpContext.Request.Cookies["UserId"];

					doc.IsActive = true;
					doc.CreatedAt = DateTime.Now;
					doc.CreatedBy = int.Parse(userId);

					var result = await _docService.AddDoc(doc);
					doc.Id = result;
					string message = result == -1 ? "Doc already exists." :
					 result == 0 ? "Failed to add Doc." :
					 $"Doc added successfully.";
					bool success = result > 0;
					return Json(new { success = success, message = message, doc });
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error during add");
					return Json(new { success = false, message = ex.Message });
				}
			}
			return Json(new { success = false, message = "Invalid data.", errors = ModelState.Values.SelectMany(v => v.Errors) });
		}
		public async Task<IActionResult> update([FromForm] DocDto doc)
		{
			if (!ModelState.IsValid)
			{
				return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
			}

			try
			{
				var userId = HttpContext.Request.Cookies["UserId"];

				doc.UpdatedBy = int.Parse(userId);
				var result = await _docService.UpdateDoc(doc);
				string message = result == -2 ? "No record Found." :
				   result == -1 ? "Doc already exists." :
				   result == 0 ? "Failed to update Doc." :
				   "Doc updated successfully.";
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
