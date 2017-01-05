
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JN.Data;
using JN.Data.Service;
using MvcCore.Controls;
using PagedList;
using JN.Services.Manager;
using JN.Services.Tool;
using JN.Services.CustomException;

namespace JN.Web.Areas.UserCenter.Controllers
{
    public class AgentController : BaseController
    {
        private static List<Data.SysParam> cacheSysParam = null;

        private readonly IUserService UserService;
        private readonly ISysParamService SysParamService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public AgentController(ISysDBTool SysDBTool, IUserService UserService, ISysParamService SysParamService, IActLogService ActLogService)
        {
            this.UserService = UserService;
            this.SysParamService = SysParamService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();

        }

        public ActionResult Index(int? page)
        {
            ActMessage = "我管辖的会员";
            var list = UserService.List(x => x.AgentID == Umodel.ID).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult ApplyAgent()
        {
            if (Umodel.Investment != cacheSysParam.SingleAndInit(x => x.ID == 1005).Value.ToDecimal())
            {
                ViewBag.ErrorMsg = "你的用户级别无法申请报单中心。";
                return View("Error");
            }

            int ztrs = Users.GetAllChild(Umodel, 1).Where(x => x.Investment > 0).Count();
            //int tdrs = Users.GetAllChild(Umodel).Count();
            //|| tdrs < cacheSysParam.SingleAndInit(x => x.ID == 1801).Value2.ToInt()
            if (ztrs < cacheSysParam.SingleAndInit(x => x.ID == 1804).Value.ToInt())
            {
                ViewBag.ErrorMsg = "您还没有完成申请报单中心的所需的业绩条件：推荐" + cacheSysParam.SingleAndInit(x => x.ID == 1804).Value + "位有效会员!";
                return View("Error");
            }
            return View(Umodel);
        }


        /// <summary>
        /// 激活会员 Wallet2004激活币 Code:Mrc
        /// </summary>
        /// <param name="id">激活会员id</param>
        /// <param name="id2">推荐人id</param>
        /// <returns></returns>
        public ActionResult Activite(int id)
        { 
            ReturnResult result = new ReturnResult();
            int doAcID = JN.Services.UserLoginHelper.CurrentUser().ID;
           
            var doAcIDModel = UserService.Single(doAcID);

            if (UserService.Single(id).IsActivation)
            {
                result.Message = "账户已激活，无需再激活";
                result.Status = 500;
                return Json(result);
            }
            else { 
            if ((doAcIDModel.Wallet2004) >=200)
            {
                var usermodel = UserService.Single(id);
                if (usermodel != null)
                {
                    usermodel.IsActivation = true;
                    doAcIDModel.Wallet2004 -= 200;
                    UserService.Update(usermodel);
                    UserService.Update(doAcIDModel);
                    SysDBTool.Commit();
                    result.Message = "成功激活" + usermodel.UserName + "会员账号！";
                    result.Status = 200;
                    return Json(result);
                }
                else 
                {
                    result.Message = "激活失败，请稍后再试！";
                    result.Status = 500;
                    return Json(result);
                }

            }
            else
            {
                result.Message = "账户激活币不足，需200激活币才能激活会员！";
                result.Status = 500;
                return Json(result);
            }

            }
        }







        [HttpPost]
        public ActionResult ApplyAgent(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string username = Umodel.UserName;
                string agentname = Umodel.UserName;
                //string username = form["username"];
                //string agentname = form["agentname"];
                string remark = form["agentremark"];

                var onUser = UserService.Single(x => x.UserName == username.Trim());
                if (onUser == null) throw new CustomException("用户不存在");
                if (string.IsNullOrEmpty(agentname)) throw new CustomException("报单中心编号不能为空");
                if (UserService.List(x => x.AgentName == agentname.Trim()).Count() > 0) throw new CustomException("报单中心编号已被使用");
                if (remark.Trim().Length > 100) throw new CustomException("备注长度不能超过100个字节");
                if ((onUser.IsAgent ?? false)) throw new CustomException("该会员已是报单中心，无需要重复申请");
                if (!String.IsNullOrEmpty(onUser.AgentName)) throw new CustomException("您已提交过申请，请耐心等待审核");
                var ids = onUser.RefereePath.Split(',');
                //if (UserService.List(x => ids.Contains(x.ID.ToString()) || x.ID == onUser.RefereeID).Count() <= 0) throw new CustomException("只充许给伞下推荐关系的会员申请报单中心");

                //更新申请状态
                onUser.AgentName = agentname.Trim();
                onUser.ApplyAgentRemark = remark;
                onUser.ApplyAgentTime = DateTime.Now;
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
    }
}
