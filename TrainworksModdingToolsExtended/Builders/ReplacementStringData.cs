using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.Managers;

namespace Trainworks.BuildersV2
{
    public class ReplacementStringDataBuilder
    {
        public string Keyword { get; set; }
        public string ReplacementTextKey { get; set; }
        public ReplacementStringData BuildAndRegister()
        {
            var data = Build();
            CustomReplacementStringManager.RegisterReplacementString(data);
            return data;
        }

        public ReplacementStringData Build()
        {
            ReplacementStringData replacementStringData = new ReplacementStringData();
            AccessTools.Field(typeof(ReplacementStringData), "_keyword").SetValue(replacementStringData, Keyword);
            AccessTools.Field(typeof(ReplacementStringData), "_replacement").SetValue(replacementStringData, ReplacementTextKey);
            return replacementStringData;
        }
    }
}
