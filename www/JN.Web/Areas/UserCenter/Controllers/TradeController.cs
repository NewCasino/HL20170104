using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JN.Data;
using JN.Data.Service;
using MvcCore.Controls;
using PagedList;
using MvcCore.Extensions;
using JN.Data.Enum;
using JN.Services.Manager;
using System.Data.Entity.SqlServer;
using JN.Services.Tool;
using JN.Services.CustomException;
using System.Data.Entity.Validation;

namespace JN.Web.Areas.UserCenter.Controllers
{
    public class TradeController : BaseController
    {
        private static List<Data.SysParam> cacheSysParam = null;
        private static Data.SysSetting syssetting = null;
        private readonly IUserService UserService;
        private readonly IStockChartDayService StockChartDayService;
        private readonly IStockChartHourService StockChartHourService;
        private readonly IStockEntrustsTradeService StockEntrustsTradeService;
        private readonly IStockIssueService StockIssueService;
        private readonly IStockSubscribeService StockSubscribeService;
        private readonly IStockTradeService StockTradeService;
        private readonly ISysSettingService SysSettingService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public TradeController(ISysDBTool SysDBTool, 
            IUserService UserService, 
            IStockChartDayService StockChartDayService,
            IStockChartHourService StockChartHourService,
            IStockEntrustsTradeService StockEntrustsTradeService,
            IStockIssueService StockIssueService,
            IStockSubscribeService StockSubscribeService,
            IStockTradeService StockTradeService,
     ISysSettingService SysSettingService,
            IActLogService ActLogService)
        {
            this.UserService = UserService;
            this.StockChartDayService = StockChartDayService;
            this.StockChartHourService = StockChartHourService;
            this.StockEntrustsTradeService = StockEntrustsTradeService;
            this.StockIssueService = StockIssueService;
            this.StockSubscribeService = StockSubscribeService;
            this.StockTradeService = StockTradeService;
            this.SysSettingService = SysSettingService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();
            syssetting = MvcCore.Unity.Get<ISysSettingService>().Single(1);
            
        }

