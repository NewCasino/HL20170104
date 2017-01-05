using System.Web;
using System.Web.Mvc;

namespace JN.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionAttribute());

            //整个站点都会被强迫使用https连接访问
            if (JN.Services.Tool.ConfigHelper.GetConfigBool("HaveHttps"))
                filters.Add(new RequireHttpsAttribute());
        }
    }

    public class ExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            //记录日志
            JN.Services.Manager.logs.WriteErrorLog(filterContext.HttpContext.Request.Url.ToString(), filterContext.Exception);

            //转向
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectResult("~/Home/Error");
        }
    }
}
