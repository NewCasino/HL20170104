using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using JN.Data;
using JN.Data.Service;
using MvcCore.Controls;
using PagedList;
using JN.Services.Manager;
using JN.Services.Tool;
using JN.Services.CustomException;
using System.IO;

namespace JN.Web.Areas.UserCenter.Controllers
{
    public class FinanceController : BaseController
    {
        private static List<Data.SysParam> cacheSysParam = null;

        private readonly IUserService UserService;
        private readonly ITakeCashService TakeCashService;
        private readonly ITransferService TransferService;
        private readonly IRemittanceService RemittanceService;
        private readonly IBonusDetailService BonusDetailService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;
        private readonly IInvestmentService InvestmentService;
        private readonly IWalletLogService WalletLogService;

        public FinanceController(ISysDBTool SysDBTool,
            IUserService UserService,
            ITakeCashService TakeCashService,
            ITransferService TransferService,
            IRemittanceService RemittanceService,
            IBonusDetailService BonusDetailService,
            IActLogService ActLogService,
            IInvestmentService InvestmentService,
            IWalletLogService WalletLogService)
        {
            this.UserService = UserService;
            this.TakeCashService = TakeCashService;
            this.TransferService = TransferService;
            this.RemittanceService = RemittanceService;
            this.BonusDetailService = BonusDetailService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            this.InvestmentService = InvestmentService;
            this.WalletLogService = WalletLogService;
            cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();

        }


        public ActionResult PINCode()
        {
            ActMessage = "激活码";
            return View();
        }

