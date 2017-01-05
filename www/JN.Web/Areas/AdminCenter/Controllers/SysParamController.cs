﻿using JN.Data;
using JN.Data.Service;
using JN.Services.Tool;
using MvcCore.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PagedList;
using JN.Data.Extensions;

namespace JN.Web.Areas.AdminCenter.Controllers
{
    [ValidateInput(false)]
    public class SysParamController : BaseController
    {
        private readonly ISysParamService SysParamService;
        private readonly IUserVerifyService UserVerifyService;
        private readonly ISysSettingService SysSettingService;
        private readonly ILanguageService LanguageService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public SysParamController(ISysDBTool SysDBTool,
            ISysParamService SysParamService,
            IUserVerifyService UserVerifyService,
            ISysSettingService SysSettingService,
            ILanguageService LanguageService,
            IActLogService ActLogService)
        {
            this.SysParamService = SysParamService;
            this.UserVerifyService = UserVerifyService;
            this.SysSettingService = SysSettingService;
            this.LanguageService = LanguageService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
        }

        public ActionResult SysParam(int? pid)
        {
            //if (Session["CheckPwd"] == null)
            //{
            //    Session["Url"] = Request.Url.PathAndQuery;
            //    return Redirect("/AdminCenter/Home/CheckPassWord2");
            //}
            int parentid = pid ?? 1000;
            var list = SysParamService.List(x => x.PID == parentid && x.IsUse).OrderByDescending(x => x.Sort).ToList();
            ViewBag.CurrentPID = parentid;
            ViewBag.CurrentLock = SysParamService.SingleAndInit(parentid).IsLock;
            ViewBag.plst = SysParamService.List(x => x.PID == 0 && x.IsUse).OrderBy(x => x.ID).ToList();
            ViewBag.Title = "初始参数设置";
            ActMessage = ViewBag.Title;
            return View(list);
        }

