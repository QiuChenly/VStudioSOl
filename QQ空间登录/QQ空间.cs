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
        public string NextUrl { set; get; }
        public string GTK { set; get; }
        public string qzonetoken { set; get; }
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

        #region 登录QQ空间

        /// <summary>
        /// 登录QQ空间 返回值说明：3=验证失败，请重试 1=OK 2=密码或账号错误 4=未知错误 5=部分参数获取失败
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <param name="verify">验证码，如果没有可以为空</param>
        /// <returns></returns>
        public int loginQQ(string pwd, out string nick, string verify = null)
        {
            string url = "";
            string result = "";
            string vcode = "0";
            nick = "";
            if (IsNeedCapCD)
            {
                vcode = "1";
                sub.verifycode = verify;
                url = @"http://captcha.qq.com/cap_union_new_verify?random=1486187199987";

                #region POST数据
                //collect参数不知道什么鬼
                string data = @"aid=549000912&asig=&captype=&protocol=http&clientype=2&disturblevel=&apptype=2&curenv=inner&noBorder=noborder&showtype=embed&uid=" + sub.uin + "&cap_cd=" + sub.cap_cd + "&lang=2052&rnd=962036&subcapclass=0&vsig=" + sub.vsig + "&cdata=0&collect=OD6q9t0AraWJf%2Bdtq0j8ViP0fBFqffR42tiTwYOa5%2BXw9PwlGmQP4273X2%2BxpVHyLLgJTPQfNF%2BtwEV3nxNAEF8zYdaQW5E%2BeB2rcRL9sw5Jwc7z6%2BYBueEPnXh8HoKxXpVXTZNnGgpGdV4z943LTvYqHfpY0GZE0v88lds6uvAAyFXVZd1tCH9Br3t2gI6tJ3vlMALsIaE5drIqnbATlZ13Llbma5r1uEkCBI%2BlNXV7OCSKSOj%2F%2BTqe0X035wPb%2B6mU92EC9%2B56645Ig%2BEO8BiX7VbIMY49a8QwpXPubRIHXjiqdkdgzoEfYw%2BUfIF%2FJu55s06zY%2FbitKLN4hhcVzC%2FY3TUCuZiwK4N1kr73GodFZ7Dmn%2BE%2F4m0TUWUBKlqRdmRfteUYZCHuAGdzTSwIeQnPh0lzHHVhZnmre%2B2FnIBl9lBzyATJYn5vFgn2LsvTbUfB%2BQlDNaVTkMS9fHJWVcEgIiXzGdmnT30tKXV4zqpsKX%2FlWz3kwesnuUxo9OI8ZtEe5HIZRaSgsOo%2Btn7Nbx1RZSSlFCHswsadYLjBSeXuwTrFbbJex2JUenAYUcwV6l3EyJJurErD7Jh8g7haoy7FhuXbjrnuhq8PV73qM61X3YIALf7T%2FFbb2LjTjtUII94FE1Yf7G%2BxbEsIzbMVaGWzynggtwroM%2B%2FimIA%2F4uQsMBqOUIgYS%2BoRc3hYZ43CzXEdJWGxlxsN7z2rQ85RJadbp3owOCO0oiDRJDrlGLGAgJgKc%2FvZHhAAyaAYLXlp%2BH3BAmxpcOLo4j0FZcppBrBPocH2OQSUrUfvykPr0H6vlWFVwRuIR0F%2F5Bk%2FEPZjeRnwyDJsgtcJ5wYAVwz6jL0zVFr6iJ5Vr8Mv7QlW%2BxjWNPETMlCBhZfZgZ7LHeYtPYhyL9gHygCGJnfd1B0HlaoW%2FTuMYP2CirA6%2F5AYuyDEt3qmV0zJhxlcAdozfcBYGmtADxpEdBSOae7iA2speswnPpye2z5p1Jr6ZI%2FyuNnQ%2Bjcd6eJoItVgy6qZf459K3g8o0hjLRyBcdjztUl1uj2IOiECNCsmt1TtZQmmJPzEpz0g%2B6Fzquf1gy%2BN4m6vZOfHyyKVC1uZcJCCJmr1llQKiGTjie30WJEu%2BL%2B4N9mhvEUB8bRRxhBrQXK3AfChv0gNC%2BbEVc%2F5jB%2FuGDNOk6tWBa1QNoWcfDGZ%2FjusVz%2FxJt%2FKiAMFv%2BI5%2BhJ3Xjm8ogrM0FZx1bMKC3yIQ6DcaRs996VX0WNgGg1rzZliC283xqPw9Bn6XlF4SLa11%2F4LhY24O2GUjql%2FM%2FSs93tHN%2BD2tPUfaRxu4hQzuEURYC5xLGHxt5Q7%2BymyDFjXrmhJR4q7IaMGejPwgd3%2FMbCgIMeLKkkCWGRTsOmyAHrCb3TSSTgDldXp2P999mHNHuxNUtx%2FvuBBGbvAhl3YUxBAANBgDstIysXfWjx%2BgNDA7UX4rwpdFXu8sjXkKWofv7Eo7NevSiEx75gnGsxbMVpYEvqtHeTxBvoE%2FUvSyPxs%2F56jWgiWjmXA9fY%2FViifbmvyWZhahhgvYHmySzcSSE7pql1HwqCYW5yP%2BcuedCxuSIb0VnZ9Zo1qxdKjwR%2FyP5ydctjnfh6CIyFt9S4085iIq%2BFnrrY4rdmtDQjT5loOgIFVW5qxtqKsqm1n%2F6nVsehfrJMxVEU57%2BJr%2F7CPPYyazJIezDB5SiTunwglstFsJAioJaPXkS6B0aUNcnx7pQosurw%2FTyEfEW9A33KNK1ksplorcfqD%2FolmDGEqELgzMOwB%2FW8C2lXrfWVKe75lYVf2CoxQmZkyWVNPv1VCEq48LXXxw0KxM2AXbdXeGmUujuwlMMWuAW9fdl0xPFhBAxgOImr8%2BmT46lIvJiiozYpBxWRHiOtyuFoKzPPCkFwQl4cJQn%2Fk85zFDewZ%2FMVkX91BD3apaYjXShwd931wHdtNDtzGbOZ98fk6oZ5moy4uNA%3D&ans=" + verify;
                #endregion

                result = web.HttpPost(url, data);
                if (result.Contains("验证失败") == true)
                {
                    return 3;//此处切换验证码
                }
                else if (result.Contains("\"errMessage\":\"OK\"") == true)
                {
                    Regex regexA = new Regex("\"randstr\" : \"(.*?)\" , \"ticket\" : \"(.*?)\"");
                    Match matchA = regexA.Match(result);
                    sub.verifycode = matchA.Groups[1].Value;
                    sub.pt_verifysession_v1 = matchA.Groups[2].Value;
                    if (string.IsNullOrWhiteSpace(sub.verifycode) || string.IsNullOrWhiteSpace(sub.pt_verifysession_v1))
                    {
                        return 5;//部分参数获取失败
                    }
                    //return 1; login去也
                }
                else
                {
                    return 4;
                }
            }
            pwd = qiuchen.JavaScriptEval(Res.RSA, "getpwd('" + pwd + "','" + sub.uin + "','" + sub.verifycode + "')");
            url = @"http://ptlogin2.qq.com/login?u=" + sub.uin + "&verifycode=" + sub.verifycode + "&pt_vcode_v1=" + vcode + "&pt_verifysession_v1=" + sub.pt_verifysession_v1 + "&p=" + pwd + "&pt_randsalt=2&u1=http%3A%2F%2Fqzs.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&ptredirect=0&h=1&t=1&g=1&from_ui=1&ptlang=2052&action=5-28-1486182421852&js_ver=10193&js_type=1&login_sig=" + sub.loginsig + "&pt_uistyle=40&aid=549000912&daid=5&";
            result = web.HttpGet(url);
            Regex regex = new Regex(@"\('(.*?)','(.*?)','(.*?)','(.*?)','(.*?)', '(.*?)'\)", RegexOptions.None);
            Match match = regex.Match(result);
            switch (match.Groups[1].Value)
            {
                case "0":
                    nick = match.Groups[6].Value;//昵称
                    sub.NextUrl = match.Groups[3].Value;

                    return 1;
                default:
                    return 2;
            }
        }
        #endregion

        #region 切换验证码
        public Image getVerifyPic()
        {
            string url = @"http://captcha.qq.com/cap_union_new_getsig?aid=549000912&asig=&captype=&protocol=http&clientype=2&disturblevel=&apptype=2&curenv=inner&noBorder=noborder&showtype=embed&uid=" + sub.uin + "&cap_cd=" + sub.cap_cd + "&lang=2052&rnd=962036&rand=0.8517151636700704ischartype=1";
            string result = web.HttpGet(url);
            sub.vsig = qiuchen.BetweenText(result, "\"vsig\":\"", "\"");
            url = @"http://captcha.qq.com/cap_union_new_getcapbysig?aid=549000912&asig=&captype=&protocol=http&clientype=2&disturblevel=&apptype=2&curenv=inner&noBorder=noborder&showtype=embed&uid=" + sub.uin + "&cap_cd=" + sub.cap_cd + "lang=2052&rnd=962036&rand=0.161329225875243&vsig=" + sub.vsig + "&ischartype=1";
            Image img = Image.FromStream(web.HttpGetMemoryStream(url));
            return img;
        }
        #endregion

        #region 计算GTK方法
        /// <summary>
        /// 计算GTK
        /// </summary>
        /// <param name="p_skey"></param>
        /// <returns></returns>
        public string GTK(string p_skey)
        { return qiuchen.JavaScriptEval(@"function gtk(str) {
    var hash = 5381;
    for (var i = 0, len = str.length;i < len;++i) {
      hash += (hash << 5) + str.charCodeAt(i);
    }
    return hash & 2147483647;
  }", "gtk('" + p_skey + "')"); }
        #endregion
        
        #region 重定向初始化计算GTK值
        /// <summary>
        /// 初始化计算GTK
        /// </summary>
        public void initlogin()
        {
            string url = sub.NextUrl;
            web.SetReDirectState(false);
            string result = web.HttpGet(url);
            sub.GTK = GTK(web.getCookieValue("p_skey"));
            web.SetReDirectState(true);//取消禁止重定向
        }
        #endregion
    }
}
