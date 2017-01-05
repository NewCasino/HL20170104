using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JN.Data.Service;
using MvcCore.Controls;
using PagedList;
using JN.Data.Extensions;
using JN.Services.Tool;
using JN.Services.Manager;
using System.IO;
using System.Text.RegularExpressions;
using JN.Services.CustomException;
using System.Data.Entity.Validation;

namespace JN.Web.Areas.UserCenter.Controllers
{
    public class UserController : BaseController
    {
        private static List<Data.SysParam> cacheSysParam = null;

        private readonly IUserService UserService;
        private readonly IAccountRelationService AccountRelationService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;
        private readonly IInvestmentService InvestmentService;
        private readonly IInvestmentService InvestmentService2;

        public UserController(ISysDBTool SysDBTool, IUserService UserService, IAccountRelationService AccountRelationService, IInvestmentService InvestmentService, IActLogService ActLogService)
        {
            this.UserService = UserService;
            this.AccountRelationService = AccountRelationService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            this.InvestmentService = InvestmentService;
            cacheSysParam = MvcCore.Unity.Get<ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();

        }

        #region 已激活会员列表
        public ActionResult Activation(int? page)
        {
            ActMessage = "已激活会员列表";
            var list = UserService.List(x => x.IsActivation && (x.CreateBy == Umodel.UserName || x.RefereeID == Umodel.ID)).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            return View("Index", list.ToPagedList(page ?? 1, 20));
        }
        #endregion

        #region 未激活会员列表
        /// <summary>
        /// 未激活会员
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public ActionResult UnActivation(int? page)
        {
            ActMessage = "待激活会员列表";

            var list = UserService.List(x => x.IsActivation == false && (x.CreateBy == Umodel.UserName || x.RefereeID == Umodel.ID)).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            return View("index", list.ToPagedList(page ?? 1, 20));
        }
        #endregion

        #region 删除用户（未激活状态下）
        /// <summary>
        /// 删除用户，未激活时才可以
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            //只允许删除您推荐的用户
            var onUser = UserService.Single(id);
            if (onUser != null)
            {
                if (Umodel.UserName == onUser.CreateBy || Umodel.AgentID == onUser.AgentID)
                {
                    if (!onUser.IsActivation)
                    {
                        //删除时上级子节点减少
                        if (onUser.ParentID > 0)
                        {
                            Data.User _parentUser = UserService.Single(onUser.ParentID);
                            _parentUser.Child = _parentUser.Child - 1;
                            UserService.Update(_parentUser);
                        }
                        UserService.Delete(id);
                        SysDBTool.Commit();
                        ViewBag.SuccessMsg = "会员已被删除！";
                        ActMessage = "删除会员";
                        return View("Success");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "该会员已被激活，无法删除。";
                        return View("Error");
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "只允许删除您创建的用户。";
                    return View("Error");
                }
            }
            ViewBag.ErrorMsg = "系统异常！请查看系统日志。";
            return View("Error");
        }
        #endregion

        #region WP160817  激活
        //public ActionResult doPass(int ID)
        //{
        //    var onUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(x => x.ID == ID);
        //    if (onUser != null)
        //    {
        //        onUser.IsActivation = true;
        //        onUser.ActivationTime = DateTime.Now;
        //        MvcCore.Unity.Get<JN.Data.Service.IUserService>().Update(onUser);
        //        MvcCore.Unity.Get<Data.Service.ISysDBTool>().Commit();
        //        ViewBag.SuccessMsg = "激活成功！";

        //        return View("Success");
        //    }
        //    else
        //    {

        //        ViewBag.ErrorMsg = "会员不存在！";
        //        return View("Error");
        //    }

        //}

        #endregion

        public ActionResult Repeat()
        {
            ReturnResult result = new ReturnResult();
            try
            {
                decimal Money = cacheSysParam.Single(x => x.ID == 1001).Value.ToDecimal();  //需要金额
                if (Umodel.Wallet2001 < Money) throw new CustomException("您的帐户余额不足，无法复投！");


                int InvestmentNum = InvestmentService.List(x => x.UID == Umodel.ID).Count();
                if (InvestmentNum > 0)
                {
                    //decimal ADDTotal = MvcCore.Unity.Get<JN.Data.Service.IBonusDetailService>().List(x => x.UID == Umodel.ID && (x.BonusID == 1106 || x.BonusID == 1108 || x.BonusID == 1103 || x.BonusID == 1102)).Count() > 0 ? MvcCore.Unity.Get<JN.Data.Service.IBonusDetailService>().List(x => x.UID == Umodel.ID && (x.BonusID == 1106 || x.BonusID == 1108 || x.BonusID == 1103 || x.BonusID == 1102)).Sum(x => x.BonusMoney) : 0;
                    //int Num = Math.Floor((ADDTotal) / Money).ToInt();
                    int Num = Math.Floor(((Umodel.Addup1102 ?? 0) + (Umodel.Addup1103 ?? 0) + (Umodel.Addup1106 ?? 0) + (Umodel.Addup1802 ?? 0)) / Money).ToInt();
                    if (Num < InvestmentNum)
                    {
                        throw new CustomException("总收益未达成复投条件，无法复投！");
                    }
                }

                if (InvestmentNum >= 100) throw new CustomException("投资总数等于或超过100次，无法复投！");

                #region 事务操作
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    string No = Bonus.GetRechargeNo();
                    //复投所需要的金币
                    var onUser = UserService.Single(Umodel.ID);
                    onUser.Investment += Money;
                    UserService.Update(onUser);

                    Wallets.changeWallet(Umodel.ID, 0 - Money, 2001, "复投投资" + No);

                    //写入汇款表
                    var model = new Data.Investment
                    {
                        InvestmentNo = No,
                        InvestmentType = 2,
                        UID = Umodel.ID,
                        UserName = Umodel.UserName,
                        ApplyInvestment = Money,
                        HaveMoney = Money,
                        SettlementMoney = 0,
                        IsBalance = true,
                        Period = 0,
                        Status = (int)JN.Data.Enum.InvestmentStatus.Transaction,
                        CreateTime = DateTime.Now,
                        Remark = "复投投资",

                    };
                    InvestmentService.Add(model);

                    //推荐奖
                    Bonus.RefreeBonus(model.UID, model.InvestmentNo, model.ApplyInvestment, model.ApplyInvestment);
                    //报单奖
                   // Bonus.AgentBonus(model.UID, model.InvestmentNo, model.ApplyInvestment);

                    //Bonus.Balance(Umodel.ID, model.InvestmentNo, model.HaveMoney); //第一次结算奖金


                    SysDBTool.Commit();
                    ts.Complete();
                    result.Status = 200;
                }
                #endregion
            }
            catch (CustomException ex)
            {
                result.Message = ex.Message;
            }
            return Json(result);

        }

