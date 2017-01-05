using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using JN.Data;
using JN.Data.Service;
using MvcCore.Controls;
using System.Web.Security;
using JN.Data.Enum;
using JN.Services;
using JN.Services.Tool;
using JN.Services.CustomException;
using JN.Services.Manager;

namespace JN.Web.Areas.AdminCenter.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAdminUserService AdminUserService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public LoginController(ISysDBTool SysDBTool, IAdminUserService AdminUserService, IActLogService ActLogService)
        {
            this.AdminUserService = AdminUserService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(string username, string password, string code)
        {
            string oldurl = Request["oldUrl"];
            string vcode = (Session["AdminValidateCode"] ?? "").ToString();
            Session.Remove("AdminValidateCode");

            ReturnResult result = new ReturnResult();
            try
            {
                //if (string.IsNullOrEmpty(vcode) || string.IsNullOrEmpty(code) || !vcode.Equals(code, StringComparison.InvariantCultureIgnoreCase))
                //   throw new CustomException("验证码错误");

                if (string.IsNullOrEmpty(username) | string.IsNullOrEmpty(password))
                    throw new CustomException("用户名或密码不能为空");

                var entity = AdminLoginHelper.GetAdminLoginBy(username, password);
                if (entity != null)
                {
                    if (!entity.IsPassed) throw new CustomException("您的帐号已被冻结,请联系你的推荐人!");
                    var log = new ActLog();
                    log.ActContent = "管理员“" + username + "”登录成功！";
                    log.CreateTime = DateTime.Now;
                    log.IP = Request.UserHostAddress;
                    if (Request.UrlReferrer != null)
                        log.Location = Request.UrlReferrer.ToString();
                    log.Platform = "后台";
                    log.Source = "";
                    log.UID = entity.ID;
                    log.UserName = entity.AdminName;
                    ActLogService.Add(log);
                    SysDBTool.Commit();

                    result.Status = 200;
                    result.Message = oldurl ?? "/AdminCenter/Home";

                    //如果勾选记住密码，则保存密码一个星期
                    DateTime expiration = DateTime.Now.AddMinutes(20);
                    //if (rp == "1")
                    //    expiration = DateTime.Now.AddDays(7);

                    // 设置Ticket信息
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        entity.ID.ToString(),
                        DateTime.Now,
                        expiration,
                        false, LoginInfoType.Manager.ToString());

                    // 加密验证票据
                    string strTicket = FormsAuthentication.Encrypt(ticket);

                    // 使用新userdata保存cookie
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strTicket);
                    cookie.Expires = ticket.Expiration;
                    this.Response.Cookies.Add(cookie);
                }
                else
                    throw new CustomException("用户名或密码错误");
            }
            catch (CustomException ex)
            {
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                result.Message = "网络系统繁忙，请稍候再试!";
                logs.WriteErrorLog(HttpContext.Request.Url.ToString(), ex);
            }
            return Json(result);
        }


        public FileResult GetCodeImage(int? width, int? height)
        {
            string code = ValidateWhiteBlackImgCode.RandemCode(4);
            Session["AdminValidateCode"] = code;
            //return File(ValidateWhiteBlackImgCode.Img(code, 200, 75), "image/png");
            // change by chenzw :改为如果前台请求有指定尺寸的，则生成指定尺寸的验证码
            return File(ValidateWhiteBlackImgCode.Img(code, width ?? 200, height ?? 75), "image/png");
        }

    }
}
