using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sina微博的登录
{
    public partial class FrmMain : Form
    {
        weibo web = new weibo();
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            web.init();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            switch (web.loginweibo(txtuser.Text.Trim(), txtpwd.Text.Trim()))
            {
                case 1:
                    MessageBox.Show("密码或账号错误~2333");
                    break;
                case 0:
                    MessageBox.Show("登录成功（？）！~2333");
                    break;
                default:
                    MessageBox.Show("见鬼！你是怎么运行到这里的~2333");
                    break;
            }
            
        }
    }
}
