using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trainworks.Managers
{
    public class CustomReplacementStringManager
    {
        public static readonly IDictionary<string, ReplacementStringData> CustomReplacementStrings = new Dictionary<string, ReplacementStringData>();

        public static void RegisterReplacementString(ReplacementStringData data)
        {
            if (!CustomReplacementStrings.ContainsKey(data.Keyword))
            {
                CustomReplacementStrings.Add(data.Keyword, data);
            }
            else
            {
                Trainworks.Log(LogLevel.Warning, "Attempted to register duplicate replacement string with keyword: " + data.Keyword);
            }
        }
    }
}
