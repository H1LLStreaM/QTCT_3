using System;
using System.Collections.Generic;
using System.Text;

namespace WY.Library.Model
{
    //时间区间
    public class Dateinterval
    {
        private int monthType;
        /// <summary>
        /// 月度类型
        /// </summary>
        public int MonthType
        {
            get { return monthType; }
            set { monthType = value; }
        }

        private DateTime startDate;
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartDate
        {
          get { return startDate; }
          set { startDate = value; }
        }

        private DateTime endDate;
        //截止时间
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private decimal money;
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money
        {
            get { return money; }
            set { money = value; }
        }
    }
}
