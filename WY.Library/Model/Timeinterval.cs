using System;
using System.Collections.Generic;
using System.Text;

namespace WY.Library.Model
{
    //ʱ������
    public class Dateinterval
    {
        private int monthType;
        /// <summary>
        /// �¶�����
        /// </summary>
        public int MonthType
        {
            get { return monthType; }
            set { monthType = value; }
        }

        private DateTime startDate;
        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        public DateTime StartDate
        {
          get { return startDate; }
          set { startDate = value; }
        }

        private DateTime endDate;
        //��ֹʱ��
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private decimal money;
        /// <summary>
        /// ���
        /// </summary>
        public decimal Money
        {
            get { return money; }
            set { money = value; }
        }
    }
}