        [HttpPost]
        public ActionResult SysParam(FormCollection fc)
        {
            ActMessage = "添加/修改系统参数";
            try
            {
                var entity = SysParamService.SingleAndInit(fc["ID"].ToInt());
                TryUpdateModel(entity, fc.AllKeys);
                if (string.IsNullOrEmpty(entity.Value))
                    return Json(new { result = "err", refMsg = "参数值为必填项" }, JsonRequestBehavior.AllowGet);

                if (entity.ID > 0)
                {
                    if (entity.ValueType == "scale" && (entity.Value.ToDouble() < 0 || entity.Value.ToDouble() > 1))
                        return Json(new { result = "err", refMsg = "参数值1为百分比，只能在0 ~ 1之间设定" }, JsonRequestBehavior.AllowGet);
                    if (entity.Value2Type == "scale" && (entity.Value2.ToDouble() < 0 || entity.Value2.ToDouble() > 1))
                        return Json(new { result = "err", refMsg = "参数值1为百分比，只能在0 ~ 1之间设定" }, JsonRequestBehavior.AllowGet);
                    if (entity.Value3Type == "scale" && (entity.Value3.ToDouble() < 0 || entity.Value3.ToDouble() > 1))
                        return Json(new { result = "err", refMsg = "参数值1为百分比，只能在0 ~ 1之间设定" }, JsonRequestBehavior.AllowGet);
                    SysParamService.Update(entity);
                }
                else
                {
                    entity.ValueType = "string";
                    entity.Value2Type = "string";
                    entity.Value3Type = "string";
                    entity.IsLock = false;
                    entity.IsUse = true;
                    entity.LastUpdateTime = DateTime.Now;
                    entity.ID = SysParamService.List(x => x.PID == entity.PID).Count() > 0 ? SysParamService.List(x => x.PID == entity.PID).Max(x => x.ID) + 1 : (entity.PID + 1);
                    SysParamService.Add(entity);
                }
                SysDBTool.Commit();
            }
            catch (Exception ex)
            {
                Services.Manager.logs.WriteErrorLog(HttpContext.Request.Url.ToString(), ex);
                return Json(new { result = "err", refMsg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Modify(int id, int? pid)
        {
            var model = SysParamService.SingleAndInit(id);
            if (model.ID == 0)
            {
                int thepid = pid ?? 1000;
                model.PID = thepid;
                model.Sort = SysParamService.List(x => x.PID == thepid).Max(x => x.Sort) + 1;
            }
            return Json(new { result = "ok", data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var model = SysParamService.Single(id);
            if (model != null)
            {
                if (SysParamService.Single(model.PID).IsLock)
                {
                    ViewBag.ErrorMsg = "系统参数不能删除！";
                    return View("Error");
                }
                else
                {
                    ActPacket = model;
                    SysParamService.Delete(id);
                    SysDBTool.Commit();
                    ViewBag.SuccessMsg = "“" + model.Name + "”已被删除！";
                    ActMessage = "删除" + model.Name + "参数";
                    return View("Success");
                }
            }
            ViewBag.ErrorMsg = "记录不存在或已被删除！";
            return View("Error");
        }

        public ActionResult SysSetting()
        {
            ActMessage = "系统运行设置";
            Data.SysSetting model = SysSettingService.Single(1);
            ViewBag.Title = "系统运行设置";
            return View(model);
        }

        [HttpPost]
        public ActionResult SysSetting(FormCollection fc)
        {
            var entity = SysSettingService.Single(1);
            TryUpdateModel(entity, fc.AllKeys);


            string isregisteropen = fc["isregisteropen"];
            string isopenup = fc["isopenup"];
            string RegistNeedSMSVerification = fc["RegistNeedSMSVerification"];
            string iswrongpwdlock = fc["iswrongpwdlock"];
            entity.IsOpenUp = isopenup.ToBool();
            entity.IsRegisterOpen = isregisteropen.ToBool();
            entity.IsWrongPwdLock = iswrongpwdlock.ToBool();
            entity.RegistNeedSMSVerification = RegistNeedSMSVerification.ToBool();
            SysSettingService.Update(entity);
            SysDBTool.Commit();

            string ModifyWebConfig = fc["ModifyWebConfig"];
            string AdminPath = fc["AdminPath"];
            string HaveHttps = fc["HaveHttps"];
            string HaveEncrypt = fc["HaveEncrypt"];

            if (ModifyWebConfig.ToBool())
            {
                if (HaveEncrypt.ToBool())
                {
                    string PublicKey = ConfigHelper.GetConfigString("EncryptKey");
                    ConfigHelper.UpdatetConfig("AdminPath", DESEncrypt.Encrypt(AdminPath, PublicKey));
                    string connstring = ConfigHelper.GetConfigString("ConnectionString");
                    ConfigHelper.UpdatetConfig("ConnectionString", DESEncrypt.Encrypt(connstring, PublicKey));
                    ConfigHelper.UpdatetConfig("HaveEncrypt", "True");
                }
                else
                {
                    string PublicKey = ConfigHelper.GetConfigString("EncryptKey");
                    ConfigHelper.UpdatetConfig("AdminPath", DESEncrypt.Encrypt(AdminPath, PublicKey));
                }
                ConfigHelper.UpdatetConfig("HaveHttps", HaveHttps);
            }

            ViewBag.SuccessMsg = "系统运行设置修改成功！";
            ActMessage = "修改运行设置";
            return View("Success");
        }

        public ActionResult UserVerify()
        {
            var list = UserVerifyService.List().ToList();
            ViewBag.Title = "会员平台各页面进入密码级别设置";
            ActMessage = ViewBag.Title;
            return View(list);
        }

        [HttpPost]
        public ActionResult UserVerify(FormCollection form)
        {
            //string id = form["theid"]);
            //string verifylevel = form["verifylevel"]);
            //string reamrk = form["reamrk"]);

            //TUserVerify model = userverifys.GetModel(TypeConverter.StrToInt(id));
            //model.VerifyLevel = TypeConverter.StrToInt(verifylevel);
            //model.Remark = reamrk;
            //model.LastUpdateTime = DateTime.Now;

            //if (userverifys.Update(model))
            //{
            //    ViewBag.SuccessMsg = "“" + model.Name + "”用户验证方式修改成功！";
            //    ActMessage = ViewBag.SuccessMsg;
            //    ActPacket = model;
            //    return RedirectToAction("UserVerify", "SysParam");
            //}
            ViewBag.ErrorMsg = "系统异常！请查看系统日志。";
            return View("Error");
        }

        public JsonResult ModifyVerify(int id)
        {
            Data.UserVerify model = UserVerifyService.Single(id);
            if (model != null)
            {
                return Json(new { result = "ok", data = model }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = "err", refMsg = "记录不存在或已被删除" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Language(string sv, int? pid, int? page)
        {
            int parentid = pid ?? 4001;
            ActMessage = "多语言设置";
            ViewBag.CurrentPID = parentid;
            var param = SysParamService.Single(parentid);
            if (param != null)
            {
                var list = LanguageService.List(x => x.LanguageName == param.Value2).WhereDynamic(FormatQueryString(HttpUtility.ParseQueryString(Request.Url.Query))).OrderByDescending(x => x.Location);
                if (!string.IsNullOrEmpty(sv))
                    return View(list.Where(x => string.IsNullOrEmpty(x.Value)).ToList().ToPagedList(page ?? 1, 20));
                return View(list.ToList().ToPagedList(page ?? 1, 15));
            }
            ViewBag.ErrorMsg = "无语言资源配置。";
            return View("Error");
        }

        [HttpPost]
        public ActionResult Language(FormCollection form)
        {
            string id = form["theid"];
            string name = form["paramname"];
            string value = form["paramvalue"];
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                ViewBag.ErrorMsg = "名称、值为必填项目。";
                return View("Error");
            }

            var model = LanguageService.Single(id.ToInt());
            if (model != null)
            {
                model.Value = value;
                LanguageService.Update(model);
                SysDBTool.Commit();
                return Redirect(Request.UrlReferrer.ToString());
            }
            ViewBag.ErrorMsg = "未知错误。";
            return View("Error");
        }

        public JsonResult ModifyLanguage(int id)
        {
            Data.Language model = LanguageService.Single(id);
            if (model != null)
            {
                return Json(new { result = "ok", data = model }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = "err", refMsg = "记录不存在或已被删除" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteLanguage(int id)
        {
            Data.Language model = LanguageService.Single(id);
            if (model != null)
            {
                LanguageService.Delete(id);
                SysDBTool.Commit();
                ViewBag.SuccessMsg = "“" + model.Name + "”已被删除！";
                ActMessage = ViewBag.SuccessMsg;
                return View("Success");
            }
            ViewBag.ErrorMsg = "记录不存在或已被删除！";
            return View("Error");
        }

        /// <summary>
        /// 生成语言XML
        /// </summary>
        /// <returns></returns>
        public ActionResult MakeLangXML()
        {
            ActMessage = "生成多语言资源包";
            var Path = System.Web.HttpContext.Current.Server.MapPath("/Language/");
            IOHelper.DeleteDirectory(Path);
            IOHelper.CreateDirectory(Path);
            var paramList = SysParamService.List(x => x.PID == 4000).ToList();
            foreach (Data.SysParam param in paramList)
            {
                XmlHelper.CreateXmlDocument(Path + "/" + param.Value2 + ".xml", "dicts", "utf-8");
                List<Data.Language> langList = LanguageService.List(x => x.LanguageName == param.Value2).ToList();
                foreach (Data.Language lang in langList)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("name", lang.Name);
                    ht.Add("value", lang.Value);
                    XmlHelper.InsertNode(Path + "/" + param.Value2 + ".xml", "dict", true, "dicts", ht, null);
                }
            }
            ViewBag.SuccessMsg = "成功创建语言包！";
            ActMessage = ViewBag.SuccessMsg;
            return View("Success");
        }
    }
}