using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 百度登录;

namespace 百度登录
{
    public partial class FrmMain : Form
    {
        百度登录类 baidu = new 百度登录类();
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            switch (baidu.loginbaidu(txtpwd.Text.Trim(), out string nick))
            {
                case 1:
                    MessageBox.Show("登录百度账号成功!账号:" + nick);
                    break;
                case 2:
                    MessageBox.Show("登录百度账号失败!");
                    break;
                default:
                    break;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            txtuser.Leave += Txtuser_Leave;
            txtverify.Leave += Txtverify_Leave;
            baidu.init();
        }

        private void Txtuser_Leave(object sender, EventArgs e)
        {
            baidu.checkverify(txtuser.Text);
            baidu.getpubkey();
            picverify.Image = baidu.getverifyPic();
        }
        private void Txtverify_Leave(object sender, EventArgs e)
        {
            baidu.checkverifyState(txtverify.Text);
        }
    }
}
