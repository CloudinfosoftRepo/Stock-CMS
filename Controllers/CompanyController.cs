using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Models;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IRtaService _rtaService;
        private readonly ICompanyService _companyService;
        private readonly ILogger<MasterController> _logger;
        public CompanyController(ILogger<MasterController> logger, IRtaService RtaService, ICompanyService CompanyService)
        {
            _logger = logger;
            _rtaService = RtaService;
            _companyService = CompanyService;
        }
        public IActionResult Rta()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetRta()
        {
            try
            {
                var Rta = await _rtaService.GetRta();
                return Json(Rta);
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
        public async Task<IActionResult> createRta([FromBody] RtaDto data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    data.IsActive = true;
                    data.CreatedAt = DateTime.Now;
                    data.CreatedBy = int.Parse(userId);

                    var result = await _rtaService.AddRta(data);
                    data.Id = result;
                    string message = result == -1 ? "Rta already exists." :
                     result == 0 ? "Failed to add Rta." :
                     $"Rta added successfully.";
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
        public async Task<IActionResult> updateRta([FromBody] RtaDto data)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                data.UpdatedBy = int.Parse(userId);
                var result = await _rtaService.UpdateRta(data);
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "Rta already exists." :
                   result == 0 ? "Failed to update Rta." :
                   "Rta updated successfully.";
                bool success = result > 0;
                return Json(new { success = success, message = message });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Update");
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult Company()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetCompany()
        {
            try
            {
                var data = await _companyService.GetCompany();
                return Json(data);
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
        public async Task<IActionResult> createCompany([FromBody] CompanyDto data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    data.IsActive = true;
                    data.CreatedAt = DateTime.Now;
                    data.CreatedBy = int.Parse(userId);

                    var result = await _companyService.AddCompany(data);
                    data.Id = result;
                    string message = result == -1 ? "Company already exists." :
                     result == 0 ? "Failed to add Company." :
                     $"Company added successfully.";
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
        public async Task<IActionResult> updateCompany([FromBody] CompanyDto data)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                data.UpdatedBy = int.Parse(userId);
                var result = await _companyService.UpdateCompany(data);
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "Company already exists." :
                   result == 0 ? "Failed to update Company." :
                   "Company updated successfully.";
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
