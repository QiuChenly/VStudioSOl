using System;
using System.Net;
using System.IO;
using MSScriptControl;
using System.Text;

public class httphelper
{
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
    /// <summary>
    /// JS计算
    /// </summary>
    /// <param name="CurrentDir">js文件地址</param>
    /// <param name="EvalStr">计算公式 例如 "getpwd('pwd')"</param>
    /// <returns></returns>
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

}
