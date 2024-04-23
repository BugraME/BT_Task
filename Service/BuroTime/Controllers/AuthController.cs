using BuroTime.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace BuroTime.Controllers;
[ApiController, Route("[controller]/[action]")]
public class AuthController : ControllerBase {
	[HttpPost]
	public IActionResult Login(string username, string pass) {
		if (username != "bürotime" || pass != "123") return Content("Kullanıcı adı veya şifre yanlış!\r\n----------\r\nbürotime \r\n123");
		return Ok(JwtHelper.GenerateToken());
	}
}