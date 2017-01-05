using JN.Data.Service;
using JN.Services.Manager;
using MvcCore.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JN.Web.Areas.AdminCenter.Controllers
{
    public class HomeController : BaseController
    {
        private static List<Data.SysParam> cacheSysParam = MvcCore.Unity.Get<ISysParamService>().List(x => x.ID < 4000).ToList();

        //
        // GET: /AdminCenter/Index/
        public ActionResult Index()
        {
            //if (Request["dodo"] == "yes")
            //{
            //    int count = 0;
            //    string usernames = "";
            //    var startdate = ("2016-06-07 10:01:00").ToDateTime();
            //    var enddate = ("2016-06-07 10:29:44").ToDateTime();
            //    var subs = MvcCore.Unity.Get<Data.Service.ICFBSubscribeService>().List(x => x.Period == 2 && x.CreateTime > startdate && x.CreateTime < enddate).OrderByDescending(x => x.ID).ToList();
            //    foreach (var item in subs)
            //    {
            //        Services.Manager.Wallets.changeWallet(item.UID, item.SubscribeNumber, 2004, "第3期拆分补充");
            //        usernames += item.UserName + ",";
            //        count++;
            //    }
            //    return Content("整了" + count + "条," + usernames);
            //}

            //if (Request["dodo"] == "yes")
            //{
            //    int count = 0;
            //    string usernames = "";
            //    int startid = Request["sid"].ToInt();
            //    int endid = Request["eid"].ToInt();
            //    var subs = MvcCore.Unity.Get<IUserService>().List(x => x.IsActivation && x.ID >= startid && x.ID<endid).OrderByDescending(x => x.ID).ToList();
            //    foreach (var item in subs)
            //    {
            //        var onUser = MvcCore.Unity.Get<IUserService>().Single(item.ID);
            //        var purlist = MvcCore.Unity.Get<IUSDPurchaseService>().List(x => x.UID == onUser.ID && x.Status == (int)Data.Enum.USDStatus.Deal).ToList();
            //        decimal totalbuy = purlist.Count() > 0 ? purlist.Sum(x => x.BuyMoney) : 0;

            //        decimal tz1001 = cacheSysParam.SingleAndInit(x => x.ID >= 1001).Value.ToDecimal();
            //        decimal tz1002 = cacheSysParam.SingleAndInit(x => x.ID >= 1002).Value.ToDecimal();
            //        decimal tz1003 = cacheSysParam.SingleAndInit(x => x.ID >= 1003).Value.ToDecimal();
            //        decimal tz1004 = cacheSysParam.SingleAndInit(x => x.ID >= 1004).Value.ToDecimal();
            //        //decimal tz1005 = cacheSysParam.SingleAndInit(x => x.ID >= 1005).Value.ToDecimal();

            //        if (totalbuy >= tz1002 && totalbuy < tz1003 && onUser.Investment < tz1002)
            //        {
            //            usernames += item.UserName + "(" + item.Investment + "->" + tz1002 + "),";
            //            onUser.Investment = tz1002;
            //            MvcCore.Unity.Get<IUserService>().Update(onUser);
            //            MvcCore.Unity.Get<ISysDBTool>().Commit();
            //            count++;
            //        }
            //        else if (totalbuy >= tz1003 && totalbuy < tz1004 && onUser.Investment < tz1003)
            //        {
            //            usernames += item.UserName + "(" + item.Investment + "->" + tz1003 + "),";
            //            onUser.Investment = tz1003;
            //            MvcCore.Unity.Get<IUserService>().Update(onUser);
            //            MvcCore.Unity.Get<ISysDBTool>().Commit();
            //            count++;
            //        }

            //        //if (totalbuy >= tz1004 && totalbuy < tz1005 && onUser.Investment < tz1004)
            //        //{
            //        //    usernames += item.UserName + "(" + item.Investment + "->" + tz1004 + "),";
            //        //    onUser.Investment = tz1004;
            //        //    count++;
            //        //}
            //        //if (totalbuy >= tz1005 && onUser.Investment < tz1005)
            //        //{
            //        //    usernames += item.UserName + "(" + item.Investment + "->" + tz1005 + "),";
            //        //    onUser.Investment = tz1005;
            //        //    count++;
            //        //}


            //    }
            //    return Content("整了" + count + "条," + usernames);
            //}

            //if (Request["dodo"] == "yes")
            //{
            //    int count = 0;
            //    string usernames = "";
            //    int startid = Request["sid"].ToInt();
            //    int endid = Request["eid"].ToInt();
            //    var subs = MvcCore.Unity.Get<IUserService>().List(x => x.IsActivation && x.ID >= startid && x.ID < endid).OrderByDescending(x => x.ID).ToList();
            //    foreach (var item in subs)
            //    {
            //        var onUser = MvcCore.Unity.Get<IUserService>().Single(item.ID);
            //        var mysubs = MvcCore.Unity.Get<ICFBSubscribeService>().List(x => x.UID == item.ID);
            //        if (mysubs.Count() > 0)
            //        {
            //            decimal totalbuy = mysubs.Count() > 0 ? mysubs.Sum(x => x.SubscribeNumber) : 0;
            //            var mysells = MvcCore.Unity.Get<IWalletLogService>().List(x => x.UID == item.ID && x.CoinID == 2004 && x.Description.Contains("卖出"));
            //            decimal totalsell = mysells.Count() > 0 ? mysells.Sum(x => x.ChangeMoney) : 0;
            //            onUser.Addup1106 = totalbuy;
            //            onUser.Addup1107 = totalsell;
            //            MvcCore.Unity.Get<IUserService>().Update(onUser);
            //            MvcCore.Unity.Get<ISysDBTool>().Commit();
            //            count++;
            //        }
            //    }
            //    return Content("整了" + count + "条," + usernames);
            //}

            //if (Request["dodo"] == "yes")
            //{
            //                UPDATE[user]   SET RefereeCount = 0

            //DECLARE @ID INT,@RCOUNT INT
            //DECLARE cursor_fuit CURSOR FOR
            //SELECT[ID],(SELECT COUNT(*) FROM[user] where RefereeID = A.ID) as RCount FROM[USER] A

            //OPEN cursor_fuit
            // FETCH NEXT FROM cursor_fuit INTO @ID, @RCOUNT

            // WHILE @@FETCH_STATUS = 0

            //    BEGIN
            //        UPDATE[user]   SET RefereeCount = @RCOUNT WHERE ID = @ID
            //FETCH NEXT FROM cursor_fuit INTO @ID, @RCOUNT
            //END
            //CLOSE cursor_fuit
            //DEALLOCATE cursor_fuit



            //    return Content("整了" + count + "条," + usernames);
            //}

            return View();
        }

        public ActionResult NoAuthority()
        {
            return View();
        }

        //退出
        public ActionResult Logout()
        {
            ActMessage = "退出系统";
            Services.AdminLoginHelper.AdminUserLogout();
            return Redirect(Url.Action("Index", "Login"));
        }

        public ActionResult ClearAllCache()
        {
            List<String> caches = MvcCore.Extensions.CacheExtensions.GetAllCache();
            foreach (var cachename in caches)
                MvcCore.Extensions.CacheExtensions.ClearCache(cachename);
            List<String> caches2 = Services.Tool.DataCache.GetAllCache();
            foreach (var cachename in caches2)
                Services.Tool.DataCache.ClearCache(cachename);
            Services.AdminLoginHelper.AdminUserLogout();
            return Redirect(Url.Action("Index", "Login"));
        }

        public ActionResult ChangePassword()
        {
            ActMessage = "修改密码";
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            string oldpassword = form["oldpassword"];
            string newpassword = form["newpassword"];
            string connewpassword = form["newpassword2"];

            string new2password = form["new2password"];
            string connew2password = form["new2password2"];
            string strErr = "";

            if (oldpassword.Trim().Length == 0 || newpassword.Trim().Length == 0 || new2password.Trim().Length == 0)
                strErr += "密码不能为空";

            if (newpassword != connewpassword)
                strErr += "新密码与确认密码不相符";
            if (new2password != connew2password)
                strErr += "，新密码与确认密码不相符";

            if (Amodel.Password2 != oldpassword.ToMD5().ToMD5())
                strErr += "，原密码不正确";
            if (strErr != "")
            {
                ViewBag.ErrorMsg = strErr + "，请核实后重新提交！";
                //ViewBag.ErrorMsg = "无效的ID";
                return View("Error");
            }

            Amodel.Password = newpassword.ToMD5().ToMD5();
            Amodel.Password2 = new2password.ToMD5().ToMD5();
            MvcCore.Unity.Get<IAdminUserService>().Update(Amodel);
            MvcCore.Unity.Get<ISysDBTool>().Commit();
            ActMessage = "密码修改成功";
            return View("Success");
        }

        public ActionResult CheckPassWord2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckPassWord2(FormCollection form)
        {
            var result = new ReturnResult();
            try
            {
                string password2 = form["password2"];
                string Url = form["Url"];

                if (password2.ToMD5().ToMD5() != Amodel.Password2)
                {
                    throw new Exception("二级密码不正确！");
                }
                Session["CheckPwd"] = "true";
                result.Status = 200;
                result.Message = Session["Url"].ToString();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                logs.WriteErrorLog(HttpContext.Request.Url.ToString(), ex);
            }
            return Json(result);
        }
    }
}