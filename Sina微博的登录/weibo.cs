using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina微博的登录
{
    class loginsub
    {
        public string servertime { get; set; }
        public string nonce { get; set; }
        public string pubkey { get; set; }
    }
    class weibo
    {
        loginsub sub = new loginsub();
        qiuchenhelper qiuchen = new qiuchenhelper();
        HttpHelper web = new HttpHelper();

        /// <summary>
        /// 初始化登录
        /// </summary>
        public void init()
        {
            string url = @"https://login.sina.com.cn/sso/prelogin.php?entry=weibo&callback=sinaSSOController.preloginCallBack&su=&rsakt=mod&client=ssologin.js(v1.4.18)&_=" + qiuchen.getDataTime13();
            string result = web.HttpGet(url);
            //"sinaSSOController.preloginCallBack({\"retcode\":0,\"servertime\":1486281123,\"pcid\":\"gz-5f7f80fa5305469446210a9c12f6ca894ae6\",\"nonce\":\"7Q14WU\",\"pubkey\":\"EB2A38568661887FA180BDDB5CABD5F21C7BFD59C090CB2D245A87AC253062882729293E5506350508E7F9AA3BB77F4333231490F915F6D63C55FE2F08A49B353F444AD3993CACC02DB784ABBB8E42A9B1BBFFFB38BE18D78E87A0E41B9B8F73A928EE0CCEE1F6739884B9777E4FE9E88A1BBE495927AC4A799B3181D6442443\",\"rsakv\":\"1330428213\",\"exectime\":10})"
            sub.nonce = qiuchen.BetweenText(result, "\"nonce\":\"", "\"");
            sub.servertime = qiuchen.BetweenText(result, "\"servertime\":", ","); ;
            sub.pubkey = qiuchen.BetweenText(result, "\"pubkey\":\"","\""); ;
        }

        /// <summary>
        /// 登录微博，测试免码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public int loginweibo(string user,string pwd)
        {
            web.SetContentType("application/x-www-form-urlencoded");
            web.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36 OPR/42.0.2393.517");

            string url = @"http://login.sina.com.cn/sso/login.php?client=ssologin.js(v1.4.18)&_="+qiuchen.getDataTime13();
            string data = @"entry=weibo&gateway=1&from=&savestate=7&useticket=1&pagerefer=&vsnf=1&su=" + qiuchen.JavaScriptEval(Res.RSA, "Base64E('" + user + "')") + "&service=miniblog&servertime=1486279022&nonce=B0PK9F&pwencode=rsa2&rsakv=1330428213&sp=" + qiuchen.JavaScriptEval(Res.RSA, "getpwd('" + sub.servertime + "','" + sub.nonce + "','" + sub.pubkey + "','" + pwd + "')") + "&sr=1440*900&encoding=UTF-8&cdult=2&domain=weibo.com&prelt=202&returntype=TEXT";
            string result = web.HttpPost(url, data);
            //"{\"retcode\":\"101\",\"reason\":\"\\u767b\\u5f55\\u540d\\u6216\\u5bc6\\u7801\\u9519\\u8bef\"}"
            //账号忘了，抓不到正确的包。嗯。酱紫把。
            if (result.Contains("\"reason\":\"\\u767b\\u5f55\\u540d\\u6216\\u5bc6\\u7801\\u9519\\u8bef\"")==true)
            {
                return 1;
            }
            return 0;//预设值：0=登录成功，因为没有抓到包，暂时不做
        }
    }
}
