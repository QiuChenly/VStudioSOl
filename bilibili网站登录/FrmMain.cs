using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bilibili网站登录
{
    public partial class FrmMain : Form
    {
        bilibili bili = new bilibili();
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            switch (bili.loginbilibili(txtuser.Text, txtpwd.Text, out string errCode, txtverify.Text))
            {
                case 0:
                    MessageBox.Show("登录成功");
                    break;
                case 1:
                    MessageBox.Show("验证码有问题");
                    break;
                case 2:
                    MessageBox.Show("密码错误");
                    break;
                case 3:
                    MessageBox.Show("账号不存在");
                    break;
                case 4:
                    MessageBox.Show("加密的密码过期是TM什么鬼"," bilibili后端你过来");
                    break;
                default:
                    MessageBox.Show("见鬼！你是怎么运行到这里的！");
                    break;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            picverify_Click(null,null);
        }

        private void picverify_Click(object sender, EventArgs e)
        {
            bili.init();
            picverify.Image = bili.getVerify();
        }
    }
}