        #region 激活用户 HL16102101
        public ActionResult doPass()
        {
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 激活用户，激活用户产生直推奖，见点奖
        /// </summary>
        /// <returns></returns>
        public ActionResult doPass(FormCollection fc)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                //if (Umodel.IsActivation) throw new CustomException("您的帐户已经激活，请勿重复操作！");
                decimal Money = cacheSysParam.Single(x => x.ID == 1001).Value.ToDecimal();  //需要金额
                if (Umodel.Wallet2001 < Money) throw new CustomException("您的帐户余额不足，无法激活您的帐号！");


                if (Umodel.Investment <= 0)
                {
                    //激活所需要的金币
                    #region 事务操作
                    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                    {
                        //激活会员
                        Wallets.changeWallet(Umodel.ID, 0 - Money, 2001, "投资成为会员");
                        //Users.ActivationAccount(Umodel);

                        var onUser = UserService.Single(Umodel.ID);
                        onUser.Investment += Money;
                        UserService.Update(onUser);

                        //写入汇款表
                        var model = new Data.Investment
                        {
                            InvestmentNo = Bonus.GetRechargeNo(),
                            InvestmentType = 1,
                            UID = Umodel.ID,
                            UserName = Umodel.UserName,
                            ApplyInvestment = Money,
                            HaveMoney = Money,
                            SettlementMoney = 0,
                            IsBalance = true,
                            Period = 0,
                            Status = (int)JN.Data.Enum.InvestmentStatus.Transaction,
                            CreateTime = DateTime.Now,
                            Remark = "",

                        };
                        InvestmentService.Add(model);
                        SysDBTool.Commit();
                        //直推奖
                        Bonus.RefreeBonus(model.UID, model.InvestmentNo, model.ApplyInvestment, model.ApplyInvestment);
                        //报单奖
                        Bonus.AgentBonus(model.UID, model.InvestmentNo, model.ApplyInvestment);
                        Bonus.Balance(Umodel.ID, model.InvestmentNo, model.HaveMoney); //第一次结算奖金

                        SysDBTool.Commit();
                        ts.Complete();
                        result.Status = 200;
                    }
                    #endregion
                }
                else
                {
                    throw new CustomException("首次投资已成功，请勿重复操作！");
                }
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

        public ActionResult MyLink()
        {
            return View();
        }

        #region 激活子帐号(未用)
        //public ActionResult doSubpass(int id)
        //{
        //    //更新激活标记
        //    var onUser = UserService.Single(id);
        //    if (onUser != null)
        //    {
        //        if (Umodel.ID == onUser.MainAccountID || Umodel.ID == onUser.ReserveInt1)
        //        {
        //            if (!onUser.IsActivation)
        //            {
        //                //需再推一个人才可以激活子帐户
        //                //int PARAM_TJRS = 1; // TypeConverter.StrToInt(sysparams.GetModel(2004).Value2);
        //                //if ((users.GetRecordCount("RefereeID=" + Umodel.ID + " and IsSubAccount=0") - onUser.ReserveInt1) >= PARAM_TJRS)

        //                onUser.IsActivation = true;
        //                onUser.ActivationTime = DateTime.Now;
        //                UserService.Update(onUser);
        //                SysDBTool.Commit();
        //            }
        //            else
        //            {
        //                ViewBag.ErrorMsg = "该子帐号已被激活，请不要重复激活。";
        //                return View("Error");
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.ErrorMsg = "只充许激活您自己的子帐号。";
        //            return View("Error");
        //        }
        //    }
        //    ViewBag.ErrorMsg = "系统异常！请查看系统日志。";
        //    return View("Error");
        //}
        #endregion

        #region 子帐号列表
        /// <summary>
        /// 子帐号
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns></returns>
        //public ActionResult SubAccount(int? page)
        //{
        //    int pageIndex = page ?? 1;
        //    int rowcount = 0;
        //    List<TUser> lst = users.GetModelList(pageIndex, 15, "IsSubAccount=1 and isnull(ReserveStr2,'')<>'追加投资' and (ReserveInt1=" + Umodel.ID + " or MainAccountID=" + Umodel.ID + ")" + SqlWhere(df, dr, kf, kv), ref rowcount);
        //    PagedList<TUser> pagedlist = new PagedList<TUser>(lst, pageIndex, 15, rowcount);
        //    ViewBag.Title = "会员子帐号管理";
        //    if (SqlWhere(df, dr, kf, kv) != " order by Id desc")
        //    {
        //        ViewBag.Title += "查询";
        //        ActPacket = new JsonModel("", "df=" + df + ",dr=" + "dr" + ",kf=" + kf + ",kv=" + kv + ",page=" + page);
        //    }
        //    ActMessage = ViewBag.Title;
        //    return View("Sub", pagedlist);
        //}
        #endregion

        #region 追投帐号列表(未用)
        /// <summary>
        /// 子帐号
        /// </summary>
        /// <param name="df">日期字段</param>
        /// <param name="dr">日期范围</param>
        /// <param name="kf">查询字段</param>
        /// <param name="kv">查询关键字</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        //public ActionResult Investment(string df, string dr, string kf, string kv, int? page)
        //{
        //    int pageIndex = page ?? 1;
        //    int rowcount = 0;
        //    List<TUser> lst = users.GetModelList(pageIndex, 15, "IsSubAccount=1 and ReserveStr2='追加投资' and (ReserveInt1=" + Umodel.ID + " or MainAccountID=" + Umodel.ID + ")" + SqlWhere(df, dr, kf, kv), ref rowcount);
        //    PagedList<TUser> pagedlist = new PagedList<TUser>(lst, pageIndex, 15, rowcount);
        //    ViewBag.Title = "追投帐号列表";
        //    if (SqlWhere(df, dr, kf, kv) != " order by Id desc")
        //    {
        //        ViewBag.Title += "查询";
        //        ActPacket = new JsonModel("", "df=" + df + ",dr=" + "dr" + ",kf=" + kf + ",kv=" + kv + ",page=" + page);
        //    }
        //    ActMessage = ViewBag.Title;
        //    return View(pagedlist);
        //}
        #endregion

        #region 关联帐号列表
        /// <summary>
        /// 关联帐号列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Relation()
        {
            var lst = AccountRelationService.List(x => x.UID == Umodel.ID);
            ViewBag.Title = "关联帐号管理";
            ActMessage = ViewBag.Title;
            return View(lst);
        }

        /// <summary>
        /// 取消被关联
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelRelation()
        {
            var ar = AccountRelationService.Single(x => x.RelationUID == Umodel.ID);
            if (ar != null)
            {
                AccountRelationService.Delete(ar.ID);
                ViewBag.SuccessMsg = "关联成功取消！";
                ActMessage = ViewBag.SuccessMsg;
                return View("Success");
            }
            ViewBag.ErrorMsg = "系统异常！请查看系统日志。";
            return View("Error");
        }

        [HttpPost]
        public ActionResult Relation(FormCollection form)
        {
            //string relationusername = form["relationusername"];
            //string relationpwd1 = form["relationpwd1"];
            //string relationpwd2 = form["relationpwd2"];
            //string relationpwd3 = form["relationpwd3"];
            //string relationcode = form["relationcode"];

            ////校验验证码
            //if (Session["UserValidateCode"] == null || relationcode != Session["UserValidateCode"].ToString())
            //{
            //    ViewBag.ErrorMsg = "验证码错误。";
            //    return View("Error");
            //}



            //TUser rUser = users.GetModel("username='" + relationusername + "' and Password='" + Tool.Md5(relationpwd1) + "' and Password2='" + Tool.Md5(relationpwd2) + "' and Password3='" + Tool.Md5(relationpwd3) + "'");
            //if (rUser != null)
            //{
            //    if (rUser.IsLock || (rUser.WalletLock ?? false))
            //    {
            //        ViewBag.ErrorMsg = "关联帐号已被冻结或关闭钱包权限。";
            //        return View("Error");
            //    }

            //    if (accountrelations.Exists("RelationUserName='" + relationusername + "'"))
            //    {
            //        ViewBag.ErrorMsg = "此帐号已被其它帐号关联！";
            //        return View("Error");
            //    }

            //    TAccountRelation model = new TAccountRelation();

            //    model.CreateTime = DateTime.Now;
            //    model.RelationTime = DateTime.Now;
            //    model.RelationUID = users.GetModel("username='" + relationusername + "'").ID;
            //    model.RelationUserName = relationusername;
            //    model.Status = 2;
            //    model.UID = Umodel.ID;
            //    model.UserName = Umodel.UserName;

            //    if (accountrelations.Add(model) > 0)
            //    {
            //        ViewBag.SuccessMsg = "帐号关联成功！";
            //        ActMessage = ViewBag.SuccessMsg;
            //        ActPacket = model;
            //        return View("Success");
            //    }
            //}
            //else
            //{
            //    ViewBag.ErrorMsg = "关联帐号不存在或密码错误！";
            //    return View("Error");
            //}
            ViewBag.ErrorMsg = "系统异常！请查看系统日志。";
            return View("Error");
        }
        #endregion

        #region 添加一个会员
        public ActionResult Add()
        {
            var sys = MvcCore.Unity.Get<JN.Data.Service.ISysSettingService>().Single(1);
            if (!sys.IsRegisterOpen)
            {
                ViewBag.ErrorMsg = sys.CloseRegisterHint;
                return View("Error");
            }
            ViewBag.NewUserName = "C" + Users.GetUserCode(7);

            ViewBag.Title = "添加一个新会员";
            ActMessage = ViewBag.Title;

            ViewBag.CurrentUser = Umodel.UserName;
            if (Umodel.IsAgent ?? false)
                ViewBag.AgentName = Umodel.AgentName;
            else
                ViewBag.AgentName = Umodel.AgentUser;

            int _parentid = Request["ParentID"].ToInt();

            if (UserService.List(x => x.ID == _parentid && x.IsActivation).Count() > 0)
                ViewBag.ParentUser = UserService.Single(_parentid).UserName;

            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection fc)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                var sysEntity = MvcCore.Unity.Get<JN.Data.Service.ISysSettingService>().Single(1);
                var entity = new Data.User();

                //20160711安全更新 ---------------- start
                if (!string.IsNullOrEmpty(fc["wallet2001"]) || !string.IsNullOrEmpty(fc["wallet2002"]) || !string.IsNullOrEmpty(fc["wallet2003"]) || !string.IsNullOrEmpty(fc["wallet2004"]) || !string.IsNullOrEmpty(fc["wallet2005"]))
                {
                    var wlog = new Data.WarningLog();
                    wlog.CreateTime = DateTime.Now;
                    wlog.IP = Request.UserHostAddress;
                    if (Request.UrlReferrer != null)
                        wlog.Location = Request.UrlReferrer.ToString();
                    wlog.Platform = "会员";
                    wlog.WarningMsg = "试图在添加会员时篡改数据(试图篡改钱包等敏感数据)";
                    wlog.WarningLevel = "严重";
                    wlog.ResultMsg = "拒绝";
                    wlog.UserName = Umodel.UserName;
                    MvcCore.Unity.Get<IWarningLogService>().Add(wlog);

                    Umodel.IsLock = true;
                    Umodel.LockTime = DateTime.Now;
                    Umodel.LockReason = "试图在添加会员时篡改数据(详情查看日志)";
                    MvcCore.Unity.Get<IUserService>().Update(Umodel);
                    MvcCore.Unity.Get<ISysDBTool>().Commit();
                    throw new CustomException("非法数据请求，您的帐号已被冻结");
                }
                //20160711安全更新  --------------- end


                TryUpdateModel(entity, fc.AllKeys);
                if (!Regex.IsMatch(entity.UserName, @"^[A-Za-z0-9_]+$")) throw new CustomException("用户名只能为字母、数字和下划线");
                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");
                if (fc["password"] != fc["passwordconfirm"]) throw new CustomException("一级密码与确认密码不相符");
                if (fc["password2"] != fc["password2confirm"]) throw new CustomException("二级密码与确认密码不相符");

                if (sysEntity.RegistNeedSMSVerification ?? false)  //开启注册手机短信验证
                {
                    string smscode = fc["smscode"];
                    if (string.IsNullOrEmpty(smscode)) throw new CustomException("手机验证码不能为空");
                    if (Session["SMSValidateCode"] == null || smscode.Trim() != Session["SMSValidateCode"].ToString()) throw new CustomException("验证码不正确或已过期");
                    if (Session["GetSMSUser"] == null || entity.Mobile != Session["GetSMSUser"].ToString()) throw new CustomException("手机号码与接收验证码的设备不相符");
                }
                if (string.IsNullOrEmpty(entity.UserName)) throw new CustomException("会员编号不能为空");
                if (entity.FristApplyInvestment <= 0) throw new CustomException("请选择注册类型");
                if (string.IsNullOrEmpty(entity.Password) || string.IsNullOrEmpty(entity.Password2)) throw new CustomException("一级密码、二级密码不能为空");
                if (UserService.List(x => x.UserName == entity.UserName.Trim()).Count() > 0) throw new CustomException("用户名已被使用");
                //if (entity.RealName != entity.BankUser) throw new CustomException("真实姓名与银行卡户名不相符");


                if (string.IsNullOrEmpty(entity.NickName) && sysEntity.RegistNotNullItems.Contains(",NickName,")) throw new CustomException("昵称不能为空");
                if (string.IsNullOrEmpty(entity.RealName) && sysEntity.RegistNotNullItems.Contains(",RealName,")) throw new CustomException("真实姓名不能为空");
                if (string.IsNullOrEmpty(entity.Mobile) && sysEntity.RegistNotNullItems.Contains(",Mobile,")) throw new CustomException("手机号码不能为空");
                if (string.IsNullOrEmpty(entity.Email) && sysEntity.RegistNotNullItems.Contains(",Email,")) throw new CustomException("邮箱不能为空");
                if (string.IsNullOrEmpty(entity.IDCard) && sysEntity.RegistNotNullItems.Contains(",IDCard,")) throw new CustomException("身份证号码不能为空");
                if (string.IsNullOrEmpty(entity.AliPay) && sysEntity.RegistNotNullItems.Contains(",AliPay,")) throw new CustomException("昵称不能为空");
                if (string.IsNullOrEmpty(entity.BankName) && sysEntity.RegistNotNullItems.Contains(",BankName,")) throw new CustomException("银行名称不能为空");
                if (string.IsNullOrEmpty(entity.BankCard) && sysEntity.RegistNotNullItems.Contains(",BankCard,")) throw new CustomException("银行卡卡号不能为空");
                if (string.IsNullOrEmpty(entity.BankUser) && sysEntity.RegistNotNullItems.Contains(",BankUser,")) throw new CustomException("银行卡户名不能为空");
                if (string.IsNullOrEmpty(entity.BankOfDeposit) && sysEntity.RegistNotNullItems.Contains(",BankOfDeposit,")) throw new CustomException("银行卡开户行不能为空");
                if (string.IsNullOrEmpty(entity.Question) && sysEntity.RegistNotNullItems.Contains(",Question,")) throw new CustomException("取回密码问题不能为空");
                if (string.IsNullOrEmpty(entity.Answer) && sysEntity.RegistNotNullItems.Contains(",Answer,")) throw new CustomException("取回密码答案不能为空");
                if (sysEntity.RegistNotNullItems.Contains(",Mobile,") && !StringHelp.IsPhone(entity.Mobile)) throw new CustomException("请填写正确的手机号码");


                if (UserService.List(x => x.NickName == entity.NickName.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",NickName,")) throw new CustomException("昵称已被使用");
                if (UserService.List(x => x.Mobile == entity.Mobile.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",Mobile,")) throw new CustomException("手机号码已被使用");
                if (UserService.List(x => x.Email == entity.Email.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",Email,")) throw new CustomException("邮箱已被使用");
                if (UserService.List(x => x.IDCard == entity.IDCard.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",IDCard,")) throw new CustomException("身份证号码已被使用");
                if (UserService.List(x => x.AliPay == entity.AliPay.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",AliPay,")) throw new CustomException("支付宝帐号已被使用");
                if (UserService.List(x => x.WeiXin == entity.WeiXin.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",WeiXin,")) throw new CustomException("微信帐号已被使用");
                if (UserService.List(x => x.BankCard == entity.BankCard.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",BankCard,")) throw new CustomException("银行卡号已被使用");


                if (UserService.List(x => x.UserName == entity.RefereeUser).Count() <= 0) throw new CustomException("推荐人不存在或未激活");
                else
                {
                    if (UserService.Single(x => x.UserName == entity.RefereeUser).Investment <= 0) throw new CustomException("推荐人未投资");
                }
                if (ConfigHelper.GetConfigString("MemberAtlas") != "sun")
                {
                    if (UserService.List(x => x.UserName == entity.ParentUser).Count() <= 0) throw new CustomException("安置人不存在或未激活");
                }
                if (ConfigHelper.GetConfigBool("HaveAgent"))
                {
                    if (UserService.List(x => x.AgentName == entity.AgentUser && x.IsAgent == true).Count() <= 0 || entity.AgentUser == null) throw new CustomException("报单中心不存在");
                    var agentUser = UserService.Single(x => x.AgentName == entity.AgentUser);
                    entity.AgentID = agentUser.ID;
                    entity.AgentUser = agentUser.UserName;
                }

                Data.User _parentUser = null;
                if (ConfigHelper.GetConfigString("MemberAtlas") == "double")
                {
                    _parentUser = UserService.Single(x => x.UserName == entity.ParentUser);
                    if (_parentUser != null)
                    {
                        if (UserService.List(x => x.ParentID == _parentUser.ID).Count() >= 2) throw new CustomException("安置人安置名额已满");
                    }
                    if (entity.ChildPlace > 2 || entity.ChildPlace < 1) throw new CustomException("安置参数不正确");
                    if (UserService.List(x => x.ParentUser == entity.ParentUser && x.ChildPlace == entity.ChildPlace).Count() > 0) throw new CustomException("当前位置无法安置:" + entity.ParentUser + "/" + entity.ChildPlace);
                }
                else if (ConfigHelper.GetConfigString("MemberAtlas") == "three")
                {
                    _parentUser = UserService.Single(x => x.UserName == entity.ParentUser);
                    if (_parentUser != null)
                    {
                        if (UserService.List(x => x.ParentID == _parentUser.ID).Count() >= 3) throw new CustomException("安置人安置名额已满");
                    }

                    if (entity.ChildPlace > 3 || entity.ChildPlace < 1) throw new CustomException("安置参数不正确");
                    if (UserService.List(x => x.ParentUser == entity.ParentUser && x.ChildPlace == entity.ChildPlace).Count() > 0) throw new CustomException("当前位置无法安置:" + entity.ParentUser + "/" + entity.ChildPlace);
                }
                else
                _parentUser = UserService.Single(x => x.UserName == entity.RefereeUser);

                entity.IsActivation = false;
               // entity.IsActivation = true;
                entity.ActivationTime = DateTime.Now;


                entity.IsAgent = false;
                entity.IsLock = false;
                entity.IsShareBouns = true;
                //entity.Investment = cacheSysParam.SingleAndInit(x => x.ID == 1001).Value.ToDecimal();

                //节点部分
                var _refereeUser = UserService.Single(x => x.UserName == entity.RefereeUser);

                //if (_parentUser.RootID != _refereeUser.RootID || Umodel.RootID != _parentUser.RootID) throw new CustomException("推荐人和安置人以及您自己必须同一网内会员");
                entity.RefereeDepth = _refereeUser.RefereeDepth + 1;
                entity.RefereePath = _refereeUser.RefereePath + "," + _refereeUser.ID;
                entity.RefereeID = _refereeUser.ID;
                entity.RefereeUser = _refereeUser.UserName;
                entity.MainAccountID = 0;

                //节点部分
                entity.ParentID = _parentUser.ID;
                entity.ParentUser = _parentUser.UserName;
                entity.RootID = _parentUser.RootID;
                entity.Depth = _parentUser.Depth + 1;
                entity.ParentPath = _parentUser.ParentPath + "," + _parentUser.ID;
                entity.Child = 0;
                entity.Password = entity.Password.ToMD5().ToMD5();
                entity.Password2 = entity.Password2.ToMD5().ToMD5();
                entity.CreateTime = DateTime.Now;
                entity.CreateBy = Umodel.UserName;

                entity.Investment = 0;

                entity.ReserveDecamal1 = entity.Investment;

                if (ConfigHelper.GetConfigString("MemberAtlas") == "double")
                    entity.DepthSort = (_parentUser.DepthSort - 1) * 2 + entity.ChildPlace;
                else if (ConfigHelper.GetConfigString("MemberAtlas") == "three")
                    entity.DepthSort = (_parentUser.DepthSort - 1) * 3 + entity.ChildPlace;
                else
                {
                    entity.DepthSort = 0;
                    entity.ChildPlace = UserService.List(x => x.ParentID == _parentUser.ID).Count() > 0 ? UserService.List(x => x.ParentID == _parentUser.ID).Max(x => x.ChildPlace) + 1 : 1;
                }

                entity.Wallet2001 = 0;
                entity.Wallet2002 = 0;
                entity.Wallet2003 = 0;
                entity.Wallet2004 = 0;
                entity.Wallet2005 = 0;
                entity.Wallet2006 = 0;

                UserService.Add(entity);
                _parentUser.Child = _parentUser.Child + 1;
                UserService.Update(_parentUser);
                _refereeUser.RefereeCount = _refereeUser.RefereeCount + 1;
                UserService.Update(_refereeUser);
                SysDBTool.Commit();
                result.Status = 200;
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
        public ActionResult Account()
        {
            ActMessage = "币主帐号";
            return View(Umodel);
        }

        public ActionResult UserInfo()
        {
            ViewBag.Title = "账户信息";
            ActMessage = ViewBag.Title;
            return View(Umodel);
        }

        #region 修改个人资料
        public ActionResult Modify()
        {
            //if (Umodel.CreateTime.AddMinutes(cacheSysParam.Single(x => x.ID == 3010).Value.ToInt()) < DateTime.Now)
            //{
            //    ViewBag.ChangeStatus = 1;
            //}
            ViewBag.ChangeStatus = 0;
            ViewBag.Title = "修改个人资料";
            ActMessage = ViewBag.Title;
            return View(Umodel);
        }

        [HttpPost]
        public ActionResult Modify(FormCollection fc)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                var entity = UserService.SingleAndInit(Umodel.ID);
                var onUser = entity.ToModel<Data.User>();
                string password2 = fc["vierypassword"];
                if (Umodel.Password2 != password2.ToMD5().ToMD5()) throw new CustomException("二级密码不正确");
                if (Umodel.IsLock) throw new CustomException("您的帐号受限，无法进行相关操作");

                //20160711安全更新 ---------------- start
                if (!string.IsNullOrEmpty(fc["wallet2001"]) || !string.IsNullOrEmpty(fc["wallet2002"]) || !string.IsNullOrEmpty(fc["wallet2003"]) || !string.IsNullOrEmpty(fc["wallet2004"]) || !string.IsNullOrEmpty(fc["wallet2005"]))
                {
                    var wlog = new Data.WarningLog();
                    wlog.CreateTime = DateTime.Now;
                    wlog.IP = Request.UserHostAddress;
                    if (Request.UrlReferrer != null)
                        wlog.Location = Request.UrlReferrer.ToString();
                    wlog.Platform = "会员";
                    wlog.WarningMsg = "试图在修改资料时篡改数据(试图篡改钱包等敏感数据)";
                    wlog.WarningLevel = "严重";
                    wlog.ResultMsg = "拒绝";
                    wlog.UserName = Umodel.UserName;
                    MvcCore.Unity.Get<IWarningLogService>().Add(wlog);

                    Umodel.IsLock = true;
                    Umodel.LockTime = DateTime.Now;
                    Umodel.LockReason = "试图在修改资料时篡改数据(详情查看日志)";
                    MvcCore.Unity.Get<IUserService>().Update(Umodel);
                    MvcCore.Unity.Get<ISysDBTool>().Commit();
                    throw new CustomException("非法数据请求，您的帐号已被冻结");
                }
                //20160711安全更新  --------------- end

                //string vcode = (Session["SMSValidateCode"] ?? "").ToString();
                //Session.Remove("SMSValidateCode");
                //string code = fc["smscode"];
                //if (string.IsNullOrEmpty(vcode) || string.IsNullOrEmpty(code) || !vcode.Equals(code, StringComparison.InvariantCultureIgnoreCase))
                //    throw new CustomException("验证码错误");


                //if (Umodel.Mobile != (Session["GetSMSUser"] ?? "").ToString())
                //{
                //    var wlog = new Data.WarningLog();
                //    wlog.CreateTime = DateTime.Now;
                //    wlog.IP = Request.UserHostAddress;
                //    if (Request.UrlReferrer != null)
                //        wlog.Location = Request.UrlReferrer.ToString();
                //    wlog.Platform = "会员";
                //    wlog.WarningMsg = "试图使用非法手机号取得验证码修改会员资料“" + Umodel.UserName + "”（手机号：" + Umodel.Mobile + "）资料，使用手机号：" + (Session["GetSMSUser"] ?? "").ToString();
                //    wlog.WarningLevel = "中等";
                //    wlog.ResultMsg = "拒绝";
                //    wlog.UserName = entity.UserName;
                //    MvcCore.Unity.Get<IWarningLogService>().Add(wlog);
                //    throw new CustomException("非法操作，您的行为已被记录！");
                //}
                //if (Umodel.CreateTime.AddMinutes(cacheSysParam.Single(x => x.ID == 3010).Value.ToInt()) < DateTime.Now)
                //{
                //    throw new CustomException("超出修改时间限制，无法修改！");
                //}

                TryUpdateModel(entity, fc.AllKeys);
                entity.LastUpdateTime = DateTime.Now;
                UserService.Update(entity);
                SysDBTool.Commit();

                string msg = "";
                if (onUser.Mobile != entity.Mobile) msg += "手机变更：" + onUser.Mobile + " => " + entity.Mobile;
                if (onUser.RealName != entity.RealName) msg += "　姓名变更：" + onUser.RealName + " => " + entity.RealName;
                if (onUser.AliPay != entity.AliPay) msg += "　支付宝变更：" + onUser.AliPay + " => " + entity.AliPay;
                if (onUser.BankCard != entity.BankCard) msg += "　银行卡变更：" + onUser.BankCard + " => " + entity.BankCard;


                var wlog2 = new Data.WarningLog();
                wlog2.CreateTime = DateTime.Now;
                wlog2.IP = Request.UserHostAddress;
                if (Request.UrlReferrer != null)
                    wlog2.Location = Request.UrlReferrer.ToString();
                wlog2.Platform = "会员";
                wlog2.WarningMsg = "会员自主修改资料，验证成功" + (!string.IsNullOrEmpty(msg) ? ",涉及改动信息：" + msg : "");
                wlog2.WarningLevel = "一般";
                wlog2.ResultMsg = "通过";
                wlog2.UserName = entity.UserName;
                MvcCore.Unity.Get<IWarningLogService>().Add(wlog2);
                SysDBTool.Commit();

                result.Status = 200;
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

        #region 个人信息
        public ActionResult MyInfo()
        {
            ViewBag.Title = "个人信息";
            ActMessage = ViewBag.Title;
            return View(Umodel);
        }
        #endregion

        #region 验证会员信息(返回JSON)
        /// <summary>
        /// 验证会员信息(返回JSON)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CheckUserInfo(string username)
        {
            var model = UserService.Single(x => x.UserName == username);
            if (model != null)
            {
                string okmsg = "";// "会员编号：" + model.UserName + "";
                //if (model.RealName.Length > 0)
                //    okmsg += "，姓名：*" + model.RealName.Substring(1, model.RealName.Length - 1);

                // if (model.Mobile.Length > 7)
                //     okmsg += "，电话：" + model.Mobile.Substring(0,3) + "****" + model.Mobile.Substring(7, model.Mobile.Length - 7);

                return Json(new { result = "ok", refMsg = "会员验证成功，" + okmsg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "err", refMsg = "该用户不存在" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult CheckUserName(string username)
        {
            var model = UserService.Single(x => x.UserName == username);
            if (model != null)
            {
                string okmsg = "会员编号已被使用";
                return Json(new { result = "err", refMsg = okmsg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "ok", refMsg = "恭喜您，该会员编号可以使用" }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region 树视图（太阳线）
        /// <summary>
        /// 树状视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TreeView()
        {
            if (!Umodel.IsActivation) return RedirectToAction("doPass", "User");
            ViewBag.Title = "会员图谱";
            ActMessage = ViewBag.Title;
            return View(Umodel);
        }


        /// <summary>
        /// 获取树状节点数据json格式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetTreeJson(int id)
        {
            //int allchild;
            int child;
            IList<TreeNode> list = new List<TreeNode>();
            int tjrs = MvcCore.Unity.Get<JN.Data.Service.IUserService>().List(x => x.RefereeID == Umodel.ID && x.IsActivation).Count();
            tjrs = Math.Min(7, tjrs);
            //取所有父ID与参数相符数据封装到 List<TUser> 并以JSON格式返回
            var ulst = UserService.List(x => x.RefereeID == id).ToList();
            foreach (var mt in ulst)
            {
                child = UserService.List(x => x.RefereeID == mt.ID).Count();
                //allchild = users.GetRecordCount("ParentPath like '%," + mt.ID.ToString() + ",%' ") + child;
                TreeNode p = new TreeNode();
                p.id = mt.ID;
                if (mt.IsActivation)
                {
                    if (mt.Investment > 0)
                        p.text = "" + mt.UserName + "(已投资,推荐" + child + "人)";
                    else
                        p.text = "" + mt.UserName + "(未投资)";
                    //if (mt.IsAgent ?? false)
                    //    p.text = "<i style='color:#f00'>" + mt.UserName + "(经理" + mt.RealName + ",投资额" + mt.Investment.ToString("F2") + ",推荐" + child + "人)</i>";
                    //else
                    //p.text = "" + mt.UserName + "(" + mt.RealName + ",推荐" + child + "人)";
                }
                else
                    p.text = "<em style='color:#999'>" + mt.UserName + "(未激活)</em>";

                if (mt.RefereeID == 0)
                    p.type = "root";

                if (child > 0 && mt.RefereeDepth < (Umodel.RefereeDepth + tjrs))
                {
                    p.icon = "fa fa-users";
                    p.children = true;
                }
                else
                {
                    p.icon = "fa fa-user";
                    //p.state = "disabled=true";
                    p.children = false;
                }
                list.Add(p);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 列表视图（太阳线）
        /// <summary>
        /// 列表视图
        /// </summary>
        /// <param name="df">日期字段</param>
        /// <param name="dr">日期范围</param>
        /// <param name="kf">查询字段</param>
        /// <param name="kv">查询关键字</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public ActionResult ListView(int? page)
        {
            ActMessage = "自属业绩";
            var list = UserService.List(x => x.RefereeID == Umodel.ID && x.IsActivation).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }
        #endregion

        #region 双轨视图
        [HttpPost]
        public ActionResult DoubleTrack(FormCollection form)
        {
            string kv = form["kv"];

            if (UserService.List(x => x.UserName == kv).Count() > 0)
            {
                return RedirectToAction("DoubleTrack", new { ID = UserService.Single(x => x.UserName == kv).ID });
            }
            else
            {
                ViewBag.ErrorMsg = "查询的用户不存在。";
                return View("Error");
            }
        }
        /// <summary>
        /// 双轨视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DoubleTrack(int parentid = 0)
        {

            var model = parentid == 0 ? Umodel : UserService.Single(parentid);

            ViewBag.Title = "会员双轨视图";
            ActMessage = ViewBag.Title;

            if (model == null)
            {
                ViewBag.ErrorMsg = "查询的用户不存在。";
                return View("Error");
            }

            if (model != Umodel && !(model.ParentPath + ",").Contains("," + Umodel.ID + ","))
            {
                ViewBag.ErrorMsg = "非法查询请求。";
                return View("Error");
            }

            var lst = new List<Data.User>();
            if (model != null)
            {
                lst.Add(model);

                //第一层
                var U11 = UserService.Single(x => x.ParentID == model.ID && x.ChildPlace == 1);
                if (U11 == null) U11 = new Data.User { ID = -1, ParentID = model.ID };
                lst.Add(U11);

                var U12 = UserService.Single(x => x.ParentID == model.ID && x.ChildPlace == 2);
                if (U12 == null) U12 = new Data.User { ID = -1, ParentID = model.ID };
                lst.Add(U12);


                //第二层
                var U21 = UserService.Single(x => x.ParentID == U11.ID && x.ChildPlace == 1);
                if (U21 == null) U21 = new Data.User { ID = -1, ParentID = U11.ID };
                lst.Add(U21);

                var U22 = UserService.Single(x => x.ParentID == U11.ID && x.ChildPlace == 2);
                if (U22 == null) U22 = new Data.User { ID = -1, ParentID = U11.ID };
                lst.Add(U22);

                var U23 = UserService.Single(x => x.ParentID == U12.ID && x.ChildPlace == 1);
                if (U23 == null) U23 = new Data.User { ID = -1, ParentID = U12.ID };
                lst.Add(U23);

                var U24 = UserService.Single(x => x.ParentID == U12.ID && x.ChildPlace == 2);
                if (U24 == null) U24 = new Data.User { ID = -1, ParentID = U12.ID };
                lst.Add(U24);

                //第三层
                var U31 = UserService.Single(x => x.ParentID == U21.ID && x.ChildPlace == 1);
                if (U31 == null) U31 = new Data.User { ID = -1, ParentID = U21.ID };
                lst.Add(U31);

                var U32 = UserService.Single(x => x.ParentID == U21.ID && x.ChildPlace == 2);
                if (U32 == null) U32 = new Data.User { ID = -1, ParentID = U21.ID };
                lst.Add(U32);

                var U33 = UserService.Single(x => x.ParentID == U22.ID && x.ChildPlace == 1);
                if (U33 == null) U33 = new Data.User { ID = -1, ParentID = U22.ID };
                lst.Add(U33);

                var U34 = UserService.Single(x => x.ParentID == U22.ID && x.ChildPlace == 2);
                if (U34 == null) U34 = new Data.User { ID = -1, ParentID = U22.ID };
                lst.Add(U34);

                var U35 = UserService.Single(x => x.ParentID == U23.ID && x.ChildPlace == 1);
                if (U35 == null) U35 = new Data.User { ID = -1, ParentID = U23.ID };
                lst.Add(U35);

                var U36 = UserService.Single(x => x.ParentID == U23.ID && x.ChildPlace == 2);
                if (U36 == null) U36 = new Data.User { ID = -1, ParentID = U23.ID };
                lst.Add(U36);

                var U37 = UserService.Single(x => x.ParentID == U24.ID && x.ChildPlace == 1);
                if (U37 == null) U37 = new Data.User { ID = -1, ParentID = U24.ID };
                lst.Add(U37);

                var U38 = UserService.Single(x => x.ParentID == U24.ID && x.ChildPlace == 2);
                if (U38 == null) U38 = new Data.User { ID = -1, ParentID = U24.ID };
                lst.Add(U38);

                //第四层
                var U41 = UserService.Single(x => x.ParentID == U31.ID && x.ChildPlace == 1);
                if (U41 == null) U41 = new Data.User { ID = -1, ParentID = U31.ID };
                lst.Add(U41);

                var U42 = UserService.Single(x => x.ParentID == U31.ID && x.ChildPlace == 2);
                if (U42 == null) U42 = new Data.User { ID = -1, ParentID = U31.ID };
                lst.Add(U42);

                var U43 = UserService.Single(x => x.ParentID == U32.ID && x.ChildPlace == 1);
                if (U43 == null) U43 = new Data.User { ID = -1, ParentID = U32.ID };
                lst.Add(U43);

                var U44 = UserService.Single(x => x.ParentID == U32.ID && x.ChildPlace == 2);
                if (U44 == null) U44 = new Data.User { ID = -1, ParentID = U32.ID };
                lst.Add(U44);

                var U45 = UserService.Single(x => x.ParentID == U33.ID && x.ChildPlace == 1);
                if (U45 == null) U45 = new Data.User { ID = -1, ParentID = U33.ID };
                lst.Add(U45);

                var U46 = UserService.Single(x => x.ParentID == U33.ID && x.ChildPlace == 2);
                if (U46 == null) U46 = new Data.User { ID = -1, ParentID = U33.ID };
                lst.Add(U46);

                var U47 = UserService.Single(x => x.ParentID == U34.ID && x.ChildPlace == 1);
                if (U47 == null) U47 = new Data.User { ID = -1, ParentID = U34.ID };
                lst.Add(U47);

                var U48 = UserService.Single(x => x.ParentID == U34.ID && x.ChildPlace == 2);
                if (U48 == null) U48 = new Data.User { ID = -1, ParentID = U34.ID };
                lst.Add(U48);

                var U49 = UserService.Single(x => x.ParentID == U35.ID && x.ChildPlace == 1);
                if (U49 == null) U49 = new Data.User { ID = -1, ParentID = U35.ID };
                lst.Add(U49);

                var U50 = UserService.Single(x => x.ParentID == U35.ID && x.ChildPlace == 2);
                if (U50 == null) U50 = new Data.User { ID = -1, ParentID = U35.ID };
                lst.Add(U50);

                var U51 = UserService.Single(x => x.ParentID == U36.ID && x.ChildPlace == 1);
                if (U51 == null) U51 = new Data.User { ID = -1, ParentID = U36.ID };
                lst.Add(U51);

                var U52 = UserService.Single(x => x.ParentID == U36.ID && x.ChildPlace == 2);
                if (U52 == null) U52 = new Data.User { ID = -1, ParentID = U36.ID };
                lst.Add(U52);

                var U53 = UserService.Single(x => x.ParentID == U37.ID && x.ChildPlace == 1);
                if (U53 == null) U53 = new Data.User { ID = -1, ParentID = U37.ID };
                lst.Add(U53);

                var U54 = UserService.Single(x => x.ParentID == U37.ID && x.ChildPlace == 2);
                if (U54 == null) U54 = new Data.User { ID = -1, ParentID = U37.ID };
                lst.Add(U54);

                var U55 = UserService.Single(x => x.ParentID == U38.ID && x.ChildPlace == 1);
                if (U55 == null) U55 = new Data.User { ID = -1, ParentID = U38.ID };
                lst.Add(U55);

                var U56 = UserService.Single(x => x.ParentID == U38.ID && x.ChildPlace == 2);
                if (U56 == null) U56 = new Data.User { ID = -1, ParentID = U38.ID };
                lst.Add(U56);

                return View(lst);
            }
            ViewBag.ErrorMsg = "记录不存在或已被删除！";
            return View("Error");
        }
        #endregion


    }
}