        [HttpPost]
        public ActionResult BuyPINCode(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string buyNum = form["buyNum"];
                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");
                if (buyNum.ToInt() <= 0) throw new CustomException("请填写正确的数量");
                decimal totalmoney = buyNum.ToInt() * cacheSysParam.SingleAndInit(x => x.ID == 1001).Value.ToDecimal();
                if (Umodel.Wallet2002 < totalmoney) throw new CustomException("您的帐户余额不足");
                for (int i = 0; i < buyNum.ToInt(); i++)
                {
                    //生成PIN码
                    string keycode = GetPINNumber(15);
                    var model = new Data.PINCode
                    {
                        CreateTime = DateTime.Now,
                        OriginDescribe = "用户制作",
                        Investment = 0,
                        IsUsed = false,
                        KeyCode = keycode,
                        UID = Umodel.ID,
                        UserName = Umodel.UserName,
                        OriginUID = Umodel.ID,
                        OriginUserName = Umodel.UserName
                    };
                    MvcCore.Unity.Get<JN.Data.Service.IPINCodeService>().Add(model);
                    SysDBTool.Commit();
                }
                Wallets.changeWallet(Umodel.ID, 0 - totalmoney, 2002, "制作PIN码");
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

        #region 申请投资及追加  wp160817
        //申请投资  wp160817
        public ActionResult ApplyInvestment()
        {
            return View("Error");
            if (Umodel.Investment >= MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().List(x => x.PID == 1000 && x.IsUse).OrderByDescending(x => x.ID).First().Value.ToDecimal())
            {
                ViewBag.ErrorMsg = "已达到最高级别，无法继续！";
                return View("Error");
            }

            if (InvestmentService.Single(x => x.UID == Umodel.ID && x.InvestmentType >= 1) == null) //首次投资
            {
                ViewBag.InvestmentType = 1;
                ViewBag.Investment = Umodel.FristApplyInvestment;
            }
            else  //复投
            {
                ViewBag.InvestmentType = 2;
                ViewBag.Investment = 0;
            }
            ViewBag.CheckMoney = Umodel.Investment;  //检查投资额参数

            ActMessage = "投资";
            return View(Umodel);
        }

        [HttpPost]
        public ActionResult ApplyInvestment(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string InvestmentType = form["InvestmentType"];
                string BuyMoney = form["BuyMoney"];

                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");
                if (BuyMoney.ToDecimal() <= 0) throw new CustomException("申请金额不正确！");
                //写入汇款表
                var model = new Data.Investment
                {
                    InvestmentNo = Bonus.GetRechargeNo(),
                    InvestmentType = InvestmentType.ToInt(),
                    UID = Umodel.ID,
                    UserName = Umodel.UserName,
                    ApplyInvestment = BuyMoney.ToDecimal(),
                    HaveMoney = BuyMoney.ToDecimal(),
                    SettlementMoney = 0,
                    IsBalance = true,
                    Period = 0,
                    Status = (int)JN.Data.Enum.InvestmentStatus.Apply,
                    CreateTime = DateTime.Now,
                    Remark = "模拟测试 现金投资",

                };
                InvestmentService.Add(model);
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

        #endregion

        #region 复投钱包复投  wp160817
        public ActionResult RepeatInvestment()
        {

            ViewBag.Title = "申请复投";
            ActMessage = ViewBag.Title;
            ViewBag.MixMoney = cacheSysParam.Single(x => x.ID == 2002).Value2;
            ViewBag.Beisu = cacheSysParam.Single(x => x.ID == 2002).Value3;
            ViewBag.InvestmentType = 3; //其他复投

            return View(Umodel);
        }

        [HttpPost]
        public ActionResult RepeatInvestment(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string InvestmentType = form["InvestmentType"];
                string BuyMoney = form["drawmoney"];
                string Remark = form["Remark"];

                if (InvestmentType.Length == 0) throw new CustomException("类型有误！");
                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");
                if (BuyMoney.ToDecimal() <= 0) throw new CustomException("申请金额不正确！");
                if (BuyMoney.ToDecimal() > Umodel.Wallet2002) throw new CustomException("你的余额不足！");
                if (BuyMoney.ToDecimal() < cacheSysParam.Single(x => x.ID == 2002).Value2.ToDecimal()) throw new CustomException("申请金额不能小于" + cacheSysParam.Single(x => x.ID == 2002).Value2);
                if (BuyMoney.ToDecimal() % cacheSysParam.Single(x => x.ID == 2002).Value3.ToDecimal() != 0) throw new CustomException("申请金额必须为" + cacheSysParam.Single(x => x.ID == 2002).Value3 + "的倍数");
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    string NewNo = Bonus.GetRechargeNo();
                    //写入汇款表
                    var model = new Data.Investment
                    {
                        InvestmentNo = NewNo,
                        InvestmentType = InvestmentType.ToInt(),
                        UID = Umodel.ID,
                        UserName = Umodel.UserName,
                        ApplyInvestment = BuyMoney.ToDecimal(),
                        HaveMoney = BuyMoney.ToDecimal(),
                        SettlementMoney = 0,
                        IsBalance = true,
                        Period = 0,
                        Status = (int)JN.Data.Enum.InvestmentStatus.Transaction,
                        CreateTime = DateTime.Now,
                        Remark = Remark,

                    };
                    InvestmentService.Add(model);
                    SysDBTool.Commit();

                    //扣币
                    Wallets.changeWallet(Umodel.ID, 0 - BuyMoney.ToDecimal(), 2002, "【" + NewNo + "】复投消耗" + BuyMoney);
                    result.Status = 200;
                    ts.Complete();
                }
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


        #region 支付宝/银行卡  充值汇款  wp160817
        public ActionResult ApplyRemittance()
        {
            string Check = Balances.GetRadom();
            Session["CheckForm"] = Check;
            ViewBag.CheckForm = Check;
            if (InvestmentService.Single(x => x.UID == Umodel.ID && x.InvestmentType >= 1) == null) //首次投资
            {
                ViewBag.InvestmentType = 1;
                ViewBag.Investment = Umodel.FristApplyInvestment;
            }
            else  //复投
            {
                if (Umodel.Investment >= MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().List(x => x.PID == 1000 && x.IsUse).OrderByDescending(x => x.ID).First().Value.ToDecimal())
                {
                    ViewBag.ErrorMsg = "已达到最高级别，无法继续！";
                    return View("Error");
                }
                ViewBag.InvestmentType = 2;
                ViewBag.Investment = 0;
            }
            ViewBag.CheckMoney = Umodel.Investment;  //检查投资额参数

            ActMessage = "充值汇款";
            return View(Umodel);
        }

        [HttpPost]
        public ActionResult ApplyRemittance(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string CheckForm = Session["CheckForm"].ToString();
                string FormValue = Request["CheckValue"];
                if (FormValue.Length == 0 || CheckForm.Length == 0 || CheckForm != FormValue)
                {
                    ViewBag.ErrorMsg = "已有操作在处理，检测到重复提交，已屏蔽！";
                    return View("Error");
                }
                Session["CheckForm"] = "";


                if (MvcCore.Unity.Get<JN.Data.Service.IRemittanceService>().List(x => x.UID == Umodel.ID && x.Status == 1).Count() > 0)
                {
                    ViewBag.ErrorMsg = "已有申请充值在处理，无法继续！";
                    return View("Error");
                }

                int startHour = cacheSysParam.Single(x => x.ID == 3011).Value.Split('~')[0].ToInt();
                int endHour = cacheSysParam.Single(x => x.ID == 3011).Value.Split('~')[1].ToInt();
                int nowHour = DateTime.Now.Hour;
                if (nowHour < startHour)
                {
                    throw new CustomException("今日入金时间为" + startHour + ":00-" + endHour + ":00，当前未到充值时间，请稍后再来！！");
                }
                else if (nowHour >= endHour)
                {
                    throw new CustomException("今日入金时间为" + startHour + ":00-" + endHour + ":00，当前已过充值时间，请明天继续充值！！");
                }

                string rechargeway = form["rechargeway"];  //支付宝账户
                //string rechargedate = form["rechargedate"];
                string RemittanceType = form["RemittanceType"];
                string remark = form["remark"];
                string InvestmentType = form["InvestmentType"];
                string BuyMoney = form["BuyMoney"];

                if (RemittanceType == "") throw new CustomException("请选择支付方式！");
                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");
                if (BuyMoney.ToDecimal() <= 0) throw new CustomException("申请金额不正确！");

                //if (rechargeamount.ToDecimal() <= 0) throw new CustomException("请输入正确的金额");
                //if (!Services.Tool.StringHelp.IsDateString(rechargedate)) throw new CustomException("日期格式不正确");
                if (remark.Trim().Length > 100) throw new CustomException("备注长度不能超过100个字节");

                HttpPostedFileBase file = Request.Files["PayImg"];
                string imgurl = "";
                if (string.IsNullOrEmpty(file.FileName)) throw new CustomException("请上传支付图片凭证！");
                if (!string.IsNullOrEmpty(file.FileName))
                {
                    if (!FileValidation.IsAllowedExtension(file, new FileExtension[] { FileExtension.PNG, FileExtension.JPG, FileExtension.BMP }))
                    {
                        ViewBag.ErrorMsg = "非法上传，您只可以上传图片格式的文件！";
                        return View("Error");
                    }
                    //20160711安全更新 ---------------- start
                    var newfilename = "MAIL_User_" + Umodel.RealName + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName).ToLower();
                    if (!Directory.Exists(Request.MapPath("~/upfile")))
                        Directory.CreateDirectory(Request.MapPath("~/upfile"));

                    if (Path.GetExtension(file.FileName).ToLower().Contains("aspx"))
                    {
                        var wlog = new Data.WarningLog();
                        wlog.CreateTime = DateTime.Now;
                        wlog.IP = Request.UserHostAddress;
                        if (Request.UrlReferrer != null)
                            wlog.Location = Request.UrlReferrer.ToString();
                        wlog.Platform = "会员";
                        wlog.WarningMsg = "试图上传木马文件";
                        wlog.WarningLevel = "严重";
                        wlog.ResultMsg = "拒绝";
                        wlog.UserName = Umodel.RealName;
                        MvcCore.Unity.Get<IWarningLogService>().Add(wlog);

                        Umodel.IsLock = true;
                        MvcCore.Unity.Get<IUserService>().Update(Umodel);
                        MvcCore.Unity.Get<ISysDBTool>().Commit();
                        throw new Exception("试图上传木马文件，您的帐号已被冻结");
                    }

                    var fileName = Path.Combine(Request.MapPath("~/upfile"), newfilename);
                    try
                    {
                        file.SaveAs(fileName);
                        var thumbnailfilename = UploadPic.MakeThumbnail(fileName, Request.MapPath("~/upfile/"), 1024, 768, "EQU");
                        imgurl = "/upfile/" + thumbnailfilename;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("上传失败：" + ex.Message);
                    }
                    finally
                    {
                        System.IO.File.Delete(fileName); //删除原文件
                    }
                    //20160711安全更新  --------------- end
                }

                //获得单号
                string InvestmentNo = Bonus.GetRechargeNo();
                //写入汇款表
                var modelInvestment = new Data.Investment
                {
                    InvestmentNo = InvestmentNo,
                    InvestmentType = InvestmentType.ToInt(),
                    UID = Umodel.ID,
                    UserName = Umodel.UserName,
                    ApplyInvestment = BuyMoney.ToDecimal(),
                    HaveMoney = BuyMoney.ToDecimal(),
                    SettlementMoney = 0,
                    IsBalance = true,
                    Period = 0,
                    Status = (int)JN.Data.Enum.InvestmentStatus.Doing,
                    CreateTime = DateTime.Now,
                    Remark = remark,

                };
                InvestmentService.Add(modelInvestment);


                //写入汇款表
                var modelRemittance = new Data.Remittance
                {
                    CreateTime = DateTime.Now,
                    ChongNumber = "A_" + InvestmentNo,
                    PayOrderNumber = rechargeway,
                    PayImg = imgurl,
                    Platform = 0,
                    RechargeAmount = BuyMoney.ToDecimal(),
                    RechargeWay = RemittanceType,
                    Remark = remark,
                    UID = Umodel.ID,
                    UserName = Umodel.UserName,
                    Status = 1,
                    RechargeDate = DateTime.Now
                };
                RemittanceService.Add(modelRemittance);
                SysDBTool.Commit();
                result.Status = 200;
            }
            catch (CustomException ex)
            {
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                //result.Message = "网络系统繁忙，请稍候再试!";
                logs.WriteErrorLog(HttpContext.Request.Url.ToString(), ex);
                ViewBag.ErrorMsg = "网络系统繁忙，请稍候再试!";
                return View("Error");

            }
            if (result.Status == 200)
            {
                ViewBag.SuccessMsg = "申请成功,请耐心等待审核!";
                return View("Success");
            }
            else
            {
                ViewBag.ErrorMsg = result.Message;
                return View("Error");
            }
            //return Json(result);
        }
        #endregion

