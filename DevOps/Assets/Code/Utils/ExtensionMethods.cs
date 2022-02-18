using System.Collections.Generic;

public static class ExtensionMethods
{
    public const char SINGLE_PARAM_SPLITTER = ',';
    public const char PARAM_VALUE_SPLITTER  = ':';
    public static Dictionary<string, string> HandleParams(this string _params)
    {
        Dictionary<string, string> dict      = new Dictionary<string, string>();
        string[]                   paramList = _params.Split(SINGLE_PARAM_SPLITTER);
        foreach (var param in paramList)
        {
            string[] singleParam = param.Split(PARAM_VALUE_SPLITTER);
            dict.Add(singleParam[0],singleParam[1]);
        }

        return dict;
    }
}