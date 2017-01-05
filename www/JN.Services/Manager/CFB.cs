using JN.Data.Enum;
using JN.Data.Service;
using JN.Services.Tool;
using MvcCore.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Linq;
namespace JN.Services.Manager
{
    public partial class CFB
    {
        private static List<Data.SysParam> cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().List(x => x.ID < 4000).ToList();

        /// <summary>
        /// 获得拆分股当前价格
        /// </summary>
        /// <returns></returns>
        public static decimal getcurrentprice()
        {
            //先找日K线表的最后一条，有的话取收盘价
            var chart = MvcCore.Unity.Get<ICFBChartService>().List().OrderByDescending(x => x.CreateTime).ToList().FirstOrDefault();
            if (chart != null)
                return chart.Price;
            else //没有就找发行价
            {
                var issue = MvcCore.Unity.Get<ICFBIssueService>().List().OrderByDescending(x => x.ID).ToList().FirstOrDefault();
                if (issue != null)
                    return issue.Price;
                else
                    return 0;
            }
        }
        #region 预购
        /// <summary>
        /// 认购解冻操作
        /// </summary>
        public static void PreOrder(decimal _preMoney, int onUserID, string origin, string remark)
        {
            var onUser = MvcCore.Unity.Get<IUserService>().Single(onUserID);
            var issue = MvcCore.Unity.Get<ICFBIssueService>().Single(x => x.Status == 1);
            if (issue != null)
            {
                decimal currentprice = CFB.getcurrentprice();
                //if ((issue.HaveSubscribe + _amount) > issue.TotalStock) throw new Exception("很抱歉，当期发行已超额认购！");

                //添加预购记录
                MvcCore.Unity.Get<ICFBPreOrderService>().Add(new Data.CFBPreOrder
                {
                    HaveSubscribeMoney = 0,
                    PreMoney = _preMoney,
                    PreNumber = (int)(_preMoney / currentprice),
                    PrePrice = currentprice,
                    CreateTime = DateTime.Now,
                    Status = 0,
                    OrderNumber = GetPreOrderNumber(8),
                    UID = onUser.ID,
                    Remark = remark,
                    Origin = origin,
                    UserName = onUser.UserName
                });
                MvcCore.Unity.Get<ISysDBTool>().Commit();
            }
            else
            {
                throw new Exception("很抱歉，当前无发行中的拆分股！");
            }
        }

        #region 生成随机编号
        //生成随机编号
        public static string GetPreOrderNumber(int num)
        {
            DateTime dateTime = DateTime.Now;
            string result = Tool.StringHelp.GetRandomNumber(num);//7位随机数字
            if (IsHavePreOrderNumber(result))
            {
                return GetPreOrderNumber(num);
            }
            return result;
        }

        //检查订单号是否重复
        private static bool IsHavePreOrderNumber(string number)
        {
            return MvcCore.Unity.Get<ICFBPreOrderService>().List(x => x.OrderNumber == "Y" + number).Count() > 0;
        }
        #endregion
        #endregion

