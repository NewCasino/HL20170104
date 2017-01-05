using JN.Data.Service;
using JN.Services.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace JN.Services.Manager
{
    /// <summary>
    ///奖金结算
    /// </summary>
    public partial class Balances
    {
        // private static List<Data.SysParam> cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().List(x => x.ID < 10000).ToList();

        #region HL16102001 分红
        /// <summary>
        /// 股权分红（周结）
        /// </summary>
        public static void WeekSettlement(
            ISettlementService SettlementService,
            IUserService UserService,
             IWalletLogService WalletLogService,
            IBonusDetailService BonusDetailService,
             IInvestmentService InvestmentService,
            ISysDBTool SysDBTool,
             List<Data.SysParam> cacheSysParam,
            int cmode,  //结算类型
            int CheckPeriod,   //结算期数
            out string Msg
            )
        {
            Msg = "";
            if (cacheSysParam.Single(x => x.ID == 1210).Value != "0")  //防止多发操作
            {
                DateTime CheckTime = new DateTime();
                logs.WriteLog("1缓存状态：" + MvcCore.Extensions.CacheExtensions.CheckCache("Check_time").ToString());
                if (MvcCore.Extensions.CacheExtensions.CheckCache("Check_time").ToString() == "True")
                {
                    CheckTime = MvcCore.Extensions.CacheExtensions.GetCache<DateTime>("Check_time");
                    logs.WriteLog("4缓存状态：" + MvcCore.Extensions.CacheExtensions.CheckCache("Check_time").ToString() + CheckTime);
                }
                else
                {
                    MvcCore.Extensions.CacheExtensions.SetCache("Check_time", DateTime.Now);
                    logs.WriteLog("2缓存状态：" + MvcCore.Extensions.CacheExtensions.CheckCache("Check_time").ToString());
                }

                if (CheckTime.AddHours(1) > DateTime.Now) return;
                logs.WriteLog("3缓存状态：" + MvcCore.Extensions.CacheExtensions.CheckCache("Check_time").ToString());
            }

            int DoNum = 0;
            int AgentNum = 0;
            var periodcount = SettlementService.List(x => x.Period > 0).Count();
            int period = (periodcount > 0 ? SettlementService.List().Max(x => x.Period) : 0);  //操作期数
            int TimeCheck = period;
            if (CheckPeriod > 0 && CheckPeriod <= TimeCheck)
            {
                period = CheckPeriod;   //补发
                Msg += "【补发】——";
            }
            else
            {
                period = period + 1;  //新发
                Msg += "【新发】——";
            }

            #region 先操作利息表，方便补发操作判断

            DateTime CheckLastTime = DateTime.Now;  //过滤超过期数的单
            var SettlementModel = SettlementService.Single(x => x.Period == period);
            if (SettlementModel == null)
            {
                SettlementService.Add(new Data.Settlement
                {
                    BalanceMode = cmode,
                    CreateTime = DateTime.Now,
                    Period = period,
                    TotalBonus = 0,
                    TotalUser = 0
                });
            }
            else
            {
                CheckLastTime = SettlementModel.CreateTime;
            }
            SysDBTool.Commit();

            #endregion  //分红
            var Parm1102 = cacheSysParam.Single(x => x.ID == 1201);

            List<Data.BonusDetail> cacheBonusPeriod = BonusDetailService.ListCache("cacheBonusPeriods", x => x.Period == period).ToList();
            //统计所有未进行结算的投资
            var InvestmentList = InvestmentService.List(x => x.Status == (int)JN.Data.Enum.InvestmentStatus.Transaction && x.IsBalance).Where(x => x.CreateTime <= CheckLastTime).OrderByDescending(x => x.ID).ToList();

            foreach (var item in InvestmentList)
            {
                //防止重发
                if (cacheBonusPeriod.Where(x => x.UID == item.UID && x.Description.Contains(item.InvestmentNo) && x.Period == period && x.BonusID == 1102).Count() > 0) continue;

                var onUser = UserService.Single(item.UID);

                if (onUser.IsShareBouns != true) continue;  //排除掉不可分红会员

                decimal stockright = Parm1102.Value.ToDecimal();

                //给当前用户的分红
                decimal bonusMoney = stockright * item.HaveMoney;
                string bonusDesc = "单号:" + item.InvestmentNo + "," + typeof(JN.Data.Enum.InvestmentType).GetEnumDesc(item.InvestmentType) + ",【" + DateTime.Now.ToShortDateString() + "】,获得" + bonusMoney.ToString("F2") + "分红";
                //写入明细
                WalletLogService.Add(new Data.WalletLog
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
                BonusDetailService.Add(new Data.BonusDetail
                {
                    Period = period,
                    BalanceTime = DateTime.Now,
                    BonusMoney = bonusMoney,
                    BonusID = 1102,
                    BonusName = cacheSysParam.Single(x => x.ID == 1102).Name,
                    CreateTime = DateTime.Now,
                    Description = bonusDesc,
                    BonusMoneyCF = 0,
                    BonusMoneyCFBA = (stockright * 100),  //当日比例
                    IsBalance = true,
                    UID = onUser.ID,
                    UserName = onUser.UserName
                });

                //更新用户钱包
                onUser.Wallet2001 = onUser.Wallet2001 + bonusMoney;
                onUser.Addup1102 = (onUser.Addup1102 ?? 0) + bonusMoney;
                onUser.AddStr1102 = (onUser.AddStr1102 ?? "") + "—添加" + bonusMoney.ToString("F2") + "_" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                UserService.Update(onUser);

                var UpdateSettlement = SettlementService.Single(x => x.Period == period);
                UpdateSettlement.TotalBonus += bonusMoney;
                UpdateSettlement.TotalUser += 1;
                SettlementService.Update(UpdateSettlement);

                SysDBTool.Commit();

                DoNum++;  //累计

            }
            //分红完成
            var CheckEnd = SettlementService.Single(x => x.Period == period);
            CheckEnd.Bonus = 1;
            SettlementService.Update(CheckEnd);
            SysDBTool.Commit();

            #region 1106 团队奖
            var Param1106 = cacheSysParam.Single(x => x.ID == 1106);
            decimal Money1106 = 0;

            List<Data.User> cacheUser1106 = UserService.ListCache("cacheUser1106s", x => x.ID > 0).ToList();
            var bonus1102list = BonusDetailService.List(x => x.Period == period && x.BonusID == 1102).ToList();
            if (bonus1102list.Count > 0)
            {
                foreach (var item1102 in bonus1102list)
                {
                    string No = item1102.Description.Substring(item1102.Description.IndexOf(":") + 1, 10);  //来自单号
                    var onUser = cacheUser1106.Single(x => x.ID == item1102.UID);  //计算会员
                    decimal bonusMoney = item1102.BonusMoney;  //计算奖金

                    string[] ids = onUser.RefereePath.Split(',');
                    var gljlist = cacheUser1106.Where(x => ids.Contains(x.ID.ToString())).OrderByDescending(x => x.RefereeDepth).Take(11).ToList(); //查找整条线上的人
                    if (gljlist.Count > 0)
                    {
                        foreach (var item2 in gljlist)
                        {
                            var ReUser = UserService.Single(x => x.ID == item2.ID);

                                //检查是否重复
                                if (cacheBonusPeriod.Where(x => x.UID == ReUser.ID && x.Description.Contains(No) && x.Period == period && x.BonusID == 1106).Count() > 0) continue;
                          
                            int DeptDiffer = onUser.RefereeDepth - ReUser.RefereeDepth;  //代数
                            decimal PARAM_JLJBL = 0;
                            int RefreeNum = cacheUser1106.Where(x => x.RefereeID == item2.ID).Count();   //推荐人数
                            if (RefreeNum >= DeptDiffer)  //多少星拿多少代
                            {
                                switch (DeptDiffer)
                                {
                                    case 1:
                                        PARAM_JLJBL = cacheSysParam.Single(x => x.ID == 1601).Value.ToDecimal();
                                        break;
                                    case 2:
                                        PARAM_JLJBL = cacheSysParam.Single(x => x.ID == 1602).Value.ToDecimal();
                                        break;
                                    case 3:
                                        PARAM_JLJBL = cacheSysParam.Single(x => x.ID == 1603).Value.ToDecimal();
                                        break;
                                    case 4:
                                        PARAM_JLJBL = cacheSysParam.Single(x => x.ID == 1604).Value.ToDecimal();
                                        break;
                                    case 5:
                                        PARAM_JLJBL = cacheSysParam.Single(x => x.ID == 1605).Value.ToDecimal();
                                        break;
                                    case 6:
                                        PARAM_JLJBL = cacheSysParam.Single(x => x.ID == 1606).Value.ToDecimal();
                                        break;
                                    case 7:
                                        PARAM_JLJBL = cacheSysParam.Single(x => x.ID == 1607).Value.ToDecimal();
                                        break;
                                    case 8:
                                        PARAM_JLJBL = cacheSysParam.Single(x => x.ID == 1608).Value.ToDecimal();
                                        break;
                                    case 9:
                                        PARAM_JLJBL = cacheSysParam.Single(x => x.ID == 1609).Value.ToDecimal();
                                        break;
                                    case 10:
                                        PARAM_JLJBL = cacheSysParam.Single(x => x.ID == 1610).Value.ToDecimal();
                                        break;
                                }
                            }

                            if (PARAM_JLJBL > 0)
                            {
                                Money1106 = PARAM_JLJBL * bonusMoney;
                                string bonusDesc1106 = "来自会员" + onUser.UserName + "单号:" + No + "," + "第" + DeptDiffer + "代的奖金" + "(" + bonusMoney.ToString("F2") + "×" + PARAM_JLJBL + ")";
                                //写入明细
                                WalletLogService.Add(new Data.WalletLog
                                {
                                    ChangeMoney = Money1106,
                                    Balance = ReUser.Wallet2001 + Money1106,
                                    CoinID = 2001,
                                    CoinName = cacheSysParam.Single(x => x.ID == 2001).Name,
                                    CreateTime = DateTime.Now,
                                    Description = bonusDesc1106,
                                    UID = ReUser.ID,
                                    UserName = ReUser.UserName
                                });
                                //写进奖金
                                BonusDetailService.Add(new Data.BonusDetail
                                {
                                    Period = period,
                                    BalanceTime = DateTime.Now,
                                    BonusMoney = Money1106,
                                    BonusID = Param1106.ID,
                                    BonusName = Param1106.Name,
                                    CreateTime = DateTime.Now,
                                    Description = bonusDesc1106,
                                    BonusMoneyCF = 0,
                                    BonusMoneyCFBA = (PARAM_JLJBL * 100),  //当日比例
                                    IsBalance = true,
                                    UID = ReUser.ID,
                                    UserName = ReUser.UserName
                                });

                                ReUser.Wallet2001 = ReUser.Wallet2001 + Money1106;
                                ReUser.Addup1106 = (ReUser.Addup1106 ?? 0) + Money1106;
                                ReUser.AddStr1106 = (ReUser.AddStr1106 ?? "") + "—添加" + Money1106 + "_" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                UserService.Update(ReUser);

                                SysDBTool.Commit();
                                AgentNum++;
                            }
                        }
                    }
                }
            }
            #endregion
          

            //分红完成
            var CheckEnd2 = SettlementService.Single(x => x.Period == period);
            CheckEnd2.Bonus = 2;
            SettlementService.Update(CheckEnd2);
            SysDBTool.Commit();

        }
        #endregion

        #region 返回一个随机的字符
        public static string GetRadom()
        {
            string result = string.Empty;
            while (result.Length == 0)
            {
                string BaseCode = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012345678901234567890123456789012345678901234567890123456789";
                Random random = new Random();
                while (result.Length < 7)
                    result += BaseCode[random.Next(0, BaseCode.Length)];
            }
            return result;
        }
        #endregion
    }
}