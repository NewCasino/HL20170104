using JN.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
namespace JN.Services.Manager
{
    public partial class Users
    {
        private static List<Data.SysParam> cacheSysParam = MvcCore.Unity.Get<ISysParamService>().List(x => x.ID < 4000).ToList();

        #region 获得团队人数集合，正向查询会员/反向查询会员

        /// <summary>
        /// 获得用户所有子节点会员集合（安置关系，按上至下左至右排序）
        /// </summary>
        /// <param name="onUser">会员实体</param>
        /// <param name="countDepth">层深（几层内）</param>
        /// <returns></returns>
        public static List<Data.User> GetAllChild(Data.User onUser, int countDepth = 0)
        {
            var userlist = MvcCore.Unity.Get<Data.Service.IUserService>().List(x => (x.ParentPath.Contains("," + onUser.ID + ",") || x.ParentID == onUser.ID) && x.IsActivation).OrderBy(x => x.RefereeDepth).ThenBy(x => x.DepthSort).ToList();
            if (countDepth > 0)
                userlist = userlist.Where(x => x.Depth <= (onUser.Depth + countDepth)).ToList();
            return userlist;
        }

        /// <summary>
        /// 获得用户所有子节点会员集合（推荐关系，按上至下左至右排序）
        /// </summary>
        /// <param name="onUser">会员实体</param>
        /// <param name="countDepth">几代内</param>
        /// <returns></returns>
        public static List<Data.User> GetAllRefereeChild(Data.User onUser, int countDepth = 0)
        {
            var userlist = MvcCore.Unity.Get<Data.Service.IUserService>().List(x => (x.RefereePath.Contains("," + onUser.ID + ",") || x.RefereeID == onUser.ID) && x.IsActivation).OrderBy(x => x.RefereeDepth).ThenBy(x => x.DepthSort).ToList();
            if (countDepth > 0)
                userlist = userlist.Where(x => x.Depth <= (onUser.RefereeDepth + countDepth)).ToList();
            return userlist;
        }

        /// <summary>
        /// 获得用户所有父节点会员集合（安置关系，反向查询）
        /// </summary>
        /// <param name="onUser">会员实体</param>
        /// <param name="countDepth">反向查询几层</param>
        /// <returns></returns>
        public static List<Data.User> GetAllParent(Data.User onUser, int countDepth = 0)
        {
            if (!string.IsNullOrEmpty(onUser.ParentPath))
            {
                string[] ids = onUser.ParentPath.Split(',');
                var userlist = MvcCore.Unity.Get<Data.Service.IUserService>().List(x => ids.Contains(x.ID.ToString())).OrderByDescending(x => x.Depth).ToList();
                if (countDepth > 0)
                    userlist = userlist.Where(x => x.Depth >= (onUser.Depth - countDepth)).ToList();
                return userlist;
            }
            return null;
        }

        /// <summary>
        /// 获得用户所有父节点会员集合（推荐关系，反向查询）
        /// </summary>
        /// <param name="onUser">会员实体</param>
        /// <param name="countDepth">反向查询几代</param>
        /// <returns></returns>
        public static List<Data.User> GetAllRefereeParent(Data.User onUser, int countDepth = 0)
        {
            if (!string.IsNullOrEmpty(onUser.RefereePath))
            {
                string[] ids = onUser.RefereePath.Split(',');
                var userlist = MvcCore.Unity.Get<Data.Service.IUserService>().List(x => ids.Contains(x.ID.ToString())).OrderByDescending(x => x.RefereeDepth).ToList();
                if (countDepth > 0)
                    userlist = userlist.Where(x => x.Depth >= (onUser.RefereeDepth - countDepth)).ToList();
                return userlist;
            }
            return null;
        }

        #endregion

