using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WY.Common.Utility
{
    public class DateTimePickerHelper
    {
        private DateTimePicker _from;

        private DateTimePicker _to;

        #region ÈÕÆÚ·¶Î§

        public void InitDatePikterRelation(DateTimePicker from, DateTimePicker to)
        {
            _from = from;
            _to = to;

            from.ValueChanged += new EventHandler(from_ValueChanged);
            to.ValueChanged += new EventHandler(to_ValueChanged);
        }

        private void to_ValueChanged(object sender, EventArgs e)
        {
            if (_to.Value < _from.Value)
            {
                _from.Value = _to.Value;
            }
        }

        private void from_ValueChanged(object sender, EventArgs e)
        {
            if (_from.Value > _to.Value)
            {
                _to.Value = _from.Value;
            }
        }

        #endregion
    }
}
