using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MSScriptControl;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Drawing;

/// <summary>
/// C#重写QQ空间登录操作
/// 2017.02.03 秋城落叶
/// </summary>
namespace QQClass
{
    class QQloginsub
    {
        public string pt_login_sign { get; set; }
        public string cap_cd { get; set; }
        public string ptvfsession { get; set; }
        public string verifyCode { get; set; }
        public string uin { get; set; }
    }
    class QQ
    {
        private QQloginsub loginsub = new QQloginsub();
        private bool IsNeedCap;
        #region 初始化pt_login-sign
        /// <summary>
        /// 初始化pt-login-sign值
        /// </summary>
        public bool init()
        {
            HttpHelper web = new HttpHelper();
            web.InitCookie();
            web.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.22 Safari/537.36 SE 2.X MetaSr 1.0");
            web.SetTimeOut(10000);
            web.SetAccept("*/*");
            string str = web.HttpGet(@"https://xui.ptlogin2.qq.com/cgi-bin/xlogin?appid=522005705&daid=4&s_url=https://mail.qq.com/cgi-bin/login?vt=passport%26vm=wpt%26ft=loginpage%26target=&style=25&low_login=1");
            loginsub.pt_login_sign = web.getCookieValue("pt_login_sig");
            if (loginsub.pt_login_sign == "" || loginsub.pt_login_sign == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
#endregion

        #region 检查是否需要验证码
        /// <summary>
        /// 检查是否需要验证码 返回1表示不需要，返回2表示出现异常，返回3表示需要
        /// </summary>
        /// <param name="uin">QQ号</param>
        /// <returns>返回1表示不需要，返回2表示出现异常，返回3表示需要</returns>
        public int CheckVerify(string uin)
        {
            loginsub.uin = uin;
            string url = @"https://ssl.ptlogin2.qq.com/check?regmaster=&pt_tea=2&pt_vcode=0&uin=" + uin + "&appid=522005705&js_ver=10193&js_type=1&login_sig=" + loginsub.pt_login_sign + "&u1=https%3A%2F%2Fmail.qq.com%2Fcgi-bin%2Flogin%3Fvt%3Dpassport%26vm%3Dwpt%26ft%3Dloginpage%26target%3D&r=0.8285527726120261&pt_uistyle=25";
            HttpHelper web = new HttpHelper();
            web.SetAccept("*/*");
            web.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.22 Safari/537.36 SE 2.X MetaSr 1.0");
            string str = web.HttpGet(url);
            Regex regular = new Regex(@"checkVC\('(.*?)','(.*?)','(.*?)','(.*?)','(.*?)'\)", RegexOptions.None);
            Match match = regular.Match(str);
            if (match.Success)
            {
                if (match.Groups[2].Value.Contains("!") == true)
                {
                    loginsub.verifyCode = match.Groups[2].Value;
                    loginsub.cap_cd = match.Groups[4].Value;
                    IsNeedCap = false;
                    return 1;
                }
                else
                {
                    loginsub.verifyCode = "";
                    loginsub.cap_cd = match.Groups[2].Value;
                    IsNeedCap = true;
                    return 3;
                }
            }
            else
            {
                return 2;
            }
        }
        #endregion

        #region 切换图片验证码
        /// <summary>
        /// 切换验证码
        /// </summary>
        /// <returns>返回图片</returns>
        public Image getCapture()
        {
            string url = @"https://ssl.captcha.qq.com/getimage?uin=" + loginsub.uin + "&aid=522005705&cap_cd=" + loginsub.cap_cd + "&0.5807991355107538";
            HttpHelper web = new HttpHelper();
            MemoryStream stream = web.HttpGetMemoryStream(url);
            return Image.FromStream(stream);
        }
#endregion

        #region 取出中间文本
        /// <summary>
        /// 取出中间文本
        /// </summary>
        /// <param name="alltext">全部文本</param>
        /// <param name="lefttext"></param>
        /// <param name="righttext"></param>
        /// <returns>返回取到的文本</returns>
        public string BetweenText
            (string alltext, string lefttext, string righttext)
        {
            int index = alltext.IndexOf(lefttext, 0) + lefttext.Length;
            return alltext.Substring(index, alltext.IndexOf(righttext, index) - index);
        }
        #endregion

        #region JavaScriptEval计算
        /// <summary>
        /// JS计算
        /// </summary>
        /// <param name="CurrentDir">js文件地址</param>
        /// <param name="EvalStr">计算公式 例如 "getpwd('pwd')"</param>
        /// <returns>返回加密结果</returns>
        public string JavaScriptEval(string CurrentDir, string EvalStr)
        {
            ScriptControl script = new ScriptControl();
            StreamReader str = new StreamReader(CurrentDir, Encoding.Unicode);
            string stri = str.ReadToEnd();
            str.Close();
            str.Dispose();
            script.Language = "JavaScript";
            script.AddCode(stri);
            stri = script.Eval(EvalStr);
            return stri;
        }
        #endregion
    }
}
