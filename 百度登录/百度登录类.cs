using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 百度登录
{
    class loginsub
    {
        public string bdtoken { get; set; }
        public string codeString { get; set; }
        public string vcodetype { get; set; }
        public string pubkey { get; set; }
        public string key { get; set; }
    }
    class 百度登录类
    {
        HttpHelper web = new HttpHelper();
        loginsub sub = new loginsub();
        qiuchenhelper qiuchen = new qiuchenhelper();

        public void init()
        {
            string url = @"https://www.baidu.com";
            //init
            web.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36 OPR/42.0.2393.517");
            web.SetEncoding(Encoding.UTF8);
            web.SetTimeOut(30000);
            //end
            web.HttpGet(url);
            url = @"https://passport.baidu.com/v2/api/?getapi&tpl=mn&apiver=v3";
            string str = web.HttpGet(url);
            sub.bdtoken = qiuchen.BetweenText(str, "\"token\" : \"", "\"");
        }

        public void checkverify(string user)
        {
            string url = @"https://passport.baidu.com/v2/api/?logincheck&token=" + sub.bdtoken + "&tpl=mn&apiver=v3&tt=1486021723872&sub_source=leadsetpwd&username=" + user.Trim().Replace("@", "%40") + "&isphone=false&dv=TtkAQQANAOQAAAAAAMQWAAcBAB7T0tLS0vCk5avsvv-y7bLiseG-htmG9pfkl-CP_ZkHAQAGy8vLy8vUCAEAAcsUAQAGy8rOhVacFwEABMvLy8ABAQAIy8vKk26ipBUCAQAGz8_NzPjKDAEAFd_u2uLU5Nbn0OLR5t_r0eXT4tfu3BMBABfLy56ejPeS5IXpwZ6n_KOTyP_HmsfssxIBAAHLCwEAGsvd3d21wbXFtoyjjPuM-9W31r_broDjjOHOAwEAKMvLy8vLy8vLy8vPsLCws8XFxcODg4OHh4eHgcHBwcIaGhocXFxcX4cWAQAEy8vLyw&callback=bd__cbs__axfx7f";
            string result = web.HttpGet(url);
            sub.codeString = qiuchen.BetweenText(result, "\"codeString\" : \"", "\"");
            sub.vcodetype = qiuchen.BetweenText(result, "\"vcodetype\" : \"", "\"");
        }
        public Image getverifyPic() { string url = @"https://passport.baidu.com/cgi-bin/genimage?" + sub.codeString; return Image.FromStream(web.HttpGetMemoryStream(url)); }
        public void getpubkey() {
            string url = @"https://passport.baidu.com/v2/getpublickey?token=" + sub.bdtoken + "&tpl=mn&apiver=v3&tt=1486021723865&gid=7D6587C-2DCE-408A-A2FF-D9EF4D191FB9&callback=bd__cbs__397xvb";
            url = web.HttpGet(url);
            sub.pubkey = qiuchen.BetweenText(url, "\"pubkey\":'", "'");
            sub.key = qiuchen.BetweenText(url, "\"key\":'", "'");
        }

        //gpd('123123','')

    }
}
