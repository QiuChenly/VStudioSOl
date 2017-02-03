using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QQClass;

namespace WinFrm_TestDemo
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            txtuser.Leave += txtuser_Leave;
        }

        private void txtuser_Leave(object sender, EventArgs e)
        {
            //检查是否需要验证码
            QQ qq = new QQ();
            qq.init();
        }

        private void btnMe_Click(object sender, EventArgs e)
        {

        }
    }
}
