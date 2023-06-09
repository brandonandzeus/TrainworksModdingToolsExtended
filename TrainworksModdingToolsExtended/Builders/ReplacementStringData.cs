using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trainworks.BuildersV2
{
    class ReplacementStringDataBuilder
    {
        public string Keyword { get; set; }
        public string ReplacementTextKey { get; set; }
        public ReplacementStringData Build()
        {
            ReplacementStringData replacementStringData = new ReplacementStringData();
            AccessTools.Field(typeof(ReplacementStringData), "_keyword").SetValue(replacementStringData, Keyword);
            AccessTools.Field(typeof(ReplacementStringData), "_replacement").SetValue(replacementStringData, ReplacementTextKey);
            return replacementStringData;
        }
    }
}
