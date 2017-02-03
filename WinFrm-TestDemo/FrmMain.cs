using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFrm_TestDemo
{
    public partial class FrmMain : Form
    {
        httphelper web = new httphelper();
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnMe_Click(object sender, EventArgs e)
        {

        }
    }
}