        #region 认购
        /// <summary>
        /// 认购操作
        /// </summary>
        public static void Subscription(int preOrderID)
        {
            var preOrder = MvcCore.Unity.Get<ICFBPreOrderService>().Single(preOrderID);
            if (preOrder.Status >= 0 && preOrder.Status < 2)
            {
                var onUser = MvcCore.Unity.Get<IUserService>().Single(preOrder.UID);
                var issue = MvcCore.Unity.Get<ICFBIssueService>().Single(x => x.Status == 1);
                if (issue != null)
                {
                    decimal currentprice = CFB.getcurrentprice();
                    decimal ordermoney = preOrder.PreMoney - preOrder.HaveSubscribeMoney;
                    decimal _amount = (int)(ordermoney / currentprice);

                    //if ((issue.HaveSubscribe + _amount) > issue.TotalStock) throw new Exception("很抱歉，当期发行已超额认购！");
                    decimal volume = MvcCore.Unity.Get<ICFBSubscribeService>().List(x => x.Period == issue.Period && x.Price == currentprice).Count() > 0 ? MvcCore.Unity.Get<ICFBSubscribeService>().List(x => x.Period == issue.Period && x.Price == currentprice).Sum(x => x.SubscribeNumber) : 0;

                    if ((volume + _amount) > issue.StepNumber)  //达到上涨条件，可能会被拆单
                    {
                        decimal firstbuy = issue.StepNumber - volume;  //本次最多可买
                        decimal firstbuymoney = firstbuy * currentprice;  //本次买入金额

                        //添加抢购记录
                        MvcCore.Unity.Get<ICFBSubscribeService>().Add(new Data.CFBSubscribe { ApplyNumber = firstbuy, SubscribeNumber = firstbuy, Period = issue.Period, TotalAmount = firstbuymoney, CanBeUsed = 0, CreateTime = DateTime.Now, Price = currentprice, Status = 0, Title = issue.Title, UsedTimes = 0, UID = onUser.ID, UserName = onUser.UserName, Origin = preOrder.Origin, Remark = preOrder.Remark });

                        issue.HaveSubscribe = issue.HaveSubscribe + firstbuy;
                        MvcCore.Unity.Get<ICFBIssueService>().Update(issue);

                        preOrder.HaveSubscribeMoney = preOrder.HaveSubscribeMoney + firstbuymoney;
                        preOrder.Status = 1;
                        MvcCore.Unity.Get<ICFBPreOrderService>().Update(preOrder);
                        MvcCore.Unity.Get<ISysDBTool>().Commit();

                        //更新图表，并产生新的指数
                        CFB.UpdateChart(currentprice, firstbuy, issue.Period);


                        //有可进行拆分，取新的发行信息
                        var issue2 = MvcCore.Unity.Get<ICFBIssueService>().Single(x => x.Status == 1);
                        if (issue2 != null)
                        {
                            //剩下的部分再以新价格配股
                            currentprice = CFB.getcurrentprice();
                            decimal nextbuy = (int)((ordermoney - firstbuymoney) / currentprice);

                            //添加抢购记录
                            MvcCore.Unity.Get<ICFBSubscribeService>().Add(new Data.CFBSubscribe { ApplyNumber = nextbuy, SubscribeNumber = nextbuy, Period = issue2.Period, TotalAmount = (ordermoney - firstbuymoney), CanBeUsed = 0, CreateTime = DateTime.Now, Price = currentprice, Status = 0, Title = issue.Title, UsedTimes = 0, UID = onUser.ID, UserName = onUser.UserName, Origin = preOrder.Origin, Remark = preOrder.Remark });

                            issue2.HaveSubscribe = issue2.HaveSubscribe + nextbuy;
                            MvcCore.Unity.Get<ICFBIssueService>().Update(issue2);

                            preOrder.HaveSubscribeMoney = preOrder.HaveSubscribeMoney + (ordermoney - firstbuymoney);
                            preOrder.Status = 2;
                            MvcCore.Unity.Get<ICFBPreOrderService>().Update(preOrder);
                            MvcCore.Unity.Get<ISysDBTool>().Commit();

                            CFB.UpdateChart(currentprice, nextbuy, issue2.Period);
                        }
                    }
                    else
                    {
                        //添加抢购记录
                        MvcCore.Unity.Get<ICFBSubscribeService>().Add(new Data.CFBSubscribe { ApplyNumber = _amount, SubscribeNumber = _amount, Period = issue.Period, TotalAmount = ordermoney, CanBeUsed = 0, CreateTime = DateTime.Now, Price = currentprice, Status = 0, Title = issue.Title, UsedTimes = 0, UID = onUser.ID, UserName = onUser.UserName, Origin = preOrder.Origin, Remark = preOrder.Remark });

                        //Wallets.changeWallet(onUser.ID, ktb, 2004, "激活时系统自动购买");
                        issue.HaveSubscribe = issue.HaveSubscribe + _amount;
                        MvcCore.Unity.Get<ICFBIssueService>().Update(issue);
                        MvcCore.Unity.Get<ISysDBTool>().Commit();

                        CFB.UpdateChart(currentprice, _amount, issue.Period);

                        preOrder.HaveSubscribeMoney = preOrder.HaveSubscribeMoney + ordermoney;
                        preOrder.Status = 2;
                        MvcCore.Unity.Get<ICFBPreOrderService>().Update(preOrder);
                        MvcCore.Unity.Get<ISysDBTool>().Commit();
                    }
                }
                else
                {
                    throw new Exception("很抱歉，当前无发行中的拆分股！");
                }
            }
        }
        #endregion


        #region 卖出
        public static void Sell(decimal _amount, int onUserID, string selldesc)
        {
            //卖出拆分股得到收益(拆分盘卖出时10%作为手续费，63%可以提现(美金)，27%必须回购)
            decimal PARAM_SELLPOUNDAGE = cacheSysParam.SingleAndInit(x => x.ID == 2401).Value.ToDecimal();
            decimal currentprice = CFB.getcurrentprice();
            decimal _ordermoney = _amount * currentprice * (1 - PARAM_SELLPOUNDAGE);
            decimal _myepmoney = _ordermoney * cacheSysParam.SingleAndInit(x => x.ID == 2401).Value2.ToDecimal();
            Wallets.changeWallet(onUserID, _myepmoney, 2002, selldesc + "拆分股，手续费" + PARAM_SELLPOUNDAGE);
            Wallets.changeWallet(onUserID, 0 - _amount, 2004, selldesc);

            //回购（直接拔币）
            decimal hgje = _ordermoney * cacheSysParam.SingleAndInit(x => x.ID == 2401).Value3.ToDecimal();
            //int number = (int)(hgje / currentprice);
            PreOrder(hgje, onUserID, "OP", "卖出时回购");
        }
        #endregion

