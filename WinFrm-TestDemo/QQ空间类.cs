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

/// <summary>
/// C#重写QQ空间登录操作
/// 2017.02.03 秋城落叶
/// </summary>
namespace QQClass
{
    class QQloginsub
    {
        public string pt_login_sign { get; set; }

    }
    class QQ
    {
        private QQloginsub loginsub = new QQloginsub();
        private string cook { get; set; }
        /// <summary>
        /// 初始化pt-login-sign值
        /// </summary>
        public void init()
        {
            HttpHelper web = new HttpHelper();
            web.InitCookie();
            web.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.22 Safari/537.36 SE 2.X MetaSr 1.0");
            web.SetTimeOut( 10000);
            web.SetAccept("*/*");
            string str = web.HttpGet(@"https://xui.ptlogin2.qq.com/cgi-bin/xlogin?appid=522005705&daid=4&s_url=https://mail.qq.com/cgi-bin/login?vt=passport%26vm=wpt%26ft=loginpage%26target=&style=25&low_login=1");
            CookieCollection cookie = web.getCookieCollection();
            string a = cookie["pt_login_sig"].Value;



        }
        public bool CheckVerify(string uin)
        {

            return false;
        }
        #region //取出中间文本
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
        #region //JavaScriptEval计算
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
