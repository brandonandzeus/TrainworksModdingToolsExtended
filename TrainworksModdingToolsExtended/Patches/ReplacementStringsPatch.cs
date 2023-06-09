using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.Managers;

namespace Trainworks.Patches
{
    [HarmonyPatch(typeof(LocalizationGlobalParameterHandler), "InitDict")]
    class ReplacementStringsPatch
    {
        public static void Postfix(ref ReplacementStringsData ____replacementStringsData, ref Dictionary<string, ReplacementStringData> ____replacements)
        {
            var replacementStrings = (Malee.ReorderableArray<ReplacementStringData>)AccessTools.Field(typeof(ReplacementStringsData), "_replacementStrings").GetValue(____replacementStringsData);

            foreach (var replacementString in CustomReplacementStringManager.CustomReplacementStrings)
            {
                ____replacements[replacementString.Key] = replacementString.Value;
                replacementStrings.Add(replacementString.Value);
            }
        }
    }
}
