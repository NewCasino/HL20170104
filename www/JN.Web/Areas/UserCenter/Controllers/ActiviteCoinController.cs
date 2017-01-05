using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JN.Data;
using JN.Data.Service;
using MvcCore.Controls;
using JN.Services.Manager;
using JN.Services.Tool;
using JN.Services.CustomException;
namespace JN.Web.Areas.UserCenter
{
    public class ActiviteCoinController : Controller
    {
        private readonly IUserService UserService;
        private readonly ISysParamService SysParamService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;
        private readonly IInvestmentService InvestmentService;

        public ActiviteCoinController(ISysDBTool SysDBTool, IUserService UserService, ISysParamService SysParamService, IActLogService ActLogService, IInvestmentService InvestmentService)
        {
            this.UserService = UserService;
            this.SysParamService = SysParamService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            this.InvestmentService = InvestmentService;

        }
        //
        // GET: /UserCenter/ActiviteCoin/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 购买激活币
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BuyActiviteCoin(FormCollection fc)
        {
            int id = JN.Services.UserLoginHelper.CurrentUser().ID;
            var usermodel = UserService.Single(id);
            string passworld2 = fc["password2"];
            string BuyNumCoinStr = fc["BuyNumber"];
            int BuyNumCoin = BuyNumCoinStr.ToDecimal().ToInt();
            ReturnResult result = new ReturnResult();

            if((BuyNumCoin%200)!=0){
                result.Message = "请输入200的倍数";
                return Json(result);
            }

            if (string.IsNullOrEmpty(passworld2))
            {
                result.Message = "二级密码不能为空！";

            }
            if (BuyNumCoin > 0)
            {

                if (usermodel.Password2 == passworld2.ToMD5().ToMD5())
                {
                    if (BuyNumCoin > usermodel.Wallet2001)
                    {
                        result.Message = "余额不足已购买激活币！";
                    }
                    else
                    {
                        usermodel.Wallet2004 += BuyNumCoin;
                        usermodel.Wallet2001 -= BuyNumCoin;
                        UserService.Update(usermodel);
                        SysDBTool.Commit();
                        result.Status = 200;
                    }
                }
                else
                {
                    result.Message = "二级密码错误！";
                }
            }
            else
            {
               result.Message = "输入购买数量错误！";
            }
            return Json(result);
        }
        /// <summary>
        /// 激活币转账
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TransferCoin() {
            var Umodel = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(JN.Services.UserLoginHelper.CurrentUser().ID);
            if (!(Umodel.WalletLock ?? false))
            {
                ViewBag.Title = "激活币转帐";
                ViewBag.Poundage =200;
                ViewBag.Beisu = 200;
                ViewBag.transCoin = 2004;
                return View(Umodel);
            }
            else
            {
                ViewBag.ErrorMsg = "您的转帐功能已被关闭，详情请联系管理员。";
                return View("Error");
            }
            //return View();
        }
        [HttpPost]
        public ActionResult TransferCoin(FormCollection form) {

                 ReturnResult result = new ReturnResult();
           
                var Umodel = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(JN.Services.UserLoginHelper.CurrentUser().ID);
                string recusername = form["recuser"];
                string transfermoney = form["transfermoney"];
                string remark = form["remark"];
                string password2 = form["password2"];
                if (Umodel.IsLock) result.Message="您的帐号受限，无法进行相关操作";
                if (Umodel.Password2 != password2.ToMD5().ToMD5()) result.Message="二级密码不正确";
                if (transfermoney.ToDecimal() <= 0) result.Message="请输入正确的金额";
                var recUser = UserService.Single(x => x.UserName == recusername.Trim());
                if (recUser == null) result.Message="转入用户不存在";
                if (Umodel.UserName == recusername.Trim()) result.Message="不可以给自己转帐";
                if (remark.Trim().Length > 100) result.Message="备注长度不能超过100个字节" ;
                if (Umodel.Wallet2004 < transfermoney.ToDecimal()) result.Message="帐户余额不足";
                if (!(Umodel.RefereePath + ",").Contains("," + recUser.ID.ToString() + ",") && !(recUser.RefereePath + ",").Contains("," + Umodel.ID.ToString() + ",")) result.Message="请在推荐关系一条线内进行转账";

                #region 事务操作
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    Wallets.doTransfer(Umodel, recUser, transfermoney.ToDecimal(), 2004, remark);
                    ts.Complete();
                    result.Status = 200;
                }
                #endregion
            
            return Json(result);
        }
        /// <summary>
        /// 投资钱包
        /// </summary>
        /// <returns></returns>
        public ActionResult InvestmentWallet() {

            return View();
        }

        [HttpPost]    
        public ActionResult getInvestmentWallet(FormCollection fc)
        {
            ReturnResult reslut = new ReturnResult();

            var Umodel = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(JN.Services.UserLoginHelper.CurrentUser().ID);
            //账户是否已投资
            if (Umodel.Investment > 0) {
                reslut.Message = "账户已投资！";
                reslut.Status = 500;
                return Json(reslut);
            }
            string investmentnum = fc["investmentNum"];
            int investment =(int)investmentnum.ToDecimal();

            if (investment > 0&&investment<=Umodel.Wallet2001)
            {
                Umodel.Investment = investment;
                Umodel.Wallet2001 -= investment;
                UserService.Update(Umodel);
                SysDBTool.Commit();

                //写入汇款表
                var model = new Data.Investment
                {
                    InvestmentNo = Bonus.GetRechargeNo(),
                    InvestmentType = 1,
                    UID = Umodel.ID,
                    UserName = Umodel.UserName,
                    ApplyInvestment = investment,
                    HaveMoney = investment,
                    SettlementMoney = 0,
                    IsBalance = true,
                    Period = 0,
                    Status = (int)JN.Data.Enum.InvestmentStatus.Transaction,
                    CreateTime = DateTime.Now,
                    Remark = "",

                };
                InvestmentService.Add(model);
                SysDBTool.Commit();
                //进行首次分红 计算直推奖 
                Bonus.CalculateBonus(Umodel.ID);
                reslut.Status = 200;
                return Json(reslut);
            }
            return Json(reslut);
        }      
    }

}