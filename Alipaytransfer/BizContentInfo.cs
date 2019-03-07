using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alipaytransfer
{
    /// <summary>
    /// <see cref="https://docs.open.alipay.com/api_28/alipay.fund.trans.toaccount.transfer/"/>
    /// </summary>
    class BizContentInfo
    {
        public string out_biz_no { get; set; } = "";

        public string payee_type { get; } = "ALIPAY_LOGONID";//ALIPAY_USERID 或者 ALIPAY_LOGONID
        public string payee_account { get; set; } = "";
        public decimal amount { get; set; } = 0;
        public string payer_show_name { get; set; } = "";

        /// <summary>
        /// 可选
        /// </summary>
        public string payee_real_name { get; set; } = "";

        /// <summary>
        /// 可选
        /// </summary>
        public string remark { get; set; } = "";

        public override string ToString()
        {
            return "{" +
            "\"out_biz_no\":\"" + out_biz_no + "\"," +
            "\"payee_type\":\"" + payee_type + "\"," +
            "\"payee_account\":\"" + payee_account + "\"," +
            "\"amount\":\"" + amount.ToString("0.00") + "\"," +
            "\"payer_show_name\":\"" + payer_show_name + "\"," +
            "\"payee_real_name\":\"" + payee_real_name + "\"," +
            "\"remark\":\"" + remark + "\"" +
            "}";
        }
    }
}
