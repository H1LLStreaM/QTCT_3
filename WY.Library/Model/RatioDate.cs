using System;
using System.Collections.Generic;
using System.Text;

namespace WY.Library.Model
{
    public class RatioDate
    {
        private decimal ratioMian;   //主销售渠道提成比例

        public decimal RatioMian
        {
            get { return ratioMian; }
            set { ratioMian = value; }
        }
        private decimal ratioSaler;  //销售渠道提成比了

        public decimal RatioSaler
        {
            get { return ratioSaler; }
            set { ratioSaler = value; }
        }
        private decimal ratioWrite;  //完工录入者提成比例

        public decimal RatioWrite
        {
            get { return ratioWrite; }
            set { ratioWrite = value; }
        }
        private decimal taxSaler;

        public decimal TaxSaler
        {
            get { return taxSaler; }
            set { taxSaler = value; }
        }
        private decimal taxWrite;

        public decimal TaxWrite
        {
            get { return taxWrite; }
            set { taxWrite = value; }
        }
        private decimal taxMain;

        public decimal TaxMain
        {
            get { return taxMain; }
            set { taxMain = value; }
        }
    }
}
