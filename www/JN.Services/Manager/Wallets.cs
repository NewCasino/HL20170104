using System;
using System.Collections.Generic;
using System.Linq;
namespace JN.Services.Manager
{
    /// <summary>
    ///钱包管理
    /// </summary>
    public partial class Wallets
    {
        private static List<Data.SysParam> cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().List(x => x.ID < 4000).ToList();

        #region 会员提现
        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="onUser">操作的用户</param>
        public static void doTakeCash(Data.User onUser, decimal drawmoney, Data.SysParam takeCoin, string remark)
        {
            decimal WalletMoney = 0;
            decimal PARAM_POUNDAGEBL = 0;
            if (takeCoin.ID == 2001)
            {
                WalletMoney = onUser.Wallet2001;
                PARAM_POUNDAGEBL = cacheSysParam.SingleAndInit(x => x.ID == 1903).Value2.ToDecimal(); //提现手续费
            }
            if (takeCoin.ID == 2003)
            {
                WalletMoney = onUser.Wallet2001;
                PARAM_POUNDAGEBL = cacheSysParam.SingleAndInit(x => x.ID == 1904).Value2.ToDecimal(); //提现手续费
            }
            if (takeCoin.ID == 2006)
            {
                WalletMoney = onUser.Wallet2001;
                PARAM_POUNDAGEBL = cacheSysParam.SingleAndInit(x => x.ID == 1905).Value2.ToDecimal(); //提现手续费
            }

            //系统参数



            decimal actualChangeMoney = drawmoney * (1 - PARAM_POUNDAGEBL);
            decimal poundage = drawmoney * PARAM_POUNDAGEBL;
            string description = "提取现金";
            if (PARAM_POUNDAGEBL > 0) description += "(手续费:" + poundage + ")";

            //写入提现表
            MvcCore.Unity.Get<JN.Data.Service.ITakeCashService>().Add(new Data.TakeCash
            {
                ActualDrawMoney = actualChangeMoney,
                Balance = WalletMoney - drawmoney,
                BankCard = onUser.BankCard,
                BankName = onUser.BankName,
                BankOfDeposit = onUser.BankOfDeposit,
                BankUser = onUser.BankUser,
                Alipay = onUser.AliPay,
                CreateTime = DateTime.Now,
                DrawMoney = drawmoney,
                Status = (int)JN.Data.Enum.TakeCaseStatus.Wait,
                Poundage = Convert.ToDecimal(poundage),
                Remark = remark,
                UID = onUser.ID,
                UserName = takeCoin.ID.ToString()
            });
            MvcCore.Unity.Get<Data.Service.ISysDBTool>().Commit();
            //写入资金明细表
            changeWallet(onUser.ID, 0 - drawmoney, takeCoin.ID, description);
        }
        #endregion

        #region 会员转帐
        /// <summary>
        /// 转帐
        /// </summary>
        public static void doTransfer(Data.User onUser, Data.User recUser, decimal transfermoney, int transCoin, string remark)
        {
            //系统参数
            decimal PARAM_POUNDAGEBL = 0;// cacheSysParam.SingleAndInit(x => x.ID == 1901).Value2.ToDecimal(); //转帐手续费
            decimal actualChangeMoney = transfermoney;// * (1 - PARAM_POUNDAGEBL);
            decimal poundage = 0;// transfermoney * PARAM_POUNDAGEBL;
            string description = "会员转帐（转给【" + recUser.UserName + "】";

            //写入转帐表
            MvcCore.Unity.Get<JN.Data.Service.ITransferService>().Add(new Data.Transfer
            {
                ActualTransferMoney = actualChangeMoney,
                CreateTime = DateTime.Now,
                Poundage = poundage,
                Remark = remark,
                ToUID = recUser.ID,
                ToUserName = recUser.UserName,
                TransferMoney = transfermoney,
                UID = onUser.ID,
                UserName = onUser.UserName
            });
            MvcCore.Unity.Get<Data.Service.ISysDBTool>().Commit();

            //写入资金明细表（转帐方）
            changeWallet(onUser.ID, 0 - transfermoney, transCoin, description);

            //写入资金明细表（接收方）
            description = "会员转帐（从【" + onUser.UserName + "】转入";
            changeWallet(recUser.ID, actualChangeMoney, transCoin, description);


        }
        #endregion

