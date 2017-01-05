using JN.Data;
using JN.Data.Service;
using MvcCore.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using JN.Services.Filters;
using JN.Services.Manager;
using JN.Services.Tool;
using JN.Services.CustomException;

namespace JN.Web.Areas.AdminCenter.Controllers
{
    public class FinanceController : BaseController
    {
        private static List<Data.SysParam> cacheSysParam = null;

        private readonly IUserService UserService;
        private readonly ITakeCashService TakeCashService;





        private readonly ITransferService TransferService;
        private readonly IRemittanceService RemittanceService;
        private readonly IInvestmentService InvestmentService;
        private readonly IBonusDetailService BonusDetailService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;
        private readonly IWalletLogService WalletLogService;

        public FinanceController(ISysDBTool SysDBTool,
            IUserService UserService,
            ITakeCashService TakeCashService,
            ITransferService TransferService,
            IRemittanceService RemittanceService,
             IInvestmentService InvestmentService,
            IBonusDetailService BonusDetailService,
            IActLogService ActLogService,
            IWalletLogService WalletLogService)
        {
            this.UserService = UserService;
            this.TakeCashService = TakeCashService;
            this.TransferService = TransferService;
            this.RemittanceService = RemittanceService;
            this.InvestmentService = InvestmentService;
            this.BonusDetailService = BonusDetailService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            this.WalletLogService = WalletLogService;
            cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();

        }