        #region 用户星级
        public static string GetUserLevelImages(decimal investment)
        {
            string imagetext = "";
            if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1001).Value.ToDecimal())
                imagetext = "<img src=\"/images/xing.png\" style=\"border:0\">";
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1002).Value.ToDecimal())
                imagetext = "<img src=\"/images/xing.png\" style=\"border:0\"><img src=\"/images/xing.png\" style=\"border:0\">";
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1003).Value.ToDecimal())
                imagetext = "<img src=\"/images/xing.png\" style=\"border:0\"><img src=\"/images/xing.png\" style=\"border:0\"><img src=\"/images/xing.png\" style=\"border:0\">";
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1004).Value.ToDecimal())
                imagetext = "<img src=\"/images/xing.png\" style=\"border:0\"><img src=\"/images/xing.png\" style=\"border:0\"><img src=\"/images/xing.png\" style=\"border:0\"><img src=\"/images/xing.png\" style=\"border:0\">";
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1005).Value.ToDecimal())
                imagetext = "<img src=\"/images/xing.png\" style=\"border:0\"><img src=\"/images/xing.png\" style=\"border:0\"><img src=\"/images/xing.png\" style=\"border:0\"><img src=\"/images/xing.png\" style=\"border:0\"><img src=\"/images/xing.png\" style=\"border:0\">";
            return imagetext;
        }
        #endregion

        #region 用户投资级别
        public static string GetUserLevel(decimal investment)
        {
            string result = "";
            if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1001).Value.ToDecimal())
                result = cacheSysParam.SingleAndInit(x => x.ID == 1001).Name;
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1002).Value.ToDecimal())
                result = cacheSysParam.SingleAndInit(x => x.ID == 1002).Name;
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1003).Value.ToDecimal())
                result = cacheSysParam.SingleAndInit(x => x.ID == 1003).Name;
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1004).Value.ToDecimal())
                result = cacheSysParam.SingleAndInit(x => x.ID == 1004).Name;
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1005).Value.ToDecimal())
                result = cacheSysParam.SingleAndInit(x => x.ID == 1005).Name;
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1006).Value.ToDecimal())
                result = cacheSysParam.SingleAndInit(x => x.ID == 1006).Name;
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1007).Value.ToDecimal())
                result = cacheSysParam.SingleAndInit(x => x.ID == 1007).Name;
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1008).Value.ToDecimal())
                result = cacheSysParam.SingleAndInit(x => x.ID == 1008).Name;
            else if (investment == cacheSysParam.SingleAndInit(x => x.ID == 1009).Value.ToDecimal())
                result = cacheSysParam.SingleAndInit(x => x.ID == 1009).Name;
            return result;
        }
        #endregion

        #region 会员激活操作
        public static void ActivationAccount(Data.User onUser)
        {
            var cacheSysParam = MvcCore.Unity.Get<JN.Data.Service.ISysParamService>().ListCache("sysparam", MvcCore.Extensions.CacheTimeType.ByHours, 24, x => x.ID < 4000);
            //更新激活标记
            onUser.IsActivation = true;
            onUser.ActivationTime = DateTime.Now;
            MvcCore.Unity.Get<JN.Data.Service.IUserService>().Update(onUser);
            MvcCore.Unity.Get<Data.Service.ISysDBTool>().Commit();
            //激活会员扣除激活币
            decimal Money = cacheSysParam.Single(x => x.ID == 1001).Value.ToDecimal();  //需要金额
            Wallets.changeWallet(onUser.ID, 0 - Money, 2001, "投资激活帐号");

            //更新整线会员的对碰余量（双轨时）
            if (!string.IsNullOrEmpty(onUser.ParentPath))
            {
                string[] ids_dp = onUser.ParentPath.Split(',');
                var lst_DPUser = MvcCore.Unity.Get<Data.Service.IUserService>().List(x => ids_dp.Contains(x.ID.ToString())).OrderBy(x => x.Depth).ThenBy(x => x.ChildPlace).ToList();
                foreach (var dpUser in lst_DPUser)
                {
                    Data.User updateUser = MvcCore.Unity.Get<Data.Service.IUserService>().Single(dpUser.ID);
                    if (onUser.Depth - dpUser.Depth == 1)
                    {
                        if (onUser.ChildPlace == 1)
                        {
                            updateUser.LeftDpMargin = (updateUser.LeftDpMargin ?? 0) + onUser.Investment;
                            updateUser.LeftAchievement = (updateUser.LeftAchievement ?? 0) + onUser.Investment;
                        }
                        else
                        {
                            updateUser.RightDpMargin = (updateUser.RightDpMargin ?? 0) + onUser.Investment;
                            updateUser.RightAchievement = (updateUser.RightAchievement ?? 0) + onUser.Investment;
                        }
                    }
                    else
                    {
                        //左区安置点
                        var leftchild = MvcCore.Unity.Get<Data.Service.IUserService>().Single(x => x.ParentID == dpUser.ID && x.ChildPlace == 1);
                        //如果出现在左区安置点
                        if (leftchild != null && (onUser.ParentPath + ",").Contains("," + leftchild.ID + ","))
                        {
                            updateUser.LeftDpMargin = (updateUser.LeftDpMargin ?? 0) + onUser.Investment;
                            updateUser.LeftAchievement = (updateUser.LeftAchievement ?? 0) + onUser.Investment;
                        }
                        else
                        {
                            updateUser.RightDpMargin = (updateUser.RightDpMargin ?? 0) + onUser.Investment;
                            updateUser.RightAchievement = (updateUser.RightAchievement ?? 0) + onUser.Investment;
                        }
                    }
                    MvcCore.Unity.Get<Data.Service.IUserService>().Update(updateUser);
                    MvcCore.Unity.Get<Data.Service.ISysDBTool>().Commit();
                }
            }

            //计算奖金
            //Bonus.CalculateBonus(onUser.ID);

            //激活会员50%作为终身一次性开户费，剩下的购买X币
            //decimal xjbbl = 0;

            //if (onUser.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1002).Value.ToDecimal())
            //    xjbbl = cacheSysParam.SingleAndInit(x => x.ID == 1002).Value2.ToDecimal();
            //else if (onUser.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1003).Value.ToDecimal())
            //    xjbbl = cacheSysParam.SingleAndInit(x => x.ID == 1003).Value2.ToDecimal();
            //else if (onUser.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1004).Value.ToDecimal())
            //    xjbbl = cacheSysParam.SingleAndInit(x => x.ID == 1004).Value2.ToDecimal();
            //else if (onUser.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1005).Value.ToDecimal())
            //    xjbbl = cacheSysParam.SingleAndInit(x => x.ID == 1005).Value2.ToDecimal();
            //else if (onUser.Investment == cacheSysParam.SingleAndInit(x => x.ID == 1001).Value.ToDecimal())
            //    xjbbl = cacheSysParam.SingleAndInit(x => x.ID == 1001).Value2.ToDecimal();

            //decimal xjb = onUser.Investment * xjbbl;

            //////////////此处需要修改成认购或交易得来X币
            //CFB.PreOrder(xjb, onUser.ID, "EP", "注册时系统配币");
        }
        #endregion

        #region 会员升级
        public static void UpdateLevel(int uid)
        {
            var onUser = MvcCore.Unity.Get<IUserService>().Single(uid);
            if (onUser != null)
            {
                var purlist = MvcCore.Unity.Get<IUSDPurchaseService>().List(x => x.UID == onUser.ID && x.Status == (int)Data.Enum.USDStatus.Deal).ToList();
                decimal totalbuy = purlist.Count() > 0 ? purlist.Sum(x => x.BuyMoney) : 0;

                decimal tz1001 = cacheSysParam.SingleAndInit(x => x.ID >= 1001).Value.ToDecimal();
                decimal tz1002 = cacheSysParam.SingleAndInit(x => x.ID >= 1002).Value.ToDecimal();
                decimal tz1003 = cacheSysParam.SingleAndInit(x => x.ID >= 1003).Value.ToDecimal();
                decimal tz1004 = cacheSysParam.SingleAndInit(x => x.ID >= 1004).Value.ToDecimal();
                decimal tz1005 = cacheSysParam.SingleAndInit(x => x.ID >= 1005).Value.ToDecimal();

                if (totalbuy >= tz1002 && totalbuy < tz1003 && onUser.Investment < tz1002)
                    onUser.Investment = tz1002;
                else if (totalbuy >= tz1003 && totalbuy < tz1004 && onUser.Investment < tz1003)
                    onUser.Investment = tz1003;
                if (totalbuy >= tz1004 && totalbuy < tz1005 && onUser.Investment < tz1004)
                    onUser.Investment = tz1004;
                if (totalbuy >= tz1005 && onUser.Investment < tz1005)
                    onUser.Investment = tz1005;

                MvcCore.Unity.Get<IUserService>().Update(onUser);

            }
        }
        #endregion

        #region WP160817会员自动升级
        public static void AutoLevel(int uid)
        {
            var onUser = MvcCore.Unity.Get<IUserService>().Single(uid);
            if (onUser != null)
            {
                if (onUser.AgentLevel == 3) return; //顶级

                decimal TeamMoeny = GetAllRefereeChild(onUser, 0).Sum(x => x.Investment);   //团队业绩不含自己
                int RefreeNum = GetAllRefereeChild(onUser, 1).Count();   //直推人数
                int NeedNum = 0;
                decimal NeedMoney = 0;
                string AgentName = "";

                if (onUser.AgentLevel == 2)
                {
                    NeedNum = cacheSysParam.SingleAndInit(x => x.ID >= 1803).Value.ToInt();
                    NeedMoney = cacheSysParam.SingleAndInit(x => x.ID >= 1803).Value2.ToDecimal();
                    AgentName = cacheSysParam.SingleAndInit(x => x.ID >= 1803).Name;
                }
                else if (onUser.AgentLevel == 1)
                {
                    NeedNum = cacheSysParam.SingleAndInit(x => x.ID >= 1802).Value.ToInt();
                    NeedMoney = cacheSysParam.SingleAndInit(x => x.ID >= 1802).Value2.ToDecimal();
                    AgentName = cacheSysParam.SingleAndInit(x => x.ID >= 1802).Name;
                }
                else if (onUser.AgentLevel == 0)
                {
                    NeedNum = cacheSysParam.SingleAndInit(x => x.ID >= 1801).Value.ToInt();
                    NeedMoney = cacheSysParam.SingleAndInit(x => x.ID >= 1801).Value2.ToDecimal();
                    AgentName = cacheSysParam.SingleAndInit(x => x.ID >= 1801).Name;
                }

                if (RefreeNum >= NeedNum && TeamMoeny >= NeedMoney && NeedNum != 0 && NeedMoney != 0)
                {
                    onUser.AgentLevel = (onUser.AgentLevel ?? 0) + 1;
                    onUser.IsAgent = true;
                    onUser.AgentName = AgentName;
                    MvcCore.Unity.Get<IUserService>().Update(onUser);
                    Bonus.MonthAgentBouns();  //检查经理是否拿了当月奖金
                }
            }
        }
        #endregion

        //生成随机编号
        public static string GetUserCode(int num)
        {
            DateTime dateTime = DateTime.Now;
            string result = Tool.StringHelp.GetRandomNumber(num);//7位随机数字
            if (IsHave(result))
            {
                return GetUserCode(num);
            }
            return result;
        }

        //检查订单号是否重复
        private static bool IsHave(string number)
        {
            return MvcCore.Unity.Get<JN.Data.Service.IUserService>().List(x => x.UserName == "C" + number).Count() > 0;
        }
    }
}