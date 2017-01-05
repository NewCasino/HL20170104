using JN.Data.Enum;
using JN.Data.Service;
using JN.Services.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Linq;
namespace JN.Services.Manager
{
    public partial class USD
    {
        private static List<Data.SysParam> cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().List(x => x.ID < 4000).ToList();
        /// <summary>
        /// 对超时未付款的用户进行冻结处理 
        /// </summary>
        /// <returns></returns>
        public static void OvertimeNotPay()
        {
            int overtimes = cacheSysParam.SingleAndInit(b => b.ID == 2202).Value.ToInt();
            var list = MvcCore.Unity.Get<IUSDPurchaseService>().List(x => x.Status == 1 && SqlFunctions.DateDiff("Minute", x.CreateTime, DateTime.Now) > overtimes).OrderByDescending(x => x.ID).ToList();
            foreach (var item in list)
            {
                var model = MvcCore.Unity.Get<IUSDPurchaseService>().Single(item.ID);
                model.Status = -1;
                MvcCore.Unity.Get<IUSDPurchaseService>().Update(model);
                MvcCore.Unity.Get<ISysDBTool>().Commit();

                var updatePutOn = MvcCore.Unity.Get<IUSDPutOnService>().Single(model.PutOnID);
                if (updatePutOn != null)
                {
                    updatePutOn.Status = 1;
                    MvcCore.Unity.Get<IUSDPutOnService>().Update(updatePutOn);
                    MvcCore.Unity.Get<ISysDBTool>().Commit();
                }

                var newSeek = MvcCore.Unity.Get<IUSDSeekService>().Single(model.SeekID ?? 0);
                if (newSeek != null)
                {
                    newSeek.Status = (int)Data.Enum.USDStatus.Cancel;
                    MvcCore.Unity.Get<IUSDSeekService>().Update(newSeek);
                    MvcCore.Unity.Get<ISysDBTool>().Commit();
                }

                var onUser = MvcCore.Unity.Get<IUserService>().Single(item.UID);
                if (onUser != null)
                {
                    onUser.IsLock = true;
                    onUser.LockReason = "EP交易超时未付款";
                    MvcCore.Unity.Get<IUserService>().Update(onUser);
                    MvcCore.Unity.Get<ISysDBTool>().Commit();
                }

            }
        }

        public static void OvertimeNotConfirmed()
        {
            int overtimes = cacheSysParam.SingleAndInit(b => b.ID == 2203).Value.ToInt();
            var list = MvcCore.Unity.Get<IUSDPurchaseService>().List(x => x.Status == 2 && SqlFunctions.DateDiff("Minute", (x.PayTime ?? DateTime.Now), DateTime.Now) > overtimes).OrderByDescending(x => x.ID).ToList();
            foreach (var item in list)
            {
                var model = MvcCore.Unity.Get<IUSDPurchaseService>().Single(item.ID);
                model.Status = 3;
                MvcCore.Unity.Get<IUSDPurchaseService>().Update(model);
                MvcCore.Unity.Get<ISysDBTool>().Commit();

                decimal ChangeMoney = model.BuyAmount;
                string description = "EP交易入帐";
                var onUser = MvcCore.Unity.Get<IUserService>().Single(model.UID);
                if (onUser != null)
                    Wallets.changeWallet(onUser.ID, model.BuyAmount, 2002, description);

                var updatePutOn = MvcCore.Unity.Get<IUSDPutOnService>().Single(model.PutOnID);
                if (updatePutOn != null)
                {
                    updatePutOn.Status = 3;
                    MvcCore.Unity.Get<IUSDPutOnService>().Update(updatePutOn);
                    MvcCore.Unity.Get<ISysDBTool>().Commit();
                }

                var sellUser = MvcCore.Unity.Get<IUserService>().Single(item.SellUID);
                if (sellUser != null)
                {
                    sellUser.IsLock = true;
                    sellUser.LockReason = "EP交易超时未确认";
                    MvcCore.Unity.Get<IUserService>().Update(onUser);
                    MvcCore.Unity.Get<ISysDBTool>().Commit();
                }

                var newSeek = MvcCore.Unity.Get<IUSDSeekService>().Single(model.SeekID ?? 0);
                if (newSeek != null)
                {
                    if (newSeek.SeekMoney == (newSeek.HaveDeal ?? 0))
                    {
                        newSeek.Status = (int)Data.Enum.USDStatus.Deal;
                        MvcCore.Unity.Get<IUSDSeekService>().Update(newSeek);
                        MvcCore.Unity.Get<ISysDBTool>().Commit();
                    }
                }

                //用户升级
                Users.UpdateLevel(onUser.ID);
            }
        }
    }
}