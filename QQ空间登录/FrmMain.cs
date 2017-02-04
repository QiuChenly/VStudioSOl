using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QQ空间登录
{
    public partial class FrmMain : Form
    {
        QQ空间 qq = new QQ空间();
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtuser.Leave += Txtuser_Leave;
        }

        private void Txtuser_Leave(object sender, EventArgs e)
        {
            if (qq.CheckVerify(txtuser.Text.Trim())) { verifyPic.Image = qq.getVerifyPic(); this.Text = "检测到需要填写验证码："; }
            else
            {
                this.Text = "检测到本次登录不需要验证码。";
                verifyPic.Image = null;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            int i = qq.loginQQ(txtpwd.Text.Trim(), out string nick, txtverify.Text.Trim());
            switch (i)
            {
                case 3:
                    this.Text = "验证码错误！已自动切换！";
                    Txtuser_Leave(null, null);
                    break;
                case 1:
                    MessageBox.Show("登录成功！" + nick);
                    break;
                case 2:
                    MessageBox.Show("账号或密码错误！" + nick);
                    break;
                case 4:
                    MessageBox.Show("出现异常！！" + nick);
                    break;
                case 5:
                    MessageBox.Show("部分参数获取失败！" + nick);
                    break;
                default:
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Txtuser_Leave(null, null);
        }
    }
}