        #region 币种操作
        /// <summary>
        /// 直接对币种进行操作
        /// </summary>
        /// <param name="onUser">操作对像</param>
        /// <param name="changeMoney">变更金额</param>
        /// <param name="changeCoin">变更币种</param>
        /// <param name="description">备注</param>
        public static void changeWallet(int onUserID, decimal changeMoney, int coinid, string description)
        {
            Data.User onUser = MvcCore.Unity.Get<Data.Service.IUserService>().Single(onUserID);
            decimal changeWallet = 0;
            switch (coinid)
            {
                case 2001:
                    changeWallet = onUser.Wallet2001;
                    onUser.Wallet2001 = onUser.Wallet2001 + changeMoney;
                    break;
                //case 2002:
                //    changeWallet = onUser.Wallet2002;
                //    onUser.Wallet2002 = onUser.Wallet2002 + changeMoney;
                //    break;
                //case 2003:
                //    changeWallet = onUser.Wallet2003;
                //    onUser.Wallet2003 = onUser.Wallet2003 + changeMoney;
                //    break;
                case 2004:
                    changeWallet = onUser.Wallet2004;
                    onUser.Wallet2004 = onUser.Wallet2004 + changeMoney;
                    //if (description.Contains("")) onUser.Addup1107 = (onUser.Addup1107 ?? 0) + (0 - changeMoney);
                    //if (description.Contains("解冻")) onUser.Addup1106 = (onUser.Addup1106 ?? 0) + changeMoney;
                    break;
                //case 2005:
                //    changeWallet = onUser.Wallet2005;
                //    onUser.ReserveDecamal1 = (onUser.ReserveDecamal1 ?? 0) + changeMoney;
                //    onUser.Wallet2005 = onUser.Wallet2005 + changeMoney;
                //    break;
                //case 2006:
                //    changeWallet = onUser.Wallet2006;
                //    onUser.Wallet2006 = onUser.Wallet2006 + changeMoney;
                //    break;
            }

            //写入明细
            MvcCore.Unity.Get<JN.Data.Service.IWalletLogService>().Add(new Data.WalletLog
            {
                ChangeMoney = changeMoney,
                Balance = changeWallet + changeMoney,
                CoinID = coinid,
                CoinName = cacheSysParam.Single(x => x.ID == coinid).Value,
                CreateTime = DateTime.Now,
                Description = description,
                UID = onUser.ID,
                UserName = onUser.UserName
            });
            //更新用户钱包
            MvcCore.Unity.Get<JN.Data.Service.IUserService>().Update(onUser);
            MvcCore.Unity.Get<Data.Service.ISysDBTool>().Commit();
        }
        #endregion

        #region 币种互兑
        /// <summary>
        /// 互兑
        /// </summary>
        public static void doExchange(int onUserID, decimal drawmoney, int fromCoin, int toCoin)
        {
            Data.User onUser = MvcCore.Unity.Get<Data.Service.IUserService>().Single(onUserID);
            ////币种
            decimal PARAM_POUNDAGEBL = cacheSysParam.Single(x => x.ID == 1903).Value2.ToDecimal(); //手续费

            decimal actualChangeMoney = drawmoney * (1 - PARAM_POUNDAGEBL);
            decimal poundage = drawmoney * PARAM_POUNDAGEBL;
            string description = "兑换";
            Wallets.changeWallet(onUser.ID, 0 - actualChangeMoney, fromCoin, description);
            Wallets.changeWallet(onUser.ID, actualChangeMoney, toCoin, description);

            //卖出拆分股得到收益(拆分盘卖出时10%作为手续费，63%可以提现(美金)，27%必须回购)
            //decimal PARAM_SELLPOUNDAGE = cacheSysParam.SingleAndInit(x => x.ID == 1903).Value2.ToDecimal();
            //decimal currentprice = CFB.getcurrentprice();
            //decimal actualdrawmoney = drawmoney * (1 - PARAM_SELLPOUNDAGE);

            //Wallets.changeWallet(onUser.ID, 0 - drawmoney, fromCoin, cacheSysParam.Single(x => x.ID == toCoin).Name + "兑换" + cacheSysParam.Single(x => x.ID == toCoin).Name);
            //Wallets.changeWallet(onUserID, actualdrawmoney * (1 - cacheSysParam.SingleAndInit(x => x.ID == 1903).Value3.ToDecimal()), 2002, cacheSysParam.Single(x => x.ID == toCoin).Name + "兑换" + cacheSysParam.Single(x => x.ID == toCoin).Name+",手续费" + PARAM_SELLPOUNDAGE);
            ////回购（直接拔币）
            //decimal hgje = actualdrawmoney * cacheSysParam.SingleAndInit(x => x.ID == 1903).Value3.ToDecimal();
            ////int number = (int)(hgje / currentprice);
            //CFB.PreOrder(hgje, onUserID, "JP", "JP兑换EP时回购");

        }
        #endregion

    }
}