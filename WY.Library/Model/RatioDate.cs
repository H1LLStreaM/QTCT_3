using System;
using System.Collections.Generic;
using System.Text;

namespace WY.Library.Model
{
    public class RatioDate
    {
        private decimal ratioMian;   //������������ɱ���

        public decimal RatioMian
        {
            get { return ratioMian; }
            set { ratioMian = value; }
        }
        private decimal ratioSaler;  //����������ɱ���

        public decimal RatioSaler
        {
            get { return ratioSaler; }
            set { ratioSaler = value; }
        }
        private decimal ratioWrite;  //�깤¼������ɱ���

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
