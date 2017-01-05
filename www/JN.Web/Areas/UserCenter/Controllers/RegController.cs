using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using JN.Data;
using JN.Data.Service;
using MvcCore.Controls;
using System.Web.Security;
using JN.Data.Enum;
using JN.Services;
using JN.Services.Tool;
using System.Text.RegularExpressions;
using JN.Services.Manager;
using System.Data.Entity.Validation;
using JN.Services.CustomException;

namespace JN.Web.Areas.UserCenter.Controllers
{
    public class RegController : Controller
    {
        private static List<Data.SysParam> cacheSysParam = null;

        private readonly IUserService UserService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public RegController(ISysDBTool SysDBTool, IUserService UserService, IActLogService ActLogService)
        {
            this.UserService = UserService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();
        }
        [HttpGet]
        public ActionResult Index()
        {
            string username = Request["r"];
            var sysEntity = MvcCore.Unity.Get<JN.Data.Service.ISysSettingService>().Single(1);
            if (!sysEntity.IsRegisterOpen)
            {
                Response.Write(sysEntity.CloseRegisterHint);
                Response.End();
            }

            if (!string.IsNullOrEmpty(username))
                ViewBag.username = username;
            return View();
        }


        #region 添加会员

        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult index(FormCollection fc)
        {
            string vcode = (Session["UserValidateCode"] ?? "").ToString();
            Session.Remove("UserValidateCode");
            //string code = fc["code"];
            ReturnResult result = new ReturnResult();
            try
            {
                //if (string.IsNullOrEmpty(vcode) || string.IsNullOrEmpty(code) || !vcode.Equals(code, StringComparison.InvariantCultureIgnoreCase))
                //    throw new CustomException("验证码错误");
                if (fc["password"] != fc["passwordconfirm"]) throw new CustomException("一级密码与确认密码不相符");
                if (fc["password2"] != fc["password2confirm"]) throw new CustomException("二级密码与确认密码不相符");

                var sysEntity = MvcCore.Unity.Get<JN.Data.Service.ISysSettingService>().Single(1);
                var entity = new Data.User();
                TryUpdateModel(entity, fc.AllKeys);


                if (sysEntity.RegistNeedSMSVerification ?? false)  //开启注册手机短信验证
                {
                    string smscode = fc["smscode"];
                    if (string.IsNullOrEmpty(smscode)) throw new CustomException("手机验证码不能为空");
                    if (Session["SMSValidateCode"] == null || smscode.Trim() != Session["SMSValidateCode"].ToString()) throw new CustomException("验证码不正确或已过期");
                    if (Session["GetSMSUser"] == null || entity.Mobile != Session["GetSMSUser"].ToString()) throw new CustomException("手机号码与接收验证码的设备不相符");
                }

                if (!Regex.IsMatch(entity.UserName, @"^[A-Za-z0-9_]+$")) throw new CustomException("用户名只能为字母、数字和下划线");
                if (string.IsNullOrEmpty(entity.UserName)) throw new CustomException("会员编号不能为空");
                if (entity.FristApplyInvestment <= 0) throw new CustomException("请选择注册类型");
                if (string.IsNullOrEmpty(entity.Password) || string.IsNullOrEmpty(entity.Password2)) throw new CustomException("一级密码、二级密码不能为空");
                if (UserService.List(x => x.UserName == entity.UserName.Trim()).Count() > 0) throw new CustomException("用户名已被使用");

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

                if (UserService.List(x => x.NickName == entity.NickName.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",NickName,")) throw new CustomException("昵称已被使用");
                if (UserService.List(x => x.Mobile == entity.Mobile.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",Mobile,")) throw new CustomException("手机号码已被使用");
                if (UserService.List(x => x.Email == entity.Email.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",Email,")) throw new CustomException("邮箱已被使用");
                if (UserService.List(x => x.IDCard == entity.IDCard.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",IDCard,")) throw new CustomException("身份证号码已被使用");
                if (UserService.List(x => x.AliPay == entity.AliPay.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",AliPay,")) throw new CustomException("支付宝帐号已被使用");
                if (UserService.List(x => x.WeiXin == entity.WeiXin.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",WeiXin,")) throw new CustomException("微信帐号已被使用");
                if (UserService.List(x => x.BankCard == entity.BankCard.Trim()).Count() > 0 && sysEntity.RegistOnlyOneItems.Contains(",BankCard,")) throw new CustomException("银行卡号已被使用");



                //if (entity.RealName != entity.BankUser) throw new CustomException("真实姓名与银行卡户名不相符");
                if (UserService.List(x => x.UserName == entity.RefereeUser).Count() <= 0) throw new CustomException("推荐人不存在或未激活");
                else
                {
                    if (UserService.Single(x => x.UserName == entity.RefereeUser).Investment <= 0) throw new CustomException("推荐人未投资");
                }
                //if (ConfigHelper.GetConfigString("MemberAtlas") != "sun")
                //{
                //    if (UserService.List(x => x.UserName == entity.ParentUser).Count() <= 0) throw new CustomException("安置人不存在或未激活");
                //}
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
                    if (UserService.List(x => x.ParentUser == entity.ParentUser && x.ChildPlace == entity.ChildPlace).Count() > 0) throw new CustomException("当前位置无法安置");
                }
                else if (ConfigHelper.GetConfigString("MemberAtlas") == "three")
                {
                    _parentUser = UserService.Single(x => x.UserName == entity.ParentUser);
                    if (_parentUser != null)
                    {
                        if (UserService.List(x => x.ParentID == _parentUser.ID).Count() >= 3) throw new CustomException("安置人安置名额已满");
                    }

                    if (entity.ChildPlace > 3 || entity.ChildPlace < 1) throw new CustomException("安置参数不正确");
                    if (UserService.List(x => x.ParentUser == entity.ParentUser && x.ChildPlace == entity.ChildPlace).Count() > 0) throw new CustomException("当前位置无法安置");
                }
                else
                {

                  _parentUser = UserService.Single(x => x.UserName == entity.RefereeUser);
                }
                

                entity.IsActivation = false;
               // entity.IsActivation = true;
                entity.ActivationTime = DateTime.Now;

                entity.IsAgent = false;
                entity.IsLock = false;
                entity.IsShareBouns = true;
                entity.Investment = cacheSysParam.SingleAndInit(x => x.ID == 1001).Value.ToDecimal();

                //节点部分
                var _refereeUser = UserService.Single(x => x.UserName == entity.RefereeUser);

                //if (_parentUser.RootID != _refereeUser.RootID || Umodel.RootID != _parentUser.RootID) throw new CustomException("推荐人和安置人以及您自己必须同一网内会员");
                entity.RefereeDepth = _refereeUser.RefereeDepth + 1;
                entity.RefereePath = _refereeUser.RefereePath + "," + _refereeUser.ID;
                entity.RefereeID = _refereeUser.ID;
                entity.RefereeUser = _refereeUser.UserName;

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
                entity.CreateBy = entity.UserName;
                entity.Investment = 0;
                entity.ReserveDecamal1 = entity.Investment;


                //var parentmodel = MvcCore.Unity.Get<Data.Service.IUserService>().Single(entity.ParentID);
                if (ConfigHelper.GetConfigString("MemberAtlas") == "double")
                {
                    entity.DepthSort = (_parentUser.DepthSort - 1) * 2 + entity.ChildPlace;
                }

                else if (ConfigHelper.GetConfigString("MemberAtlas") == "three")
                {
                    entity.DepthSort = (_parentUser.DepthSort - 1) * 3 + entity.ChildPlace;
                }

                else
                {
                    entity.ChildPlace = UserService.List(x => x.ParentID == _parentUser.ID).Count() > 0 ? UserService.List(x => x.ParentID == _parentUser.ID).Max(x => x.ChildPlace) + 1 : 1;
                    entity.DepthSort = 0;
                    


                }

                entity.Wallet2001 = 0;
                entity.Wallet2002 = 0;
                entity.Wallet2003 = 0;
                entity.Wallet2004 = 0;
                entity.Wallet2005 = 0;
                entity.Wallet2006 = 0;
                //UserService.Update();
                UserService.Add(entity);
                _parentUser.Child = _parentUser.Child + 1;
                SysDBTool.Commit();
                //为二叉树的父节点添加 左右孩子ID
                if (entity.ChildPlace == 1)
                {
                    _parentUser.LeftChild = entity.ID;
                }
                else
                {
                    _parentUser.RightChild = entity.ID;
                }
                UserService.Update(_parentUser);
                
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


        public ActionResult SendMobileMsm()
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string phone = Request["myphone"];
                if (string.IsNullOrEmpty(phone)) throw new CustomException("请您填写手机号码");
                if (!StringHelp.IsPhone(phone)) throw new CustomException("请输入正确的手机号码");

                if (Session["SMSSendTime"] != null)
                {
                    if (!DateTimeDiff.DateDiff_minu(DateTime.Parse(Session["SMSSendTime"].ToString())))
                        throw new CustomException("每次获取验证码间隔不能少于1分钟");
                }

                System.ValidateCode vEmailCode = new System.ValidateCode();
                string smscode = vEmailCode.CreateRandomCode(4, "1,2,3,4,5,6,7,8,9,0");
                Session["SMSValidateCode"] = smscode;
                Session["GetSMSUser"] = phone;
                Session["SMSSendTime"] = DateTime.Now;

                bool b = SMSHelper.WebChineseMSM(phone, "您本次操作的验证码为：" + smscode + "");
                if (b)
                    result.Status = 200;
                else
                {
                    Session["SMSValidateCode"] = null;
                    Session["GetSMSUser"] = null;
                    result.Message = "短信发送失败,详情请查阅发送记录";
                    result.Status = 500;
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

        public ActionResult SendMobileVoice()
        {
            ReturnResult result = new ReturnResult();
            try
            {
                string phone = Request["myphone"];
                if (string.IsNullOrEmpty(phone)) throw new CustomException("请您填写手机号码");
                if (!StringHelp.IsPhone(phone)) throw new CustomException("请输入正确的手机号码");

                if (Session["SMSSendTime"] != null)
                {
                    if (!DateTimeDiff.DateDiff_minu(DateTime.Parse(Session["SMSSendTime"].ToString())))
                        throw new CustomException("每次获取验证码间隔不能少于1分钟");
                }

                System.ValidateCode vEmailCode = new System.ValidateCode();
                string smscode = vEmailCode.CreateRandomCode(4, "1,2,3,4,5,6,7,8,9,0");
                Session["SMSValidateCode"] = smscode;
                Session["GetSMSUser"] = phone;
                Session["SMSSendTime"] = DateTime.Now;

                bool b = SMSHelper.VoiceVerify(phone, smscode, "");
                if (b)
                    result.Status = 200;
                else
                {
                    Session["SMSValidateCode"] = null;
                    Session["GetSMSUser"] = null;
                    result.Status = 500;
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
    }
}
