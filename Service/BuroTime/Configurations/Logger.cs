using BT.Data.Entity;
using BT.ServiceHelper;
using Microsoft.AspNetCore.Mvc.Filters;
namespace BuroTime.Middleware;
public class Logger(LogService logService) : IActionFilter {
	private readonly LogService LogService = logService;
	public void OnActionExecuting(ActionExecutingContext context) { }
	public void OnActionExecuted(ActionExecutedContext context) {
		string controller = context.HttpContext.Request.RouteValues["controller"]?.ToString();
		string action = context.HttpContext.Request.RouteValues["action"]?.ToString();
		Log log = new() {
			YOL = $"{controller}/{action}",
			KULLANICI_IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
			ISLEM_TARIHI = DateTime.Now,
		};
		if (context.Exception != null) {
			log.HATA = false;
			log.HATAMESAJI = context.Exception.Message;
		}
		LogService.InsertAndComplete(log);
	}
}