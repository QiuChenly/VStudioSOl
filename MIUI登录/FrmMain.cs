using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIUI登录
{
    public partial class FrmMain : Form
    {
        /// <summary>
        /// 区区小网站，懒得单独写类
        /// </summary>
        class miui
        {
            HttpHelper web = new HttpHelper();
            qiuchenhelper qiuchen = new qiuchenhelper
                ();
            public string callback { get; set; }
            public string _sign { get; set; }
            public string qs { get; set; }
            public void init()
            {
                string url = @"http://www.miui.com/member.php?mod=logging&action=miuilogin";
                web.RandUserAgent();
                string result = web.HttpGet(url,true);
                //小姐姐の无奈
                result = web.HttpGet(result);
                //302重定向 手动处理
                //Debug.WriteLine(result);
                callback =qiuchen.UrlEnCode( qiuchen.BetweenText(result, "callback:\"", "\""));
                _sign = qiuchen.UrlEnCode(qiuchen.BetweenText(result, "\"_sign\":\"", "\""));
                qs = qiuchen.UrlEnCode(qiuchen.BetweenText(result, "qs:\"", "\""));
            }

            /// <summary>
            /// 登录miui 返回值：1=请填写验证码 0=OK 110=狗带
            /// </summary>
            /// <param name="user"></param>
            /// <param name="pwd"></param>
            /// <param name="verifyCode"></param>
            /// <returns></returns>
            public int loginMIUI(string user, string pwd,out string userid, string verifyCode = "")
            {
                web.SetEncoding(Encoding.UTF8);
                string url = @"https://account.xiaomi.com/pass/serviceLoginAuth2?_dc=" + qiuchen.getDataTime13();
                string data = "";
                if (verifyCode!="")
                {
                    data = @"_json=true&callback=" + callback + "&sid=miuibbs&qs=" + qs + "&_sign=" + _sign + "&serviceParam=%7B%22checkSafePhone%22%3Afalse%7D&user=" + user + "&hash=" + qiuchen.JavaScriptEval(Res.MD5, "getpwd('" + pwd + "')")+ "&captCode="+ verifyCode;
                }
                else
                {
                    data = @"_json=true&callback=" + callback + "&sid=miuibbs&qs=" + qs + "&_sign=" + _sign + "&serviceParam=%7B%22checkSafePhone%22%3Afalse%7D&user=" + user + "&hash=" + qiuchen.JavaScriptEval(Res.MD5, "getpwd('" + pwd + "')");
                }
                data=web.HttpPost(url, data);
                userid = "";
                if (data.Contains("验证码输入错误"))
                {
                    callback = qiuchen.UrlEnCode(qiuchen.BetweenText(data, "\"callback\":\"", "\""));
                    _sign = qiuchen.UrlEnCode(qiuchen.BetweenText(data, "\"_sign\":\"", "\""));
                    qs = qiuchen.UrlEnCode(qiuchen.BetweenText(data, "\"qs\":\"", "\""));
                    return 1;
                }
                else if (data.Contains("成功"))
                {
                    userid = qiuchen.BetweenText(data, "\"userId\":", ",");
                    return 0;
                }
                else
                {
                    return 110;
                }
            }

            public Image getimage() { string url = @"https://account.xiaomi.com/pass/getCode?icodeType=login&" + qiuchen.getDataTime13();
                return Image.FromStream(web.HttpGetMemoryStream(url));
            }
        }
        miui mi = new miui();
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //某小姐姐の原文
            //(｡•ˇ‸ˇ•｡)哼！都怪你 
            //(`ȏ´) 也不哄哄人家
            //(〃′o`)人家超想哭的，捶你胸口，大坏蛋！！！
            //(￣^￣)ゞ咩QAQ 捶你胸口 你好讨厌！
            //(=ﾟωﾟ)ﾉ要抱抱嘤嘤嘤哼，人家拿小拳拳捶你胸口！！！
            //(｡• ︿•̀｡)大坏蛋，打死你(つд⊂)
            mi.init();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            picverify.Image = mi.getimage();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            switch (mi.loginMIUI(txtuser.Text.Trim(), txtpwd.Text.Trim(),out string userid,txtverify.Text.Trim()))
            {
                case 1:
                    picverify.Image = mi.getimage();
                    this.Text = "请输入验证码！";
                    break;
                case 0:
                    MessageBox.Show("登录成功!账号id："+ userid);
                    break;
                case 110:
                    MessageBox.Show("出现了蜜汁异常!");
                    break;
                default:
                    break;
            }
        }
    }
}
