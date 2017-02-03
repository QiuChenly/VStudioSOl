using System;
using System.Net;
using System.IO;
using 

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
        int index = alltext.IndexOf(lefttext,0)+lefttext.Length;
        return alltext.Substring(index, alltext.IndexOf(righttext, index) - index);
    }
    public string JavaScriptEval(string CurrentDir, string EvalStr)
    {
        

    }

}
