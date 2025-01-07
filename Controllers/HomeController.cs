using log4net;
using Microsoft.AspNetCore.Mvc;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.ServiceInterface;
using System.Diagnostics;
using System.Net.Mail;

namespace Stock_CMS.Controllers
{


	public class HomeController : Controller
	{

		private readonly IUserService _user;
		private readonly IRoleService _role;
		private static readonly ILog _logger = LogManager.GetLogger(typeof(HomeController));


		public HomeController(IUserService userService, IRoleService role)
		{
			_user = userService;
			_role = role;
		}


		[HttpPost]
		public async Task<JsonResult> GetUsers(bool status)
		{
			try
			{
				var user = await _user.GetUser(status);
				return Json(user);
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				return Json(ex.Message);
			}
		}
		[HttpGet]
		public async Task<JsonResult> GetUserById()
		{
			try
			{
				int id = int.Parse(Request.Cookies["UserId"]);
				var user = await _user.GetUserById(id);
				return Json(user);
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				return Json(ex.Message);
			}
		}
		[HttpPost]
		public async Task<ActionResult> AddEditUser([FromBody] UserDto user, int flag)
		{
			try
			{
				if (flag == 0)
				{
					user.CreatedBy = int.Parse(Request.Cookies["UserId"]);
				}
				else
				{
					user.UpdatedBy = int.Parse(Request.Cookies["UserId"]);
				}
				var result = await _user.AddUser(user, flag);
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				return StatusCode(500, ex.Message);
			}
		}

        //Login Page
        public IActionResult Index()
        {
            try
            {
                var cookieValue = Request.Cookies["UserId"];
                var userName = Request.Cookies["Username"].ToString();
                var role = Request.Cookies["Role"].ToString();

                if (cookieValue != null)
                {  
                    //return RedirectToAction("Dashboard", "Home");
					return RedirectToAction("Client", "Master");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                if (ex.InnerException != null)
                    _logger.Error(ex.InnerException.ToString());
            }
            return View();
        }

        public IActionResult ForgotPass()
		{
			return View();
		}
		public IActionResult Privacy()
		{
			return View();
		}

		//public IActionResult Dashboard()
		//{
		//	return View();
		//}

		[HttpPost]
		public async Task<JsonResult> Login([FromBody] SignInRequest signInReques)
		{
			try
			{

				var userList = await _user.Login(signInReques.LoginUsername, signInReques.Password);
				var roleList = await _role.GetRole();

				if (!userList.Any())
				{
					var result = new
					{
						status = false,
						message = "Usename or Password not match",
					};

					return Json(result);
				}
				else
				{
					var response = userList.FirstOrDefault();
					var role = roleList.Where(x => x.Id == response?.RoleId).First();

					HttpContext.Session.SetString("UserId", response.Id.ToString());
					CookieOptions useridCookieOptions = new CookieOptions
					{
						Expires = DateTime.Now.AddDays(24)
					};
					Response.Cookies.Append("UserId", response.Id.ToString(), useridCookieOptions);
					//
					HttpContext.Session.SetString("Username", response.Name.ToString());
					CookieOptions usernameCookieOptions = new CookieOptions
					{
						Expires = DateTime.Now.AddDays(24)
					};
					Response.Cookies.Append("Username", response.Name.ToString(), usernameCookieOptions);
					//
					HttpContext.Session.SetString("Role", role.Name);
					CookieOptions RoleCookieOptions = new CookieOptions
					{
						Expires = DateTime.Now.AddDays(24)
					};
					Response.Cookies.Append("Role", role.Name, RoleCookieOptions);


					var result = new
					{
						status = true,
						message = "Success",
					};

					return Json(result);


				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				var result = new
				{
					status = false,
					message = ex.Message,
				};

				return Json(result);
			}
		}

		public IActionResult LogOut()
		{

			try
			{
				// Remove session values
				HttpContext.Session.Remove("UserId");
				HttpContext.Session.Remove("Username");
				HttpContext.Session.Remove("Role");


				// Remove cookies
				Response.Cookies.Delete("UserId");
				Response.Cookies.Delete("Username");
				Response.Cookies.Delete("Role");

				//return View("Index");
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				return View("Error");
			}
		}
		public IActionResult Dashboard()
		{
			var cookieValue = Request.Cookies["UserId"];

			//var Password = Crypter.Decrypt("TZAz6GrpJwaFpzogJv2hgQ==");

			// Check if the cookie exists
			if (cookieValue == null)
			{
				// Use the cookie value
				// For example, you can pass it to a view or process it further
				//ViewBag.CookieValue = cookieValue;
				return View("Index");
			}
			return View();
		}
		[HttpPost]
		public async Task<string> ForgotPassword(string email)
		{
			try
			{
				var query = await _user.ForgotPassword(email);

				if (query.Any())
				{
					if (query.First().Password.ToString() != null)
					{
						//string Senderid = "support@cloudinfosoft.com";
						//string senderpwd = "";

						string Senderid = "vp2172002@gmail.com";
						string senderpwd = "";

						string msg = "Your Password is '" + query.First().Password.ToString() + "'";
						string subject = "Password Recovery";

						SmtpClient client = new SmtpClient();

						client.Port = 587;
						// client.Host = "smtp.zoho.com";
						client.Host = "smtp.gmail.com";
						client.EnableSsl = true;
						client.Timeout = 100000;
						client.DeliveryMethod = SmtpDeliveryMethod.Network;
						client.UseDefaultCredentials = false;
						client.Credentials = new System.Net.NetworkCredential(Senderid, senderpwd);

						MailMessage message = new MailMessage();
						message.From = new MailAddress(Senderid);
						message.To.Add(email);
						message.Subject = subject;
						message.Body = msg;

						client.Send(message);


						return "Success";
					}
					else
					{
						return "No User is Registered with this email.";
					}
				}
				else
				{
					return "No User is Registered with this email.";
				}
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		



	}
}
