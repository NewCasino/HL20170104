using JN.Data;
using JN.Data.Service;
using MvcCore.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using JN.Services.Tool;

namespace JN.Web.Areas.AdminCenter.Controllers
{
    public class StockController : BaseController
    {
        private readonly IUserService UserService;
        private readonly IStockIssueService StockIssueService;
        private readonly IStockEntrustsTradeService StockEntrustsTradeService;
        private readonly IStockTradeService StockTradeService;
        private readonly IStockChartDayService StockChartDayService;
        private readonly IStockChartHourService StockChartHourService;
        private readonly IStockSubscribeService StockSubscribeService;
        private readonly ISysParamService SysParamService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;
        private readonly IWalletLogService WalletLogService;

        public StockController(ISysDBTool SysDBTool,
            IUserService UserService,
            IStockIssueService StockIssueService,
            IStockEntrustsTradeService StockEntrustsTradeService,
            IStockTradeService StockTradeService,
            IStockChartDayService StockChartDayService,
            IStockChartHourService StockChartHourService,
            IStockSubscribeService StockSubscribeService,
            ISysParamService SysParamService,
            IActLogService ActLogService, 
            IWalletLogService WalletLogService)
        {
            this.UserService = UserService;
            this.StockIssueService = StockIssueService;
            this.StockEntrustsTradeService = StockEntrustsTradeService;
            this.StockTradeService = StockTradeService;
            this.StockChartDayService = StockChartDayService;
            this.StockChartHourService = StockChartHourService;
            this.StockSubscribeService = StockSubscribeService;
            this.SysParamService = SysParamService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            this.WalletLogService = WalletLogService;
        }

        public ActionResult Exchange()
        {
            ActMessage = "开启关闭虚拟币交易市场";
            return View();
        }

        public ActionResult ExchangeCommand(string commandtype)
        {
            var sysEntity = MvcCore.Unity.Get<JN.Data.Service.ISysSettingService>().Single(1);
            if (commandtype.ToLower() == "open")
                sysEntity.IsOpenTrade = true;
            else if (commandtype.ToLower() == "close")
                sysEntity.IsOpenTrade = false;
            MvcCore.Unity.Get<JN.Data.Service.ISysSettingService>().Update(sysEntity);
            SysDBTool.Commit();
            return RedirectToAction("Exchange");
        }

