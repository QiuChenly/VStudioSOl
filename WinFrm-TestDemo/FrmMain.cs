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
        QQ qq = new QQ();
        public FrmMain()
        {
            InitializeComponent();
            txtuser.Leave += txtuser_Leave;
        }

        private void txtuser_Leave(object sender, EventArgs e)
        {
            //检查是否需要验证码
            int state = qq.CheckVerify(txtuser.Text.Trim());
            if (state == 3)
            {
                this.Text = "检测到需要验证码。";
                pic_verify.Image = qq.getCapture();
            }
            else if (state == 2)
            {
                MessageBox.Show("获取验证码出现错误！请重新打开本程序！");
            }
            else if (state ==1)
            {
                this.Text = "检测到不需要验证码。";
            }
        }



        private void FrmMain_Load(object sender, EventArgs e)
        {
            //初始化一些登录参数
            if (!qq.init())
            {
                MessageBox.Show("初始化失败！", "2333");
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            int state=qq.loginQQ(txtpwd.Text.Trim(), txtVerify.Text.Trim(), out string nick);
            switch (state)
            {
                case 1:
                    MessageBox.Show("登录成功！欢迎你：" + nick, "恭喜！");
                    break;
                case 2:
                    MessageBox.Show("账号或密码错误！", "提示：");
                    txtuser_Leave(null,null);
                    break;
                case 3:
                    MessageBox.Show("验证码错误！", "提示：");
                    pic_verify.Image = qq.getCapture();
                    break;
                default:
                    MessageBox.Show("出现致命错误！导致无法正常访问！", "提示：");
                    break;
            }
        }
    }
}
