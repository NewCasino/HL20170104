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
using JN.Data.Extensions;
using JN.Data.Enum;
using JN.Services.Manager;
using System.Text.RegularExpressions;
using JN.Services.Tool;
using JN.Services.CustomException;
using System.Data.Entity.Validation;

namespace JN.Web.Areas.AdminCenter.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService UserService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;
        private static List<Data.SysParam> cacheSysParam = null;
        public UserController(ISysDBTool SysDBTool, IUserService UserService, IActLogService ActLogService)
        {
            this.UserService = UserService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
            cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().ListCache("sysparams", x => x.ID < 4000).ToList();
        }



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _AddUser()
        {

            return View();
        }

        [HttpPost]
        public ActionResult _AddUser(FormCollection fc)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                if (CFB.getcurrentprice() <= 0) throw new CustomException("当前股权未发行，无法激活会员！");

                var entity = new Data.User();

                //20160711安全更新 ---------------- start
                if (!string.IsNullOrEmpty(fc["wallet2001"]) || !string.IsNullOrEmpty(fc["wallet2002"]) || !string.IsNullOrEmpty(fc["wallet2003"]))
                {
                    var wlog = new Data.WarningLog();
                    wlog.CreateTime = DateTime.Now;
                    wlog.IP = Request.UserHostAddress;
                    if (Request.UrlReferrer != null)
                        wlog.Location = Request.UrlReferrer.ToString();
                    wlog.Platform = "会员";
                    wlog.WarningMsg = "试图在修改或添加会员时篡改数据(试图篡改钱包等敏感数据)";
                    wlog.WarningLevel = "严重";
                    wlog.ResultMsg = "拒绝";
                    wlog.UserName = Amodel.AdminName;
                    MvcCore.Unity.Get<IWarningLogService>().Add(wlog);
                    SysDBTool.Commit();
                    throw new CustomException("非法数据请求");
                }
                //20160711安全更新  --------------- end


                TryUpdateModel(entity, fc.AllKeys);
                if (!Regex.IsMatch(entity.UserName, @"^[A-Za-z0-9_]+$")) throw new CustomException("用户名只能为字母、数字和下划线");
                //if (fc["password"] != fc["passwordconfirm"]) throw new CustomException("一级密码与确认密码不相符");
                //if (fc["password2"] != fc["password2confirm"]) throw new CustomException("二级密码与确认密码不相符");
                if (string.IsNullOrEmpty(entity.UserName) || string.IsNullOrEmpty(entity.RealName) || string.IsNullOrEmpty(entity.Mobile)) throw new CustomException("会员编号、真实姓名、手机号码不能为空");
                if (string.IsNullOrEmpty(entity.Email)) throw new CustomException("电子邮箱不能为空");
                if (entity.FristApplyInvestment <= 0) throw new CustomException("请选择注册类型");
                if (string.IsNullOrEmpty(entity.Password) || string.IsNullOrEmpty(entity.Password2)) throw new CustomException("一级密码、二级密码不能为空");
                if (UserService.List(x => x.UserName == entity.UserName.Trim()).Count() > 0) throw new CustomException("用户名已被使用");
                //if (string.IsNullOrEmpty(entity.BankCard) || string.IsNullOrEmpty(entity.BankUser) || string.IsNullOrEmpty(entity.BankOfDeposit)) throw new CustomException("请输入银行卡信息");
                //if (entity.RealName != entity.BankUser) throw new CustomException("真实姓名与银行卡户名不相符");
                if (UserService.List(x => x.UserName == entity.RefereeUser && x.IsActivation).Count() <= 0) throw new CustomException("推荐人不存在或未激活");

                //if (ConfigHelper.GetConfigString("MemberAtlas") != "sun")
                //{
                //    if (UserService.List(x => x.UserName == entity.ParentUser && x.IsActivation).Count() <= 0) throw new CustomException("安置人不存在或未激活");
                //}
                if (ConfigHelper.GetConfigBool("HaveAgent"))
                {
                    if (UserService.List(x => x.AgentName == entity.AgentUser).Count() <= 0 || entity.AgentUser == null) throw new CustomException("报单中心不存在");
                    var agentUser = UserService.Single(x => x.AgentName == entity.AgentUser);
                    entity.AgentID = agentUser.ID;
                    entity.AgentUser = agentUser.UserName;
                }

                Data.User _parentUser = null;
                //if (ConfigHelper.GetConfigString("MemberAtlas") == "double")
                //{
                //    _parentUser = UserService.Single(x => x.UserName == entity.ParentUser);
                //    if (_parentUser != null)
                //    {
                //        if (UserService.List(x => x.ParentID == _parentUser.ID).Count() >= 2) throw new CustomException("安置人安置名额已满");
                //    }
                //    if (entity.ChildPlace > 2 || entity.ChildPlace < 1) throw new CustomException("安置参数不正确");
                //    if (UserService.List(x => x.ParentUser == entity.ParentUser && x.ChildPlace == entity.ChildPlace).Count() > 0) throw new CustomException("当前位置无法安置");
                //}
                //else if (ConfigHelper.GetConfigString("MemberAtlas") == "three")
                //{
                //    _parentUser = UserService.Single(x => x.UserName == entity.ParentUser);
                //    if (_parentUser != null)
                //    {
                //        if (UserService.List(x => x.ParentID == _parentUser.ID).Count() >= 3) throw new CustomException("安置人安置名额已满");
                //    }

                //    if (entity.ChildPlace > 3 || entity.ChildPlace < 1) throw new CustomException("安置参数不正确");
                //    if (UserService.List(x => x.ParentUser == entity.ParentUser && x.ChildPlace == entity.ChildPlace).Count() > 0) throw new CustomException("当前位置无法安置");
                //}
                //else
                _parentUser = UserService.Single(x => x.UserName == entity.RefereeUser);

                entity.IsActivation = false;
                entity.IsAgent = false;
                entity.IsLock = false;
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
                entity.CreateBy = "Admin";
                entity.Investment = entity.FristApplyInvestment ?? 0;
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



                UserService.Add(entity);
                _parentUser.Child = _parentUser.Child + 1;
                UserService.Update(_parentUser);
                _refereeUser.RefereeCount = _refereeUser.RefereeCount + 1;
                UserService.Update(_refereeUser);
                SysDBTool.Commit();
                Wallets.changeWallet(entity.ID, entity.Investment, 2002, "后台注册赠送");
                if (!entity.IsActivation) Users.ActivationAccount(entity);
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
        public ActionResult List(int? page)
        {
            ActMessage = "会员管理";
            //动态构建查询
            var list = UserService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            string isactivation = Request["isactivation"];
            if (!string.IsNullOrEmpty(isactivation))
            {
                bool isactive = (isactivation == "1");
                list = list.Where(x => x.IsActivation == isactive && x.IsLock == false).ToList();
            }

            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToList().ToPagedList(page ?? 1, 20));
        }

        public ActionResult ZtList(int? page)
        {
            ActMessage = "直推会员管理";
            //动态构建查询
            var list = UserService.List().WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).ToList();
            if (Request["OrderFiled"] == "RefereeCount")
                list = list.OrderByDescending(x => x.RefereeCount).ToList();
            if (Request["OrderFiled"] == "Investment")
                list = list.OrderByDescending(x => x.Investment).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToList().ToPagedList(page ?? 1, 20));
        }

        /// <summary>
        /// 被冻结的会员
        /// </summary>
        /// <returns></returns>
        public ActionResult Lock(int? page)
        {
            ActMessage = "被冻结的会员";
            var list = UserService.List(x => x.IsLock).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.ID).ToList();
            if (Request["IsExport"] == "1")
            {
                string FileName = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
                MvcCore.Extensions.ExcelHelperV2.ToExcel(list.ToList()).SaveToExcel(Server.MapPath("/upfile/" + FileName + ".xls"));
                return File(Server.MapPath("/upfile/" + FileName + ".xls"), "application/ms-excel", FileName + ".xls");
            }
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Modify(int? id)
        {
            //if (Session["CheckPwd"] == null)
            //{
            //    Session["Url"] = Request.Url.PathAndQuery;
            //    return Redirect("/AdminCenter/Home/CheckPassWord2");
            //}
            if (id.HasValue)
            {
                ActMessage = "编辑会员资料";
                return View(UserService.Single(id));
            }
            else
            {
                ActMessage = "新增初始会员";
                return View(new Data.User());
            }
        }

        [HttpPost]
        public ActionResult Modify(FormCollection fc)
        {
            ReturnResult result = new ReturnResult();
            try
            {
                var entity = UserService.SingleAndInit(fc["ID"].ToInt());
                var onUser = entity.ToModel<Data.User>();
                TryUpdateModel(entity, fc.AllKeys);

                //20160711安全更新 ---------------- start
                if (!string.IsNullOrEmpty(fc["wallet2001"]) || !string.IsNullOrEmpty(fc["wallet2002"]) || !string.IsNullOrEmpty(fc["wallet2003"]))
                {
                    var wlog = new Data.WarningLog();
                    wlog.CreateTime = DateTime.Now;
                    wlog.IP = Request.UserHostAddress;
                    if (Request.UrlReferrer != null)
                        wlog.Location = Request.UrlReferrer.ToString();
                    wlog.Platform = "会员";
                    wlog.WarningMsg = "试图在修改或添加会员时篡改数据(试图篡改钱包等敏感数据)";
                    wlog.WarningLevel = "严重";
                    wlog.ResultMsg = "拒绝";
                    wlog.UserName = Amodel.AdminName;
                    MvcCore.Unity.Get<IWarningLogService>().Add(wlog);
                    SysDBTool.Commit();
                    throw new CustomException("非法数据请求");
                }
                //20160711安全更新  --------------- end


                if (entity.ID > 0)
                {
                    string msg = "";
                    if (string.IsNullOrEmpty(entity.UserName) || string.IsNullOrEmpty(entity.Mobile)) throw new CustomException("会员编号、联系电话不能为空");
                    //if (string.IsNullOrEmpty(entity.BankCard) || string.IsNullOrEmpty(entity.BankUser) || string.IsNullOrEmpty(entity.BankOfDeposit)) throw new CustomException("请输入银行卡信息");
                    //if (entity.RealName != entity.BankUser) throw new CustomException("真实姓名与银行卡户名不相符");
                    string resetpwd2 = fc["resetpassowrd2"];
                    string resetpwd = fc["resetpassowrd"];
                    if (!string.IsNullOrEmpty(resetpwd))
                    {
                        entity.Password = resetpwd.ToMD5().ToMD5();
                        msg += " 修改登录密码";
                    }
                    if (!string.IsNullOrEmpty(resetpwd2))
                    {
                        entity.Password2 = resetpwd2.ToMD5().ToMD5();
                        msg += " 修改二级密码";
                    }

                    if (onUser.Mobile != entity.Mobile) msg += " 手机变更：" + onUser.Mobile + " => " + entity.Mobile;
                    if (onUser.RealName != entity.RealName) msg += "　姓名变更：" + onUser.RealName + " => " + entity.RealName;
                    if (onUser.AliPay != entity.AliPay) msg += "　支付宝变更：" + onUser.AliPay + " => " + entity.AliPay;
                    if (onUser.BankCard != entity.BankCard) msg += "　银行卡变更：" + onUser.BankCard + " => " + entity.BankCard;

                    var wlog2 = new Data.WarningLog();
                    wlog2.CreateTime = DateTime.Now;
                    wlog2.IP = Request.UserHostAddress;
                    if (Request.UrlReferrer != null)
                        wlog2.Location = Request.UrlReferrer.ToString();
                    wlog2.Platform = "后台";
                    wlog2.WarningMsg = "由管理员“" + Amodel.AdminName + "”修改会员资料" + (!string.IsNullOrEmpty(msg) ? ",涉及改动信息：" + msg : "");
                    wlog2.WarningLevel = "一般";
                    wlog2.ResultMsg = "通过";
                    wlog2.UserName = entity.UserName;
                    MvcCore.Unity.Get<IWarningLogService>().Add(wlog2);
                    SysDBTool.Commit();

                    UserService.Update(entity);
                }
                else
                {
                    if (!Regex.IsMatch(entity.UserName, @"^[A-Za-z0-9_]+$")) throw new CustomException("用户名只能为字母、数字和下划线");
                    if (string.IsNullOrEmpty(entity.UserName) || string.IsNullOrEmpty(entity.RealName) || string.IsNullOrEmpty(entity.Mobile)) throw new CustomException("会员编号、真实姓名、联系电话不能为空");
                    if (string.IsNullOrEmpty(entity.Password) || string.IsNullOrEmpty(entity.Password2)) throw new CustomException("登录密码、支付密码不能为空");
                    if (string.IsNullOrEmpty(entity.BankCard) || string.IsNullOrEmpty(entity.BankUser) || string.IsNullOrEmpty(entity.BankOfDeposit)) throw new CustomException("请输入银行卡信息");
                    if (UserService.List(x => x.UserName == entity.UserName.Trim()).Count() > 0) throw new CustomException("会员编号已被使用");

                    entity.IsActivation = true;
                    entity.IsAgent = false;//注册时报单中心
                    entity.AgentName ="";//报单中心名字
                    entity.IsLock = false;
                    entity.ActivationTime = DateTime.Now;
                    entity.IsShareBouns = true;
                    //节点部分
                    entity.RootID = 0;
                    entity.ParentID = 0;
                    entity.ParentUser = "";
                    entity.ParentPath = ",0";
                    entity.Depth = 0;
                    entity.DepthSort = 1;
                    entity.Child = 0;
                    entity.ChildPlace = 1;

                    entity.RefereeID = 0;
                    entity.RefereeUser = "";
                    entity.RefereeDepth = 0;
                    entity.RefereePath = ",0";

                    entity.Investment = cacheSysParam.SingleAndInit(x => x.ID == 1005).Value.ToDecimal();
                    entity.UserLevel = (int)Data.Enum.UserLeve.Level1;

                    entity.Password = entity.Password.ToMD5().ToMD5();
                    entity.Password2 = entity.Password2.ToMD5().ToMD5();
                    entity.CreateTime = DateTime.Now;
                    UserService.Add(entity);
                }
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

        /// <summary>
        /// 删除用户，未激活时才可以
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            var model = UserService.Single(id);
            if (model != null)
            {
                if (!model.IsActivation && model.Child == 0)
                {
                    //删除时上级子节点减少
                    if (model.ParentID > 0)
                    {
                        var _parentUser = UserService.Single(model.ParentID);
                        if (_parentUser != null)
                        {
                            _parentUser.Child = _parentUser.Child - 1;
                            UserService.Update(_parentUser);
                        }
                    }
                    UserService.Delete(id);
                    SysDBTool.Commit();
                    ViewBag.SuccessMsg = "删除成功！";
                    return View("Success");
                }
                else
                {
                    ViewBag.ErrorMsg = "该会员已被激活或伞下还有节点，无法删除。";
                    return View("Error");
                }
            }
            ViewBag.ErrorMsg = "ID不存在或已被删除。";
            return View("Error");
        }

        public ActionResult doCommand(int id, string commandtype)
        {
            var model = UserService.Single(id);
            if (commandtype.ToLower() == "lock")
            {
                model.IsLock = true;
                model.LockReason = Request["reason"];

                //取消FC币委托单
                MvcCore.Unity.Get<IStockEntrustsTradeService>().List(x => x.UID == model.ID && x.Status <= (int)TTCStatus.PartOfTheDeal && x.Status >= (int)TTCStatus.Entrusts).ToList().ForEach(
                    b => Stocks.cancelTTC(b.EntrustsNo)
                );
                //取消EP委托单
                //MvcCore.Unity.Get<IUSDPutOnService>().List(x => x.UID == model.ID && x.Status < 2 && x.Status >= 0).ToList().ForEach(
                //    b => CFB.cancelOrder(b.ID)
                //);


            }
            else if (commandtype.ToLower() == "unlock")
                model.IsLock = false;
            else if (commandtype.ToLower() == "resetinputwrong")
                model.InputWrongPwdTimes = 0;
            else if (commandtype.ToLower() == "walletlock")
                model.WalletLock = true;
            else if (commandtype.ToLower() == "unwalletlock")
                model.WalletLock = false;
            UserService.Update(model);
            SysDBTool.Commit();
            ViewBag.SuccessMsg = "操作成功！";
            return View("Success");
        }


        /// <summary>
        /// 会员帐号冻结
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public ActionResult MakeLock(int id, string reason)
        {
            var model = UserService.Single(id);
            if (model != null)
            {
                model.IsLock = true;
                model.LockReason = reason;
                model.LockTime = DateTime.Now;
                UserService.Update(model);
                SysDBTool.Commit();
                return Content("ok");
            }
            return Content("Error");
        }


        public ActionResult ShareLock(int id)
        {
            ActMessage = "设置不可分红";
            var model = UserService.Single(id);
            if (model != null)
            {
                model.IsShareBouns = false;
                UserService.Update(model);
                SysDBTool.Commit();
                return Content("ok");
            }
            return Content("Error");
        }
        public ActionResult UnShareLock(int id)
        {
            ActMessage = "设置可分红";
            var model = UserService.Single(id);
            if (model != null)
            {
                model.IsShareBouns = true;
                UserService.Update(model);
                SysDBTool.Commit();
                return Content("ok");
            }
            return Content("Error");
        }


        public ActionResult TakeLock(int id)
        {
            ActMessage = "设置不可提现";
            var model = UserService.Single(id);
            if (model != null)
            {
                model.ReserveBoolean1 = false;
                UserService.Update(model);
                SysDBTool.Commit();
                return Content("ok");
            }
            return Content("Error");
        }
        public ActionResult UnTakeLock(int id)
        {
            ActMessage = "设置可提现";
            var model = UserService.Single(id);
            if (model != null)
            {
                model.ReserveBoolean1 = true;
                UserService.Update(model);
                SysDBTool.Commit();
                return Content("ok");
            }
            return Content("Error");
        }

        /// <summary>
        /// 设置会员级别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MakeLevel(int id, string level)
        {
            var model = UserService.Single(id);
            if (model != null)
            {
                int uplevel = level.ToInt();
                switch (uplevel)
                {
                    case 1:
                        model.UserLevel = 1;
                        model.AgentName = cacheSysParam.SingleAndInit(x => x.ID >= 1801).Name;
                        break;
                    case 2:
                        model.UserLevel = 2;
                        model.AgentName = cacheSysParam.SingleAndInit(x => x.ID >= 1802).Name;
                        break;
                    case 3:
                        model.UserLevel = 3;
                        model.AgentName = cacheSysParam.SingleAndInit(x => x.ID >= 1803).Name;
                        break;
                }
                model.IsAgent = true;
                UserService.Update(model);
                SysDBTool.Commit();
                ActPacket = model;
                return Content("ok");
            }
            return Content("Error");
        }
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
        public ActionResult DoubleTrack(int id)
        {
            ActMessage = "会员双轨视图";
            var model = UserService.Single(id);
            string keyword = Request["kv"];
            if (Request["kf"] == "username" && !string.IsNullOrEmpty(keyword))
            {
                var sUser = UserService.Single(x => x.UserName == keyword);
                if (sUser != null) return Redirect(Url.Action("DoubleTrack", new { ID = sUser.ID }));
            }
            else if (Request["kf"] == "mobile" && !string.IsNullOrEmpty(keyword))
            {
                var sUser = UserService.Single(x => x.Mobile == keyword);
                if (sUser != null) return Redirect(Url.Action("DoubleTrack", new { ID = sUser.ID }));
            }
            ViewBag.Title = "“" + model.UserName + "”会员双轨视图";

            var lst = new List<Data.User>();
            if (model != null)
            {
                lst.Add(model);

                //第一层
                var U11 = UserService.Single(x => x.ParentID == model.ID && x.ChildPlace == 1);
                if (U11 == null) U11 = new Data.User { ID = -1, ParentID = model.ID, ParentUser = model.UserName, AgentUser = model.AgentUser };
                lst.Add(U11);

                var U12 = UserService.Single(x => x.ParentID == model.ID && x.ChildPlace == 2);
                if (U12 == null) U12 = new Data.User { ID = -1, ParentID = model.ID, ParentUser = model.UserName, AgentUser = model.AgentUser };
                lst.Add(U12);


                //第二层
                var U21 = UserService.Single(x => x.ParentID == U11.ID && x.ChildPlace == 1);
                if (U21 == null) U21 = new Data.User { ID = -1, ParentID = U11.ID, ParentUser = U11.UserName, AgentUser = U11.AgentUser };
                lst.Add(U21);

                var U22 = UserService.Single(x => x.ParentID == U11.ID && x.ChildPlace == 2);
                if (U22 == null) U22 = new Data.User { ID = -1, ParentID = U11.ID, ParentUser = U11.UserName, AgentUser = U11.AgentUser };
                lst.Add(U22);

                var U23 = UserService.Single(x => x.ParentID == U12.ID && x.ChildPlace == 1);
                if (U23 == null) U23 = new Data.User { ID = -1, ParentID = U12.ID, ParentUser = U12.UserName, AgentUser = U12.AgentUser };
                lst.Add(U23);

                var U24 = UserService.Single(x => x.ParentID == U12.ID && x.ChildPlace == 2);
                if (U24 == null) U24 = new Data.User { ID = -1, ParentID = U12.ID, ParentUser = U12.UserName, AgentUser = U12.AgentUser };
                lst.Add(U24);

                //第三层
                var U31 = UserService.Single(x => x.ParentID == U21.ID && x.ChildPlace == 1);
                if (U31 == null) U31 = new Data.User { ID = -1, ParentID = U21.ID, ParentUser = U21.UserName, AgentUser = U21.AgentUser };
                lst.Add(U31);

                var U32 = UserService.Single(x => x.ParentID == U21.ID && x.ChildPlace == 2);
                if (U32 == null) U32 = new Data.User { ID = -1, ParentID = U21.ID, ParentUser = U21.UserName, AgentUser = U21.AgentUser };
                lst.Add(U32);

                var U33 = UserService.Single(x => x.ParentID == U22.ID && x.ChildPlace == 1);
                if (U33 == null) U33 = new Data.User { ID = -1, ParentID = U22.ID, ParentUser = U22.UserName, AgentUser = U22.AgentUser };
                lst.Add(U33);

                var U34 = UserService.Single(x => x.ParentID == U22.ID && x.ChildPlace == 2);
                if (U34 == null) U34 = new Data.User { ID = -1, ParentID = U22.ID, ParentUser = U22.UserName, AgentUser = U22.AgentUser };
                lst.Add(U34);

                var U35 = UserService.Single(x => x.ParentID == U23.ID && x.ChildPlace == 1);
                if (U35 == null) U35 = new Data.User { ID = -1, ParentID = U23.ID, ParentUser = U23.UserName, AgentUser = U23.AgentUser };
                lst.Add(U35);

                var U36 = UserService.Single(x => x.ParentID == U23.ID && x.ChildPlace == 2);
                if (U36 == null) U36 = new Data.User { ID = -1, ParentID = U23.ID, ParentUser = U23.UserName, AgentUser = U23.AgentUser };
                lst.Add(U36);

                var U37 = UserService.Single(x => x.ParentID == U24.ID && x.ChildPlace == 1);
                if (U37 == null) U37 = new Data.User { ID = -1, ParentID = U24.ID, ParentUser = U24.UserName, AgentUser = U24.AgentUser };
                lst.Add(U37);

                var U38 = UserService.Single(x => x.ParentID == U24.ID && x.ChildPlace == 2);
                if (U38 == null) U38 = new Data.User { ID = -1, ParentID = U24.ID, ParentUser = U24.UserName, AgentUser = U24.AgentUser };
                lst.Add(U38);

                //第四层
                var U41 = UserService.Single(x => x.ParentID == U31.ID && x.ChildPlace == 1);
                if (U41 == null) U41 = new Data.User { ID = -1, ParentID = U31.ID, ParentUser = U31.UserName, AgentUser = U31.AgentUser };
                lst.Add(U41);

                var U42 = UserService.Single(x => x.ParentID == U31.ID && x.ChildPlace == 2);
                if (U42 == null) U42 = new Data.User { ID = -1, ParentID = U31.ID, ParentUser = U31.UserName, AgentUser = U31.AgentUser };
                lst.Add(U42);

                var U43 = UserService.Single(x => x.ParentID == U32.ID && x.ChildPlace == 1);
                if (U43 == null) U43 = new Data.User { ID = -1, ParentID = U32.ID, ParentUser = U32.UserName, AgentUser = U32.AgentUser };
                lst.Add(U43);

                var U44 = UserService.Single(x => x.ParentID == U32.ID && x.ChildPlace == 2);
                if (U44 == null) U44 = new Data.User { ID = -1, ParentID = U32.ID, ParentUser = U32.UserName, AgentUser = U32.AgentUser };
                lst.Add(U44);

                var U45 = UserService.Single(x => x.ParentID == U33.ID && x.ChildPlace == 1);
                if (U45 == null) U45 = new Data.User { ID = -1, ParentID = U33.ID, ParentUser = U33.UserName, AgentUser = U33.AgentUser };
                lst.Add(U45);

                var U46 = UserService.Single(x => x.ParentID == U33.ID && x.ChildPlace == 2);
                if (U46 == null) U46 = new Data.User { ID = -1, ParentID = U33.ID, ParentUser = U33.UserName, AgentUser = U33.AgentUser };
                lst.Add(U46);

                var U47 = UserService.Single(x => x.ParentID == U34.ID && x.ChildPlace == 1);
                if (U47 == null) U47 = new Data.User { ID = -1, ParentID = U34.ID, ParentUser = U34.UserName, AgentUser = U34.AgentUser };
                lst.Add(U47);

                var U48 = UserService.Single(x => x.ParentID == U34.ID && x.ChildPlace == 2);
                if (U48 == null) U48 = new Data.User { ID = -1, ParentID = U34.ID, ParentUser = U34.UserName, AgentUser = U34.AgentUser };
                lst.Add(U48);

                var U49 = UserService.Single(x => x.ParentID == U35.ID && x.ChildPlace == 1);
                if (U49 == null) U49 = new Data.User { ID = -1, ParentID = U35.ID, ParentUser = U35.UserName, AgentUser = U35.AgentUser };
                lst.Add(U49);

                var U50 = UserService.Single(x => x.ParentID == U35.ID && x.ChildPlace == 2);
                if (U50 == null) U50 = new Data.User { ID = -1, ParentID = U35.ID, ParentUser = U35.UserName, AgentUser = U35.AgentUser };
                lst.Add(U50);

                var U51 = UserService.Single(x => x.ParentID == U36.ID && x.ChildPlace == 1);
                if (U51 == null) U51 = new Data.User { ID = -1, ParentID = U36.ID, ParentUser = U36.UserName, AgentUser = U36.AgentUser };
                lst.Add(U51);

                var U52 = UserService.Single(x => x.ParentID == U36.ID && x.ChildPlace == 2);
                if (U52 == null) U52 = new Data.User { ID = -1, ParentID = U36.ID, ParentUser = U36.UserName, AgentUser = U36.AgentUser };
                lst.Add(U52);

                var U53 = UserService.Single(x => x.ParentID == U37.ID && x.ChildPlace == 1);
                if (U53 == null) U53 = new Data.User { ID = -1, ParentID = U37.ID, ParentUser = U37.UserName, AgentUser = U37.AgentUser };
                lst.Add(U53);

                var U54 = UserService.Single(x => x.ParentID == U37.ID && x.ChildPlace == 2);
                if (U54 == null) U54 = new Data.User { ID = -1, ParentID = U37.ID, ParentUser = U37.UserName, AgentUser = U37.AgentUser };
                lst.Add(U54);

                var U55 = UserService.Single(x => x.ParentID == U38.ID && x.ChildPlace == 1);
                if (U55 == null) U55 = new Data.User { ID = -1, ParentID = U38.ID, ParentUser = U38.UserName, AgentUser = U38.AgentUser };
                lst.Add(U55);

                var U56 = UserService.Single(x => x.ParentID == U38.ID && x.ChildPlace == 2);
                if (U56 == null) U56 = new Data.User { ID = -1, ParentID = U38.ID, ParentUser = U38.UserName, AgentUser = U38.AgentUser };
                lst.Add(U56);

                return View(lst);
            }
            ViewBag.ErrorMsg = "记录不存在或已被删除！";
            return View("Error");
        }
        #endregion

        #region 推荐树视图
        /// <summary>
        /// 树状视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TreeView(int id)
        {
            var model = UserService.Single(id);
            ActMessage = "会员推荐图谱";
            if (model == null)
            {
                ViewBag.ErrorMsg = "记录不存在或已被删除！";
                return View("Error");
            }
            return View(model);
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
                    if (mt.IsAgent ?? false)
                        p.text = "<i style='color:#f00'>" + mt.UserName + "(经理" + mt.RealName + ",推荐" + child + "人)</i>  <a href=\"javascript: void(0);\" onclick=\"javascript: winopen('注册会员', '" + Url.Action("_AddUser", new { parentuser = mt.UserName, refereeuser = mt.UserName, agentuser = mt.AgentUser, childplace = 1 }) + "', 500); \">注册</a>";
                    else
                        p.text = "" + mt.UserName + "(" + mt.RealName + ",推荐" + child + "人) <a href=\"javascript: void(0);\" onclick=\"javascript: winopen('注册会员', '" + Url.Action("_AddUser", new { parentuser = mt.UserName, refereeuser = mt.UserName, agentuser = mt.AgentUser, childplace = 1 }) + "', 500); \">注册</a>";
                }
                else
                    p.text = "<em style='color:#999'>" + mt.UserName + "(未激活)</em>";

                if (mt.RefereeID == 0)
                {
                    p.type = "root";
                }

                if (child > 0)
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
    }
        #endregion


}