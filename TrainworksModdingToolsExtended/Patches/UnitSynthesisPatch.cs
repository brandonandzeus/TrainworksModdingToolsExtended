using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.ManagersV2;

namespace Trainworks.Patches
{
    [HarmonyPatch(typeof(UnitSynthesisMapping), nameof(UnitSynthesisMapping.GetUpgradeData))]
    public class UnitSynthesisPatch
    {
        public static void Postfix(CharacterData characterData, ref CardUpgradeData __result)
        {
            var upgrade = CustomUpgradeManager.GetUpgradeData(characterData);
            if (upgrade != null)
            {
                __result = upgrade;
            }
        }
    }
}