        public ActionResult Order(int page = 1)
        {
            ActMessage = "交易订单";
            var list = StockEntrustsTradeService.List(x => x.UID == Umodel.ID).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page, 20));
        }

        public ActionResult HighStock()
        {
            return View();
        }

        public ActionResult Subscription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Subscription(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string amount = form["rgamount"];
                string tradePassword = form["tradePassword"];
                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");
                if (amount.Trim().Length == 0) throw new CustomException("抢购数量不能为空");
                if (Umodel.Password2 != tradePassword.ToMD5().ToMD5()) throw new CustomException("二级密码不正确");
                var issue = StockIssueService.Single(x => x.Status == 1);
                if (issue == null) throw new CustomException("未开放抢购");
                if ((issue.HaveSubscribe + amount.ToDecimal()) > issue.TotalStock) throw new CustomException("很抱歉，本次抢购已超过本期发行总量，无法抢购！");

                var rglist = StockSubscribeService.List(x => x.UID == Umodel.ID);
                decimal yrg = rglist.Count() > 0 ? rglist.Sum(x => (x.ApplyNumber ?? 0)) : 0;
                if (!(syssetting.IsOpenCFB ?? false) && (yrg + amount.ToDecimal()) > (issue.MaxBuy ?? 0)) throw new CustomException("很抱歉，您累计抢购数量已超过最大可抢购数量！");

                decimal totalamount = issue.Price * amount.ToDecimal();
                if (Umodel.Wallet2002 < totalamount) throw new CustomException("很抱歉，您的帐户余额不足");
                #region 事务操作

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    //扣除金额
                    Wallets.changeWallet(Umodel.ID, 0 - totalamount, 2002, "认购虚拟币");

                    //添加抢购记录
                    StockSubscribeService.Add(new StockSubscribe { ApplyNumber = amount.ToDecimal(), SubscribeNumber = amount.ToDecimal(), ThawType = 1, Period = issue.Period, TotalAmount = totalamount, CanBeUsed = 0, CreateTime = DateTime.Now, Price = issue.Price, Status = 2, Title = issue.Title, UsedTimes = 0, UID = Umodel.ID, UserName = Umodel.UserName });
                    SysDBTool.Commit();

                    Wallets.changeWallet(Umodel.ID, amount.ToDecimal(), 2003, "认购虚拟币得到");
                    issue.HaveSubscribe = issue.HaveSubscribe + amount.ToDecimal();
                    StockIssueService.Update(issue);
                    SysDBTool.Commit();

                    //第一次解冻
                    //users.thawStock(sid);
                    ts.Complete();
                    result.Status = 200;
                }
                #endregion
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

        /// <summary>
        /// 认购解冻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ThawStock(int id)
        {
            var model = StockSubscribeService.Single(id);
            if (model != null && model.UID == Umodel.ID)
            {
                if (model.Status <= (int)Data.Enum.SubscribeStatus.PartOfUsed)
                {
                    //添加事务 关于事务的介绍http://www.tuicool.com/articles/qaMzIb
                    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                    {
                        bool result = Stocks.thawStock(model.ID);
                        ts.Complete();
                        if (result)
                            return Content("ok");
                        else
                            return Content("解冻失败！");
                    }
                }
                else
                {
                    return Content("当前认购单状态无法解冻！");
                }
            }
            else
            {
                return Content("非法操作！");
            }
        }

        public ActionResult Transaction(int page = 1)
        {
            ActMessage = "成交查询";
            var list = StockTradeService.List(x => x.BuyUID == Umodel.ID || x.SellUID == Umodel.ID).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page, 20));
        }

        [HttpPost]
        public ActionResult TTCIn(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                decimal number = form["buynumber"].ToDecimal();
                decimal price = form["buyprice"].ToDecimal();
                string tradePassword = form["tradeinPassword"];
                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");

                decimal PARAM_BUYPOUNDAGE = cacheSysParam.SingleAndInit(x => x.ID == 2302).Value.ToDecimal(); //买入手续费

                if (!(syssetting.IsOpenTrade ?? false))
                {
                    if (MvcCore.Unity.Get<IStockIssueService>().List(x => x.Status == 1).Count() > 1)
                        throw new CustomException("FＣ币认购中，不能交易！");
                    else
                        throw new CustomException("FC币交易大厅关闭，无法交易！");
                }
               
                if (number <= 0) throw new CustomException("买入数量不正确");
                if (Umodel.Password2 != tradePassword.ToMD5().ToMD5()) throw new CustomException("二级密码不正确");

                decimal PriceRangMin = 0; //价格范围(最低)
                decimal PriceRangMax = 0; //价格范围(最高)
                decimal OpenPrice = Stocks.getopenprice();
                    PriceRangMin = OpenPrice - OpenPrice * Convert.ToDecimal(0.1);
                    PriceRangMax = OpenPrice + OpenPrice * Convert.ToDecimal(0.1);
                if (price < PriceRangMin) throw new CustomException("买入价格不正确");
                if (price > PriceRangMax) throw new CustomException("买入价格不正确");

                //总价
                decimal totalamount = price * number;
                //手续费
                decimal poundage = totalamount * PARAM_BUYPOUNDAGE;
                //扣除金额
                decimal drawmoney = totalamount + poundage;
                if (Umodel.Wallet2002 < drawmoney) throw new CustomException("很抱歉，您的帐户余额不足");
                #region 事务操作
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    //扣除金额
                    Wallets.changeWallet(Umodel.ID, 0 - drawmoney, 2002, "委托买入冻结");
                    string _entrustsNo = Guid.NewGuid().ToString();
                    //添加委托记录
                    var model = new Data.StockEntrustsTrade();
                    model.CreateTime = DateTime.Now;
                    model.Direction = 0;
                    model.EntrustsNo = _entrustsNo;
                    model.HaveTurnover = 0;
                    model.Poundage = poundage;
                    model.Price = price;
                    model.Quantity = number;
                    model.Status = 0;
                    model.TotalAmount = totalamount;
                    model.UID = Umodel.ID;
                    model.UserName = Umodel.UserName;
                    StockEntrustsTradeService.Add(model);
                    SysDBTool.Commit();
                    //交易
                    Stocks.TTC(_entrustsNo);
                    result.Status = 200;
                    ts.Complete();
                }
                #endregion
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


        #region 委托卖单
        [HttpPost]
        public ActionResult TTCOut(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                decimal number = form["sellnumber"].ToDecimal();
                decimal price = form["sellprice"].ToDecimal();
                string tradePassword = form["tradeoutPassword"];
                if (!(syssetting.IsOpenTrade ?? false))
                {
                    if (MvcCore.Unity.Get<IStockIssueService>().List(x => x.Status == 1).Count() > 1)
                        throw new CustomException("FC币认购中，不能交易！");
                    else
                        throw new CustomException("FC币交易大厅关闭，无法交易！");
                }
                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");

                decimal PARAM_SELLPOUNDAGE = cacheSysParam.SingleAndInit(x => x.ID == 2303).Value.ToDecimal(); //卖出手续费

                if (number <= 0) throw new CustomException("卖出数量不正确");
                if (Umodel.Password2 != tradePassword.ToMD5().ToMD5()) throw new CustomException("二级密码不正确");

                decimal PriceRangMin = 0; //价格范围(最低)
                decimal PriceRangMax = 0; //价格范围(最高)
                decimal OpenPrice = Stocks.getopenprice();
                    PriceRangMin = OpenPrice - OpenPrice * Convert.ToDecimal(0.1);
                    PriceRangMax = OpenPrice + OpenPrice * Convert.ToDecimal(0.1);
                if (price < PriceRangMin) throw new CustomException("卖出价格不正确");
                if (price > PriceRangMax) throw new CustomException("卖出价格不正确");

                //当天卖出数量（所有已挂单或成交）
                var selllist = StockEntrustsTradeService.List(x => x.UID == Umodel.ID && x.Direction == 1 && x.Status >= (int)Data.Enum.TTCStatus.Entrusts && SqlFunctions.DateDiff("DAY", x.CreateTime, DateTime.Now) == 0);
                decimal todaysell = selllist.Count() > 0 ? selllist.Sum(x => x.Quantity) : 0;
                //当天买入数量（已成交）
                var buylist = StockTradeService.List(x => x.BuyUID == Umodel.ID && SqlFunctions.DateDiff("DAY", x.CreateTime, DateTime.Now) == 0);
                decimal todaybuy = buylist.Count() > 0 ? buylist.Sum(x => x.Quantiry) : 0;
                decimal todayfirst = Umodel.Wallet2003 - todaybuy + todaysell;
                decimal todaymaxsell = todayfirst * cacheSysParam.SingleAndInit(x => x.ID == 2304).Value.ToDecimal();// + todaybuy * cacheSysParam.SingleAndInit(x => x.ID == 2304).Value.ToDecimal();

                //logs.WriteLog("todaybuy:" + todaybuy + ",todaysell:" + todaysell + ",todayfirst:" + todayfirst + ",todaymaxsell:" + todaymaxsell);
                if ((number + todaybuy) > todaymaxsell) throw new CustomException("最高卖出数量不能超过" + todaymaxsell + "股");
                //if (Session["TTCOutTime"] != null && Session["TTCOutNumber"] != null)
                //{
                //    if (!DateTimeDiff.DateDiff_1minu(DateTime.Parse(Session["TTCOutTime"].ToString())) && decimal.Parse(Session["TTCOutNumber"].ToString()) == number)
                //    {
                //        strErr += "请不要短时间内重复多次卖出 <br />";
                //    }
                //}

                //总价
                decimal totalamount = price * number;
                //手续费(下单数量0.1%的手续费)
                decimal poundage = number * PARAM_SELLPOUNDAGE;
                decimal totalnumber = (number + poundage);
                if (Umodel.Wallet2003 < totalnumber) throw new CustomException("很抱歉，您的虚拟币数量不足");
                #region 事务操作
                System.Web.HttpContext.Current.Session["TTCOutTime"] = DateTime.Now;
                System.Web.HttpContext.Current.Session["TTCOutNumber"] = number;
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    //扣除虚拟币
                    Wallets.changeWallet(Umodel.ID, 0 - totalnumber, 2003, "委托卖出冻结");
                    string _entrustsNo = Guid.NewGuid().ToString();
                    //添加委托记录
                    var model = new Data.StockEntrustsTrade();
                    model.CreateTime = DateTime.Now;
                    model.Direction = 1;
                    model.EntrustsNo = _entrustsNo;
                    model.HaveTurnover = 0;
                    model.Poundage = poundage;
                    model.Price = price;
                    model.Quantity = number;
                    model.Status = 0;
                    model.TotalAmount = totalamount;
                    model.UID = Umodel.ID;
                    model.UserName = Umodel.UserName;
                    StockEntrustsTradeService.Add(model);
                    SysDBTool.Commit();
                    //交易
                    Stocks.TTC(_entrustsNo);
                    result.Status = 200;
                    ts.Complete();
                }
                #endregion
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

        #endregion

        /// <summary>
        /// 取消委托
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CancelTTC(int id)
        {
            var model = StockEntrustsTradeService.Single(id);
            if (model != null && model.UID == Umodel.ID)
            {
                if (model.Status <= (int)TTCStatus.PartOfTheDeal && model.Status >= (int)TTCStatus.Entrusts)
                {
                    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                    {
                        Stocks.cancelTTC(model.EntrustsNo);
                        ts.Complete();
                        return Content("ok");
                    }
                }
                else
                {
                    return Content("您的委托单已产生交易，无法取消！");
                }
            }
            else
            {
                return Content("记录不存在或已被删除！");
            }
        }


        public ActionResult Entrusts(int direction = 0)
        {
            //DataSet ds = SQLBase.GetList("SELECT TOP 5 Price, SUM(Quantity - HaveTurnover) AS Quantity FROM StockEntrustsTrade WHERE Direction = " + direction + " and Status<=" + (int)TTCStatus.PartOfTheDeal + " and Status>=" + (int)TTCStatus.Entrusts + " GROUP BY Price ORDER BY Price DESC");
            //var list = StockEntrustsTradeService.List(x => x.Direction == direction && x.Status <= (int)TTCStatus.PartOfTheDeal && x.Status >= (int)TTCStatus.Entrusts).Select(o => new {
            //    Price = o.Price,
            //    Quantiry = o.Quantity - o.HaveTurnover,

            //}).GroupBy(x => x.Price).OrderBy(x => x.Key).Select(s=>new {Price = s.Key,Quantiry = s.Sum(t=>t.Quantiry)}).Take(5).ToList();
            var list = StockEntrustsTradeService.List(x => x.Direction == direction && x.Status <= (int)TTCStatus.PartOfTheDeal && x.Status >= (int)TTCStatus.Entrusts).Select(o => new {
                o.UserName,
                o.Price,
                o.Quantity,
                Surplus = o.Quantity - o.HaveTurnover,
                o.CreateTime
            }).Take(5).ToList();
            return Content(list.ToJson());
        }

        public JsonResult gettop10entrusts()
        {
            var data0 = StockEntrustsTradeService.List(x => x.Direction == 1 && x.Status <= (int)TTCStatus.PartOfTheDeal && x.Status >= (int)TTCStatus.Entrusts).Select(o => new
            {
                Price = o.Price,
                Quantity = o.Quantity - o.HaveTurnover,

            }).GroupBy(x => x.Price).OrderByDescending(x => x.Key).Select(s => new { ds = "卖", Price = s.Key, Quantity = s.Sum(t => t.Quantity) }).Take(5).ToList();

            var data1 = StockEntrustsTradeService.List(x => x.Direction == 0 && x.Status <= (int)TTCStatus.PartOfTheDeal && x.Status >= (int)TTCStatus.Entrusts).Select(o => new
            {
                Price = o.Price,
                Quantity = o.Quantity - o.HaveTurnover,

            }).GroupBy(x => x.Price).OrderByDescending(x => x.Key).Select(s => new { ds = "买", Price = s.Key, Quantity = s.Sum(t => t.Quantity) }).Take(5).ToList();
            return Json(new { data0 = data0, data1 = data1 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TTC()
        {
            if (!(syssetting.IsOpenTrade ?? false))
            {
                ViewBag.ErrorMsg = "即将开放敬请期待！";
                return View("Error");
            }
            return View();
            //return RedirectToAction("Wait", "Home");
        }

        public ActionResult FhbList()
        {
            return View();
        }
        public ActionResult MyEntrusts()
        {
            return PartialView("_MyEntrusts");
        }
        public ActionResult TOP28Transaction()
        {
            return PartialView("_Top28Transaction");
        }

        public JsonResult getuserinfo()
        {
            return Json(new { uname = Umodel.UserName, rmb = Umodel.Wallet2002, lmtb = Umodel.Wallet2003 }, JsonRequestBehavior.AllowGet);
        }
    }
}
