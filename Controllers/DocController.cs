﻿using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;
using Stock_CMS.ViewModel;

namespace Stock_CMS.Controllers
{
	public class DocController : Controller
	{
		private readonly IDocService _docService;
		private readonly ILogger<MasterController> _logger;
		private readonly IBankService _bankService;
        private readonly IHolderDocService _holderDocService;
        private readonly IPermissionService _permissionService;

		public DocController(ILogger<MasterController> logger, IDocService DocService,IBankService bankService, IHolderDocService holderDocService,IPermissionService permissionService)
		{
			_logger = logger;
			_docService = DocService;
			_bankService = bankService;
            _holderDocService = holderDocService;
            _permissionService = permissionService;
		}
		public async Task<IActionResult> Doc()
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

        public async Task<IActionResult> HolderDoc1()
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

        public async Task<ActionResult> GetDocById(long id)
        {
            try
            {
                var Docs = await _docService.GetDocById(id);
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
		[HttpPost]
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

        [HttpPost]
        public async Task<JsonResult> UpdateDocbyColumn([FromBody] DocDto data)
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
                var result = await _docService.UpdateDocbyColumn(data);
                string message = result == -2 ? "No record Found." :
                  result == -1 ? "Holder already exists." :
                  result == 0 ? "Failed to update Holder." :
                  "Holder updated successfully.";
                bool success = result > 0;
                return Json(new { success = success, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Update");
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetBank(long id)
        {
            try
            {
                var Docs = await _bankService.GetBankByClientId(id);
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
        public async Task<ActionResult> GetLegalHeirBank(long id)
        {
            try
            {
                var Docs = await _bankService.GetBankByLegalHeirId(id);
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
        public async Task<IActionResult> createBank([FromBody] BankDto bank)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    bank.IsActive = true;
                    bank.CreatedAt = DateTime.Now;
                    bank.CreatedBy = int.Parse(userId);

                    var result = await _bankService.AddBank(bank);
                    bank.Id = result;
                    string message = result == -1 ? "BAnk already exists." :
                     result == 0 ? "Failed to add Bank." :
                     $"Bank added successfully.";
                    bool success = result > 0;
                    return Json(new { success = success, message = message, bank });
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
        public async Task<IActionResult> createBank1([FromBody] BankDto bank)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    bank.IsActive = true;
                    bank.CreatedAt = DateTime.Now;
                    bank.CreatedBy = int.Parse(userId);

                    var result = await _bankService.AddBank1(bank);
                    bank.Id = result;
                    string message = result == -1 ? "Bank already exists." :
                     result == 0 ? "Failed to add Bank." :
                     $"Bank added successfully.";
                    bool success = result > 0;
                    return Json(new { success = success, message = message, bank });
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

        public async Task<IActionResult> updateBank([FromBody] BankDto bank)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                bank.UpdatedBy = int.Parse(userId);
                var result = await _bankService.UpdateBank(bank);
                string message = result == -2 ? "No record Found." :
                   //result == -1 ? "Doc already exists." :
                   result == 0 ? "Failed to update Bank." :
                   "Bank updated successfully.";
                bool success = result > 0;
                return Json(new { success = success, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Update");
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> updateBank1([FromBody] BankDto bank)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                bank.UpdatedBy = int.Parse(userId);
                var result = await _bankService.UpdateBank1(bank);
                string message = result == -2 ? "No record Found." :
                   //result == -1 ? "Doc already exists." :
                   result == 0 ? "Failed to update Bank." :
                   "Bank updated successfully.";
                bool success = result > 0;
                return Json(new { success = success, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Update");
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> HolderDoc()
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
        public async Task<ActionResult> GetHolderDoc(long id)
        {
            try
            {
                var HolderDocs = await _holderDocService.GetHolderDocByHolderId(id);
                return Json(HolderDocs);
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
        public async Task<ActionResult> GetHolderDoc1(long id)
        {
            try
            {
                var HolderDocs = await _holderDocService.GetHolderDocByLegalHeirId(id);
                return Json(HolderDocs);
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
        public async Task<IActionResult> createHolderDoc([FromForm] HolderDocsDto holderdoc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    holderdoc.IsActive = true;
                    holderdoc.CreatedAt = DateTime.Now;
                    holderdoc.CreatedBy = int.Parse(userId);

                    var result = await _holderDocService.AddHolderDoc(holderdoc);
                    holderdoc.Id = result;
                    string message = result == -1 ? "HolderDoc already exists." :
                     result == 0 ? "Failed to add HolderDoc." :
                     $"HolderDoc added successfully.";
                    bool success = result > 0;
                    return Json(new { success = success, message = message, holderdoc });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during add");
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Invalid data.", errors = ModelState.Values.SelectMany(v => v.Errors) });
        }
        public async Task<IActionResult> updateHolderDoc([FromForm] HolderDocsDto holderDoc)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                holderDoc.UpdatedBy = int.Parse(userId);
                var result = await _holderDocService.UpdateHolderDoc(holderDoc);
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "Doc already exists." :
                   result == 0 ? "Failed to update HolderDoc." :
                   "DolderDoc updated successfully.";
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
        public async Task<JsonResult> UpdateWitnessJson(long id, string jsonString)
        {
            if (id < 1 && string.IsNullOrEmpty(jsonString))
            {
                return Json(new { success = false, message = "Values cannot be Empty" });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];
                var result = await _docService.UpdateWitnessJson(id, jsonString, int.Parse(userId));
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "Witness already exists." :
                   result == 0 ? "Failed to update Witnessess." :
                   "Witnessess updated successfully.";
                bool success = result > 0;
                return Json(new { success = success, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Update");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRelationMapping(long id, string holdertype)
        {
            try
            {
                var relation = await _docService.GetRelationMapping(id, holdertype);
                return Json(relation);
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
        public async Task<JsonResult> AddRelationMapping([FromBody] RelationViewModel data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    foreach (var item in data.relationdata)
                    {
                        item.IsActive = true;
                        item.CreatedAt = DateTime.Now;
                        item.CreatedBy = int.Parse(userId);
                    }

                    var result = await _docService.AddRelationMapping(data.relationdata);
                    string message = result == -1 ? "RelationMapping already exists." :
                     result == 0 ? "Failed to add RelationMapping." :
                     $"RelationMapping added successfully.";
                    bool success = result > 0;
                    return Json(new { success = success, message = message });
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
        public async Task<JsonResult> UpdateRelationMapping([FromBody]RelationViewModel data)
        {
            if (!ModelState.IsValid && string.IsNullOrEmpty(data.holdertype))
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                foreach (var item in data.relationdata)
                {
                    if (item.Id > 0)
                    {
                        item.UpdatedBy = int.Parse(userId);
                        item.UpdatedAt = DateTime.Now;
                    }
                    else
                    {
                        item.IsActive = true;
                        item.CreatedAt = DateTime.Now;
                        item.CreatedBy = int.Parse(userId);
                    }

                }

                var result = await _docService.UpdateRelationMapping(data.holdertype, data.relationdata);
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "RelationMapping already exists." :
                   result == 0 ? "Failed to update RelationMapping." :
                   "RelationMapping updated successfully.";
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
