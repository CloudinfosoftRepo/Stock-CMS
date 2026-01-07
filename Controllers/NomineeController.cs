using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Models;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
    public class NomineeController : Controller
    {
        private readonly INomineeService _nomineeService;
        private readonly ILogger<NomineeController> _logger;

        public NomineeController(INomineeService nomineeService, ILogger<NomineeController> logger)
        {
            _nomineeService = nomineeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetNomineeByCustomerId(long id)
        {
            try
            {
                var Docs = await _nomineeService.GetNomineeByCustomerId(id);
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

        [HttpGet]
        public async Task<ActionResult> GetNomineeByClientId(long id)
        {
            try
            {
                var Docs = await _nomineeService.GetNomineeByClientId(id);
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

        [HttpGet]
        public async Task<ActionResult> GetNomineeByLegalHeirId(long id)
        {
            try
            {
                var Docs = await _nomineeService.GetNomineeByLegalHeirId(id);
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
        public async Task<IActionResult> createNominee([FromBody] NomineeDto data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    data.IsActive = true;
                    data.CreatedAt = DateTime.Now;
                    data.CreatedBy = int.Parse(userId);

                    var result = await _nomineeService.AddNominee(data);
                    data.Id = result;
                    string message = result == -1 ? "Nominee already exists." :
                     result == 0 ? "Failed to add Nominee." :
                     $"Nominee added successfully.";
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
        public async Task<IActionResult> updateNominee([FromBody] NomineeDto data)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                data.UpdatedBy = int.Parse(userId);
                var result = await _nomineeService.UpdateNominee(data);
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "Nominee already exists." :
                   result == 0 ? "Failed to update Nominee." :
                   "Nominee updated successfully.";
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
        public async Task<JsonResult> UpdateNomineebyColumn([FromBody] NomineeDto data)
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
                var result = await _nomineeService.UpdateNomineebyColumn(data);
                string message = result == -2 ? "No record Found." :
                  result == -1 ? "Nominee already exists." :
                  result == 0 ? "Failed to update Nominee." :
                  "Nominee updated successfully.";
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
