using JN.Data.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace JN.Services.Manager
{
    /// <summary>
    ///奖金结算
    /// </summary>
    public partial class Bonus
    {
        private static List<Data.SysParam> cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().List(x => x.ID < 4000).ToList();

        #region 写入奖金明细表并更新用户钱包
        /// <summary>
        /// 写入资金明细表并更新用户钱包（未结算只写入奖金表不进钱包及资金明细）
        /// </summary>
        /// <param name="bonusmoney">奖金金额</param>
        /// <param name="period">期数（发放利息时）</param>
        /// <param name="bonusid">奖金ID（对应参数表SysParam的ID）</param>
        /// <param name="bonusname">奖金名称（对应参数表SysParam的Name）</param>
        /// <param name="bonusdesc">获奖描述来源</param>
        /// <param name="onUserID">会员ID</param>
        /// <param name="addupfield">累计奖金字段（对应用户表User的Addup1101-1107/1802）</param>
        /// <param name="isbalance">是否结算,Ture时写入钱包明细表WalletDetails及更新User表中用户钱包余额Wallet2001-2005，Falsh时只记入奖金明细表BonusDetails</param>
        public static void UpdateUserWallet(decimal bonusmoney, int bonusid, string bonusname, string bonusdesc, int onUserID, string addupfield, bool isbalance)
        {
            var onUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(onUserID);
            if (bonusmoney > 0)
            {
                //是否马上结算
                if (isbalance)
                {
                    switch (bonusid)
                    {
                        case 1103:
                            Wallets.changeWallet(onUser.ID, bonusmoney, 2001, bonusdesc);
                            break;
                        default:
                            Wallets.changeWallet(onUser.ID, bonusmoney, 2001, bonusdesc);
                            break;
                    }
                }
                //写入奖金表
                MvcCore.Unity.Get<Data.Service.IBonusDetailService>().Add(new Data.BonusDetail
                {
                    Period = 0,
                    BalanceTime = DateTime.Now,
                    BonusMoney = bonusmoney,
                    BonusID = bonusid,
                    BonusName = bonusname,
                    CreateTime = DateTime.Now,
                    Description = bonusdesc,
                    BonusMoneyCF = 0,
                    BonusMoneyCFBA = 0,
                    IsBalance = isbalance,
                    UID = onUser.ID,
                    UserName = onUser.UserName
                });
                MvcCore.Unity.Get<Data.Service.ISysDBTool>().Commit();
            }
        }
        #endregion

        #region 激活会员时产生动态奖（报单中心津贴、直推、对碰、绩差）
        /// <summary>
        /// 计算奖金及津贴（代码层实现）
        /// </summary>
        /// <param name="onUser">当前激活的用户</param>
        public static void CalculateBonus(int onUserID)
        {
            decimal bonusMoney = 0;  //奖金
            string bonusDesc = ""; //得奖描述
            var onUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(onUserID);

            //var onUser1 = MvcCore.Unity.Get<Data.Service.IUserService>().Single();

            Data.SysParam param = null;

            #region 报单中心拿津贴
            //报单中心拿津贴
            var agUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(onUser.AgentID);
            if (agUser != null)
            {
                param = cacheSysParam.SingleAndInit(x => x.ID == 1108);
                decimal PARAM_JTBL = param.Value.ToDecimal();
                bonusMoney = onUser.Investment * PARAM_JTBL;
                bonusDesc = "来自会员【" + onUser.UserName + "】的" + param.Name + "(" + onUser.Investment + "×" + PARAM_JTBL + ")";
                UpdateUserWallet(bonusMoney, param.ID, param.Name, bonusDesc, agUser.ID, "Addup1108", true);
            }
            #endregion

            #region 直推
            var refereeUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(onUser.ParentID);
            if (refereeUser != null)
            {
                param = cacheSysParam.SingleAndInit(x => x.ID == 1103);
                decimal PARAM_JJJBL = param.Value.ToDecimal(); //直荐金比例
                bonusMoney = onUser.Investment * PARAM_JJJBL;
                bonusDesc = "来自会员【" + onUser.UserName + "】的" + param.Name + "(" + onUser.Investment + "×" + PARAM_JJJBL + ")";


                refereeUser.Addup1103 = (refereeUser.Addup1103 ?? 0) + bonusMoney;
                refereeUser.AddStr1103 = (refereeUser.AddStr1103 ?? "") + "—添加" + bonusMoney.ToString("F2") + "_" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                MvcCore.Unity.Get<JN.Data.Service.IUserService>().Update(refereeUser);
                MvcCore.Unity.Get<JN.Data.Service.ISysDBTool>().Commit();


                // public static void changeWallet(int onUserID, decimal changeMoney, int coinid, string description)
                UpdateUserWallet(bonusMoney, param.ID, param.Name, bonusDesc, refereeUser.ID, "Addup1103", true);
            }
            #endregion

            Bonus1104(onUser);
        }
        #endregion

        #region 对碰平衡奖
        public static void Bonus1104(Data.User onUser)
        {
            decimal bonusMoney = 0;  //奖金
            string bonusDesc = ""; //得奖描述
            Data.SysParam param = null;
            if (!string.IsNullOrEmpty(onUser.ParentPath))
            {
                param = cacheSysParam.SingleAndInit(x => x.ID == 1104);//对碰奖（平衡奖）

                var dpUser=MvcCore.Unity.Get<Data.Service.IUserService>().Single(onUser.ParentID);


                //string[] ids_dp = onUser.ParentPath.Split(',');//分割字符串
            //    var lst_DPUser = MvcCore.Unity.Get<Data.Service.IUserService>().List(x => ids_dp.Contains(x.ID.ToString())).OrderBy(x => x.Depth).ThenBy(x => x.ChildPlace).ToList();

            //    foreach (var dpUser in lst_DPUser)
            //    {
                    decimal PARAM_DPJBL = 0; //对碰奖比例
                    decimal PARAM_DPJRFD = 0; //对碰奖日封顶为投资额

                    if (dpUser.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1401).Value.ToDecimal())
                    {
                        PARAM_DPJBL = cacheSysParam.SingleAndInit(x => x.ID == 1401).Value2.ToDecimal();
                        PARAM_DPJRFD = cacheSysParam.SingleAndInit(x => x.ID == 1401).Value3.ToDecimal();
                    }
                    else if (dpUser.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1402).Value.ToDecimal())
                    {
                        PARAM_DPJBL = cacheSysParam.SingleAndInit(x => x.ID == 1402).Value2.ToDecimal();
                        PARAM_DPJRFD = cacheSysParam.SingleAndInit(x => x.ID == 1402).Value3.ToDecimal();
                    }
                    else if (dpUser.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1403).Value.ToDecimal())
                    {
                        PARAM_DPJBL = cacheSysParam.SingleAndInit(x => x.ID == 1403).Value2.ToDecimal();
                        PARAM_DPJRFD = cacheSysParam.SingleAndInit(x => x.ID == 1403).Value3.ToDecimal();
                    }
                    else if (dpUser.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1404).Value.ToDecimal())
                    {
                        PARAM_DPJBL = cacheSysParam.SingleAndInit(x => x.ID == 1404).Value2.ToDecimal();
                        PARAM_DPJRFD = cacheSysParam.SingleAndInit(x => x.ID == 1404).Value3.ToDecimal();
                    }
                    else if (dpUser.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1405).Value.ToDecimal())
                    {
                        PARAM_DPJBL = cacheSysParam.SingleAndInit(x => x.ID == 1405).Value2.ToDecimal();
                        PARAM_DPJRFD = cacheSysParam.SingleAndInit(x => x.ID == 1405).Value3.ToDecimal();
                    }

                    var leftchild = MvcCore.Unity.Get<Data.Service.IUserService>().List().Where(x => x.ID == dpUser.LeftChild).First();
                    var rightchild = MvcCore.Unity.Get<Data.Service.IUserService>().List().Where(x => x.ID == dpUser.RightChild).First();
                    
                    //只有左右孩子满时才可以产生对碰奖
                //  foreach(){}
                    if(rightchild!=null&&leftchild!=null) {
                    if (rightchild.Investment > leftchild.Investment)
                    {
                        bonusMoney = leftchild.Investment * PARAM_DPJBL;
                    }
                    else {
                        bonusMoney = rightchild.Investment * PARAM_DPJBL;
                    }
                    }

                    //decimal minDPYJ = 
                        //Math.Min(dpUser.LeftDpMargin ?? 0, dpUser.RightDpMargin ?? 0);
                   // bonusMoney = minDPYJ * PARAM_DPJBL;
                    if (bonusMoney > 0)
                    {
                        //本日已得对碰奖   SqlFunctions.DateDiff("DAY", x.CreateTime, DateTime.Now) 比较时间
                        var tlist = MvcCore.Unity.Get<Data.Service.IBonusDetailService>().List(x => x.BonusID == 1104 && SqlFunctions.DateDiff("DAY", x.CreateTime, DateTime.Now) == 0 && x.UID == dpUser.ID).ToList();
                        decimal todayDPJ = tlist.Count() > 0 ? tlist.Sum(x => x.BonusMoney) : 0;
                        if (todayDPJ < (decimal)PARAM_DPJRFD)
                        {
                            //如果超出封顶金额，刚取差值
                            if ((todayDPJ + bonusMoney) > PARAM_DPJRFD)
                            {
                                bonusMoney = PARAM_DPJRFD - todayDPJ;
                                bonusDesc = "来自会员【" + onUser.UserName + "】的" + param.Name + "(" +""+ "×" + PARAM_DPJBL + "，左区" + dpUser.LeftDpMargin + ",右区" + dpUser.RightDpMargin + ")达到日封顶，取差额：" + bonusMoney;
                            }
                            else
                                bonusDesc = "来自会员【" + onUser.UserName + "】的" + param.Name + "(" +""+ "×" + PARAM_DPJBL + "，左区" + dpUser.LeftDpMargin + ",右区" + dpUser.RightDpMargin + ")";

                            Data.User upDPUser = MvcCore.Unity.Get<Data.Service.IUserService>().Single(dpUser.ID);
                            upDPUser.Addup1104 += bonusMoney;
                            //upDPUser.LeftDpMargin = (upDPUser.LeftDpMargin ?? 0) - minDPYJ;
                            //upDPUser.RightDpMargin = (upDPUser.RightDpMargin ?? 0) - minDPYJ;
                            MvcCore.Unity.Get<Data.Service.IUserService>().Update(upDPUser);
                            MvcCore.Unity.Get<Data.Service.ISysDBTool>().Commit();
                            UpdateUserWallet(bonusMoney, param.ID, param.Name, bonusDesc, dpUser.ID, "Addup1104", true);

                            //培育奖
                          //  var ParentModel=MvcCore.Unity.Get<Data.Service.IUserService>().Single(dpUser.ParentID);
                          //  Bonus1105(dpUser, bonusMoney);//dpuser 对碰奖用户model
                        }
                    }
                }
            
        }
        #endregion

        /// <summary>
        /// 领导奖
        /// </summary>
        //public static void lingdaojiang( Data.User onuser,decimal bonusMoney) {
           
        //    if(onuser.ParentID!=0){
              

            
        //    }
                
        //}


        #region 领导奖
        /// <summary>
        /// 领导奖
        /// </summary>
        public static void Bonus1105(Data.User onUser, decimal bonusMoney)
        {
            if (!string.IsNullOrEmpty(onUser.RefereePath))
            {
                var ids = onUser.RefereePath.Split(',');
                var param = cacheSysParam.SingleAndInit(x => x.ID == 1105);
                int startdepth = onUser.RefereeDepth - 6;
                var rhlist = MvcCore.Unity.Get<Data.Service.IUserService>().List(x => ids.Contains(x.ID.ToString()) && x.RefereeDepth >= startdepth).OrderByDescending(x => x.RefereeDepth).ToList();
                foreach (var item in rhlist)
                {
                    decimal PARAM_PHJBL = 0; //领导奖比例
                    if (item.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1501).Value.ToDecimal())
                    {
                        switch (onUser.RefereeDepth - item.RefereeDepth)
                        {
                            case 1: //1代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1501).Value2.Split('|')[0].ToDecimal();
                                break;
                            case 2: //2代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1501).Value2.Split('|')[1].ToDecimal();
                                break;
                        }
                    }
                    else if (item.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1502).Value.ToDecimal())
                    {
                        switch (onUser.RefereeDepth - item.RefereeDepth)
                        {
                            case 1: //1代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1502).Value2.Split('|')[0].ToDecimal();
                                break;
                            case 2: //2代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1502).Value2.Split('|')[1].ToDecimal();
                                break;
                            case 3: //3代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1502).Value2.Split('|')[2].ToDecimal();
                                break;
                        }
                    }
                    else if (item.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1503).Value.ToDecimal())
                    {
                        switch (onUser.RefereeDepth - item.RefereeDepth)
                        {
                            case 1: //1代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1503).Value2.Split('|')[0].ToDecimal();
                                break;
                            case 2: //2代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1503).Value2.Split('|')[1].ToDecimal();
                                break;
                            case 3: //3代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1503).Value2.Split('|')[2].ToDecimal();
                                break;
                            case 4: //4代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1503).Value2.Split('|')[3].ToDecimal();
                                break;
                        }
                    }
                    else if (item.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1504).Value.ToDecimal())
                    {
                        switch (onUser.RefereeDepth - item.RefereeDepth)
                        {
                            case 1: //1代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1504).Value2.Split('|')[0].ToDecimal();
                                break;
                            case 2: //2代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1504).Value2.Split('|')[1].ToDecimal();
                                break;
                            case 3: //3代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1504).Value2.Split('|')[2].ToDecimal();
                                break;
                            case 4: //4代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1504).Value2.Split('|')[3].ToDecimal();
                                break;
                            case 5: //5代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1504).Value2.Split('|')[4].ToDecimal();
                                break;
                        }
                    }
                    else if (item.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1505).Value.ToDecimal())
                    {
                        switch (onUser.RefereeDepth - item.RefereeDepth)
                        {
                            case 1: //1代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1505).Value2.Split('|')[0].ToDecimal();
                                break;
                            case 2: //2代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1505).Value2.Split('|')[1].ToDecimal();
                                break;
                            case 3: //3代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1505).Value2.Split('|')[2].ToDecimal();
                                break;
                            case 4: //4代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1505).Value2.Split('|')[3].ToDecimal();
                                break;
                            case 5: //5代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1505).Value2.Split('|')[4].ToDecimal();
                                break;
                            case 6: //6代领导奖
                                PARAM_PHJBL = cacheSysParam.SingleAndInit(x => x.ID == 1505).Value2.Split('|')[5].ToDecimal();
                                break;
                        }
                    }
                    string description = "来自会员【" + onUser.UserName + "】的" + param.Name + "(" + bonusMoney + "×" + PARAM_PHJBL + ")";
                    UpdateUserWallet(bonusMoney * PARAM_PHJBL, param.ID, param.Name, description, item.ID, "Addup1105", true);
                }
            }
        }

        #endregion
        #region HL16102101 检查出局
        //HL16102001 检查出局
        public static void RefreeBonus(int UID)
        {
            var DoUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(UID);
            if (DoUser != null)
            {
                string[] ids = DoUser.RefereePath.Split(',');
                var gljlist = MvcCore.Unity.Get<Data.Service.IUserService>().List(x => ids.Contains(x.ID.ToString()) || x.ID == DoUser.ID).OrderByDescending(x => x.RefereeDepth).ToList(); ;//查找整条线上的人
                if (gljlist.Count > 0)
                {
                    foreach (var item2 in gljlist)
                    {
                        var onUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(item2.ID);

                        //应该出局数
                        int ParamNum = Math.Floor(((onUser.Addup1102 ?? 0)) / cacheSysParam.Single(x => x.ID == 1001).Value2.ToDecimal()).ToInt();

                        //已经出局数
                        int OutNum = MvcCore.Unity.Get<JN.Data.Service.IInvestmentService>().List(x => x.UID == onUser.ID && x.Status == (int)JN.Data.Enum.InvestmentStatus.Deal && x.IsBalance).Count();

                        //操作数
                        int DoOutNum = ParamNum - OutNum;
                        if (DoOutNum > 0)
                        {
                            var OutList = MvcCore.Unity.Get<JN.Data.Service.IInvestmentService>().List(x => x.Status == (int)JN.Data.Enum.InvestmentStatus.Transaction && x.IsBalance && x.UID == onUser.ID).OrderBy(x => x.CreateTime).Take(DoOutNum).ToList();
                            if (OutList.Count > 0)
                            {
                                foreach (var itemOut in OutList)
                                {
                                    var OutInvestment = MvcCore.Unity.Get<JN.Data.Service.IInvestmentService>().Single(itemOut.ID);
                                    OutInvestment.Status = (int)JN.Data.Enum.InvestmentStatus.Deal;
                                    OutInvestment.LastBalanceTime = DateTime.Now;
                                    MvcCore.Unity.Get<JN.Data.Service.IInvestmentService>().Update(OutInvestment);
                                    MvcCore.Unity.Get<JN.Data.Service.ISysDBTool>().Commit();
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region HL16102101 直推奖金
        //HL16102001 直推奖金
        public static void RefreeBonus(int UID, string InvestmentNo, decimal bonusMoney, decimal ApplyInvestment)
        {
            string bonusDesc = ""; //得奖描述
            //var Investment = MvcCore.Unity.Get<JN.Data.Service.IInvestmentService>().Single(x => x.InvestmentNo == InvestmentNo);
            var onUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(UID);
            Data.SysParam param = null;

            //if (Investment != null)
            //{
            var refereeUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(onUser.RefereeID);
            if (refereeUser != null)
            {
                param = cacheSysParam.SingleAndInit(x => x.ID == 1103);
                decimal PARAM_JJJBL = param.Value.ToDecimal(); //直荐金比例
                bonusMoney = ApplyInvestment * PARAM_JJJBL;
                bonusDesc = "来自会员【" + onUser.UserName + "】的" + param.Name + "(" + ApplyInvestment.ToString("F2") + "×" + PARAM_JJJBL + ")";
              
                refereeUser.Addup1103 = (refereeUser.Addup1103 ?? 0) + bonusMoney;
                refereeUser.AddStr1103 = (refereeUser.AddStr1103 ?? "") + "—添加" + bonusMoney.ToString("F2") + "_" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                MvcCore.Unity.Get<JN.Data.Service.IUserService>().Update(refereeUser);
                MvcCore.Unity.Get<JN.Data.Service.ISysDBTool>().Commit();
                UpdateUserWallet(bonusMoney, param.ID, param.Name, bonusDesc, refereeUser.ID, "Addup1103", true);
                //检查出局
                Bonus.RefreeBonus(refereeUser.ID);
            }
            //}
        }
        #endregion

        #region HL16102101 报单奖
        public static void AgentBonus(int UID, string InvestmentNo, decimal ApplyInvestment)
        {
            string bonusDesc = ""; //得奖描述
            //var Investment = MvcCore.Unity.Get<JN.Data.Service.IInvestmentService>().Single(x => x.InvestmentNo == InvestmentNo);
            var onUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(UID);
            Data.SysParam param = null;

            //if (Investment != null)
            //{
            var refereeUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().Single(onUser.AgentID);
            if (refereeUser != null)
            {
                if (refereeUser.IsAgent == true)
                {
                    param = cacheSysParam.SingleAndInit(x => x.ID == 1108);
                    decimal PARAM_JJJBL = cacheSysParam.SingleAndInit(x => x.ID == 1804).Value2.ToDecimal();
                    decimal bonusMoney = ApplyInvestment * PARAM_JJJBL;
                    bonusDesc = "来自会员【" + onUser.UserName + "】的" + param.Name + "(" + ApplyInvestment.ToString("F2") + "×" + PARAM_JJJBL + ")";
                    UpdateUserWallet(bonusMoney, param.ID, param.Name, bonusDesc, refereeUser.ID, "Addup1802", true);


                    refereeUser.Addup1802 = (refereeUser.Addup1802 ?? 0) + bonusMoney;
                    refereeUser.AddStr1108 = (refereeUser.AddStr1108 ?? "") + "—添加" + bonusMoney.ToString("F2") + "_" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    MvcCore.Unity.Get<JN.Data.Service.IUserService>().Update(refereeUser);
                    MvcCore.Unity.Get<JN.Data.Service.ISysDBTool>().Commit();

                    //检查出局
                    Bonus.RefreeBonus(refereeUser.ID);
                }
            }
            //}
        }
        #endregion

        #region wp160817 每月自动发奖金给经理
        //wp160817 每月自动发奖金给经理
        public static void MonthAgentBouns()
        {
            //所有经理
            var AgentList = MvcCore.Unity.Get<JN.Data.Service.IUserService>().List(x => x.IsAgent == true && x.AgentLevel > 0).ToList();
            if (AgentList.Count > 0)
            {
                foreach (var item in AgentList)
                {
                    string CheckMonth = DateTime.Now.Month + "月";
                    if (MvcCore.Unity.Get<Data.Service.IBonusDetailService>().Single(x => x.Description.Contains(CheckMonth) && x.UID == item.ID) == null)
                    {
                        decimal BounsMoney = 0;
                        switch (item.AgentLevel)
                        {
                            case 1:
                                BounsMoney = cacheSysParam.SingleAndInit(x => x.ID >= 1801).Value3.Split('|')[0].ToDecimal();
                                break;
                            case 2:
                                BounsMoney = cacheSysParam.SingleAndInit(x => x.ID >= 1802).Value3.Split('|')[0].ToDecimal();
                                break;
                            case 3:
                                BounsMoney = cacheSysParam.SingleAndInit(x => x.ID >= 1803).Value3.Split('|')[0].ToDecimal();
                                break;
                        }
                        if (BounsMoney > 0)
                        {
                            var param = cacheSysParam.SingleAndInit(x => x.ID == 1105);
                            string bonusDesc = param.Name + ",来自" + CheckMonth + "的" + item.AgentName + "奖金";
                            UpdateUserWallet(BounsMoney, param.ID, param.Name, bonusDesc, item.ID, "Addup1103", true);
                        }
                    }
                }
            }
        }

        #endregion

        #region WP160817 经理获得会员分红
        public static void GetBalanceMoney()
        {
            //获取当前分红期数
            int period = MvcCore.Unity.Get<JN.Data.Service.ISettlementService>().List().Max(x => x.Period);
            var UserList = MvcCore.Unity.Get<JN.Data.Service.IUserService>().List(x => x.IsAgent == true && x.AgentLevel > 0).ToList();
            if (UserList.Count > 0)
            {
                foreach (var item in UserList)
                {
                    var teamUser = MvcCore.Unity.Get<JN.Data.Service.IUserService>().List(x => x.RefereeID == item.ID).ToList();
                    if (teamUser.Count > 0)
                    {
                        string[] Team = null;
                        int num = 0;
                        decimal TeamBalance = 0;  //团队分红总额
                        decimal BounsMoney = 0;  //奖金
                        decimal Scale = 0;  //计算比例
                        foreach (var itemTeam in teamUser)
                        {
                            Team[num] = itemTeam.ID.ToString();
                            num++;
                        }

                        TeamBalance = MvcCore.Unity.Get<JN.Data.Service.IBonusDetailService>().List(x => x.Period == period && Team.Contains(x.ID.ToString())).Sum(x => x.BonusMoney);


                        switch (item.AgentLevel)
                        {
                            case 1:
                                Scale = cacheSysParam.SingleAndInit(x => x.ID >= 1801).Value3.Split('|')[1].ToDecimal();
                                break;
                            case 2:
                                Scale = cacheSysParam.SingleAndInit(x => x.ID >= 1802).Value3.Split('|')[1].ToDecimal();
                                break;
                            case 3:
                                Scale = cacheSysParam.SingleAndInit(x => x.ID >= 1803).Value3.Split('|')[1].ToDecimal();
                                break;
                        }

                        if (Scale > 0)
                        {
                            BounsMoney = TeamBalance * Scale;
                            var param = cacheSysParam.SingleAndInit(x => x.ID == 1105);
                            string bonusDesc = param.Name + ",来自直推会员的分红奖金" + "(" + BounsMoney.ToString("F2") + "×" + Scale + ")";
                            UpdateUserWallet(BounsMoney, param.ID, param.Name, bonusDesc, item.ID, "Addup1105", true);
                        }

                    }
                }

            }
        }
        #endregion


        #region 检查超时的订单并取消
        public static void CheckInvestmentStauts()
        {
            var InvestmentList = MvcCore.Unity.Get<InvestmentService>().List(x => x.Status == (int)JN.Data.Enum.InvestmentStatus.Apply).ToList();
            if (InvestmentList.Count > 0)
            {
                int CheckTime = cacheSysParam.Single(x => x.ID == 3006).Value.ToInt();  // 要检查的时间
                foreach (var item in InvestmentList)
                {
                    if (item.CreateTime.AddMinutes(CheckTime) >= DateTime.Now)
                    {
                        var Investment = MvcCore.Unity.Get<InvestmentService>().Single(x => x.ID == item.ID);
                        Investment.Status = (int)JN.Data.Enum.InvestmentStatus.Cancel;
                        Investment.Remark += "超时未付款，已取消。";
                        MvcCore.Unity.Get<InvestmentService>().Update(Investment);
                        MvcCore.Unity.Get<Data.Service.ISysDBTool>().Commit();
                    }
                }

            }
        }
        #endregion

        #region 生成真实订单号
        public static string GetRechargeNo()
        {
            string result = string.Empty;
            while (result.Length == 0)
            {
                string BaseCode = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890123456789";
                result = DateTime.Now.ToString("MMdd");
                Random random = new Random();
                while (result.Length < 10)
                    result += BaseCode[random.Next(0, BaseCode.Length)];
                if (MvcCore.Unity.Get<JN.Data.Service.IInvestmentService>().Single(x => x.InvestmentNo == result) != null)
                {
                    result = string.Empty;
                }
            }
            return result;
        }

        #endregion

        #region HL16102001 分红
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="InvestmentNo"></param>
        /// <param name="InvestmentMoney"></param>
        public static void Balance(int UID, string InvestmentNo, decimal InvestmentMoney)
        {
            #region 分红
            var Parm1301 = cacheSysParam.Single(x => x.ID == 1301);
            var Parm1302 = cacheSysParam.Single(x => x.ID == 1302);
            var Parm1303 = cacheSysParam.Single(x => x.ID == 1303);
            var Parm1304 = cacheSysParam.Single(x => x.ID == 1304);
            var Parm1305 = cacheSysParam.Single(x => x.ID == 1305);
            decimal investmentmoney =InvestmentMoney;
            decimal bonusMoney=0;
            if(investmentmoney==int.Parse(Parm1301.Value)){
                 bonusMoney = investmentmoney * int.Parse(Parm1301.Value2);          
            }
            if (investmentmoney == int.Parse(Parm1302.Value))
            {
                 bonusMoney = investmentmoney * int.Parse(Parm1302.Value2);
            }
            if (investmentmoney == int.Parse(Parm1303.Value))
            {
                 bonusMoney = investmentmoney * int.Parse(Parm1303.Value2);
            }
            if (investmentmoney == int.Parse(Parm1304.Value))
            {
                bonusMoney = investmentmoney * int.Parse(Parm1304.Value2);
            }
            if (investmentmoney == int.Parse(Parm1305.Value))
            {
                bonusMoney = investmentmoney * int.Parse(Parm1305.Value2);
            }

            //decimal stockright = Parm1102.Value.ToDecimal();//0.1 每日分红比例为10%

            var onUser = MvcCore.Unity.Get<JN.Data.Service.UserService>().Single(UID);//获取用户model
            //var Investment = MvcCore.Unity.Get<JN.Data.Service.InvestmentService>().Single(x => x.InvestmentNo == InvestmentNo);
            //给当前用户的分红
            //decimal bonusMoney = stockright * InvestmentMoney;
            string bonusDesc = "单号:" + InvestmentNo + ",【" + DateTime.Now.ToShortDateString() + "】,获得" + bonusMoney.ToString("F2");
            //写入明细 日记WalletLog表 
            MvcCore.Unity.Get<JN.Data.Service.WalletLogService>().Add(new Data.WalletLog
            {
                ChangeMoney = bonusMoney,
                Balance = onUser.Wallet2001 + bonusMoney,
                CoinID = 2001,
                CoinName = cacheSysParam.Single(x => x.ID == 2001).Name,
                CreateTime = DateTime.Now,
                Description = bonusDesc,
                UID = onUser.ID,
                UserName = onUser.UserName
            });


            //写进奖金
            MvcCore.Unity.Get<JN.Data.Service.IBonusDetailService>().Add(new Data.BonusDetail
            {
                Period = 0,
                BalanceTime = DateTime.Now,
                BonusMoney = bonusMoney,
                BonusID = 1102,
                BonusName = cacheSysParam.Single(x => x.ID == 1102).Name,
                CreateTime = DateTime.Now,
                Description = bonusDesc,
                BonusMoneyCF = 0,
                BonusMoneyCFBA = ((bonusMoney/investmentmoney) * 100),  //当日比例
                IsBalance = true,
                UID = onUser.ID,
                UserName = onUser.UserName
            });

            //更新用户钱包
            onUser.Wallet2001 = onUser.Wallet2001 + bonusMoney;
            onUser.Addup1102 = (onUser.Addup1102 ?? 0) + bonusMoney;
            onUser.AddStr1102 = (onUser.AddStr1102 ?? "") + "—添加" + bonusMoney.ToString("F2") + "_" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            MvcCore.Unity.Get<JN.Data.Service.UserService>().Update(onUser);
            MvcCore.Unity.Get<JN.Data.Service.SysDBTool>().Commit();

            #endregion
        }
        #endregion




    }
}