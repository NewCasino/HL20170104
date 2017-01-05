using System;
using System.Collections.Generic;
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
using System.Data.Entity.Validation;
using JN.Services.Manager;
using JN.Services.CustomException;

namespace JN.Web.Areas.UserCenter.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService UserService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public LoginController(ISysDBTool SysDBTool, IUserService UserService, IActLogService ActLogService)
        {
            this.UserService = UserService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
        }
        public ActionResult Index()
        {
            var sys = MvcCore.Unity.Get<JN.Data.Service.ISysSettingService>().Single(1);
            if (!sys.IsOpenUp)
            {
                Response.Write(sys.CloseHint);
                Response.End();
            }

            return View("neon");
        }

        [HttpPost]
        public JsonResult Index(string username, string password, string lang, string code)
        {
            string oldurl = Request["oldUrl"];
            string vcode = (Session["UserValidateCode"] ?? "").ToString();
            Session.Remove("UserValidateCode");

            ReturnResult result = new ReturnResult();
            try
            {
                //if (string.IsNullOrEmpty(vcode) || string.IsNullOrEmpty(code) || !vcode.Equals(code, StringComparison.InvariantCultureIgnoreCase))
                //    throw new CustomException("验证码错误");

                if (string.IsNullOrEmpty(username) | string.IsNullOrEmpty(password))
                    throw new CustomException("用户名或密码不能为空");
                UserLoginHelper.UserLogout();
                var entity = UserLoginHelper.GetUserLoginBy(username, password);
                if (entity != null)
                {
                    if (entity.IsLock) throw new CustomException("您的帐号已被冻结,请联系你的推荐人!");
                    var log = new ActLog();
                    log.ActContent = "会员“" + username + "”登录成功！";
                    log.CreateTime = DateTime.Now;
                    log.IP = Request.UserHostAddress;
                    if (Request.UrlReferrer != null)
                        log.Location = Request.UrlReferrer.ToString();
                    log.Platform = "会员";
                    log.Source = "";
                    log.UID = entity.ID;
                    log.UserName = entity.UserName;
                    ActLogService.Add(log);
                    SysDBTool.Commit();

                    result.Status = 200;
                    if (entity.IsActivation)
                        result.Message = oldurl ?? "/UserCenter/Home?lang=" + lang;
                    else
                    {
                        result.Status = 208;
                        result.Message = oldurl ?? "/UserCenter/Home?lang=" + lang;
                    }

                    //result.Message = oldurl ?? "/UserCenter/User/doPass";

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
                        false, LoginInfoType.User.ToString());

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
            catch (DbEntityValidationException ex)
            {
                result.Message = "网络系统繁忙，请稍候再试!";
                logs.WriteErrorLog(HttpContext.Request.Url.ToString(), ex);
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
            if (MvcCore.Unity.Get<ISysSettingService>().Single(1).VerificationCodeType == "001")
            {
                string code = ValidateWhiteBlackImgCode.RandemCode((MvcCore.Unity.Get<ISysSettingService>().Single(1).VerificationCodeLength ?? 5));
                Session["UserValidateCode"] = code;
                return File(ValidateWhiteBlackImgCode.Img(code, width ?? 200, height ?? 75), "image/png");
            }
            else
            {
                ValidateCode vCode = new ValidateCode();
                string code = vCode.CreateRandomCode((MvcCore.Unity.Get<ISysSettingService>().Single(1).VerificationCodeLength ?? 5));
                Session["UserValidateCode"] = code;
                byte[] bytes = vCode.CreateValidateGraphic(code);
                return File(bytes, @"image/jpeg");
            }
        }

        public ActionResult GetPwd()
        {
            var sys = MvcCore.Unity.Get<JN.Data.Service.ISysSettingService>().Single(1);
            if (!sys.IsOpenUp)
            {
                Response.Write(sys.CloseHint);
                Response.End();
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetPwd(string username, string email)
        {
            if (string.IsNullOrEmpty(username) | string.IsNullOrEmpty(email))
                return Json(new { status = "err", data = "用户名或取回手机号码不能为空！" }, JsonRequestBehavior.AllowGet);
            var onUser = UserService.Single(x => x.UserName == username);
            if (onUser == null) return Json(new { status = "err", data = "用户名不存在！" }, JsonRequestBehavior.AllowGet);
            if (onUser.Mobile != email)
            {
                var wlog = new WarningLog();
                wlog.CreateTime = DateTime.Now;
                wlog.IP = Request.UserHostAddress;
                if (Request.UrlReferrer != null)
                    wlog.Location = Request.UrlReferrer.ToString();
                wlog.Platform = "会员";
                wlog.WarningMsg = "试图通过取回密码修改会员“" + username + "”密码，使用手机号：" + email;
                wlog.WarningLevel = "中等";
                wlog.ResultMsg = "拒绝";
                wlog.UserName = username;
                MvcCore.Unity.Get<IWarningLogService>().Add(wlog);
                SysDBTool.Commit();

                return Json(new { status = "err", data = "非法操作，您的行为已被记录！" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(onUser.Mobile))
                return Json(new { status = "err", data = "用户未设置手机号码，无法取回密码，请联系管理员重置密码。" }, JsonRequestBehavior.AllowGet);
            else
            {
                if (onUser.Mobile == email)
                {
                    ValidateCode vEmailCode = new ValidateCode();
                    string emailcode = vEmailCode.CreateRandomCode(5);
                    Session["EmailValidateCode"] = emailcode;
                    Session["GetPwdUser"] = username;
                    JN.Services.Tool.SMSHelper.WebChineseMSM(onUser.Mobile, "您好" + username + "，您在使用找回密码功能，找回密码需要填写的激活码是：" + emailcode + "");
                    return Json(new { status = "ok", data = "/UserCenter/Login/GetPwd2" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "err", data = "用户名与手机号码不匹配" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetPwd2()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPwd2(string emailcode, string password, string password2, string password3)
        {
            if (string.IsNullOrEmpty(emailcode))
                return Json(new { result = "err", refMsg = "验证码不能为空！" }, JsonRequestBehavior.AllowGet);

            if (Session["EmailValidateCode"] != null && Session["GetPwdUser"] != null && emailcode.ToLower() == Session["EmailValidateCode"].ToString().ToLower())
            {
                var username = Session["GetPwdUser"].ToString();
                var chUser = UserService.Single(x => x.UserName == username);
                if (chUser == null) return Json(new { result = "err", refMsg = "用户信息已丢失，请重新找回密码" }, JsonRequestBehavior.AllowGet);

                chUser.Password = password.ToMD5().ToMD5();
                chUser.Password2 = password2.ToMD5().ToMD5();
                //chUser.Password3 = password3.ToMD5().ToMD5();
                UserService.Update(chUser);
                SysDBTool.Commit();

                var wlog = new WarningLog();
                wlog.CreateTime = DateTime.Now;
                wlog.IP = Request.UserHostAddress;
                if (Request.UrlReferrer != null)
                    wlog.Location = Request.UrlReferrer.ToString();
                wlog.Platform = "会员";
                wlog.WarningMsg = "通过取回密码修改密码，验证成功";
                wlog.WarningLevel = "一般";
                wlog.ResultMsg = "通过";
                wlog.UserName = username;
                MvcCore.Unity.Get<IWarningLogService>().Add(wlog);
                SysDBTool.Commit();

                return Json(new { result = "ok", refMsg = "/UserCenter/Login/GetPwd3" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = "err", refMsg = "验证码不正确或已过期" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPwd3()
        {
            return View();
        }

    }
}
