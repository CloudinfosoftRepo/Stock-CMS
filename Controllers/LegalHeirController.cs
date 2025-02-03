using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Models;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
    public class LegalHeirController : Controller
    {
        private readonly ILegalHeirService _legalHeirService;
        private readonly ILogger<LegalHeirController> _logger;

        public LegalHeirController(ILegalHeirService legalHeirService, ILogger<LegalHeirController> logger)
        {
            _legalHeirService = legalHeirService;
            _logger = logger;
        }

        public IActionResult LegalHeir()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetLegalHeir(long id)
        {
            try
            {
                var Docs = await _legalHeirService.GetLegalHeirByClientId(id);
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
        [HttpPost]
        public async Task<IActionResult> create([FromForm] LegalHeirDto data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    data.IsActive = true;
                    data.CreatedAt = DateTime.Now;
                    data.CreatedBy = int.Parse(userId);

                    var result = await _legalHeirService.AddLegalHeir(data);
                    data.Id = result;
                    string message = result == -1 ? "Legal Heir already exists." :
                     result == 0 ? "Failed to add Legal Heir." :
                     $"Legal Heir added successfully.";
                    bool success = result > 0;
                    return Json(new { success = success, message = message, data });
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
        public async Task<IActionResult> update([FromForm] LegalHeirDto data)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                data.UpdatedBy = int.Parse(userId);
                var result = await _legalHeirService.UpdateLegalHeir(data);
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "Legal Heir already exists." :
                   result == 0 ? "Failed to update Legal Heir." :
                   "Legal Heir updated successfully.";
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
        public async Task<JsonResult> UpdateLegalHeirbyColumn([FromBody] LegalHeirDto data)
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
                var result = await _legalHeirService.UpdateLegalHeirbyColumn(data);
                string message = result == -2 ? "No record Found." :
                  result == -1 ? "Legal Heir already exists." :
                  result == 0 ? "Failed to update Legal Heir." :
                  "Legal Heir updated successfully.";
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
