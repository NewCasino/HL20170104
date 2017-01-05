using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JN.Data;
using JN.Data.Service;
using MvcCore.Controls;
using PagedList;
using JN.Services.Tool;
using System.IO;

namespace JN.Web.Areas.UserCenter.Controllers
{
    [ValidateInput(false)]
    public class MailController : BaseController
    {
        private readonly IUserService UserService;
        private readonly IMessageService MessageService;
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public MailController(ISysDBTool SysDBTool,
            IMessageService MessageService,
            IUserService UserService,
            IActLogService ActLogService)
        {
            this.UserService = UserService;
            this.MessageService = MessageService;
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
        }

        public ActionResult Inbox(string key, int? page)
        {
            ActMessage = "收件箱";
            var list = MessageService.List(x => x.UID == Umodel.ID && x.ToUID == Umodel.ID).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Sent(string key, int? page)
        {
            ActMessage = "发件箱";
            var list = MessageService.List(x => x.UID == Umodel.ID && x.FormUID == Umodel.ID && x.IsSendSuccessful).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Draft(string key, int? page)
        {
            ActMessage = "草稿箱";
            var list = MessageService.List(x => x.UID == Umodel.ID && x.FormUID == Umodel.ID && !x.IsSendSuccessful).OrderByDescending(x => x.ID).ToList();
            return View(list.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Write()
        {
            ViewBag.recipient = "Admin";
            if (!string.IsNullOrEmpty(Request["r"]))
            {
                var model = MessageService.Single(Request["r"].ToInt());
                if (model != null)
                {
                    ViewBag.recipient = model.FormUserName;
                    ViewBag.subject = "回复：" + model.Title;
                    ViewBag.content = "\n\n\n\n\n----------------------------原文---------------------------- \n" + model.Content;
                }
            }

            if (!string.IsNullOrEmpty(Request["f"]))
            {
                var model = MessageService.Single(Request["f"].ToInt());
                if (model != null)
                {
                    ViewBag.recipient = "";
                    ViewBag.subject = "转发：" + model.Title;
                    ViewBag.content = "\n\n\n\n\n----------------------------原文---------------------------- \n" + model.Content;
                }
            }

            ActMessage = "写邮件";
            return View();
        }

        [HttpPost]
        public ActionResult Write(FormCollection form)
        {
            string recipient = form["recipient"];
            string subject = form["subject"];
            string content = form["content"];
            string messagetype = form["messagetype"];
            string strErr = "";

            if (recipient.Trim().Length == 0 || subject.Trim().Length == 0)
            {
                strErr += "收件人、标题不能为空 <br />";
            }

            int toUID = 0;
            if (recipient == "Admin") recipient = "管理员";
            if (recipient.Trim() != "管理员")
            {
                if (UserService.List(x => x.UserName.Equals(recipient.Trim())).Count() <= 0)
                    strErr += "收件人不存在 <br />";
                else
                    toUID = UserService.Single(x => x.UserName.Equals(recipient.Trim())).ID;

            }

            if (strErr != "")
            {
                ViewBag.ErrorMsg = "抱赚您填写的信息有误： <br />" + strErr + "请核实后重新提交！";
                return Json(new { result = "error", msg = ViewBag.ErrorMsg });
            }


            //HttpPostedFileBase file = Request.Files["imgurl"];
            //string imgurl = "";
            //if (!string.IsNullOrEmpty(file.FileName))
            //{
            //    if (!FileValidation.IsAllowedExtension(file, new FileExtension[] { FileExtension.PNG, FileExtension.JPG, FileExtension.BMP }))
            //    {
            //        ViewBag.ErrorMsg = "非法上传，您只可以上传图片格式的文件！";
            //        return Json(new { result = "error", msg = ViewBag.ErrorMsg });
            //    }

            //    //20160711安全更新 ---------------- start
            //    var newfilename = "MAIL_" + Umodel.UserName + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName).ToLower();
            //    if (!Directory.Exists(Request.MapPath("~/upfile")))
            //        Directory.CreateDirectory(Request.MapPath("~/upfile"));
              
            //    if (Path.GetExtension(file.FileName).ToLower().Contains("aspx"))
            //    {
            //        var wlog = new Data.WarningLog();
            //        wlog.CreateTime = DateTime.Now;
            //        wlog.IP = Request.UserHostAddress;
            //        if (Request.UrlReferrer != null)
            //            wlog.Location = Request.UrlReferrer.ToString();
            //        wlog.Platform = "会员";
            //        wlog.WarningMsg = "试图上传木马文件";
            //        wlog.WarningLevel = "严重";
            //        wlog.ResultMsg = "拒绝";
            //        wlog.UserName = Umodel.UserName;
            //        MvcCore.Unity.Get<IWarningLogService>().Add(wlog);

            //        Umodel.IsLock = true;
            //        Umodel.LockTime = DateTime.Now;
            //        Umodel.LockReason = "试图上传木马文件";
            //        MvcCore.Unity.Get<IUserService>().Update(Umodel);
            //        MvcCore.Unity.Get<ISysDBTool>().Commit();
            //        throw new Exception("试图上传木马文件，您的帐号已被冻结");
            //    }
         
            //    var fileName = Path.Combine(Request.MapPath("~/upfile"), newfilename);
            //    try
            //    {
            //        file.SaveAs(fileName);
            //        var thumbnailfilename = UploadPic.MakeThumbnail(fileName, Request.MapPath("~/upfile/"), 1024, 768, "EQU");
            //        imgurl = "/upfile/" + thumbnailfilename;
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewBag.ErrorMsg="上传失败：" + ex.Message;
            //        return Json(new { result = "error", msg = ViewBag.ErrorMsg });
            //    }
            //    finally
            //    {
            //        System.IO.File.Delete(fileName); //删除原文件
            //    }
            //    //20160711安全更新  --------------- end
            //}

            var model = new Data.Message();
            model.Attachment = "";
            model.MessageType = messagetype;
            model.Content = content;
            model.CreateTime = DateTime.Now;
            model.FormUID = Umodel.ID;
            model.FormUserName = Umodel.UserName;
            model.ForwardID = 0;
            model.IsFlag = false;
            model.IsForward = false;
            model.IsRead = false;
            model.IsReply = false;
            model.IsSendSuccessful = true;
            model.IsStar = false;
            model.ReplyID = 0;
            model.Title = subject;
            model.ToUID = toUID;
            model.ToUserName = recipient.Trim();
            model.UID = Umodel.ID;

            var model2 = new Data.Message();
            model2.Attachment = model.Attachment;
            model2.MessageType = model.MessageType;
            model2.Content = model.Content;
            model2.CreateTime = model.CreateTime;
            model2.FormUID = model.FormUID;
            model2.FormUserName = model.FormUserName;
            model2.ForwardID = model.ForwardID;
            model2.IsFlag = model.IsFlag;
            model2.IsForward = model.IsForward;
            model2.IsRead = model.IsRead;
            model2.IsReply = model.IsReply;
            model2.IsSendSuccessful = model.IsSendSuccessful;
            model2.IsStar = model.IsStar;
            model2.ReplyID = model.ReplyID;
            model2.Title = model.Title;
            model2.ToUID = model.ToUID;
            model2.ToUserName = model.ToUserName;
            model2.UID = model.ToUID;

            MessageService.Add(model);
            MessageService.Add(model2);
            SysDBTool.Commit();

            if (model.ID > 0 && model2.ID > 0)
            {
                model.RelationID = model2.ID;
                MessageService.Update(model);
                SysDBTool.Commit();
                model2.RelationID = model.ID;
                MessageService.Update(model2);
                SysDBTool.Commit();
                ViewBag.SuccessMsg = "邮件发送成功！";
                ActMessage = ViewBag.SuccessMsg;
                ActPacket = model;
                return Json(new { result = "ok", msg = ViewBag.ErrorMsg });
            }

            ViewBag.ErrorMsg = "系统异常！请查看系统日志。";
            return Json(new { result = "error", msg = ViewBag.ErrorMsg });
        }

        //加载邮件内容视图
        public ActionResult SubMailContent(int id)
        {
            var model = MessageService.Single(id);
            if (model == null)
            {
                ViewBag.ErrorMsg = "记录不存在或已被删除！";
                return View("Error");
            }
            model.IsRead = true;
            MessageService.Update(model);
            SysDBTool.Commit();
            ActMessage = "查看邮件";
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = MessageService.Single(id);
            if (model != null)
            {
                ActPacket = model;
                MessageService.Delete(id);
                SysDBTool.Commit();
                return RedirectToAction("inbox", "mail");
            }
            ViewBag.ErrorMsg = "记录不存在或已被删除！";
            return View("Error");
        }

        public ActionResult doCommand(string ids, string commandtype)
        {
            Dictionary<string, string> dicCommand = new Dictionary<string, string>();
            if (commandtype.ToLower() == "makeread")
                dicCommand.Add("IsRead", "1");
            else if (commandtype.ToLower() == "makeunread")
                dicCommand.Add("IsRead", "0");
            else if (commandtype.ToLower() == "makeflag")
                dicCommand.Add("IsFlag", "1");
            else if (commandtype.ToLower() == "makeunflag")
                dicCommand.Add("IsFlag", "0");
            MessageService.Update(new Data.Message(), dicCommand, "id in (" + ids.TrimEnd(',').TrimStart(',') + ")");

            SysDBTool.Commit();
            return Content("ok");
        }


        public ActionResult MultiDelete(string ids)
        {
            MessageService.Delete(a => ids.TrimEnd(',').TrimStart(',').Contains(a.ID.ToString()));
            SysDBTool.Commit();
            return Content("ok");
        }
    }
}
