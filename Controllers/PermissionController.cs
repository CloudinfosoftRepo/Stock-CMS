using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stock_CMS.Models;
using Stock_CMS.ServiceInterface;
using System;

namespace TulsiLogistics.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(PermissionController));
        public PermissionController(IPermissionService permissionService)
        {

            _permissionService = permissionService;

        }

        // Permissions

        public IActionResult Index()
        {
            var userId = int.Parse(Request.Cookies["UserId"]);

            if (userId == 1)
            {
                return View();
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        public async Task<JsonResult> GetMenu()
        {
            try
            {
                var menu = await _permissionService.GetMenu();
                return Json(menu);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Json(ex.Message);
            }
        }

        public async Task<JsonResult> GetMenuByUser(int UserId)
        {
            try
            {
                var menu = await _permissionService.GetMenuByUser(UserId);
                return Json(menu);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Json(ex.Message);
            }
        }

        public async Task<JsonResult> GetMenuByRole(int RoleId)
        {
            try
            {
                var menu = await _permissionService.GetMenuByRole(RoleId);
                return Json(menu);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Json(ex.Message);
            }
        }

        public async Task<JsonResult> GetPermissionsByRole(int roleId)
        {
            try
            {
                var perm = await _permissionService.GetPermissionsByRole(roleId);
                return Json(perm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Json(ex.Message);
            }
        }

        public async Task<JsonResult> GetPermissionsByUser(int userId)
        {
            try
            {
                var perm = await _permissionService.GetPermissionsByUserId(userId);
                return Json(perm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return Json(ex.Message);
            }
        }

        public async Task<dynamic> UpdatePermission([FromBody] PermissionDto perm)
        {
            try
            {
                perm.UpdatedBy = int.Parse(Request.Cookies["UserId"]);
                if (perm.UserId != null && perm.RoleId == null)
                {
                    var result = await _permissionService.UpdatePermissionByUserMenu(perm);
                    return result;
                }
                else if (perm.RoleId != null && perm.UserId == null)
                {
                    var result = await _permissionService.UpdatePermissionByRoleMenu(perm);
                    return result;
                }
                return new { Status = false, Message = "Failed To Update Permissions" };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return null;
            }
        }

    }
}