        #region 解冻
        /// <summary>
        /// 认购解冻操作
        /// </summary>
        public static bool thawStock(int sid, string remark)
        {
            var subscribe = MvcCore.Unity.Get<ICFBSubscribeService>().Single(sid);
            if (subscribe != null && subscribe.Status < (int)Data.Enum.SubscribeStatus.AllUsed)
            {
                decimal PARAM_JDBL = cacheSysParam.SingleAndInit(x => x.ID == 2405).Value.ToDecimal();
                int PARAM_JDJG = cacheSysParam.SingleAndInit(x => x.ID == 2405).Value2.ToInt();

                if ((subscribe.LastUsedTime ?? DateTime.Now.AddMinutes(0 - PARAM_JDJG)).AddMinutes(PARAM_JDJG) <= DateTime.Now)
                {
                    //解冻数量
                    decimal thawAmount = subscribe.SubscribeNumber * (decimal)PARAM_JDBL;
                    if (subscribe.CanBeUsed + thawAmount > subscribe.SubscribeNumber)
                    {
                        thawAmount = (decimal)(subscribe.SubscribeNumber - subscribe.CanBeUsed);
                    }

                    subscribe.CanBeUsed = (subscribe.CanBeUsed ?? 0) + thawAmount;
                    subscribe.UsedTimes = (subscribe.UsedTimes ?? 0) + 1;

                    if (subscribe.SubscribeNumber == subscribe.CanBeUsed)
                        subscribe.Status = (int)Data.Enum.SubscribeStatus.AllUsed;
                    else
                        subscribe.Status = (int)Data.Enum.SubscribeStatus.PartOfUsed;

                    subscribe.LastUsedTime = DateTime.Now;
                    subscribe.Remark = remark;
                    MvcCore.Unity.Get<ICFBSubscribeService>().Update(subscribe);
                    MvcCore.Unity.Get<ISysDBTool>().Commit();

                    Wallets.changeWallet(subscribe.UID, thawAmount, 2004, remark);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
        #endregion

        #region 超时未解冻
        /// <summary>
        /// 超时未解冻并进行解冻
        /// </summary>
        public static void OvertimeNotThaw()
        {
            var cacheSysParam = MvcCore.Unity.Get<ISysParamService>().List(x => x.ID < 4000).ToList();
            int PARAM_THAWENDHOUR = cacheSysParam.SingleAndInit(x => x.ID == 2405).Value3.ToInt(); //强制解冻 15天
            if (PARAM_THAWENDHOUR > 0)
            {
                //找出超时未付款配单(包括超时未付款及延时后超时未付款)
                var sublist = MvcCore.Unity.Get<ICFBSubscribeService>().List(x => (x.Status <= (int)Data.Enum.SubscribeStatus.PartOfUsed && SqlFunctions.DateDiff("minute", x.CreateTime, DateTime.Now) > PARAM_THAWENDHOUR)).ToList();
                foreach (var item in sublist)
                {
                    thawStock(item.ID, "超时自动解冻");
                }
            }
        }
        #endregion

        public static void UpdateChart(decimal _lastprice, decimal _volume, int _peroid)
        {
            var chartService = MvcCore.Unity.Get<ICFBChartService>();
            var cacheSysParam = MvcCore.Unity.Get<ISysParamService>().ListCache("sysparam", CacheTimeType.ByHours, 24, x => x.ID < 4000);
            decimal currentprice = getcurrentprice();
            decimal total = 0;
            decimal volume = 0;
            //decimal turnoverscale = 0;
            var issue = MvcCore.Unity.Get<ICFBIssueService>().Single(x => x.Period == _peroid);

            total = issue.StepNumber;
            volume = MvcCore.Unity.Get<ICFBSubscribeService>().List(x => x.Period == _peroid && x.Price == currentprice).Count() > 0 ? MvcCore.Unity.Get<ICFBSubscribeService>().List(x => x.Period == _peroid && x.Price == currentprice).Sum(x => x.SubscribeNumber) : 0;

            var chart = MvcCore.Unity.Get<ICFBChartService>().Single(x => x.Price == _lastprice);
            if (chart != null)
            {
                chart.Volume = chart.Volume + _volume;
                chart.TurnoverMoney = chart.TurnoverMoney + (chart.Volume * _lastprice);
                chart.TurnoverScale = chart.Volume / total;
                //chart.TurnoverScale = Math.Min(Math.Max(chart.TurnoverScale, turnoverscale), 1);
                chartService.Update(chart);
                MvcCore.Unity.Get<ISysDBTool>().Commit();
            }

            if (issue.HaveSubscribe >= issue.TotalStock) //拆分
            {
                //所有持币会员进行拆分
                var userlist = MvcCore.Unity.Get<IUserService>().List(x => x.Wallet2004 > 0).OrderByDescending(x => x.ID).ToList();
                decimal total_old2004 = userlist.Count() > 0 ? userlist.Sum(x => x.Wallet2004) : 0;
                decimal oldprice = currentprice;
                decimal newrpice = MvcCore.Unity.Get<ICFBIssueService>().List().FirstOrDefault().Price;
                int beisu = 2;

                decimal totalLX = 0;
                int j = 1;
                foreach (var item in userlist)
                {
                    decimal bonusMoney = 0;
                    //给当前用户的分红
                    string bonusDesc = "拆分股拆分【" + DateTime.Now.ToShortDateString() + "】，" + oldprice + "→" + newrpice + ",倍数：" + beisu;
                    bonusMoney = (item.Wallet2004 * beisu) - item.Wallet2004;

                    //写入明细
                    MvcCore.Unity.Get<IWalletLogService>().Add(new Data.WalletLog
                    {
                        ChangeMoney = bonusMoney,
                        Balance = item.Wallet2004 + bonusMoney,
                        CoinID = 2004,
                        CoinName = cacheSysParam.SingleAndInit(x => x.ID == 2004).Value,
                        CreateTime = DateTime.Now,
                        Description = bonusDesc,
                        UID = item.ID,
                        UserName = item.UserName
                    });
                    totalLX += bonusMoney;
                    j++;
                }

                var parameters2 = new[]{
                    new System.Data.SqlClient.SqlParameter(){ ParameterName="beisu", Value=beisu }
                };
                MvcCore.Unity.Get<ISysDBTool>().ExecuteSQL("update [User] set Wallet2004=Wallet2004 * " + beisu + ",BounsPeriod=ISNULL(BounsPeriod,0)+1 where Wallet2004>0", parameters2);
      
                //未解冻的也进行翻倍
                var parameters = new[]{
                    new System.Data.SqlClient.SqlParameter(){ ParameterName="period", Value=issue.Period }
                };
                MvcCore.Unity.Get<ISysDBTool>().ExecuteSQL("update CFBSubscribe set SubscribeNumber=ISNULL(SubscribeNumber,0)*2 where status<=1 and period<=@period", parameters);

                MvcCore.Unity.Get<ICFBSplittingService>().Add(new Data.CFBSplitting
                {
                    CreateTime = DateTime.Now,
                    OldPrice = oldprice,
                    NewPrice = newrpice,
                    Beisu = beisu,
                    TotalBonus1 = total_old2004,
                    TotalBonus2 = totalLX,
                    TotalUser = userlist.Count
                });

                var newIssue = new Data.CFBIssue();
                newIssue.Period = MvcCore.Unity.Get<ICFBIssueService>().List().Count() > 0 ? MvcCore.Unity.Get<ICFBIssueService>().List().Max(x => x.Period) + 1 : 1;
                newIssue.HaveSubscribe = 0;
                newIssue.CreateTime = DateTime.Now;
                newIssue.Remark = "";
                newIssue.Status = 1;
                newIssue.StepNumber = issue.StepNumber * 2;
                newIssue.Title = "第" + newIssue.Period + "拆分股发行";
                newIssue.TotalStock = issue.TotalStock * 2;
                newIssue.Price = issue.Price;
                newIssue.StepPrice = issue.StepPrice;
                MvcCore.Unity.Get<ICFBIssueService>().Add(newIssue);

                issue.Status = 2;
                MvcCore.Unity.Get<ICFBIssueService>().Update(issue);
                //删除大于价格的指数
                MvcCore.Unity.Get<ICFBChartService>().Delete(x => x.Price >= newrpice);
                //生成新的价格指数
                MvcCore.Unity.Get<ICFBChartService>().Add(new Data.CFBChart
                {
                    Price = newrpice,
                    CreateTime = DateTime.Now,
                    Volume = 0,
                    TurnoverMoney = 0
                });
                MvcCore.Unity.Get<ISysDBTool>().Commit();
            }
            else
            {
                if (volume >= total)  //涨价
                {
                    decimal newprice = currentprice + issue.StepPrice;
                    chartService.Add(new Data.CFBChart
                    {
                        Price = newprice,
                        CreateTime = DateTime.Now,
                        Volume = 0,
                        TurnoverMoney = 0,
                        TurnoverScale = 0
                    });
                    MvcCore.Unity.Get<ISysDBTool>().Commit();
                }
            }
        }
    }
}