using JN.Data;
using JN.Data.Service;
using MvcCore.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using JN.Services.Tool;
using JN.Services.CustomException;
using JN.Services.Manager;

namespace JN.Web.Areas.AdminCenter.Controllers
{
    public class AgentController : BaseController
    {
        private readonly IUserService UserService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public AgentController(ISysDBTool SysDBTool,
            IUserService UserService,
            IActLogService ActLogService)
        {
            this.UserService = UserService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
        }
        public ActionResult Delivery()
        {
            ActMessage = "指定一个报单中心";
            return View();
        }

        [HttpPost]
        public ActionResult Delivery(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string username = form["username"];
                string agentname = form["agentname"];
                string remark = form["remark"];

                var onUser = UserService.Single(x => x.UserName == username.Trim());
                if (onUser == null) throw new CustomException("用户不存在");
                if (string.IsNullOrEmpty(agentname)) throw new CustomException("报单中心编号不能为空");
                if (UserService.List(x => x.AgentName == agentname.Trim()).Count() > 0) throw new CustomException("报单中心编号已被使用");
                if (remark.Trim().Length > 100) throw new CustomException("备注长度不能超过100个字节");
                if ((onUser.IsAgent ?? false)) throw new CustomException("该会员已是报单中心，无需要重复申请");

                onUser.AgentName = agentname;
                onUser.IsAgent = true;
                onUser.ApplyAgentTime = DateTime.Now;
                onUser.ApplyAgentRemark = remark;
                UserService.Update(onUser);
                SysDBTool.Commit();
                result.Status = 200;
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
        public ActionResult Index(int? page)
        {
            ActMessage = "报单中心列表";
            var list = UserService.List(x => (x.IsAgent ?? false)).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult UnPassed(int? page)
        {
            ActMessage = "待审核的报单中心";
            var list = UserService.List(x => !(x.IsAgent ?? false) && !string.IsNullOrEmpty(x.AgentName)).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult doCancel(int id)
        {
            var model = UserService.Single(id);
            if (model != null)
            {
                model.IsAgent = false;
                model.AgentName = "";
                model.ApplyAgentRemark = "报单中心被取消";
                UserService.Update(model);
                SysDBTool.Commit();
                ViewBag.SuccessMsg = "会员“" + model.UserName + "”报单中心资格已被取消！";
                return View("Success");
            }
            ViewBag.ErrorMsg = "系统异常！请查看系统日志。";
            return View("Error");
        }

        public ActionResult doPass(int id)
        {
            var model = UserService.Single(id);
            if (model != null)
            {
                model.IsAgent = true;
                UserService.Update(model);
                SysDBTool.Commit();
                ViewBag.SuccessMsg = "审核通过会员“" + model.UserName + "”成为报单中心！";
                return View("Success");
            }
            ViewBag.ErrorMsg = "系统异常！请查看系统日志。";
            return View("Error");
        }
    }
}
