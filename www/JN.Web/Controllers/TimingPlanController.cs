
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using JN.Services.Manager;
using JN.Data.Service;
using JN.Services.Tool;
using System.Threading;


namespace JN.Web.Controllers
{
    public class TimingPlanController : Controller
    {
        private static List<Data.SysParam> cacheSysParam = null;

        private readonly ISettlementService SettlementService;
        private readonly IUserService UserService;
        private readonly IWalletLogService WalletLogService;
        private readonly IBonusDetailService BonusDetailService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;
        private readonly IInvestmentService InvestmentService;

        public TimingPlanController(
            ISysDBTool SysDBTool, 
            ISettlementService BalanceService, 
            IUserService UserService,
            IWalletLogService WalletLogService, 
            IBonusDetailService BonusDetailService, 
            IActLogService ActLogService,
            IInvestmentService InvestmentService)
        {
            this.SettlementService = BalanceService;
            this.UserService = UserService;
            this.WalletLogService = WalletLogService;
            this.BonusDetailService = BonusDetailService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            this.InvestmentService = InvestmentService;
            cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();
        }

        //
        // GET: /Controller/
        public ActionResult Index()
        {
            bool isExec = false;
            DateTime starttime = DateTime.Now;
            string ExecProcess = Request["ExecProcess"];
            switch (ExecProcess)
            {
                case "plan1":
                    isExec = plan1();
                    break;
                case "plan2":
                    isExec = plan2();
                    break;
                case "plan3":
                    isExec = plan3();
                    break;
                case "plan4":
                    isExec = plan4();
                    break;
                case "plan5":
                    isExec = plan5();
                    break;
            }
            ViewBag.msg = (isExec ? "成功" : "失败") + "执行作业计划“" + ExecProcess + "”，时间在" + DateTime.Now.ToString() + "，用时：" + JN.Services.Tool.DateTimeDiff.DateDiff_Sec(starttime,DateTime.Now) + "毫秒";
            logs.WriteLog(ViewBag.msg);
            return View();
        }

        //定时计划1
        public bool plan1()
        {
            DataCache.SetCache("StartTime", DateTime.Now);
            DataCache.SetCache("TotalRow", 0);
            DataCache.SetCache("CurrentRow", 0);
            DataCache.SetCache("TransInfo", "系统自动分红进行中，请稍候...");
            Thread thread = new Thread(new ThreadStart(threadproc1));
            thread.Start();
            return true;
        }

        private void threadproc1()
        {
            //using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            //{
            //    lock (obj)
            //    {
           // logs.WritePlanLog("开始执行自动日发进程.时间在" + DateTime.Now.ToString());
            string Msg;
            try
            {
                var list = MvcCore.Unity.Get<Data.Service.IUserService>().List(x =>x.Investment>0).OrderByDescending(x => x.RefereeDepth).ToList();
                foreach(var Umodel in list){
                   string InvestmentNo = Bonus.GetRechargeNo();
                    Bonus.Balance(Umodel.ID, InvestmentNo, Umodel.Investment);  // 日分红
                }
               
            }
            catch (Exception ex)
            {
                string str = "访问地址：" + System.Web.HttpContext.Current.Request.Url.PathAndQuery + "\r\n访问类名:" + MethodBase.GetCurrentMethod().DeclaringType.FullName + "\r\n执行方法:" + MethodBase.GetCurrentMethod().Name + "\r\n";

                logs.WriteErrorLog(ex);
               // logs.WritePlanLog("日发过程出错：" + str);
            }
           // logs.WritePlanLog("执行日发进程结束.时间在" + DateTime.Now.ToString());
            //    ts.Complete();
            //}
            //}
        }

        //定时计划2
        public bool plan2()
        {
            return false;
        }


        //定时计划3
        public bool plan3()
        {
            return false;
        }

        //定时计划4
        public bool plan4()
        {
            return false;
        }

        //定时计划5
        public bool plan5()
        {
            return false;
        }
    }
}
