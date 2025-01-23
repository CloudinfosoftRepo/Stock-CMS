using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Models;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
    public class FormController : Controller
    {

		private readonly ILogger<EnquiryController> _logger;
		private readonly IGenratedFormService _genratedFormService;
		private readonly IFormService _formService;

		public FormController(ILogger<EnquiryController> logger, IGenratedFormService genratedFormService,IFormService formService)
		{
			_logger = logger;
			_genratedFormService = genratedFormService;
			_formService = formService;
		}

		public IActionResult Form()
        {
            return View();
        }
		[HttpPost]
		public async Task<JsonResult> GenrateForm([FromBody] GenratedFormDto data)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var userId = HttpContext.Request.Cookies["UserId"];

					data.IsActive = true;
					data.CreatedAt = DateTime.Now;
					data.CreatedBy = int.Parse(userId);

					var result = await _genratedFormService.GenrateForm(data);
					data.Id = result;
					string message = result == -1 ? "Form already exists." :
					 result == 0 ? "Failed to add Form Data." :
					 $"Form data added successfully.";
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

		[HttpGet]
		public async Task<ActionResult> GetGenratedFormById(long id)
		{
			try
			{
				var result = await _genratedFormService.GetGenratedFormById(id);
				return Json(result);
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
        public async Task<ActionResult> GetFormList()
        {
            try
            {
                var result = await _formService.GetFormList();
                return Json(result);
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
        public async Task<ActionResult> GetGenratedFormByStockId(long id)
        {
            try
            {
                var result = await _genratedFormService.GetGenratedFormByStockId(id);
                return Json(result);
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

        public IActionResult ChangeOfAddress()
        {
            return View();
        }
		

		public IActionResult AFFIDAVITChangeOfAddress()
        {
            return View();
        }        
		public IActionResult AFFIDAVIT003COA()
		{
			return View();
		}   
        public IActionResult AffidavitApplicantNameCorrection()
        {
            return View();
        }    
        public IActionResult AFFIDAVITChangeofNameNewLinkinTime()
        {
            return View();
        }        
        public IActionResult AFFIDAVITDeceasedHolder()
        {
            return View();
        }
		public IActionResult AffidavitforCOSNewLinkinTime()
		{
			return View();
		}
        public IActionResult AffidavitforNameVariation()
		{
			return View();
		}
        public IActionResult AFFIDAVITPedigreeLegalHierCerti()
		{
			return View();
		}
        public IActionResult AgreementasRepresentativeof()
		{
			return View();
		}
        public IActionResult ChangeofAdd()
		{
			return View();
		}  
        public IActionResult ChangeofAddressCorrespondence()
		{
			return View();
		}  
        public IActionResult DocumentsList()
		{
			return View();
		}     
        public IActionResult DupAffA()
		{
			return View();
		} 
        public IActionResult DupIndemnityB()
		{
			return View();
		}  
		public IActionResult FormISR1()
		{
			return View();
		}
		public IActionResult FormISR2()
		{
			return View();
		}		
		public IActionResult FormISR3()
		{
			return View();
		}
        public IActionResult FormISR4()
        {
            return View();
        } 
		public IActionResult FormISR13()
        {
            return View();
        }
        public IActionResult IEPFinfo()
        {
            return View();
        }
        public IActionResult NameChangeAfterMrg()
        {
            return View();
        }
        public IActionResult GenerateForm()
        {
            return View();
        }
    }
}
