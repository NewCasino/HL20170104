using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace JN.Data.Enum
{
    public enum InvestmentStatus
    {
        [Description("申请中")]
        Apply = 1,

        [Description("确认中")]
        Doing = 2,

        [Description("分红中")]
        Transaction = 3,

        [Description("全部完成")]
        Deal = 4,

        [Description("已取消")]
        Cancel = -1,

        [Description("投诉纠纷")]
        Complaint = -2,
    }
}