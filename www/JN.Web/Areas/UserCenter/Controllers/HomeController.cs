using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JN.Data.Service;
using JN.Services.Tool;
using System.Text;
using System.Data.Entity.SqlServer;
using JN.Services.Manager;
using JN.Data.Extensions;

namespace JN.Web.Areas.UserCenter.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService UserService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public HomeController(ISysDBTool SysDBTool, IUserService UserService, IActLogService ActLogService)
        {
            this.UserService = UserService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
        }


        public ActionResult Index()
        {
            //16102101检查出局
            Bonus.RefreeBonus(Umodel.ID);

            //USD.OvertimeNotPay();
            //USD.OvertimeNotConfirmed();
            //CFB.OvertimeNotThaw();
            //Users.AutoLevel(Umodel.ID);   //检查升级经理
            //Bonus.MonthAgentBouns();  //检查经理是否拿了当月奖金
            //Bonus.CheckInvestmentStauts();  //检查超时的订单并取消

            ViewBag.Title = "欢迎页";
            int tdrs = Users.GetAllChild(Umodel).Count();
            var widgetlist = new List<IndexWidget>();
            //widgetlist.Add(new TIndexWidget { Description = "您的银种子帐户余额", Icon = "R", Name = "银种子帐户", Url = "#", Value = Umodel.Wallet2001.ToString("F2") });
            widgetlist.Add(new IndexWidget { Description = "", Icon = "fa fa-credit-card", Name = "EP钱包", Url = "#", Value = "$" + Umodel.Wallet2002.ToString("F2") });
            widgetlist.Add(new IndexWidget { Description = "", Icon = "fa fa-globe", Name = "FC数量", Url = "#", Value = Umodel.Wallet2003.ToString("F4") });
            widgetlist.Add(new IndexWidget { Description = "", Icon = "fa fa-bar-chart", Name = "股权数量", Url = "#", Value = Umodel.Wallet2004.ToString("F0") });
            widgetlist.Add(new IndexWidget { Description = "", Icon = "fa fa-money", Name = "AP钱包", Url = "#", Value = Umodel.Wallet2001.ToString("F0") });
            widgetlist.Add(new IndexWidget { Description = "", Icon = "fa fa-user", Name = "团队人数", Url = "#", Value = tdrs.ToString() });
            //widgetlist.Add(new IndexWidget { Description = "您的累计奖金", Icon = "", Name = "累计奖金", Url = "#", Value = TypeConverter.ObjectToInt(bonusdetails.GetFieldValue("ISNULL(SUM(BonusMoney),0)", "UID=" + JN.BLL.users.IsLogin().ID + " and IsBalance=1")).ToString() });
            ViewBag.Widgets = widgetlist;
            string viewname = ConfigHelper.GetConfigString("Theme");
            return View(viewname);
        }

        public ActionResult Wait()
        {
            return View();
        }

        //public ActionResult GetLimitTime()
        //{
        //    DateTime dt = (Umodel.ActivationTime ?? DateTime.Now).AddDays(Umodel.LeftDpMargin ?? 0);
        //    DateTime now = DateTime.Now;
        //    TimeSpan d3 = dt.Subtract(now);
        //    int limitSecond = DateTimeDiff.DateDiff_Sec(now, dt);
        //    return Content(limitSecond.ToString());
        //}

        //退出
        public ActionResult Logout()
        {
            ActMessage = "会员退出";
            Services.UserLoginHelper.UserLogout();
            return Redirect("/UserCenter/Login");
        }

        public ActionResult ChangePassword()
        {
            ViewBag.Title = "修改密码";
            ActMessage = ViewBag.Title;
            return View(Umodel);
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            string oldpassword = form["oldpassword"];
            string newpassword = form["newpassword"];
            string newpassword2 = form["newpassword2"];
            string connewpassword = form["connewpassword"];
            string connewpassword2 = form["connewpassword2"];
            string strErr = "";

            if (MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().Single(x => x.ID == 4502).Value == "1")  //开启修改密码验证
            {
                string smscode = form["smscode"];
                if (string.IsNullOrEmpty(smscode)) { ViewBag.ErrorMsg += "手机验证码不能为空"; return View("Error"); }
                if (Session["SMSValidateCode"] == null || smscode.Trim() != Session["SMSValidateCode"].ToString()) { ViewBag.ErrorMsg += "验证码不正确或已过期"; return View("Error"); }
                if (Session["GetSMSUser"] == null || Umodel.Mobile != Session["GetSMSUser"].ToString()) { ViewBag.ErrorMsg += "手机号码与接收验证码的设备不相符"; return View("Error"); }
            }

            if (oldpassword.Trim().Length == 0 || newpassword.Trim().Length == 0 || newpassword2.Trim().Length == 0)
                strErr += "原二级密码、新一级密码、新二级密码不能为空 <br />";

            if (newpassword != connewpassword)
                strErr += "新一级密码与确认密码不相符 <br />";

            if (newpassword2 != connewpassword2)
                strErr += "新二级密码与确认密码不相符 <br />";

            if (Umodel.Password2 != oldpassword.ToMD5().ToMD5())
                strErr += "原二级密码不正确 <br />";
            if (strErr != "")
            {
                ViewBag.ErrorMsg = "抱赚您填写的信息有误： <br />" + strErr + "请核实后重新提交！";
                return View("Error");
            }

            Umodel.Password = newpassword.ToMD5().ToMD5();
            Umodel.Password2 = newpassword2.ToMD5().ToMD5();
            UserService.Update(Umodel);
            SysDBTool.Commit();
            ActMessage = "密码修改成功";
            return View("Success");
        }

        public JsonResult getprice2()
        {
            var chart = MvcCore.Unity.Get<Data.Service.ICFBChartService>().List().OrderByDescending(x => x.CreateTime).ToList().FirstOrDefault();
            if (chart != null)
                return Json(new { result = "ok", data = chart }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { result = "err", }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getprice()
        {
            var chart = MvcCore.Unity.Get<IStockChartDayService>().List().OrderByDescending(x => x.StockDate).ToList().FirstOrDefault();
            if (chart != null)
                return Json(new { result = "ok", data = chart }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { result = "err", }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getopenprice()
        {
            var chart = MvcCore.Unity.Get<IStockChartDayService>().List().OrderByDescending(x => x.StockDate).ToList().FirstOrDefault();
            if (chart != null)
                return Json(new { result = "ok", data = chart }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { result = "ok", data = new Data.StockChartDay { ClosePrice = Stocks.getcurrentprice(), OpenPrice = Stocks.getcurrentprice() } }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getkchart()
        {
            List<string> days = new List<string>();
            List<Object> kdata = new List<Object>();
            var lst = MvcCore.Unity.Get<IStockChartDayService>().List().Take(100).OrderBy(x => x.StockDate).ToList();
            foreach (var item in lst)
            {
                days.Add(item.StockDate.ToShortDateString());
                kdata.Add(new decimal[] { item.OpenPrice, item.ClosePrice, item.LowPrice, item.HightPrice });
            }

            return Json(new { result = "ok", days = days, kdata = kdata }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult gethighstock()
        {
            StringBuilder retTrade = new StringBuilder(); ;
            var lst = MvcCore.Unity.Get<IStockChartDayService>().List().Take(100).OrderBy(x => x.StockDate).ToList();
            foreach (var item in lst)
            {
                //{日期^1开盘价^2收盘价^3最高价^4最低价^5成交量^6成交额^7涨跌幅^8涨跌额^9换手率^10昨日收盘价^11MA5^12MA10^13MA20^14MA30^15MA60}
                //1452765600000 ^ 1.6691 ^ 1.8360 ^ 1.8360 ^ 1.5022 ^ 12793.1670 ^ 23488.2418 ^ 10.00 ^ 0.1669 ^ 1.6691 ^ 1.6691 ^ 1.5287 ^ 1.7654 ^ 2.1981 ^ 2.7812 ^ 1.6691~
                retTrade.Append("" + DateTimeDiff.DateTimeToStamp(item.CreateTime) + "^" + item.OpenPrice.ToString("F5") + "^" + item.ClosePrice.ToString("F5") + "^" + item.HightPrice.ToString("F5") + "^" + item.LowPrice.ToString("F5") + "^" + item.Volume.ToString("F5") + "^" + item.Turnover.ToString("F5") + "^" + item.UpsAndDownsScale.ToString("F2") + "^" + item.UpsAndDownsPrice.ToString("F5") + "^null^" + item.YesterdayClosePrice.ToString("F5") + "^" + item.MA5.ToString("F5") + "^" + item.MA10.ToString("F5") + "^null^" + item.MA30.ToString("F5") + "~");
            }

            return Json(new { vl = retTrade.ToString(), ccode = "X", tag = "行情走趋", cname = "FC币" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getkstock()
        {
            //{"datas":[["1.5586","1.7145","1.4027","1.7145"],["1.7145","1.8860","1.7000","1.8860"],["1.8860","1.9199","1.8660","2.0746"],["1.9199","1.7279","1.7279","2.1119"],["1.7279","1.5551","1.5551","1.5551"],["1.5551","1.5582","1.3996","1.7106"],["1.5582","1.6875","1.4444","1.6998"],["1.6875","1.6800","1.5300","1.8500"],["1.6800","1.5800","1.5120","1.7600"],["1.5800","1.5006","1.4540","1.6200"],["1.5006","1.5952","1.4300","1.6000"],["1.5952","1.4708","1.4450","1.7000"],["1.4708","1.6179","1.3237","1.6179"],["1.6179","1.6346","1.5370","1.7797"],["1.6346","1.7000","1.5700","1.7981"],["1.7000","1.6150","1.6150","1.7600"],["1.6150","1.5342","1.5342","1.6000"],["1.5342","1.5700","1.4577","1.6876"],["1.5700","1.5892","1.5000","1.6800"],["1.5892","1.6530","1.5400","1.7370"],["1.6530","1.7201","1.6111","1.8050"],["1.7201","1.8004","1.6905","1.8350"],["1.8004","1.9100","1.7650","1.9300"],["1.9100","2.0598","1.8601","2.0700"],["2.0598","1.9568","1.9568","2.2658"],["1.9568","1.8590","1.8590","2.1525"],["1.8590","1.7810","1.7661","1.8800"],["1.7810","1.8665","1.6920","1.9591"],["1.8665","1.8900","1.7900","2.0532"],["1.8900","1.9106","1.7955","1.9990"]],"todaymaxprice":"1.9990","currprice":"1.8900","hisminprice":"0.1400","dates":["2016-03-14","2016-03-15","2016-03-16","2016-03-17","2016-03-18","2016-03-19","2016-03-20","2016-03-21","2016-03-22","2016-03-23","2016-03-24","2016-03-25","2016-03-26","2016-03-27","2016-03-28","2016-03-29","2016-03-30","2016-03-31","2016-04-01","2016-04-02","2016-04-03","2016-04-04","2016-04-05","2016-04-06","2016-04-07","2016-04-08","2016-04-09","2016-04-10","2016-04-11","2016-04-12"],"todaycount":"10685.2799","todayminprice":"1.7955","hismaxprice":"8.5971"}
            List<Object> kdata = new List<Object>();
            var lst = MvcCore.Unity.Get<IStockChartDayService>().List().Take(100).OrderBy(x => x.StockDate).ToList();
            foreach (var item in lst)
            {
                kdata.Add(new string[] { item.OpenPrice.ToString("F4"), item.ClosePrice.ToString("F4"), item.LowPrice.ToString("F4"), item.HightPrice.ToString("F4") });
            }
            return Json(new { datas = kdata, dates = lst.Select(x => x.CreateTime.ToShortDateString()) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getlinestock()
        {
            List<Object> kdata = new List<Object>();
            var lst = MvcCore.Unity.Get<IStockTradeService>().List(x => SqlFunctions.DateDiff("DAY", x.CreateTime, DateTime.Now) <= 1).OrderBy(x => x.ID).ToList(); //stocktradelogs.GetModelList("datediff(day,CreateTime,getdate())<=1");
            foreach (var item in lst)
            {
                kdata.Add(new double[] { DateTimeDiff.DateTimeToStamp(item.CreateTime), (double)item.Price });
            }
            return Json(new { data = kdata }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        public ActionResult HongBao()
        {
            //if (MvcCore.Unity.Get<IBonusDetailService>().List(x => x.UID == Umodel.ID && x.BonusID == 1106 && SqlFunctions.DateDiff("DAY", x.CreateTime, DateTime.Now) == 0).Count() > 0)
            if (MvcCore.Unity.Get<IBonusDetailService>().List(x => x.UID == Umodel.ID && x.BonusID == 1106).Count() <= 0)
            {
                int PARAM_SDJJ = MvcCore.Unity.Get<ISysParamService>().SingleAndInit(x => x.ID == 1106).Value.ToInt();
                decimal hbje = StringHelp.GetRandomNumber(PARAM_SDJJ).ToDecimal();

                Bonus.UpdateUserWallet(hbje, 1106, MvcCore.Unity.Get<ISysParamService>().SingleAndInit(x => x.ID == 1106).Name, "首次登录红包", Umodel.ID, "Addup1106", true);
                return Content(hbje.ToString());
            }
            else
                return Content("0");
        }
    }


}
