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
    public partial class Stocks
    {
        private static List<Data.SysParam> cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().List(x => x.ID < 4000).ToList();

        /// <summary>
        /// 获得X币当前价格
        /// </summary>
        /// <returns></returns>
        public static decimal getcurrentprice()
        {
            //先找日K线表的最后一条，有的话取收盘价
            var chart = MvcCore.Unity.Get<IStockChartDayService>().List().OrderByDescending(x => x.StockDate).ToList().FirstOrDefault();
            if (chart != null)
                return chart.ClosePrice;
            else //没有就找发行价
            {
                var issue = MvcCore.Unity.Get<IStockIssueService>().List().OrderByDescending(x => x.ID).ToList().FirstOrDefault();
                if (issue != null)
                    return issue.Price;
                else
                    return 0;
            }
        }

        public static decimal getopenprice()
        {
            //先找日K线表的最后一条，有的话取开盘价
            var chart = MvcCore.Unity.Get<IStockChartDayService>().List().OrderByDescending(x => x.StockDate).ToList().FirstOrDefault();
            if (chart != null)
                return chart.OpenPrice;
            else //没有就找发行价
            {
                var issue = MvcCore.Unity.Get<IStockIssueService>().List().OrderByDescending(x => x.ID).ToList().FirstOrDefault();
                if (issue != null)
                    return issue.Price;
                else
                    return 0;
            }
        }


        #region 取消交易
        /// <summary>
        /// 取消委托
        /// </summary>
        public static void cancelTTC(string entrustsno)
        {
            var etrade = MvcCore.Unity.Get<IStockEntrustsTradeService>().Single(x => x.EntrustsNo == entrustsno);
            var tradeService = MvcCore.Unity.Get<IStockTradeService>();
            if (etrade != null)
            {
                var userEntity = MvcCore.Unity.Get<IUserService>().Single(etrade.UID);
                if (etrade.Direction == 0) //买入
                {
                    if (etrade.Status == (int)TTCStatus.Entrusts)
                    {
                        decimal drawmoney = etrade.TotalAmount + etrade.Poundage;
                        //返还金额
                        Wallets.changeWallet(userEntity.ID, drawmoney, 2002, "取消委托");
                        etrade.Status = -1;
                    }
                    else //部分成交
                    {
                        //已成交数量
                        decimal ycjsl = tradeService.List(x => x.BuyEntrusNo == etrade.EntrustsNo).Sum(x => x.Quantiry);
                        //已成交手续费
                        // decimal ycjssf = 0;// tradeService.List(x => x.BuyEntrusNo == etrade.EntrustsNo).Sum(x => x.BuyPoundage);
                        decimal drawmoney = (etrade.Quantity - ycjsl) * etrade.Price;// + (etrade.Poundage - ycjssf);
                        //返还金额
                        Wallets.changeWallet(userEntity.ID, drawmoney, 2002, "取消委托");
                        etrade.Status = -1;
                    }
                }
                else if (etrade.Direction == 1) //卖出
                {
                    if (etrade.Status == (int)TTCStatus.Entrusts)
                    {
                        //返还虚拟币
                        Wallets.changeWallet(userEntity.ID, (etrade.Quantity + etrade.Poundage), 2003, "取消委托");
                        etrade.Status = -1;
                    }
                    else //部分成交
                    {
                        //已成交数量
                        decimal ycjsl = tradeService.List(x => x.SellEntrusNo == etrade.EntrustsNo).Sum(x => x.Quantiry); ;
                        //已成交手续费
                        //decimal ycjssf = tradeService.List(x => x.SellEntrusNo == etrade.EntrustsNo).Sum(x => x.SellPoundage);
                        //返还虚拟币
                        Wallets.changeWallet(userEntity.ID, (etrade.Quantity - ycjsl), 2003, "取消委托");
                        etrade.Status = -1;
                    }
                }

                MvcCore.Unity.Get<IStockEntrustsTradeService>().Update(etrade);
                MvcCore.Unity.Get<ISysDBTool>().Commit();
            }
        }
        #endregion

        #region TTC交易操作
        /// <summary>
        /// TTC交易操作
        /// </summary>
        public static void TTC(string entrustsno)
        {
            var etrade = MvcCore.Unity.Get<IStockEntrustsTradeService>().Single(x => x.EntrustsNo == entrustsno);
            if (etrade != null)
            {
                var cacheSysParam = MvcCore.Unity.Get<ISysParamService>().ListCache("sysparam", MvcCore.Extensions.CacheTimeType.ByHours, 24, x => x.ID < 4000);
                decimal PARAM_BUYPOUNDAGE = cacheSysParam.SingleAndInit(x => x.ID == 2202).Value.ToDecimal(); //买入手续费
                decimal PARAM_SELLPOUNDAGE = cacheSysParam.SingleAndInit(x => x.ID == 2203).Value.ToDecimal(); //卖出手续费
                decimal _volume = 0;  //成交量
                decimal _totalamount = 0; //成交额
                decimal _tranprice = 0;  //成交价
                decimal _difftotalamount = 0; //委托与成交额差
                decimal _diffprice = 0; //委托与成交价差
                if (etrade.Direction == 0) //买入
                {
                    //找出卖出单抵消交易
                    var pplist = MvcCore.Unity.Get<IStockEntrustsTradeService>().List(x => x.Direction == 1 && x.Price <= etrade.Price && x.HaveTurnover < x.Quantity && x.Status >= (int)TTCStatus.Entrusts).OrderBy(x => x.Price).ThenBy(x => x.CreateTime).ToList();
                    foreach (var item in pplist)
                    {
                        etrade = MvcCore.Unity.Get<IStockEntrustsTradeService>().Single(x => x.EntrustsNo == entrustsno);
                        decimal _itemsysl = (item.Quantity - item.HaveTurnover); //可交易余量
                        decimal _quantiry = 0; //匹配卖单的成交量

                        //成交价
                        //交易规则：买方挂单价≧卖方价格时成交，哪一方先挂单按哪一方的挂单价成交！
                        if (etrade.CreateTime < item.CreateTime)
                            _tranprice = etrade.Price;
                        else
                        {
                            _tranprice = item.Price;
                            _diffprice = etrade.Price - item.Price;
                        }

                        int _status; //状态
                        if (_itemsysl <= (etrade.Quantity - _volume))  //全部成交
                        {
                            _quantiry = _itemsysl;
                            _status = (int)TTCStatus.AllDeal;
                        }
                        else //部分成交
                        {
                            _quantiry = (etrade.Quantity - _volume);
                            _status = (int)TTCStatus.PartOfTheDeal;
                        }
                        //更新卖出委托
                        var _updateitem = MvcCore.Unity.Get<IStockEntrustsTradeService>().Single(item.ID);
                        _updateitem.HaveTurnover = _updateitem.HaveTurnover + _quantiry;
                        _updateitem.Status = _status;
                        _updateitem.TurnoverTime = DateTime.Now;
                        MvcCore.Unity.Get<IStockEntrustsTradeService>().Update(_updateitem);
                        MvcCore.Unity.Get<ISysDBTool>().Commit();

                        _volume = _volume + _quantiry;
                        _totalamount = _quantiry * _tranprice; //成交总金额
                        _difftotalamount = _quantiry * _diffprice;//成交总金额与委托总金额差
                        decimal _buypoundage = _quantiry * PARAM_BUYPOUNDAGE;  //买入手续费
                        decimal _sellpoundage = _quantiry * PARAM_SELLPOUNDAGE; //卖出手费费
                        //生成交易记录
                        string _tradeNo = Guid.NewGuid().ToString();
                        MvcCore.Unity.Get<IStockTradeService>().Add(new Data.StockTrade
                        {
                            BuyEntrusNo = etrade.EntrustsNo,
                            Direction = 0,
                            CreateTime = DateTime.Now,
                            BuyPoundage = _buypoundage,
                            SellPoundage = _sellpoundage,
                            Price = _tranprice,
                            Quantiry = _quantiry,
                            SellEntrusNo = item.EntrustsNo,
                            TotaAmount = _totalamount,
                            TradeNo = _tradeNo,
                            BuyUID = etrade.UID,
                            BuyUserName = etrade.UserName,
                            SellUID = item.UID,
                            SellUserName = item.UserName
                        });
                        MvcCore.Unity.Get<ISysDBTool>().Commit();
                        //卖出虚拟币得到收益
                        Wallets.changeWallet(item.UID, _totalamount, 2002, "卖出虚拟币，流水号：" + _tradeNo);
                        if (_difftotalamount > 0)
                            Wallets.changeWallet(etrade.UID, _difftotalamount, 2002, "买入虚拟币价差返还，流水号：" + _tradeNo);

                        //虚拟币进帐
                        Wallets.changeWallet(etrade.UID, _quantiry, 2003, "买入虚拟币，流水号：" + _tradeNo);

                        //计算手续费佣金
                        //users.CalculateTradeBonus(_buypoundage, _sellpoundage, _tradeNo, item.UID, etrade.UID, item.Price);

                        //更新买入委托
                        if (_volume == etrade.Quantity)  //已全部成交
                        {
                            etrade.HaveTurnover = etrade.Quantity;
                            etrade.Status = (int)TTCStatus.AllDeal;
                            etrade.TurnoverTime = DateTime.Now;
                            MvcCore.Unity.Get<IStockEntrustsTradeService>().Update(etrade);
                            MvcCore.Unity.Get<ISysDBTool>().Commit();
                            break;
                        }
                        else
                        {
                            etrade.HaveTurnover = etrade.HaveTurnover + _quantiry;
                            etrade.Status = (int)TTCStatus.PartOfTheDeal;
                            etrade.TurnoverTime = DateTime.Now;
                            MvcCore.Unity.Get<IStockEntrustsTradeService>().Update(etrade);
                            MvcCore.Unity.Get<ISysDBTool>().Commit();
                        }
                    }
                }
                else if (etrade.Direction == 1) //卖出
                {
                    //找出买入单抵消交易
                    var pplist = MvcCore.Unity.Get<IStockEntrustsTradeService>().List(x => x.Direction == 0 && x.Price >= etrade.Price && x.HaveTurnover < x.Quantity && x.Status >= (int)TTCStatus.Entrusts).OrderByDescending(x => x.Price).ThenBy(x => x.CreateTime).ToList();
                    foreach (var item in pplist)
                    {
                        etrade = MvcCore.Unity.Get<IStockEntrustsTradeService>().Single(x => x.EntrustsNo == entrustsno);
                        decimal _itemsysl = item.Quantity - item.HaveTurnover;
                        decimal _quantiry;

                        //成交价
                        //交易规则：买方挂单价≧卖方价格时成交，哪一方先挂单按哪一方的挂单价成交！
                        if (item.CreateTime < etrade.CreateTime)
                            _tranprice = item.Price;
                        else
                        {
                            _tranprice = etrade.Price;
                            _diffprice = item.Price - etrade.Price;
                        }

                        int _status; //状态
                        if (_itemsysl <= (etrade.Quantity - _volume))  //全部成交
                        {
                            _quantiry = _itemsysl;
                            _status = (int)TTCStatus.AllDeal;
                        }
                        else //部分成交
                        {
                            _quantiry = (etrade.Quantity - _volume);
                            _status = (int)TTCStatus.PartOfTheDeal;
                        }
                        //更新买入出委托
                        var _updateitem = MvcCore.Unity.Get<IStockEntrustsTradeService>().Single(item.ID);
                        _updateitem.HaveTurnover = _updateitem.HaveTurnover + _quantiry;
                        _updateitem.Status = _status;
                        _updateitem.TurnoverTime = DateTime.Now;
                        MvcCore.Unity.Get<IStockEntrustsTradeService>().Update(_updateitem);
                        MvcCore.Unity.Get<ISysDBTool>().Commit();

                        _volume = _volume + _quantiry;
                        _totalamount = _quantiry * _tranprice; //成交总金额
                        _difftotalamount = _quantiry * _diffprice;//成交总金额与委托总金额差
                        decimal _buypoundage = _quantiry * PARAM_BUYPOUNDAGE;  //买入手续费
                        decimal _sellpoundage = _quantiry * PARAM_SELLPOUNDAGE; //卖出手费费
                        //交易记录
                        string _tradeNo = Guid.NewGuid().ToString();
                        MvcCore.Unity.Get<IStockTradeService>().Add(new Data.StockTrade
                        {
                            BuyEntrusNo = item.EntrustsNo,
                            Direction = 1,
                            CreateTime = DateTime.Now,
                            BuyPoundage = _buypoundage,
                            SellPoundage = _sellpoundage,
                            Price = _tranprice,
                            Quantiry = _quantiry,
                            SellEntrusNo = etrade.EntrustsNo,
                            TotaAmount = _totalamount,
                            TradeNo = _tradeNo,
                            BuyUID = item.UID,
                            BuyUserName = item.UserName,
                            SellUID = etrade.UID,
                            SellUserName = etrade.UserName
                        });
                        MvcCore.Unity.Get<ISysDBTool>().Commit();
                        //卖出虚拟币得到收益
                        Wallets.changeWallet(etrade.UID, _totalamount, 2002, "卖出虚拟币，流水号：" + _tradeNo);
                        if (_difftotalamount > 0)
                            Wallets.changeWallet(item.UID, _difftotalamount, 2002, "买入虚拟币价差返还，流水号：" + _tradeNo);


                        //虚拟币进帐
                        Wallets.changeWallet(item.UID, _quantiry, 2003, "买入虚拟币，流水号：" + _tradeNo);
                        //计算手续费佣金
                        //users.CalculateTradeBonus(_buypoundage, _sellpoundage, _tradeNo, item.UID, etrade.UID, item.Price);
                        //更新卖出委托
                        if (_volume == etrade.Quantity)  //已全部成交
                        {
                            etrade.HaveTurnover = etrade.Quantity;
                            etrade.Status = (int)TTCStatus.AllDeal;
                            etrade.TurnoverTime = DateTime.Now;
                            MvcCore.Unity.Get<IStockEntrustsTradeService>().Update(etrade);
                            MvcCore.Unity.Get<ISysDBTool>().Commit();
                            break;
                        }
                        else
                        {
                            etrade.HaveTurnover = etrade.HaveTurnover + _quantiry;
                            etrade.Status = (int)TTCStatus.PartOfTheDeal;
                            etrade.TurnoverTime = DateTime.Now;
                            MvcCore.Unity.Get<IStockEntrustsTradeService>().Update(etrade);
                            MvcCore.Unity.Get<ISysDBTool>().Commit();
                        }
                    }
                }

                //有成交的话记数据
                if (_volume > 0)
                {
                    var chartService = MvcCore.Unity.Get<IStockChartDayService>();
                    //记K线图
                    if (chartService.List(x => SqlFunctions.DateDiff("DAY", x.StockDate, DateTime.Now) == 0).Count() <= 0)
                    {
                        decimal ycloseprice = getcurrentprice();
                        var yesterday = chartService.List().OrderByDescending(x => x.ID).FirstOrDefault();
                        if (yesterday != null) ycloseprice = yesterday.ClosePrice;

                        var ma5list = chartService.List(x => SqlFunctions.DateDiff("DAY", x.StockDate, DateTime.Now) <= 5);
                        decimal ma5 = ma5list.Count() > 0 ? ma5list.Average(x => x.ClosePrice) : 0;
                        var ma10list = chartService.List(x => SqlFunctions.DateDiff("DAY", x.StockDate, DateTime.Now) <= 10);
                        decimal ma10 = ma10list.Count() > 0 ? ma10list.Average(x => x.ClosePrice) : 0;
                        var ma30list = chartService.List(x => SqlFunctions.DateDiff("DAY", x.StockDate, DateTime.Now) <= 30);
                        decimal ma30 = ma30list.Count() > 0 ? ma30list.Average(x => x.ClosePrice) : 0;

                        chartService.Add(new Data.StockChartDay
                        {
                            YesterdayClosePrice = ycloseprice,
                            Turnover = _totalamount,
                            UpsAndDownsPrice = 0,
                            MA5 = ma5,
                            MA10 = ma10,
                            MA30 = ma30,
                            ClosePrice = _tranprice,
                            CreateTime = DateTime.Now,
                            HightPrice = _tranprice,
                            LowPrice = _tranprice,
                            OpenPrice = _tranprice,
                            StockDate = DateTime.Now,
                            Volume = _volume,
                            UpsAndDownsScale = 0,
                            TotalStock = 0
                        });
                        MvcCore.Unity.Get<ISysDBTool>().Commit();
                    }
                    else
                    {
                        var chart = chartService.Single(x => SqlFunctions.DateDiff("DAY", x.StockDate, DateTime.Now) == 0);
                        if (chart != null)
                        {
                            if (_tranprice > chart.HightPrice) chart.HightPrice = _tranprice;
                            if (_tranprice < chart.LowPrice) chart.LowPrice = _tranprice;
                            chart.Volume = chart.Volume + _volume;
                            chart.Turnover = chart.Turnover + _totalamount;
                            chart.ClosePrice = _tranprice;
                            chart.UpsAndDownsScale = (double)((chart.ClosePrice - chart.OpenPrice) / chart.OpenPrice);
                            chart.UpsAndDownsPrice = chart.ClosePrice - chart.OpenPrice;

                            var ma5list = chartService.List(x => SqlFunctions.DateDiff("DAY", x.StockDate, DateTime.Now) <= 5);
                            decimal ma5 = ma5list.Count() > 0 ? ma5list.Average(x => x.ClosePrice) : 0;
                            var ma10list = chartService.List(x => SqlFunctions.DateDiff("DAY", x.StockDate, DateTime.Now) <= 10);
                            decimal ma10 = ma10list.Count() > 0 ? ma10list.Average(x => x.ClosePrice) : 0;
                            var ma30list = chartService.List(x => SqlFunctions.DateDiff("DAY", x.StockDate, DateTime.Now) <= 30);
                            decimal ma30 = ma30list.Count() > 0 ? ma30list.Average(x => x.ClosePrice) : 0;

                            chart.MA5 = ma5;
                            chart.MA10 = ma10;
                            chart.MA30 = ma30;
                            chartService.Update(chart);
                            MvcCore.Unity.Get<ISysDBTool>().Commit();
                        }
                    }
                }
            }
        }
        #endregion

        #region 认购解冻操作
        /// <summary>
        /// 认购解冻操作
        /// </summary>
        public static bool thawStock(int sid)
        {
            var subscribe = MvcCore.Unity.Get<IStockSubscribeService>().Single(sid);
            if (subscribe != null)
            {
                decimal PARAM_JDBL = cacheSysParam.SingleAndInit(x => x.ID == 2305).Value.ToDecimal();
                int PARAM_JDJG = cacheSysParam.SingleAndInit(x => x.ID == 2305).Value2.ToInt();

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
                    MvcCore.Unity.Get<IStockSubscribeService>().Update(subscribe);
                    MvcCore.Unity.Get<ISysDBTool>().Commit();

                    Wallets.changeWallet(subscribe.UID, thawAmount, 2003, subscribe.Title + "第" + subscribe.UsedTimes + "次解冻");
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
    }
}