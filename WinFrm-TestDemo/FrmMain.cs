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
                pic_verify.Image = qq.getCapture();
            }
            else if (state == 2)
            {
                MessageBox.Show("获取验证码出现错误！请重新打开本程序！");
            }
        }

        private void btnMe_Click(object sender, EventArgs e)
        {

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (!qq.init())
            {
                MessageBox.Show("初始化失败！", "2333");
            }
        }
    }
}
