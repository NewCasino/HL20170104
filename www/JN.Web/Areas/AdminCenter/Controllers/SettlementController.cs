using JN.Data;
using JN.Data.Service;
using JN.Services.Manager;
using MvcCore.Controls;
using MvcCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using PagedList;
using JN.Services.Tool;
using System.Data.SqlClient;

namespace JN.Web.Areas.AdminCenter.Controllers
{
    public class SettlementController : BaseController
    {
        private static List<Data.SysParam> cacheSysParam = null;
        private readonly ISettlementService SettlementService;
        private readonly IUserService UserService;
        private readonly ISysParamService SysParamService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;
        private readonly IWalletLogService WalletLogService;
        private readonly IBonusDetailService BonusDetailService;
        private readonly IInvestmentService InvestmentService;

        public SettlementController(ISysDBTool SysDBTool,
            ISettlementService SettlementService,
            IUserService UserService,
            ISysParamService SysParamService,
            IActLogService ActLogService,
            IWalletLogService WalletLogService,
            IBonusDetailService BonusDetailService,
            IInvestmentService InvestmentService)
        {
            this.SettlementService = SettlementService;
            this.UserService = UserService;
            this.SysParamService = SysParamService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            this.WalletLogService = WalletLogService;
            this.BonusDetailService = BonusDetailService;
            this.InvestmentService = InvestmentService;
            cacheSysParam = MvcCore.Unity.Get<ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();
        }

        public ActionResult Index(int? page)
        {
            int aa = 39481;
            int bb = 44165;
            var test = WalletLogService.List(x => x.ID >= 39481 && x.ID <= 50000).ToList();
            foreach (var item in test)
            {
                if (aa != item.ID)
                {

                }
                aa++;
            }

            ViewBag.Title = "历史分红列表";
            var list = SettlementService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID);
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult ManualBalance()
        {
            if (Session["CheckPwd"] == null)
            {
                Session["Url"] = Request.Url.PathAndQuery;
                return Redirect("/AdminCenter/Home/CheckPassWord2");
            }

            ViewBag.MaxPeriod = SettlementService.List(x => x.ID > 0).Count() > 0 ? SettlementService.List(x => x.ID > 0).Max(x => x.Period) : 0;

            string Check = Balances.GetRadom();
            Session["CheckForm"] = Check;
            ViewBag.CheckForm = Check;

            ViewBag.Title = "手动分红";
            ActMessage = ViewBag.Title;

            var model = SettlementService.List().ToList().OrderByDescending(x => x.ID).FirstOrDefault();
            if (model != null)
                return View(model);
            else
                return View();
        }

        [HttpPost]
        public ActionResult ManualBalance(FormCollection form)
        {

            string doPeriod = Request["doPeriod"];  //系统有该期数补发，无则新发
            string CheckForm = Session["CheckForm"].ToString();
            string FormValue = Request["CheckValue"];

            if (FormValue.Length == 0 || CheckForm.Length == 0 || CheckForm != FormValue)
            {
                ViewBag.ErrorMsg = "检测到重复提交，已屏蔽！";
                return View("Error");
            }
            Session["CheckForm"] = "";

            string Msg="";
            int i=0;
            lock (this)
            {
                var list = MvcCore.Unity.Get<Data.Service.IUserService>().List(x => x.Investment > 0).OrderByDescending(x => x.RefereeDepth).ToList();
                foreach (var Umodel in list)
                {
                    string InvestmentNo = Bonus.GetRechargeNo();
                    Bonus.Balance(Umodel.ID, InvestmentNo, Umodel.Investment);  // 日分红
                    i++;
                }               
            }

            ViewBag.SuccessMsg = "分红成功！共分红" +i+"人";
            return View("Success");
        }
        public ActionResult doFH(int doPeriod)
        {

            #region 分红前先备份
            IOHelper.CreateDirectory(Server.MapPath("/DBBackUp/"));
            SqlParameter[] para_sys = new SqlParameter[]
            {
            new SqlParameter ("@BKPATH", Server.MapPath("/DBBackUp/") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak" ),
            };
            int count = MvcCore.Unity.Get<ISysDBTool>().Execute<object>("backup database " + ConfigHelper.GetConfigString("DBName") + " to disk=@BKPATH", para_sys).Count();
            #endregion


            string Msg="";

            string ss = Request.ServerVariables.Get("Remote_Addr").ToString();
            string aa = Request.ServerVariables.Get("Remote_Host").ToString();
            logs.WriteLog("【分红操作记录】手动：" + DateTime.Now.ToString() + "进入分红方法！操作人:" + Amodel.AdminName + ",IP:" + ss + ",电脑名:" + aa);

            var list = MvcCore.Unity.Get<Data.Service.IUserService>().List(x => x.Investment > 0).OrderByDescending(x => x.RefereeDepth).ToList();
            foreach (var Umodel in list)
            {
                string InvestmentNo = Bonus.GetRechargeNo();
                Bonus.Balance(Umodel.ID, InvestmentNo, Umodel.Investment);  // 日分红
            }
            //Balances.WeekSettlement(SettlementService, UserService, WalletLogService, BonusDetailService, InvestmentService, SysDBTool, cacheSysParam, 1, doPeriod.ToInt(), ref Msg);
            //线程里无法使用session
            DataCache.SetCache("StartTime", DateTime.Now);
            DataCache.SetCache("TotalRow", 0);
            DataCache.SetCache("CurrentRow", 0);
            DataCache.SetCache("TransInfo", "开始进行结算");
            //Thread thread = new Thread(new ParameterizedThreadStart(delegate { threadproc((fhzl ?? 0)); }));
            //thread.Start();
            Thread thread = new Thread(new ThreadStart(threadproc));
            thread.Start();


            ViewBag.SuccessMsg = "分红成功！共" + Msg;
            return View("Success");
            //return View("ManualBalance");
        }

        public ActionResult DayBalance()
        {
            #region 事务操作
            //using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            //{
            //        // users.DayBalance();
            //        ts.Complete();
            ViewBag.SuccessMsg = "进行手动分红";
            ActMessage = ViewBag.SuccessMsg;
            ActPacket = "";
            return View("Success");
            //}
            #endregion
        }


        private void threadproc()
        {
            //Stocks.WeekSettlement(SettlementService, UserService, WalletLogService, BonusDetailService, InvestmentService, SysDBTool, cacheSysParam, 1);
            //Balances.WeekSettlement(SettlementService, UserService, WalletLogService, BonusDetailService, InvestmentService, SysDBTool, cacheSysParam, 1);
        }

        public JsonResult getproc()
        {
            var proc = new
            {
                StartTime = DataCache.GetCache("StartTime"),
                CurrentRow = DataCache.GetCache("CurrentRow"),
                TotalRow = DataCache.GetCache("TotalRow"),
                TransInfo = DataCache.GetCache("TransInfo")
            };
            return Json(new { result = "ok", data = proc }, JsonRequestBehavior.AllowGet);
        }
    }
}