        public ActionResult ModifyStockIssue(int? id)
        {
            ActMessage = "添加发行计划";
            var model = new Data.StockIssue();
            if (id.HasValue)
            {
                ActMessage = "修改发行计划";
                model = StockIssueService.Single(id);
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult ModifyStockIssue(FormCollection fc)
        {
            var result = new ReturnResult();
            try
            {
                var entity = StockIssueService.SingleAndInit(fc["theid"].ToInt());
                TryUpdateModel(entity, fc.AllKeys);
                string strErr = "";

                if (entity.TotalStock == 0) strErr += "发行量不能为空或0<br />";
                if (entity.Price == 0) strErr += "单价不能为空或0<br />";
                //if (entity.TradeMinPrice == 0) strErr += "最低交易不能为空或0br />";
                //if (entity.TradeMaxPrice == 0) strErr += "最高交易不能为空或0<br />";
                if (strErr != "")
                {
                    ViewBag.ErrorMsg = "抱赚您填写的信息有误： <br />" + strErr + "请核实后重新提交！";
                    return View("Error");
                }

                if (entity.ID > 0)
                    StockIssueService.Update(entity);
                else
                {
                    entity.Period = StockIssueService.List().Count() > 0 ? StockIssueService.List().Max(x => x.Period) + 1 : 1;
                    entity.Status = 0;
                    entity.HaveSubscribe = 0;
                    entity.CreateTime = DateTime.Now;
                    StockIssueService.Add(entity);
                }
                SysDBTool.Commit();
                result.Status = 200;
            }
            catch (Exception ex)
            {
                 result.Message = StringHelp.FormatErrorString(ex.Message);
                Services.Manager.logs.WriteErrorLog(HttpContext.Request.Url.ToString(), ex);
            }
            return Json(result);
        }

        public ActionResult IssueCommand(int id, string commandtype)
        {
            var model = StockIssueService.Single(id);
            if (commandtype.ToLower() == "start")
            {
                model.StartTime = DateTime.Now;
                model.Status = 1;
            }
            else if (commandtype.ToLower() == "end")
            {
                model.EndTime = DateTime.Now;
                model.Status = 2;
            }
            StockIssueService.Update(model);
            SysDBTool.Commit();

            ViewBag.SuccessMsg = "操作成功";
            ActMessage = ViewBag.SuccessMsg;
            return View("Success");
        }

        /// <summary>
        /// 修复委托与真实成交
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Repair(int id)
        {
            var model = StockEntrustsTradeService.Single(id);
            if (model != null)
            {
                if (model.Status > 0)
                {
                    decimal realturnover = 0;
                    if (model.Direction == 0)
                        realturnover = StockTradeService.List(x => x.BuyEntrusNo.Equals(model.EntrustsNo)).Sum(x => x.Quantiry);
                    else
                        realturnover = StockTradeService.List(x => x.SellEntrusNo.Equals(model.EntrustsNo)).Sum(x => x.Quantiry);

                    if (realturnover != model.HaveTurnover)
                    {
                        if (realturnover < model.HaveTurnover)
                        {
                            model.HaveTurnover = realturnover;
                            //if (model.HaveTurnover < model.Quantity && model.HaveTurnover > 0)
                            //    model.Status = (int)JN.Entity.Enum.TTCStatus.PartOfTheDeal;
                            //else if (model.HaveTurnover == 0)
                            //    model.Status = (int)JN.Entity.Enum.TTCStatus.Entrusts;
                            StockEntrustsTradeService.Update(model);
                            SysDBTool.Commit();
                            //交易
                            //users.TTC(model.ID);
                            ViewBag.SuccessMsg = "修复成功！";
                            ActMessage = ViewBag.SuccessMsg;
                            return View("Success");
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "无法修复，请联系管理员。";
                            return View("Error");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "无效的执行命令。";
                        return View("Error");
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "订单状态为已撤消或未有成交，不需要修复。";
                    return View("Error");
                }
            }
            ViewBag.ErrorMsg = "ID不存在或已被删除。";
            return View("Error");
        }


        public ActionResult StockChartDay(int? page)
        {
            ActMessage = "虚拟币日交易统计表";
            var list = StockChartDayService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult StockChartHour(int? page)
        {
            ActMessage = "虚拟币日交易统计表";
            var list = StockChartHourService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult StockEntrustsTrade(int? page)
        {
            ActMessage = "虚拟币委托交易明细";
            var list = StockEntrustsTradeService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult StockEntrustsTrade2(int? page)
        {
            ActMessage = "虚拟币委托交易明细（未成交）";
            var list = StockEntrustsTradeService.List(x => x.Status < 2 && x.Status >= 0).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult StockTrade(int? page)
        {
            ActMessage = "虚拟币成交交易明细";
            var list = StockTradeService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult StockIssue(int? page)
        {
            ActMessage = "虚拟币发行计划列表";
            var list = StockIssueService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult StockSubscribe(int? page)
        {
            ActMessage = "虚拟币抢购明细";
            var list = StockSubscribeService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult MakeCancel(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var list = StockEntrustsTradeService.List(a => ids.TrimEnd(',').TrimStart(',').Contains(a.ID.ToString()));
                //using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                //{
                //    foreach (var model in list)
                //    {
                //        //if (model.Status <= (int)JN.Entity.Enum.TTCStatus.PartOfTheDeal && model.Status >= (int)JN.Entity.Enum.TTCStatus.Entrusts)
                //        //{
                //        //    users.cancelTTC(model.ID);
                //        //}
                //    }
                //    ts.Complete();
                //}
            }
            return Content("ok");
        }


    }
}
