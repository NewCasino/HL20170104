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
using System.Collections;
using System.Text;
using JN.Services.Tool;
using JN.Services.CustomException;

namespace JN.Web.Areas.UserCenter.Controllers
{
    public class CFBController : BaseController
    {
        private static List<Data.SysParam> cacheSysParam = null;
        private static Data.SysSetting syssetting = null;
        private readonly IUserService UserService;
        private readonly ICFBChartService CFBChartService;
        private readonly ICFBIssueService CFBIssueService;
        private readonly ICFBSubscribeService CFBSubscribeService;
        private readonly ISysSettingService SysSettingService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public CFBController(ISysDBTool SysDBTool, 
            IUserService UserService, 
            ICFBChartService CFBChartService,
            ICFBIssueService CFBIssueService,
            ICFBSubscribeService CFBSubscribeService,
            ISysSettingService SysSettingService,
            IActLogService ActLogService)
        {
            this.UserService = UserService;
            this.CFBChartService = CFBChartService;
            this.CFBIssueService = CFBIssueService;
            this.CFBSubscribeService = CFBSubscribeService;
            this.SysSettingService = SysSettingService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            cacheSysParam = MvcCore.Unity.Get<ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();
            syssetting = MvcCore.Unity.Get<ISysSettingService>().Single(1);
        }

        public ActionResult Order(int page = 1)
        {
            ActMessage = "交易订单";
            var list = CFBSubscribeService.List(x => x.UID == Umodel.ID).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page, 20));
        }

        public ActionResult Subscription()
        {
            return View();
        }

        /// <summary>
        /// 认购解冻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ThawStock(int id)
        {
            var model = CFBSubscribeService.Single(id);
            if (model != null && model.UID == Umodel.ID)
            {
                if (model.Status <= (int)Data.Enum.SubscribeStatus.PartOfUsed)
                {
                    lock(this)
                    {
                        using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                        {
                            bool result = CFB.thawStock(model.ID, "会员自行解冻");
                            ts.Complete();
                            if (result)
                                return Content("ok");
                            else
                                return Content("解冻失败！");
                        }
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
        public ActionResult ajax_coin_info_width()
        {
            short scale = 0;
            var chart = CFBChartService.List().OrderByDescending(x => x.Price).ToList().FirstOrDefault();
            if (chart != null)
            {
                scale = (short)(chart.TurnoverScale * 100);
            }
            return Content("[" + scale + "]");
        }

        public ActionResult ajax_coin_info_height()
        {
            var data = CFBChartService.List().OrderBy(x => x.Price);
            string table = "";
            data.ToList().ForEach(i =>
            {
                table += "[\"" + i.Price.ToString("F2") + "\"," + (i.TurnoverScale * 100) + "],";
            });
            return Content("[" + table.TrimEnd(',') + "]");
        }

        [HttpPost]
        public ActionResult Subscription(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string buymoney = form["buymoney"];
                string tradePassword = form["tradeinPassword"];
                if (Umodel.IsLock || !Umodel.IsActivation) throw new CustomException("您的帐号受限，无法进行相关操作");
                //if (amount.Trim().Length == 0) throw new CustomException("抢购数量不能为空");
                if (Umodel.Password2 != tradePassword.ToMD5().ToMD5()) throw new CustomException("二级密码不正确");
                var issue = CFBIssueService.Single(x => x.Status == 1);
                if (issue == null) throw new CustomException("未开放抢购");
                //if ((issue.HaveSubscribe + amount.ToDecimal()) > issue.TotalStock) throw new CustomException("很抱歉，本次抢购已超过本期发行总量，无法抢购！");

                var rglist = CFBSubscribeService.List(x => x.UID == Umodel.ID);
                decimal yrg = rglist.Count() > 0 ? rglist.Sum(x => (x.ApplyNumber ?? 0)) : 0;

                decimal totalamount = buymoney.ToDecimal();// issue.Price * amount.ToDecimal();
                if (totalamount <= 0) throw new CustomException("请输入正确的金额");
                if (totalamount < cacheSysParam.SingleAndInit(x => x.ID == 2403).Value.ToDecimal()) throw new CustomException("最少买入为" + cacheSysParam.SingleAndInit(x => x.ID == 2403).Value.ToDecimal() + "EP");
                if (Umodel.Wallet2002 < totalamount) throw new CustomException("很抱歉，您的帐户余额不足");

                if (MvcCore.Unity.Get<ICFBPreOrderService>().List(x => x.UID == Umodel.ID && (x.Status == 0 || x.Status == 1) && x.Origin == "EP").Count() > 0)
                    throw new CustomException("您有排队中的订单，无法买入");

                if (MvcCore.Unity.Get<ICFBSubscribeService>().List(x => x.UID == Umodel.ID && x.CanBeUsed == 0 && x.Origin == "EP").Count() > 0 )
                    throw new CustomException("请在解冻股权后再购买");

                #region 事务操作

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    //扣除金额
                    Wallets.changeWallet(Umodel.ID, 0 - totalamount, 2002, "抢购股权");
                    CFB.PreOrder(totalamount, Umodel.ID, "EP", "买入");
                    ts.Complete();
                    result.Status = 200;
                }
                #endregion
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

        #region 委托卖单
        [HttpPost]
        public ActionResult TTCOut(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                decimal number = form["sellnumber"].ToDecimal();
                string tradePassword = form["tradeoutPassword"];
                if (Umodel.IsLock || !Umodel.IsActivation) throw new CustomException("您的帐号受限，无法进行相关操作");
                if (number <= 0) throw new CustomException("卖出数量不正确");
                if (Umodel.Password2 != tradePassword.ToMD5().ToMD5()) throw new CustomException("二级密码不正确");


                if (Session["TTCOutTime"] != null && Session["TTCOutNumber"] != null)
                {
                    if (!DateTimeDiff.DateDiff_1minu(DateTime.Parse(Session["TTCOutTime"].ToString())) && decimal.Parse(Session["TTCOutNumber"].ToString()) == number)
                        throw new CustomException("请不要短时间内重复多次卖出");
                }

                if (Umodel.Wallet2004 < number.ToDecimal()) throw new CustomException("很抱歉，您的股权数量不足");
                #region 事务操作
                System.Web.HttpContext.Current.Session["TTCOutTime"] = DateTime.Now;
                System.Web.HttpContext.Current.Session["TTCOutNumber"] = number;
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    CFB.Sell(number, Umodel.ID, "个人卖出");
                    result.Status = 200;
                    ts.Complete();
                }
                #endregion
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

        #endregion
        public JsonResult getuserinfo()
        {
            return Json(new { uname = Umodel.UserName, rmb = Umodel.Wallet2002, lmtb = Umodel.Wallet2004 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TTC()
        {
            decimal currentprice = JN.Services.Manager.CFB.getcurrentprice();
            if (currentprice <= 0)
            {
                ViewBag.ErrorMsg = "交易大厅将在拆分币发行后开放。";
                return View("Error");
            }
            return View();
        }
    }
}
