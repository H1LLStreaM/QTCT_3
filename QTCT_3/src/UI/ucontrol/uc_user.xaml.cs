using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WY.Library.Model;

namespace QTCT_3.src.UI
{
    /// <summary>
    /// uc_user.xaml 的交互逻辑
    /// </summary>
    public partial class uc_user : UserControl
    {
        private TB_User mUser;

        public TB_User MUser
        {
            get { return mUser; }
            set { mUser = value; }
        }      

        public delegate void DelUser(uc_user uc);
        public event DelUser DelSelectUser;

        public uc_user(TB_User user)
        {
            InitializeComponent();
            if (user != null)
            {
                mUser = user;
                this.txtName.Text = mUser.USER_NAME;
                this.txtName.Tag = mUser;
            }
        }

        private void imgDel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DelSelectUser != null)
            {
                DelSelectUser(this);
            }
        }
    }
}
