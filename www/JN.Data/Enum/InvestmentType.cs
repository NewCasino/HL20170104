using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace JN.Data.Enum
{
    public enum InvestmentType
    {
        [Description("首次投资")]
        Frist = 1,

        [Description("补差追投")]
        Repeat = 2,

        [Description("其他复投")]
        OtherRepeat = 3,

    }
}