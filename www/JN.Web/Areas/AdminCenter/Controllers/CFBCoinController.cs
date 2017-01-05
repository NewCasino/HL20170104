using JN.Data.Service;
using MvcCore.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using JN.Services.Tool;
using System.Threading;
using JN.Services.Manager;
using JN.Services.CustomException;

namespace JN.Web.Areas.AdminCenter.Controllers
{
    public class CFBCoinController : BaseController
    {
        private static List<Data.SysParam> cacheSysParam = null;
        private readonly IUserService UserService;
        private readonly ICFBIssueService CFBIssueService;
        private readonly ICFBSplittingService CFBSplittingService;
        private readonly ICFBChartService CFBChartService;
        private readonly ICFBSubscribeService CFBSubscribeService;
        private readonly ICFBPreOrderService CFBPreOrderService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;
        private readonly IWalletLogService WalletLogService;

        public CFBCoinController(ISysDBTool SysDBTool,
            IUserService UserService,
            ICFBIssueService CFBIssueService,
            ICFBSplittingService CFBSplittingService,
            ICFBChartService CFBChartService,
            ICFBSubscribeService CFBSubscribeService,
            ICFBPreOrderService CFBPreOrderService,
            IActLogService ActLogService, 
            IWalletLogService WalletLogService)
        {
            this.UserService = UserService;
            this.CFBIssueService = CFBIssueService;
            this.CFBSplittingService = CFBSplittingService;
            this.CFBChartService = CFBChartService;
            this.CFBSubscribeService = CFBSubscribeService;
            this.CFBPreOrderService = CFBPreOrderService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            this.WalletLogService = WalletLogService;
            cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();

        }

        public ActionResult Account(int? page)
        {
            //动态构建查询
            List<Data.User> list = UserService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).ToList();

            if (Request["OrderFiled"] == "Wallet2004")
                list = list.OrderByDescending(x => x.Wallet2004).ToList();
            if (Request["OrderFiled"] == "BounsPeriod")
                list = list.OrderByDescending(x => x.BounsPeriod).ToList();
            if (Request["OrderFiled"] == "Addup1106")
                list = list.OrderByDescending(x => x.Addup1106).ToList();
            if (Request["OrderFiled"] == "Addup1107")
                list = list.OrderByDescending(x => x.Addup1107).ToList();

            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Sell(int id)
        {
            decimal sellbl = cacheSysParam.SingleAndInit(x => x.ID == 2402).Value2.ToDecimal();
            var onUser = UserService.Single(id);
            if (onUser != null && onUser.Wallet2004 > 0)
            {
                decimal number = onUser.Wallet2004 * sellbl;
                CFB.Sell(number, onUser.ID, "系统强制卖出");
                return Content("ok");
            }
            return Content("Error");
        }

        public ActionResult MultiSell(string ids)
        {
            decimal sellbl = cacheSysParam.SingleAndInit(x => x.ID == 2402).Value2.ToDecimal();
            string[] mids = ids.TrimEnd(',').TrimStart(',').Split(',');
            foreach (string id in mids)
            {
                int uid = id.ToInt();
                if (uid > 0)
                {
                    var onUser = UserService.Single(uid);
                    if (onUser != null && onUser.Wallet2004 > 0)
                    {
                        decimal number = onUser.Wallet2004 * sellbl;
                        CFB.Sell(number, onUser.ID, "系统强制卖出");
                    }
                }
            }
            return Content("ok");
        }

        public ActionResult Exchange()
        {
            ActMessage = "股权交易市场开启关闭";
            return View();
        }

        public ActionResult ThawStock()
        {
            ActMessage = "解冻股权";
            return View();
        }

        public ActionResult doThawStock()
        {
            CFB.OvertimeNotThaw();
            ViewBag.SuccessMsg = "操作成功";
            ActMessage = ViewBag.SuccessMsg;
            return View("Success");
        }

