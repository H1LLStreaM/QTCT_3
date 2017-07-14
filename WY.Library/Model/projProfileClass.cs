using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WY.Library.Model
{
    /// <summary>
    /// 项目利润
    /// </summary>
    public class projProfileClass
    {
        public string projName { get; set; } //客户名称
        public string saler { get; set; } //销售人员
        public string bills { get; set; } //发票号码
        //public string projStartDate { get; set; } //开始工期
        public string projDate { get; set; } //工期
        public string contractNumber { get; set; } //合同号
        public decimal hshtj { get; set; }  //含税合同价
        public decimal whshtj { get; set; } //未含税合同价
        public List<TB_EXPENSE> expens { get; set; } //报销
        public decimal xmmlr { get; set; } //项目毛利润
        public decimal mlv { get; set; } //毛利率
        public decimal xmjlr { get; set; } //项目净利润
        public decimal xmgcstc { get; set; }  //项目工程师提成比率
        public decimal xmgcstje { get; set; } //项目工程师提成金额
        public decimal gslc { get; set; } //公司留存
    }
}
