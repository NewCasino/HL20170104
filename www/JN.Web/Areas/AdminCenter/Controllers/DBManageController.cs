using JN.Data;
using JN.Data.Service;
using JN.Services.Tool;
using MvcCore.Controls;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace JN.Web.Areas.AdminCenter.Controllers
{
    public class DBManageController : BaseController
    {
        private readonly ISysDBTool SysDBTool;
        private readonly IActLogService ActLogService;

        public DBManageController(ISysDBTool SysDBTool, IActLogService ActLogService)
        {
            this.SysDBTool = SysDBTool;
            this.ActLogService = ActLogService;
        }

        public ActionResult Index(int? page)
        {
            //if (Session["CheckPwd"] == null)
            //{
            //    Session["Url"] = Request.Url.PathAndQuery;
            //    return Redirect("/AdminCenter/Home/CheckPassWord2");
            //}
            ViewBag.Title = "数据库备份与恢复";
            ActMessage = ViewBag.Title;

            int pageIndex = page ?? 1;
            IOHelper.CreateDirectory(Server.MapPath("/DBBackUp/"));

            List<FileInfo> files = IOHelper.GetAllFilesInDirectory(Server.MapPath("/DBBackUp/"));
            List<Data.Extensions.DbBackFile> lst = new List<Data.Extensions.DbBackFile>();
            foreach (FileInfo info in files)
            {
                lst.Add(new Data.Extensions.DbBackFile { BackFileName = info.Name, BackFileFullName = info.FullName, BackFileSize = info.Length > 0 ? info.Length / 1024 : 0, CreateTime = info.LastWriteTime });
            }

            //Linq进行排序
            IEnumerable<Data.Extensions.DbBackFile> query = from items in lst orderby items.CreateTime descending select items;
            return View(query.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Backup()
        {
            ActMessage = "备份数据库";
            IOHelper.CreateDirectory(Server.MapPath("/DBBackUp/"));
            SqlParameter[] para_sys = new SqlParameter[]
            {
            new SqlParameter ("@BKPATH", Server.MapPath("/DBBackUp/") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak" ),
            };

            int count = MvcCore.Unity.Get<ISysDBTool>().Execute<object>("backup database " + ConfigHelper.GetConfigString("DBName") + " to disk=@BKPATH", para_sys).Count();

            if (count >= 0)
            {
                ViewBag.SuccessMsg = "数据库备份成功！";
                return View("Success");
            }
            ViewBag.ErrorMsg = "数据库备份失败！请查看系统日志。";
            return View("Error");
        }

        public ActionResult Restore(string backfilename)
        {
            ActMessage = "恢复数据库";
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter ("@BKFILE", Server.MapPath("/DBBackUp/") + backfilename),
            };
            int count = MvcCore.Unity.Get<ISysDBTool>().Execute<object>("ALTER DATABASE [" + ConfigHelper.GetConfigString("DBName") + "] SET OFFLINE WITH ROLLBACK IMMEDIATE; restore database " + ConfigHelper.GetConfigString("DBName") + " from disk=@BKFILE WITH REPLACE;ALTER DATABASE [" + ConfigHelper.GetConfigString("DBName") + "] SET ONLINE", para).Count();

            if (count >= 0)
            {
                ViewBag.SuccessMsg = "数据库恢复成功！";
                return View("Success");
            }
            ViewBag.ErrorMsg = "数据库恢复失败！请查看系统日志。";
            return View("Error");
        }

        public ActionResult DBClear()
        {
            ActMessage = "清空数据库";
            ////清空前先备份
            //IOHelper.CreateDirectory(Server.MapPath("/DBBackUp/"));
            SqlParameter[] para_sys = new SqlParameter[]
            {
            new SqlParameter ("@BKPATH", Server.MapPath("/DBBackUp/") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak" ),
            };

            int count = MvcCore.Unity.Get<ISysDBTool>().Execute<object>("backup database " + ConfigHelper.GetConfigString("DBName") + " to disk=@BKPATH", para_sys).Count();

            DbParameter[] param = new DbParameter[] { };
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [WalletLog]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [ActLog]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [StockLog]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [SysLog]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [SMSLog]", param);

            SysDBTool.ExecuteSQL("TRUNCATE TABLE [Settlement]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [BonusDetail]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [StockEntrustsTrade]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [StockSubscribe]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [StockIssue]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [StockTrade]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [StockChartDay]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [StockChartHour]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [Message]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [Notice]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [TakeCash]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [Transfer]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [User]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [Remittance]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [AccountRelation]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [ShopInfo]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [ShopOrder]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [ShopOrderDetail]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [ShopProduct]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [CFBChart]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [CFBPreOrder]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [CFBIssue]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [CFBSplitting]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [CFBSubscribe]", param);
            //SysDBTool.ExecuteSQL("TRUNCATE TABLE [CFBTrade]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [USDPurchase]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [USDPutOn]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [USDSeek]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [PINCode]", param);
            SysDBTool.ExecuteSQL("TRUNCATE TABLE [AdminAuthority]", param);

            SysDBTool.ExecuteSQL("TRUNCATE TABLE [Investment]", param);

            ViewBag.SuccessMsg = "除管理员表及系统参数设置外全部数据已全部清空！清空数据后会员平台必须重新登录。";

            return View("Success");
        }
    }
}