        public ActionResult PreOrder(int? page)
        {
            ActMessage = "股权预购单";
            var list = CFBPreOrderService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderBy(x => x.ID).ToList();
            string status = Request["status"];
            if (!string.IsNullOrEmpty(status))
            {
                int tjstatus = status.ToInt();
                if (tjstatus == 0)
                    list = list.Where(x => x.Status >= 0 && x.Status < 2).ToList();
                if (tjstatus == 2)
                    list = list.Where(x => x.Status == 2).ToList();
            }
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        /// <summary>
        /// 通过预购单转入购买
        /// </summary>
        public ActionResult passedPreOrder(string ids)
        {
            ActMessage = "审核预购单";
            var result = new ReturnResult();
            try
            {
                if (string.IsNullOrEmpty(ids)) throw new CustomException("请选择审核的预购单");

                string[] mids = ids.TrimEnd(',').TrimStart(',').Split(',');
                foreach (string id in mids)
                {
                    CFB.Subscription(id.ToInt());
                }
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

        public ActionResult PreOrderCommand(int id, string commandtype)
        {
            ActMessage = "置顶预购单";
            var model = CFBPreOrderService.Single(id);
            if (commandtype.ToLower() == "ontop")
                model.IsTop = true;
            else if (commandtype.ToLower() == "untop")
                model.IsTop = false;
            CFBPreOrderService.Update(model);
            SysDBTool.Commit();
            return RedirectToAction("PreOrder", "CFBCoin", new { status = 0 });
        }


        public ActionResult ExchangeCommand(string commandtype)
        {
            var sysEntity = MvcCore.Unity.Get<ISysSettingService>().Single(1);
            if (commandtype.ToLower() == "open")
                sysEntity.IsOpenCFB = true;
            else if (commandtype.ToLower() == "close")
                sysEntity.IsOpenCFB = false;
            MvcCore.Unity.Get<ISysSettingService>().Update(sysEntity);
            SysDBTool.Commit();
            return RedirectToAction("Exchange");
        }

        public ActionResult ModifyCFBIssue(int? id)
        {
            ActMessage = "添加股权发行计划";
            var model = new Data.CFBIssue();
            if (id.HasValue)
            {
                ActMessage = "修改发行计划";
                model = CFBIssueService.Single(id);
            }
            else
            {
                model.Period = CFBIssueService.List().Count() > 0 ? CFBIssueService.List().Max(x => x.Period) + 1 : 1;
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult ModifyCFBIssue(FormCollection fc)
        {
            var result = new ReturnResult();
            try
            {
                var entity = CFBIssueService.SingleAndInit(fc["theid"].ToInt());
                TryUpdateModel(entity, fc.AllKeys);
                if (entity.TotalStock == 0) throw new CustomException("发行量不能为空或0");
                if (entity.Price == 0) throw new CustomException("单价不能为空或0");
                //if (entity.TradeMinPrice == 0) throw new CustomException("最低交易不能为空或0");
                //if (entity.TradeMaxPrice == 0) throw new CustomException("最高交易不能为空或0"); 

                if (entity.ID > 0)
                    CFBIssueService.Update(entity);
                else
                {
                    if (CFBIssueService.List().Count() > 0) throw new CustomException("已经有发行计划，无法添加");
                    entity.Period = CFBIssueService.List().Count() > 0 ? CFBIssueService.List().Max(x => x.Period) + 1 : 1;
                    entity.Status = 1;
                    entity.HaveSubscribe = 0;
                    entity.CreateTime = DateTime.Now;
                    CFBIssueService.Add(entity);

                    //生成新的价格指数
                    CFBChartService.Add(new Data.CFBChart
                    {
                        Price = entity.Price,
                        CreateTime = DateTime.Now,
                        Volume = 0,
                        TurnoverMoney = 0
                    });
                }
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

        public ActionResult IssueCommand(int id, string commandtype)
        {
            //var model = CFBIssueService.Single(id);
            //CFBIssueService.Update(model);
            //SysDBTool.Commit();

            ViewBag.SuccessMsg = "操作成功";
            ActMessage = ViewBag.SuccessMsg;
            return View("Success");
        }

        public ActionResult CFBChart(int? page)
        {
            ActMessage = "股权交易统计表";
            var list = CFBChartService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult CFBIssue(int? page)
        {
            ActMessage = "股权发行计划列表";
            var list = CFBIssueService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult CFBSubscribe(int? page)
        {
            ActMessage = "股权抢购明细";
            var list = CFBSubscribeService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Splitting(int page = 1)
        {
            ActMessage = "股权拆分";
            var list = CFBSplittingService.List().OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page, 20));
        }

        //public ActionResult doCFH(decimal cfbl)
        //{
        //    ActMessage = "股权拆分";
        //    //线程里无法使用session
        //    DataCache.SetCache("StartTime", DateTime.Now);
        //    DataCache.SetCache("TotalRow", 0);
        //    DataCache.SetCache("CurrentRow", 0);
        //    //DataCache.SetCache("TransInfo", "分红进行中，请稍候...");
        //    Thread thread = new Thread(new ParameterizedThreadStart(delegate { threadprocH(cfbl); }));
        //    thread.Start();

        //    var list = CFBSplittingService.List().OrderByDescending(x => x.ID).ToList();
        //    return View("Splitting", list.ToPagedList(1, 20));
        //}

        //private void threadprocH(decimal newprice)
        //{
        //    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
        //    {
        //        //CFB.WeekSettlement(CFBEntrustsTradeService, CFBSplittingService, CFBTradeService, UserService, WalletLogService, CFBChartService, SysDBTool, LogDBTool, CFBIssueService, cacheSysParam, newprice);

        //        int _hperiod = cacheSysParam.SingleAndInit(x => x.ID == 2402).Value.ToInt();
        //        var userlist = UserService.List(x => x.Wallet2004 > 0 && (x.BounsPeriod ?? 0)>= _hperiod).OrderByDescending(x => x.ID).ToList();
        //        foreach (var item in userlist)
        //        {
        //            //帐号最拆分次数累计达3（参数1）次后强制卖出50%（参数2）
        //            if (MvcCore.Unity.Get<ISysSettingService>().Single(1).IsOpenCFB ?? false)//从交易大厅购买
        //            {
        //                //string _entrustsNo = Guid.NewGuid().ToString();
        //                ////添加委托记录
        //                //var model = new Data.CFBEntrustsTrade();
        //                //model.CreateTime = DateTime.Now;
        //                //model.Direction = 0;
        //                //model.EntrustsNo = _entrustsNo;
        //                //model.HaveTurnover = 0;
        //                //model.Poundage = 0;
        //                //model.Price = CFB.getcurrentprice();
        //                //model.Quantity = item.Wallet2004 * cacheSysParam.SingleAndInit(x => x.ID== 2402).Value2.ToDecimal();
        //                //model.Status = 0;
        //                //model.TotalAmount = model.Quantity * model.Price;
        //                //model.UID = item.ID;
        //                //model.UserName = item.UserName;
        //                //MvcCore.Unity.Get<ICFBEntrustsTradeService>().Add(model);
        //                //MvcCore.Unity.Get<ISysDBTool>().Commit();

        //                //var onUser = UserService.Single(item.ID);
        //                //onUser.BounsPeriod = 0;
        //                //UserService.Update(onUser);
        //                //SysDBTool.Commit();

        //                ////交易
        //                //CFB.Trading(_entrustsNo);
        //            }
        //        }
        //        ts.Complete();
        //    }
        //}

        //public JsonResult getproc()
        //{
        //    var proc = new
        //    {
        //        StartTime = DataCache.GetCache("StartTime"),
        //        CurrentRow = DataCache.GetCache("CurrentRow"),
        //        TotalRow = DataCache.GetCache("TotalRow"),
        //        TransInfo = DataCache.GetCache("TransInfo")
        //    };
        //    return Json(new { result = "ok", data = proc }, JsonRequestBehavior.AllowGet);
        //}
    }
}