        public ActionResult Account(int? page)
        {
            //动态构建查询
            List<Data.User> list = UserService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Delivery()
        {
            //if (Session["CheckPwd"] == null)
            //{
            //    Session["Url"] = Request.Url.PathAndQuery;
            //    return Redirect("/AdminCenter/Home/CheckPassWord2");
            //}
            ActMessage = "赠送电子币";
            ViewData["SysParamList"] = new SelectList(cacheSysParam.Where(x => x.PID == 2000 && x.IsUse).OrderBy(x => x.ID).ToList(), "ID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Delivery(FormCollection form)
        {
            string username = form["username"];
            string coinid = form["coinid"];
            string deliverynumber = form["deliverynumber"];
            string remark = form["remark"];
            var result = new ReturnResult();
            try
            {
                var onUser = UserService.Single(x => x.UserName.Equals(username.Trim()));
                if (onUser == null) throw new CustomException("用户不存在");
                if (remark.Trim().Length > 100) throw new CustomException("备注长度不能超过100个字节");

                Wallets.changeWallet(onUser.ID, deliverynumber.ToDecimal(), coinid.ToInt(), "ZH：" + remark + "(" + Amodel.AdminName + ")");
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

        public ActionResult DeliveryDetail(int? coin, int? page)
        {
            for (int j = 1; j <= 1; j++)
            {
                int CheckID = 0;
                switch (j)
                {
                    case 1:
                        CheckID = 1102;
                        break;
                    case 2:
                        CheckID = 1106;
                        break;
                    //case 3:
                    //    CheckID = 1108;
                    //    break;
                }
                var CheckError = MvcCore.Unity.Get<JN.Data.Service.IInvestmentService>().List();
                foreach (var item in CheckError.ToList())
                {
                    for (int i = 5; i < 6; i++)  //前6期
                    {
                        //int Num = BonusDetailService.List(x => x.Description.Contains(item.InvestmentNo) && x.BonusID == CheckID && x.Period == i).Count();
                        //if (Num > 1)  //该期获得超过1次以上记录
                        //{
                        //var DeleteList = BonusDetailService.List(x => x.Description.Contains(item.InvestmentNo) && x.BonusID == CheckID && x.Period == i).Take(Num - 1).ToList();
                        var DeleteList = BonusDetailService.List(x => x.Description.Contains(item.InvestmentNo) && x.BonusID == CheckID && x.Period == i).ToList();
                        if (DeleteList.Count > 0)
                        {
                            foreach (var itemDelete in DeleteList)
                            {
                                //余额
                                var DoUser = UserService.Single(itemDelete.UID);
                                if (DoUser != null)
                                {
                                    DoUser.Wallet2001 = DoUser.Wallet2001 - itemDelete.BonusMoney;
                                    UserService.Update(DoUser);
                                }

                                //日志
                                var DoLog = WalletLogService.List(x => x.Description.Contains(item.InvestmentNo) && x.ChangeMoney == itemDelete.BonusMoney).OrderByDescending(x => x.CreateTime).FirstOrDefault();
                                if (DoLog != null)
                                {
                                    WalletLogService.Delete(DoLog.ID);  //删除多余记录
                                }


                                BonusDetailService.Delete(itemDelete.ID);  //删除多余记录
                                SysDBTool.Commit();
                            }
                        }
                        //}
                    }
                }
            }
            var set = MvcCore.Unity.Get<JN.Data.Service.ISettlementService>().Single(x => x.Period == 5);
            if (set != null)
            {
                MvcCore.Unity.Get<JN.Data.Service.ISettlementService>().Delete(set.ID);
                SysDBTool.Commit();
            }

            ActMessage = "赠送电子币明细";
            int coinID = coin ?? 2002;
            ViewBag.ParamList = cacheSysParam.Where(x => x.PID == 2000 && x.IsUse == true).OrderBy(x => x.Sort).ToList();
            var list = WalletLogService.List(x => x.CoinID == coinID && x.Description.Contains("ZH：")).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult TakeCash(int? status, int? page)
        {
            ActMessage = "提现明细";
            int Status = status ?? 0;
            var list = TakeCashService.List(x => x.Status == Status).OrderByDescending(x => x.ID).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Transfer(int? page)
        {
            ActMessage = "转帐明细";
            var list = TransferService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Statistics(int? page)
        {
            var parameters = new[]{
                new System.Data.SqlClient.SqlParameter(){ ParameterName="1", Value=1 }
            };
            var list = SysDBTool.Execute<Data.Extensions.View_Statistics>("SELECT [jstime],[newuser],[takecash],[income],[outlay],[profit] FROM [View_Statistics]", parameters);
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult TakeCashCommand(int id)
        {
            string commandtype = Request["commandtype"];
            var model = TakeCashService.Single(id);
            if (model != null)
            {
                if (commandtype.ToLower() == "dopass")
                {
                    model.Status = (int)JN.Data.Enum.TakeCaseStatus.Passed;
                    TakeCashService.Update(model);
                    SysDBTool.Commit();
                }
                else if (commandtype.ToLower() == "dopay")
                {
                    model.Status = (int)JN.Data.Enum.TakeCaseStatus.Payed;
                    model.PayTime = DateTime.Now;
                    TakeCashService.Update(model);
                    SysDBTool.Commit();

                    var param = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().Single(x => x.ID == 4503);
                    if (param.Value == "1")  //开启确认提醒
                    {
                        var onUser = UserService.Single(model.UID);
                        if (onUser != null)
                        {
                            SMSHelper.VoiceVerify(onUser.Mobile, "", param.Value2);
                        }

                    }

                }
                else if (commandtype.ToLower() == "docancel")
                {
                    if (model.Status < (int)JN.Data.Enum.TakeCaseStatus.Payed)
                    {
                        Wallets.changeWallet(model.UID, model.DrawMoney, model.UserName.ToInt(), "拒绝提现申请");
                        model.Status = (int)JN.Data.Enum.TakeCaseStatus.Refusal;
                        model.PayTime = DateTime.Now;  //支付时间和拒绝时间公用同个字段   wp160817
                        TakeCashService.Update(model);
                        SysDBTool.Commit();
                        ViewBag.SuccessMsg = "成功取消提现申请！";
                        return View("Success");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "提现已支付，不能取消。";
                        return View("Error");
                    }
                }

                return RedirectToAction("TakeCash", "Finance");
            }
            else
            {
                ViewBag.ErrorMsg = "无效的ID";
                return View("Error");
            }
        }

        public ActionResult ApplyInvertment(int? status, int? page)
        {
            int Status = status ?? 0;
            var list = InvestmentService.List(x => x.Status == Status).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }


        public ActionResult Remittance(int? status, int? page)
        {
            int Status = status ?? 0;
            var list = RemittanceService.List(x => x.Status == Status).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult RemittanceCommand(int doid, string commandtype)
        {
            Data.Remittance model = RemittanceService.Single(doid);
            if (model != null)
            {
                string InvestmentNo = model.ChongNumber.Split('_')[1];
                var PostModel = InvestmentService.Single(x => x.InvestmentNo == InvestmentNo);
                //更新用户投资额
                var onUser = UserService.Single(x => x.ID == PostModel.UID);
                if (PostModel != null)
                {
                    if (commandtype.ToLower() == "dopass")
                    {
                        using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                        {
                            #region 完成申请，可分红
                            PostModel.Status = (int)JN.Data.Enum.InvestmentStatus.Transaction;  //已经提交完成
                            #endregion

                            PostModel.PayName = "";
                            PostModel.PayNumber = model.RechargeWay;
                            PostModel.PayBank = "在线充值充值完成";

                            #region 计算奖金
                            //Bonus.RefreeBonus(PostModel.InvestmentNo, PostModel.ApplyInvestment);

                            #endregion

                            //if (PostModel.InvestmentType > 1)//如果是复投状态累加
                            onUser.Investment += PostModel.ApplyInvestment;

                            UserService.Update(onUser);
                            SysDBTool.Commit();


                            model.Status = (int)JN.Data.Enum.RechargeSatus.Sucess;
                            InvestmentService.Update(PostModel);  //更新
                            RemittanceService.Update(model);
                            SysDBTool.Commit();
                            ts.Complete();

                        }
                        //if (MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().Single(x => x.ID == 4507).Value == "1")  //开启提现验证
                        //{
                        //    bool b = SMSHelper.WebChineseMSM(onUser.Mobile, "银河星城的投资商您好，您的提现银河星城已经受理，具体到账时间，以银行到账时间为准！ ");
                        //}
                    }
                    else if (commandtype.ToLower() == "docancel")
                    {

                        PostModel.InvestmentType = 1;
                        PostModel.Status = (int)JN.Data.Enum.InvestmentStatus.Cancel;  //取消
                        model.Status = (int)JN.Data.Enum.RechargeSatus.Fail;
                        InvestmentService.Update(PostModel);  //更新
                        RemittanceService.Update(model);
                        SysDBTool.Commit();

                    }

                }


                SysDBTool.Commit();
                return RedirectToAction("Remittance", "Finance");
            }
            else
            {
                ViewBag.ErrorMsg = "无效的ID";
                return View("Error");
            }
        }

        //public ActionResult RemittanceCommand(int id)
        //{
        //    Data.Remittance model = RemittanceService.Single(id);
        //    if (model != null)
        //    {
        //        if (commandtype.ToLower() == "dopass")
        //        {
        //            Wallets.changeWallet(model.UID, model.RechargeAmount, 2002, "会员充值");
        //            model.Status = (int)JN.Data.Enum.RechargeSatus.Sucess;
        //        }
        //        else if (commandtype.ToLower() == "docancel")
        //            model.Status = (int)JN.Data.Enum.RechargeSatus.Un;
        //        RemittanceService.Update(model);
        //        SysDBTool.Commit();
        //        return RedirectToAction("TakeCash", "Finance");
        //    }
        //    else
        //    {
        //        ViewBag.ErrorMsg = "无效的ID";
        //        return View("Error");
        //    }
        //}

        public ActionResult AccountDetail(int? coin, int? page)
        {
            ActMessage = "财务流水";
            int coinID = coin ?? 2002;
            ViewBag.ParamList = cacheSysParam.Where(x => x.PID == 2000 && x.IsUse == true).OrderBy(x => x.Sort).ToList();
            var list = WalletLogService.List(x => x.CoinID == coinID).OrderByDescending(x => x.ID).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult BonusDetail(int? bonusid, int? page)
        {
            ActMessage = "奖金明细";
            ViewBag.ParamList = cacheSysParam.Where(x => x.PID == 1100 && x.IsUse == true).OrderBy(x => x.Sort).ToList();
            var list = BonusDetailService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (bonusid.HasValue)
                list = list.Where(x => x.BonusID == (bonusid ?? 0)).ToList();

            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }
    }
}