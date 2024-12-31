using Microsoft.AspNetCore.Mvc;

namespace Stock_CMS.Controllers
{
	public class EnquiryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