        #region 提交第三方付款
        [HttpPost]
        public ActionResult PostInvestment(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            return Json(result);
            try
            {
                string InvestmentNo = form["InvestmentNo"];
                string PayName = form["PayName"];
                string PayNumber = form["PayNumber"];
                string PayBank = form["PayBank"];

                var PostModel = InvestmentService.Single(x => x.InvestmentNo == InvestmentNo.Trim());
                if (PostModel != null)
                {
                    if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");
                    if (PayNumber.Length == 0 || PayName.Length == 0 || PayBank.Length == 0) throw new CustomException("付款信息不能为空！");
                    #region 提交到第三方

                    //提交到第三方
                    //PostModel.Status = (int)JN.Data.Enum.InvestmentStatus.Doing;  //提交第三方付款确认

                    PostModel.Status = (int)JN.Data.Enum.InvestmentStatus.Transaction;  //模拟测试已经提交完成
                    #endregion

                    PostModel.PayName = PayName;
                    PostModel.PayNumber = PayNumber;
                    PostModel.PayBank = PayBank;

                    InvestmentService.Update(PostModel);  //更新

                    #region 模拟测试 已经完成
                    //Bonus.RefreeBonus(PostModel.InvestmentNo, PostModel.ApplyInvestment);

                    #endregion

                    //更新用户投资额
                    var onUser = UserService.Single(x => x.ID == PostModel.UID);
                    onUser.Investment += PostModel.ApplyInvestment;
                    UserService.Update(onUser);

                    SysDBTool.Commit();
                    result.Status = 200;

                }
                else
                {
                    throw new CustomException("交易代码输入有误！");
                }
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

        //提交第三方结算   wp160817
        public ActionResult Pay_Post(int ID)
        {

            ReturnResult result = new ReturnResult();
            try
            {
                var PostInvertment = InvestmentService.Single(ID);
                if (PostInvertment != null)
                {
                    //凭编码充值。

                    result.Status = 200;
                }
                else
                {
                    throw new CustomException("该编码记录已失效！");
                }

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

        //返回结果  wp160817
        public ActionResult Pay_Result()
        {
            bool CheckResult = true;
            if (CheckResult)
            {

            }

            return View();
        }

        #endregion

        #region 申请投资列表

        //待提交付款
        public ActionResult ApplyInvertmentList(int? page)
        {
            ActMessage = "我的充值列表";
            var list = InvestmentService.List(x => x.UID == Umodel.ID && x.Status == (int)JN.Data.Enum.InvestmentStatus.Apply || x.Status == (int)JN.Data.Enum.InvestmentStatus.Doing).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }

        //已完成，可发红利
        public ActionResult InvertmentList(int? page)
        {
            ActMessage = "我的充值列表";
            var list = InvestmentService.List(x => x.UID == Umodel.ID && x.Status >= (int)JN.Data.Enum.InvestmentStatus.Transaction).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }
        #endregion


        public ActionResult TakeMoneyMsg(int id, int type)
        {
            var model = InvestmentService.Single(id);

            ViewBag.id = id;
            ViewBag.type = type;
            ViewBag.Msg = "无违约金。";
            ViewBag.DoStatus = "TRUE";

            int StartTime = 0, EndTime = 0;
            decimal deduction = 0;

            if (model.InvestmentType == 1 || model.InvestmentType == 2)  //初次投资和现金复投
            {
                if (type == 1)
                {
                    StartTime = cacheSysParam.Single(x => x.ID == 3002).Value.Split('|')[0].ToInt();
                    EndTime = cacheSysParam.Single(x => x.ID == 3002).Value.Split('|')[1].ToInt();

                    if (DateTime.Now > model.CreateTime.AddMinutes(StartTime) && DateTime.Now < model.CreateTime.AddMinutes(EndTime))
                    {
                        deduction = cacheSysParam.Single(x => x.ID == 3002).Value2.ToDecimal();
                        ViewBag.Msg = "您投资未满一个月，如果现在提取，您将成为违约方，" + (deduction * 100) + "%违约金！";

                    }
                    if (DateTime.Now < model.CreateTime.AddMinutes(StartTime))
                    {
                        ViewBag.DoStatus = "FALSE";
                        ViewBag.Msg = "您投资未满十天，暂且不能提现！";
                    }
                }
            }
            else if (model.InvestmentType == 3)  //其他复投
            {
                StartTime = cacheSysParam.Single(x => x.ID == 3003).Value.Split('|')[0].ToInt();
                EndTime = cacheSysParam.Single(x => x.ID == 3003).Value.Split('|')[1].ToInt();

                if (DateTime.Now > model.CreateTime.AddMinutes(StartTime) && DateTime.Now < model.CreateTime.AddMinutes(EndTime))
                {
                    deduction = cacheSysParam.Single(x => x.ID == 3003).Value2.ToDecimal();
                    ViewBag.Msg = "您好，您的复投红利未满3个月，如现在提取您将成为违约方，扣除" + (deduction * 100) + "%违约金！";

                }
                if (DateTime.Now < model.CreateTime.AddMinutes(StartTime))
                {
                    ViewBag.DoStatus = "FALSE";
                    ViewBag.Msg = "您好，您的复投红利未满一个月，暂且不能提现！";
                }
            }

            return View(model);
        }

        #region 提取本金或利息 wp160817
        [HttpPost]
        public ActionResult TakeMoney(FormCollection form)
        {

            int id = form["id"].ToInt();
            decimal Money = form["Money"].ToDecimal();
            int type = form["type"].ToInt();

            decimal TakeMoney = Money.ToDecimal();
            var model = InvestmentService.Single(id);

            if (type == 1)
            {
                var TakeParam = cacheSysParam.Single(x => x.ID == 1906);
                if (TakeParam.Value != "1")
                {
                    throw new CustomException(TakeParam.Value2);
                }

                if (Umodel.ReserveBoolean1 ?? false)
                {
                    throw new CustomException(TakeParam.Value2);
                }
            }

            if (model != null)
            {
                string TakeName = "";
                string CheckDeduction = "";
                decimal deduction = 0;
                int StartTime = 0, EndTime = 0;
                if (model.InvestmentType == 1 || model.InvestmentType == 2)  //现金复投
                {
                    if (type == 1)
                    {
                        StartTime = cacheSysParam.Single(x => x.ID == 3002).Value.Split('|')[0].ToInt();
                        EndTime = cacheSysParam.Single(x => x.ID == 3002).Value.Split('|')[1].ToInt();

                        if (DateTime.Now > model.CreateTime.AddMinutes(StartTime) && DateTime.Now < model.CreateTime.AddMinutes(EndTime))
                        {
                            deduction = cacheSysParam.Single(x => x.ID == 3002).Value2.ToDecimal();
                        }
                        if (DateTime.Now < model.CreateTime.AddMinutes(StartTime))
                        {
                            ViewBag.ErrorMsg = "您投资未满十天，暂且不能提现！";
                            return View("Error");
                        }
                    }
                }
                else if (model.InvestmentType == 3)  //其他复投
                {
                    StartTime = cacheSysParam.Single(x => x.ID == 3003).Value.Split('|')[0].ToInt();
                    EndTime = cacheSysParam.Single(x => x.ID == 3003).Value.Split('|')[1].ToInt();

                    if (DateTime.Now > model.CreateTime.AddMinutes(StartTime) && DateTime.Now < model.CreateTime.AddMinutes(EndTime))
                    {
                        deduction = cacheSysParam.Single(x => x.ID == 3003).Value2.ToDecimal();
                    }
                    if (DateTime.Now < model.CreateTime.AddMinutes(StartTime))
                    {
                        ViewBag.ErrorMsg = "您好，您的复投红利未满一个月，暂且不能提现！";
                        return View("Error");
                    }

                }

                if (type == 1)
                {
                    if (model.HaveMoney < TakeMoney)
                    {
                        ViewBag.ErrorMsg = "可提现额不足，无法继续！";
                        return View("Error");
                    }
                    model.HaveMoney -= TakeMoney;  //计算剩余
                    TakeName = "本金";

                }
                else if (type == 2)
                {
                    if (model.SettlementMoney < TakeMoney)
                    {
                        ViewBag.ErrorMsg = "利息不足，无法继续！";
                        return View("Error");
                    }
                    model.SettlementMoney -= TakeMoney;  //计算剩余

                    TakeName = "利息";
                }

                if (model.HaveMoney == 0 && model.SettlementMoney == 0)
                {
                    model.Status = (int)JN.Data.Enum.InvestmentStatus.Deal;
                }
                if (deduction > 0)
                {
                    TakeMoney = TakeMoney * (1 - deduction);
                    CheckDeduction = ",违约金" + (deduction * 100) + "%";
                }
                InvestmentService.Update(model);
                Wallets.changeWallet(model.UID, TakeMoney, 2001, "提取【" + model.InvestmentNo + "】" + TakeName + CheckDeduction);
                SysDBTool.Commit();
                ActPacket = model;
                //ActMessage = "成功！";
                ViewBag.SuccessMsg = "提取成功！";
                return View("Success");
            }
            else
            {
                ViewBag.ErrorMsg = "不存该记录。";
                return View("Error");
            }
        }
        #endregion

        //生成真实订单号
        public string GetPINNumber(int num)
        {
            DateTime dateTime = DateTime.Now;
            string result = JN.Services.Tool.StringHelp.GettRandomCode(num);//4位随机数字
            if (IsHave(result))
            {
                return GetPINNumber(num);
            }
            return result;
        }

        //检查订单号是否重复
        public bool IsHave(string number)
        {
            return MvcCore.Unity.Get<JN.Data.Service.IPINCodeService>().List(x => x.KeyCode == number).Count() > 0;
        }

        [HttpPost]
        public ActionResult TransferPinCode(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string transferUser = form["transferUser"];
                string transferNum = form["transferNum"];
                var shUser = UserService.Single(x => x.UserName == transferUser.Trim());
                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");
                if (shUser == null) throw new CustomException("用户不存在");
                if (transferNum.ToInt() <= 0) throw new CustomException("请填写正确的转让数量");

                var codes = MvcCore.Unity.Get<JN.Data.Service.IPINCodeService>().List(x => x.UID == Umodel.ID && x.IsUsed == false).OrderBy(x => x.ID).ToList();
                if (codes.Count < transferNum.ToInt()) throw new CustomException("数量不足");
                for (int i = 0; i < transferNum.ToInt(); i++)
                {
                    var model = codes[i];
                    model.UID = shUser.ID;
                    model.UserName = shUser.UserName;
                    MvcCore.Unity.Get<JN.Data.Service.IPINCodeService>().Update(model);
                    SysDBTool.Commit();
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

        #region 收益明细列表
        public ActionResult BonusTotal(int? page)
        {
            ActMessage = "我的收益";
            var list = BonusDetailService.List(x => x.UID == Umodel.ID).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }
        #endregion

        #region 提现历史列表
        public ActionResult TakeCash(int? page)
        {
            ActMessage = "我的提现列表";
            var list = TakeCashService.List(x => x.UID == Umodel.ID).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }
        #endregion

        #region 转帐明细列表
        public ActionResult Transfer(int? page)
        {
            ActMessage = "转帐记录";
            var list = TransferService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }
        #endregion

        #region 充值历史列表
        public ActionResult Remittance(int? page)
        {
            ActMessage = "我的支付宝充值列表";
            var list = RemittanceService.List(x => x.UID == Umodel.ID).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }
        #endregion



        #region 帐户明细列表
        public ActionResult AccountDetail(int? coin, int? page)
        {
            ActMessage = "个人钱包";
            int coinID = coin ?? 2001;
            ViewBag.ParamList = cacheSysParam.Where(x => x.PID == 2000 && x.IsUse == true).OrderBy(x => x.Sort).ToList();
            var list = WalletLogService.List(x => x.CoinID == coinID && x.UID == Umodel.ID).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }
        #endregion

        #region 奖金明细列表
        public ActionResult BonusDetail(int? bonusid, int? page)
        {
            ActMessage = "奖金明细表";
            int BonusID = bonusid ?? 1102;
            ViewBag.ParamList = cacheSysParam.Where(x => x.PID == 1100 && x.IsUse == true).OrderBy(x => x.Sort).ToList();
            var list = BonusDetailService.List(x => x.BonusID == BonusID && x.UID == Umodel.ID).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }
        #endregion

        #region 申请及取消提现
        public ActionResult ApplyTakeCash()
        {
            string Check = Balances.GetRadom();
            Session["CheckForm"] = Check;
            ViewBag.CheckForm = Check;

            var takeCoin = cacheSysParam.Single(x => x.ID == Request["takeCoin"].ToInt());
            if (takeCoin != null)
            {
                if (takeCoin.ID != 2001 && takeCoin.ID != 2003 && takeCoin.ID != 2006)
                {
                    ViewBag.ErrorMsg = "不可提现！";
                    return View("Error");
                }
                if (!(Umodel.WalletLock ?? false))
                {
                    ViewBag.Title = takeCoin.Name + "申请提现";
                    ActMessage = ViewBag.Title;
                    ViewBag.Poundage = (takeCoin.ID == 2001 ? cacheSysParam.Single(x => x.ID == 1903).Value2 : takeCoin.ID == 2003 ? cacheSysParam.Single(x => x.ID == 1905).Value2 : cacheSysParam.Single(x => x.ID == 1904).Value2);
                    //ViewBag.Beisu = cacheSysParam.Single(x => x.ID == 1902).Value;
                    ViewBag.takeCoin = takeCoin.ID;
                    return View(Umodel);
                }
                else
                {
                    ViewBag.ErrorMsg = "您的钱包提现功能已被关闭，详情请联系管理员。";
                    return View("Error");
                }
            }
            else
            {
                ViewBag.ErrorMsg = "错误的参数。";
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult ApplyTakeCash(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string CheckForm = Session["CheckForm"].ToString();
                string FormValue = Request["CheckValue"];
                if (FormValue.Length == 0 || CheckForm.Length == 0 || CheckForm != FormValue)
                {
                    throw new CustomException("已有操作在处理，检测到重复提交，已屏蔽！"); 
                }
                Session["CheckForm"] = "";


                //int startHour = cacheSysParam.Single(x => x.ID == 3011).Value2.Split('~')[0].ToInt();
                //int endHour = cacheSysParam.Single(x => x.ID == 3011).Value2.Split('~')[1].ToInt();
                //int nowHour = DateTime.Now.Hour;
                //if (nowHour < startHour)
                //{
                //    throw new CustomException("今日提现时间为" + startHour + ":00-" + endHour + ":00，当前未到提现时间，请稍后再来！！");
                //}
                //else if (nowHour >= endHour)
                //{
                //    throw new CustomException("今日提现时间为" + startHour + ":00-" + endHour + ":00，当前已过提现时间！！");
                //}

                string drawmoney = form["drawmoney"];
                string remark = form["remark"];

                //if (MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().Single(x => x.ID == 4507).Value == "1")  //开启申请提现验证
                //{
                //    string smscode = form["smscode"];
                //    if (string.IsNullOrEmpty(smscode)) throw new CustomException("手机验证码不能为空");
                //    if (Session["SMSValidateCode"] == null || smscode.Trim() != Session["SMSValidateCode"].ToString()) throw new CustomException("验证码不正确或已过期");
                //    if (Session["GetSMSUser"] == null || Umodel.Mobile != Session["GetSMSUser"].ToString()) throw new CustomException("手机号码与接收验证码的设备不相符");
                //}


                if (drawmoney.ToDecimal() <= 0) throw new CustomException("请输入正确的提现金额");
                if (remark.Trim().Length > 100) throw new CustomException("备注长度不能超过100个字节");
                //if (string.IsNullOrEmpty(Umodel.BankCard)) throw new CustomException("您尚未绑定银行卡信息，请到“会员管理>个人信息”编辑银行卡信息");

                var takeCoin = cacheSysParam.Single(x => x.ID == Request["takeCoin"].ToInt());
                if (takeCoin == null) throw new CustomException("错误的参数");
                if (takeCoin.ID.ToInt() == 2001)
                {
                    if (Umodel.Wallet2001 < drawmoney.ToDecimal()) throw new CustomException("您的帐户金币余额不足");
                }
                else if (takeCoin.ID.ToInt() == 2003) { if (Umodel.Wallet2003 < drawmoney.ToDecimal()) throw new CustomException("您的帐户金币余额不足"); }
                else if (takeCoin.ID.ToInt() == 2006) { if (Umodel.Wallet2006 < drawmoney.ToDecimal()) throw new CustomException("您的帐户金币余额不足"); }
                else
                {
                    throw new CustomException("错误的参数");
                }
                #region 事务操作
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    Wallets.doTakeCash(Umodel, drawmoney.ToDecimal(), takeCoin, remark);
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

        /// <summary>
        /// 取消提现
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CancelTakeCash(int id)
        {
            var model = TakeCashService.Single(id);
            if (model != null && model.UID == Umodel.ID)
            {
                if (model.Status == (int)JN.Data.Enum.TakeCaseStatus.Wait)
                {
                    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                    {
                        Wallets.changeWallet(model.UID, model.DrawMoney, model.UserName.ToInt(), "取消提现");
                        model.Status = (int)JN.Data.Enum.TakeCaseStatus.Cancel;
                        model.PayTime = DateTime.Now;
                        TakeCashService.Update(model);
                        SysDBTool.Commit();
                        ts.Complete();
                        ActMessage = "成功取消提现申请！";
                        return View("Success");
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "您的提现申请无法取消。";
                    return View("Error");
                }
            }
            ViewBag.ErrorMsg = "记录不存在或已被删除！";
            return View("Error");
        }
        #endregion

        #region 会员转帐
        public ActionResult ApplyTransfer()
        {
            string Check = Balances.GetRadom();
            Session["CheckForm"] = Check;
            ViewBag.CheckForm = Check;
            if (!(Umodel.WalletLock ?? false))
            {
                ViewBag.Title = "会员转帐";
                ActMessage = ViewBag.Title;
                ViewBag.Poundage = cacheSysParam.Single(x => x.ID == 1901).Value2;
                ViewBag.Beisu = cacheSysParam.Single(x => x.ID == 1901).Value;
                ViewBag.transCoin = 2001;
                return View(Umodel);
            }
            else
            {
                ViewBag.ErrorMsg = "您的钱包转帐功能已被关闭，详情请联系管理员。";
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult ApplyTransfer(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {

                string CheckForm = Session["CheckForm"].ToString();
                string FormValue = Request["CheckValue"];
                if (FormValue.Length == 0 || CheckForm.Length == 0 || CheckForm != FormValue)
                {
                   throw new CustomException("已有操作在处理，检测到重复提交，已屏蔽！");
                }
                Session["CheckForm"] = "";

                string recusername = form["recuser"];
                string transfermoney = form["transfermoney"];
                string remark = form["remark"];
                string password2 = form["password2"];
                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");
                if (Umodel.Password2 != password2.ToMD5().ToMD5()) throw new CustomException("二级密码不正确");
                if (transfermoney.ToDecimal() <= 0) throw new CustomException("请输入正确的金额");
                var recUser = UserService.Single(x => x.UserName == recusername.Trim());
                if (recUser == null) throw new CustomException("转入用户不存在");
                if (Umodel.UserName == recusername.Trim()) throw new CustomException("不可以给自己转帐");
                if (remark.Trim().Length > 100) throw new CustomException("备注长度不能超过100个字节");
                if (Umodel.Wallet2001 < transfermoney.ToDecimal()) throw new CustomException("帐户余额不足");
                if (!(Umodel.RefereePath + ",").Contains("," + recUser.ID.ToString() + ",") && !(recUser.RefereePath + ",").Contains("," + Umodel.ID.ToString() + ",")) throw new CustomException("请在推荐关系一条线内进行转账");

                #region 事务操作
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    Wallets.doTransfer(Umodel, recUser, transfermoney.ToDecimal(), 2001, remark);
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
        #endregion


        #region 兑换
        public ActionResult ApplyExchange()
        {
            int fromid = Request["fromCoin"].ToInt();
            int toid = Request["toCoin"].ToInt();
            if (fromid != 2001 || toid != 2003)
            {
                ViewBag.ErrorMsg = "错误的参数。";
                return View("Error");
            }

            var fromCoin = cacheSysParam.Single(x => x.ID == fromid);
            var toCoin = cacheSysParam.Single(x => x.ID == toid);

            if (fromCoin != null && toCoin != null)
            {
                ViewBag.Title = fromCoin.Name + "兑换" + toCoin.Name;
                ActMessage = ViewBag.Title;
                ViewBag.Poundage = cacheSysParam.Single(x => x.ID == 1903).Value2;
                ViewBag.Beisu = cacheSysParam.Single(x => x.ID == 1903).Value;

                ViewBag.fromCoin = fromCoin.ID;
                ViewBag.toCoin = toCoin.ID;
                return View(Umodel);
            }
            else
            {
                ViewBag.ErrorMsg = "错误的参数。";
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult ApplyExchange(FormCollection form)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string drawmoney = form["drawmoney"];
                string formcoin = form["fromCoin"];
                string tocoin = form["toCoin"];
                if (drawmoney.ToDecimal() <= 0) throw new CustomException("请输入正确的兑换数量");

                if (formcoin.ToInt() != 2001 && tocoin.ToInt() != 2003) throw new CustomException("非法操作" + formcoin + "/" + tocoin);

                //if (MvcCore.Unity.Get<ICFBPreOrderService>().List(x => x.UID == Umodel.ID && (x.Status == 0 || x.Status == 1) && x.Origin == "JP").Count() > 0)
                //    throw new CustomException("您有排队中的订单，无法兑出");

                //if (MvcCore.Unity.Get<ICFBSubscribeService>().List(x => x.UID == Umodel.ID && x.CanBeUsed == 0 && x.Origin == "JP").Count() > 0)
                //    throw new CustomException("请在解冻股权后再兑出");

                var fromCoin = cacheSysParam.Single(x => x.ID == formcoin.ToInt());
                var toCoin = cacheSysParam.Single(x => x.ID == tocoin.ToInt());
                if (fromCoin == null || toCoin == null) throw new CustomException("错误的参数");
                decimal fromWallet = 0;
                if (fromCoin.ID == 2001) fromWallet = Umodel.Wallet2001;
                if (fromWallet < drawmoney.ToDecimal()) throw new CustomException("您的帐户余额不足");


                #region 事务操作
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    Wallets.doExchange(Umodel.ID, drawmoney.ToDecimal(), fromCoin.ID, toCoin.ID);
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
        #endregion

        #region 从子帐号转出主帐号
        /// <summary>
        /// 从子帐号转出主帐号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public ActionResult RollOut(int id, string number)
        {
            //TUser model = users.GetModel(id);
            //if (model != null)
            //{
            //    if (model.ReserveInt1 == Umodel.ID)
            //    {
            //        if (model.Wallet2002 >= (decimal)TypeConverter.StrToFloat(number))
            //        {
            //            #region 事务操作

            //            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            //            {
            //                lock (obj)
            //                {
            //                    users.doTransfer(model, Umodel, TypeConverter.StrToFloat(number), 2002, "从子帐号“" + model.UserName + "”提出");
            //                    ts.Complete();
            //                    return Content("ok");
            //                }
            //            }
            //            #endregion
            //        }
            //        else
            //            return Content("帐户余额不足！");
            //    }
            //    else
            //        return Content("非法操作！");
            //}
            return Content("非法操作！");
        }
        #endregion

        #region 从关联帐号转出
        /// <summary>
        /// 从子帐号转出主帐号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public ActionResult RelationOut(int id, string number)
        {
            //TAccountRelation model = accountrelations.GetModel(id);
            //if (model != null)
            //{
            //    if (model.UID == Umodel.ID)
            //    {
            //        TUser onUser = users.GetModel(model.RelationUID);
            //        TUser recUser = users.GetModel(model.UID);
            //        if (onUser.Wallet2002 >= (decimal)TypeConverter.StrToFloat(number))
            //        {
            //            #region 事务操作

            //            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            //            {
            //                lock (obj)
            //                {
            //                    users.doTransfer(onUser, recUser, TypeConverter.StrToFloat(number), 2002, "从关联帐号“" + onUser.UserName + "”提出");
            //                    ts.Complete();
            //                    return Content("ok");
            //                }
            //            }
            //            #endregion
            //        }
            //        else
            //            return Content("帐户余额不足！");
            //    }
            //    else
            //        return Content("非法操作！");
            //}
            return Content("非法操作！");
        }
        #endregion
    }
}
