using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Drawing;

namespace bilibili网站登录
{
    class loginsub
    {
        public string hash { get; set; }
        public string pubkey { get; set; }
        public string user { get; set; }
        public string pwd { get; set; }
        public string NextUrl { get; set; }
    }
    class bilibili
    {
        loginsub sub = new loginsub();
        string result = "";
        string url = "";
        HttpHelper web = new HttpHelper();
        qiuchenhelper qiuchen = new qiuchenhelper();

        ///接口使用live.bilibili.comMINI登录接口 Cookie问题导致加密失效
        ///留待后续修复
        ///验证码读取出现断流——>类库问题，即将修复BUG
        #region 初始化登录必须参数
        /// <summary>
        /// 初始化登录必须参数
        /// </summary>
        public void init()
        {
            web.InitCookie();
            web.SetAccept("User-Agent:Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36 OPR/42.0.2393.517");
            web.AddHeader("X-Requested-With", "XMLHttpRequest");
            url = @"http://live.bilibili.com";
            //小姐姐の尖叫
            result = web.HttpGet(url);
            url = @"https://passport.bilibili.com/login?act=getkey&_=";
            result = web.HttpGet(url);
            sub.hash = qiuchen.BetweenText(result, "{\"hash\":\"", "\"");
            sub.pubkey = qiuchen.BetweenText(result, "\"key\":\"", "\"}");
        }
        #endregion

        #region 获取验证码图片
        public Image getVerify()
        {
            url = @"https://passport.bilibili.com/captcha?_=1486273174818";
            return Image.FromStream(web.HttpGetMemoryStream(url));
        }
        #endregion

        #region 登录比利比利
        /// <summary>
        /// 登录比利比利，返回值：0=ok，1=验证码の问题，2=密码の错误，3=账号の无效 4=加密后的密码已过期
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="errinfo"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public int loginbilibili(string user, string pwd, out string errinfo, string verifyCode = "")
        {
            pwd = qiuchen.UrlEnCode(qiuchen.JavaScriptEval(Res.RSA, "getpwd('" + sub.pubkey + "','" + sub.hash + "','" + pwd + "')"));
            url = @"https://passport.bilibili.com/ajax/miniLogin/login";
            string data = @"userid=" + qiuchen.UrlEnCode(user.Trim()) + "&pwd=" + pwd + "&captcha=" + verifyCode.Trim() + "&keep=1";
            web.SetEncoding(Encoding.UTF8);
            result = web.HttpPost(url, data);
            int errCode = 0;
            errinfo = qiuchen.BetweenText(result, "code\":", "}");
            if (result.Contains("{\"status\":true")==true)
            {
                sub.NextUrl = qiuchen.BetweenText(result, "\"crossDomain\":\"", "\"}");
                return errCode;
            }
            errCode = 110;
            //"{\"status\":true,\"ts\":1486276376,\"data\":{\"code\":0,\"crossDomain\":\"https://passport.biligame.com/crossDomain?DedeUserID"}}"
            //state=true LOGIN IN!
            switch (errinfo)
            {
                case "-105": errinfo = "验证码错误";
                    errCode = 1;
                    break;
                case "-618": errinfo = "昵称重复或含有非法字符";
                    break;
                case "-619": errinfo = "昵称不能小于3个字符或者大于30个字符"; break;
                case "-620": errinfo = "该昵称已被使用";
                    break;
                case "-622": errinfo = "Email已存在";
                    break;
                case "-625": errinfo = "密码错误次数过多";
                    break;
                case "-626": errinfo = "用户不存在";
                    errCode = 3;
                    break;
                case "-627": errinfo = "密码错误";
                    //"{\"status\":false,\"message\":{\"code\":-627}}"
                    errCode = 2;
                    break;
                case "-628": errinfo = "密码不能小于6个字符或大于16个字符";
                    break;
                case "-636": errinfo = "系统繁忙，稍后再试";
                    break;
                case "-645": errinfo = "昵称或密码过短";
                    break;
                case "-646": errinfo = "请输入正确的手机号";
                    break;
                case "-647": errinfo = "该手机已绑定另外一个账号";
                    break;
                case "-648": errinfo = "验证码发送失败";
                    break;
                case "-652": errinfo = "历史遗留问题，昵称与手机号重复，请联系管理员";
                    break;
                case "-662": errinfo = "加密后的密码已过期";
                    errCode = 4;
                    //"{\"status\":false,\"message\":{\"code\":-662}}"
                    break;
                default:
                    errinfo = "愚蠢的人类，本服务器已经没有能返回的数据了！";
                    break;
            }
            return errCode;
        }
        #endregion

    }
}
