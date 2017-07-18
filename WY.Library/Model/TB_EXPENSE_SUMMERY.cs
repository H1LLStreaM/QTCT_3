using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WY.Library.Model
{
    public class TB_EXPENSE_SUMMERY
    {
        public int index { get; set; }
        public string opname { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public decimal money { get; set; }
        public int count { get; set; }
        public int id { get; set; } //项目或员工ID
    }
}
