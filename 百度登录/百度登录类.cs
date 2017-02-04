using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public string user { get; set; }
        public string verifyCode { get; set; }

    }
    class 百度登录类
    {
        HttpHelper web = new HttpHelper();
        loginsub sub = new loginsub();
        qiuchenhelper qiuchen = new qiuchenhelper();

        public void init()
        {
            string url = @"https://www.baidu.com";
            string str = web.HttpGet(url);
            web.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36 OPR/42.0.2393.517");
            web.SetEncoding(Encoding.UTF8);
            url = @"https://passport.baidu.com/v2/api/?getapi&tpl=mn&apiver=v3";
            str= web.HttpGet(url);
            sub.bdtoken = qiuchen.BetweenText(str, "\"token\" : \"", "\"");
        }

        public void checkverify(string user)
        {
            sub.user = qiuchen.UrlEnCode(user);
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
        /// <summary>
        /// 检查验证码是否正确,正确返回0,不正确返回1
        /// </summary>
        /// <param name="verifycode"></param>
        /// <returns></returns>
        public int checkverifyState(string verifycode) {
            //智障百度 中文验证码吔屎啦
            string url = @"https://passport.baidu.com/v2/?checkvcode&token="+sub.bdtoken+"&tpl=mn&apiver=v3&tt=1486198828012&verifycode="+qiuchen.UrlEnCode(verifycode.Trim())+"&codestring="+sub.codeString+"&callback=bd__cbs__pbeaxt";
            string result =web.HttpGet(url);
            if (result.Contains("\"no\": \"0\"")==true)
            {
                sub.verifyCode = qiuchen.UrlEnCode(verifycode.Trim());
                return 0;
            }
            return 1;
        }

        public int loginbaidu(string pwd) {
            string url = @"https://passport.baidu.com/v2/api/?login";
            pwd = qiuchen.JavaScriptEval(Res.RSA, "gpd('" + pwd + "','" + sub.pubkey + "')");
            string data = @"staticpage=https%3A%2F%2Fwww.baidu.com%2Fcache%2Fuser%2Fhtml%2Fv3Jump.html&charset=UTF-8&token=" + sub.bdtoken + "&tpl=mn&subpro=&apiver=v3&tt=1486199359918&codestring=" + sub.codeString + "&safeflg=0&u=https%3A%2F%2Fwww.baidu.com%2F&isPhone=false&detect=1&gid=F490492-8C0B-4F3A-889A-4A3DAC0734E0&quick_user=0&logintype=dialogLogin&logLoginType=pc_loginDialog&idc=&loginmerge=true&splogin=rate&username=" + sub.user + "&password=" + pwd + "&verifycode=" + sub.verifyCode + "&mem_pass=on&rsakey=" + sub.key + "&crypttype=12&ppui_logintime=9677&countrycode=&dv=TtkALwAsBMUAAAAAAMQWAAcBABzd3Nzc-KD0tfu87q_iveKy4bHu1onWpdCy37bCFgEABMvLy8sGAQAh0TQ0NDQhpfGw_rnrque457fktOvTjNOlwLLbvcSH6IzpBgEAIdE0NDQ0J3svbiBnNXQ5ZjlpOmo1DVINex5sBWMaWTZSNwYBACHRNDQ0NCV5LWwiZTd2O2Q7azhoNw9QD3kcbgdhGFs0UDUGAQAh0TQ0NDQlCl4fURZEBUgXSBhLG0R8I3wKbx10EmsoRyNGBgEAIdE0NDQ0JIren9GWxIXIl8iYy5vE_KP8iu-d9JLrqMejxgcBACDR0NDQ3D1pKGYhczJ_IH8vfCxzSxRLPVgqQyVcH3AUcQcBAB7T09PT3wJWF1keTA1AH0AQQxNMdCt0BGUWZRJ9D2sFAQAm0dEeH7CwsLCwvGVlMXA-eStqJ3gndyR0KxNME2UAcht9BEcoTCkWAQAEy8vLyxYBAATLy8vLBgEAI9Ph4eHh66D0tfu87q_iveKy4bHu1onWpse0x7Dfrcm52KvYFgEABMvLy8sFAQAk09Lc3bm5ubm5vqur_77wt-Wk6bbpueq65d2C3a3Mv8y71KbCBAEAIdPSuLmWlpaQmMyNw4TWl9qF2orZidbuse6b6I3_sdC92AsBABrL3d3dtcG1xbaMo4z7jPvVt9a_266A44zhzggBAAHLDAEAFNjp3eXT4tvi0eTU4dHo0uDW4tPhFAEABsvKzoVWnBcBAATLy8vAAQEACMvLypCY-p5hAgEABs_Pzcz4yhMBAA3IyLW1vY-y7diD3Oy3EgEAAcsDAQAoy8vLy8vLy8vLy8kdHR0eeXl5fz8_Pzs7Ozs9fX19fqampqDg4ODjOxYBAATLy8vLBAEAHt7fa2uRkZGUcCRlK2w-fzJtMmIxYT4GWQZjEWMMfgQBACHT0p2c2dnZ3_uv7qDntfS55rnpuuq1jdKN-IvunNKz3rsEAQAd396en9LS0tSQxIXLjN6f0o3SgtGB3ua55oDvnfAEAQAh09L6-66urqjBldSa3Y_Og9yD04DQj7fot8em1abRvsyoBwEAHtPS0tLV0obHic6c3ZDPkMCTw5yk-6TUtca1wq3fuwYBACPT4uLi4ujZjcyCxZfWm8Sby5jIl6_wr9--zb7JptSwwKHSoRYBAATLy8vLBgEAI9Pg4ODg6m05eDZxI2IvcC9_LHwjG0Qbawp5Cn0SYAR0FWYVBgEAI9Pi4uLi6TltLGIldzZ7JHsreCh3TxBPP14tXilGNFAgQTJBBgEAI9Ph4eHh7e25-Lbxo-Kv8K__rPyjm8Sb64r5iv2S4IT0leaVBgEAI9Pg4ODg7MOX1pjfjcyB3oHRgtKNteq1xaTXpNO8zqrau8i7FgEABMvLy8sGAQAh0TQ0NDQlw5fWmN-NzIHegdGC0o216rXDptS926LhjuqPBgEAIdE0NDQ0JqL2t_m-7K3gv-Cw47Ps1IvUose13LrDgO-L7gYBACHRNDQ0NCAqfj9xNmQlaDdoOGs7ZFwDXCpPPVQySwhnA2YFAQAi3dzPzj4-Pj4-GlRUAEEPSBpbFkkWRhVFGiJ9IlEkRitCNgcBACDR0dHR9aTwsf-46qvmuea25bXq0o3SpMGz2rzFhumN6A&callback=parent.bd__pcbs__yh4pkw";
            string result = web.HttpPost(url, data);
            string href = @"https://www.baidu.com/cache/user/html/v3Jump.html?" + qiuchen.BetweenText(result, "href += \"", "\"+accounts")+ "&accounts=";
            result = web.HttpPost(url, data);
            //登录成功 返回的数据
            if (result.Contains("err_no=0&callback=parent")==true)
            {
                return 1;
            }
            return 2;
        }

        }
}
