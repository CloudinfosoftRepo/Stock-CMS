using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Models;
using Stock_CMS.Service;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUserService userService, IRoleService roleService, IPermissionService permissionService)
        {
            _logger = logger;
             _userService = userService;
            _roleService = roleService;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Users()
        {
            
            var role = Request.Cookies["Role"].ToString();

            if (role != "Admin")
            {
                // You can redirect to a different page or show an access denied message
                return RedirectToAction("Client", "Master");
            }
            var userId = int.Parse(Request.Cookies["UserId"]);
            var perm = await _permissionService.GetPermissionsByUserMenu(userId, 3);
            var actionlist = perm != null && perm.Any() && perm.FirstOrDefault().ActionList != null ? perm.FirstOrDefault().ActionList : null;
            if (actionlist != null && actionlist.Any(x => x.Action.ToUpper() == "VIEW" && x.IsEnabled == true))
            {
                //IEnumerable<ActionItem> ViewBag.ActionList = perm.FirstOrDefault().ActionList;
                return View(perm.FirstOrDefault().ActionList);
            }
            return View("~/Views/Shared/Error.cshtml");

        }

        public async Task<IActionResult> EditUser() 
        {
            var role = HttpContext.Request.Cookies["Role"];
            if (role != "Admin")
            {
                return RedirectToAction("Client", "Master");
            }
            var userId = int.Parse(Request.Cookies["UserId"]);
            var perm = await _permissionService.GetPermissionsByUserMenu(userId, 3);
            var actionlist = perm != null && perm.Any() && perm.FirstOrDefault().ActionList != null ? perm.FirstOrDefault().ActionList : null;
            if (actionlist != null && actionlist.Any(x => x.Action.ToUpper() == "VIEW" && x.IsEnabled == true))
            {
                //IEnumerable<ActionItem> ViewBag.ActionList = perm.FirstOrDefault().ActionList;
                return View(perm.FirstOrDefault().ActionList);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpGet]
        public async Task<JsonResult> GetUserByRole()
        {
            try
            {
                var user = await _userService.GetUserByRole();
                return Json(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(ex.Message);
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetRole()
        {
            try
            {
                var role = await _roleService.GetRole();
                return Json(role.Where(x => x.Id != 1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetRoles()
        {
            try
            {
                var role = await _roleService.GetRole();
                return Json(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateUsers([FromBody] UserDto user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = HttpContext.Request.Cookies["UserId"];

                    user.IsActive = true;
                    user.CreatedAt = DateTime.Now;
                    user.CreatedBy = int.Parse(userId);

                    var result = await _userService.AddUsers(user);
                    user.Id = result;
                    string message = result == -1 ? "User already exists." :
                     result == 0 ? "Failed to add User." :
                     "User added successfully.";
                    bool success = result > 0;
                    return Json(new { success = success, message = message, user });
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
        public async Task<JsonResult> UpdateUser([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                user.UpdatedBy = int.Parse(userId);
                var result = await _userService.UpdateUsers(user);
                string message = result == -2 ? "No record Found." :
                   result == -1 ? "User already exists." :
                   result == 0 ? "Failed to update User." :
                   "User updated successfully.";
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
        public async Task<JsonResult> UpdateUserbyColumn([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];

                user.UpdatedBy = int.Parse(userId);
                user.UpdatedAt = DateTime.Now;
                var result = await _userService.UpdateUserbyColumn(user);
                string message = result == -2 ? "No record Found." :
                  result == -1 ? "User already exists." :
                  result == 0 ? "Failed to delete User." :
                  "User Deleted successfully.";
                bool success = result > 0;
                return Json(new { success = success, message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Delete");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetUserById()
        {
            try
            {
                var role = HttpContext.Request.Cookies["Role"];
                if (role != "Admin")
                {
                    return null;
                }
                var userId = int.Parse(HttpContext.Request.Cookies["UserId"]);

                var user = await _userService.GetUserById(userId);
                return Json(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(ex.Message);
            }
        }
    }
}
