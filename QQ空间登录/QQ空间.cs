using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace QQ空间登录
{
    class loginsub
    {
        public string uin { set; get; }
        public string loginsig { get; set; }
        public string cap_cd { get; set; }
        public string verifycode { get; set; }
        public string pt_verifysession_v1 { get; set; }
        public string vsig { get; set; }
    }
    class QQ空间
    {
        qiuchenhelper qiuchen = new qiuchenhelper();
        loginsub sub = new loginsub();
        bool IsNeedCapCD = false;
        HttpHelper web = new HttpHelper();
        #region 初始化loginsig参数
        private void init()
        {
            string url = @"http://xui.ptlogin2.qq.com/cgi-bin/xlogin?proxy_url=http%3A//qzs.qq.com/qzone/v6/portal/proxy.html&daid=5&&hide_title_bar=1&low_login=0&qlogin_auto_login=1&no_verifyimg=1&link_target=blank&appid=549000912&style=22&target=self&s_url=http%3A%2F%2Fqzs.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&pt_qr_app=%E6%89%8B%E6%9C%BAQQ%E7%A9%BA%E9%97%B4&pt_qr_link=http%3A//z.qzone.com/download.html&self_regurl=http%3A//qzs.qq.com/qzone/v6/reg/index.html&pt_qr_help_link=http%3A//z.qzone.com/download.html";
            web.SetEncoding(Encoding.UTF8);
            web.HttpGet(url);
            try
            {
                sub.loginsig = web.getCookieValue("pt_login_sig");
            }
            catch (Exception e)
            {
                MessageBox.Show("出现错误：" + e.Message);
            }

        }
        #endregion

        #region 检查是否需要填写验证码
        public bool CheckVerify(string uin)
        {
            web.InitCookie();
            init();
            sub.uin = uin;
            string url = @"http://check.ptlogin2.qq.com/check?regmaster=&pt_tea=2&pt_vcode=1&uin=" + uin + "&appid=549000912&js_ver=10193&js_type=1&login_sig=" + sub.loginsig + "&u1=http%3A%2F%2Fqzs.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&r=0.25379978094323996&pt_uistyle=40";
            web.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36 OPR/42.0.2393.517");
            web.SetEncoding(Encoding.UTF8);
            string result = web.HttpGet(url);
            Regex regex = new Regex(@"\('(.*?)','(.*?)','(.*?)','(.*?)','(.*?)'\)", RegexOptions.None);
            Match match = regex.Match(result);
            //case:('0','!MNQ','\x00\x00\x00\x00\x72\xc2\xe2\x9c','948c4b994bab7b20834b83c3d023054ad852ba0aefe3172529d6198c7cba0614c40b0c2b10526c925b036bb123fd5a2feb0f3f4a6b07dbbd','2')
            switch (match.Groups[1].Value)
            {
                case "1":
                    //需要验证码
                    sub.cap_cd = match.Groups[2].Value;
                    IsNeedCapCD = true;
                    break;
                case "0":
                    //不需要验证码
                    sub.verifycode = match.Groups[2].Value;
                    sub.pt_verifysession_v1 = match.Groups[4].Value;
                    IsNeedCapCD = false;
                    break;
                default:
                    break;
            }
            return IsNeedCapCD;
        }
        #endregion

        #region null
        public int loginQQ(string pwd, string verify)
        {
            string url = @"http://ptlogin2.qq.com/login?u=" + sub.uin + "&verifycode=" + sub.verifycode + "&pt_vcode_v1=0&pt_verifysession_v1=" + sub.pt_verifysession_v1 + "&p=" + pwd + "&pt_randsalt=2&u1=http%3A%2F%2Fqzs.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&ptredirect=0&h=1&t=1&g=1&from_ui=1&ptlang=2052&action=5-28-1486182421852&js_ver=10193&js_type=1&login_sig=" + sub.loginsig + "&pt_uistyle=40&aid=549000912&daid=5&";

            return 0;
        }
        #endregion


        public Image getVerifyPic()
        {
            string url = @"http://captcha.qq.com/cap_union_new_getsig?aid=549000912&asig=&captype=&protocol=http&clientype=2&disturblevel=&apptype=2&curenv=inner&noBorder=noborder&showtype=embed&uid=" + sub.uin + "&cap_cd=" + sub.cap_cd + "&lang=2052&rnd=962036&rand=0.8517151636700704ischartype=1";
            string result = web.HttpGet(url);
            sub.vsig = qiuchen.BetweenText(result, "\"vsig\":\"", "\"");
            url = @"http://captcha.qq.com/cap_union_new_getcapbysig?aid=549000912&asig=&captype=&protocol=http&clientype=2&disturblevel=&apptype=2&curenv=inner&noBorder=noborder&showtype=embed&uid=" + sub.uin + "&cap_cd=" + sub.cap_cd + "lang=2052&rnd=962036&rand=0.161329225875243&vsig=" + sub.vsig + "&ischartype=1";
            Image img = Image.FromStream(web.HttpGetMemoryStream(url));
            return img;
        }
    }
}
