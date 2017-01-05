using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace JN.Data.Enum
{
    public enum InvestmentLevel
    {
        [Description("普通会员")]
        ID1001 = 100,

        [Description("铜级")]
        ID1002 = 1000,

        [Description("银级")]
        ID1003 = 3000,
        [Description("金级")]
        ID1004 = 5000,

        [Description("金钻")]
        ID1005 = 10000,
        [Description("蓝钻")]
        ID1006 = 20000,

        [Description("红宝石")]
        ID1007 = 30000,
        [Description("蓝宝石")]
        ID1008 = 50000,
        [Description("皇冠")]
        ID1009 = 100000
    }
}