using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;
using Trainworks.Managers;

namespace Trainworks.ManagersV2
{
    public static class CustomUpgradeManager
    {
        public static IDictionary<string, CardUpgradeData> CustomUpgradeData = new Dictionary<string, CardUpgradeData>();
        public static IDictionary<CharacterData, CardUpgradeData> UnitSynthesisMapping = new Dictionary<CharacterData, CardUpgradeData>();

        public static void RegisterCustomUpgrade(CardUpgradeData upgrade)
        {
            if (!CustomUpgradeData.ContainsKey(upgrade.GetID()))
            {
                CustomUpgradeData.Add(upgrade.GetID(), upgrade);

                var upgradeList = ProviderManager.SaveManager.GetAllGameData().GetAllCardUpgradeData();
                var existingEntry = upgradeList.Where(u => upgrade.GetID() == u.GetID()).FirstOrDefault();

                if (existingEntry != null)
                {
                    upgradeList.Remove(existingEntry);
                }

                upgradeList.Add(upgrade);

                if (upgrade.IsUnitSynthesisUpgrade())
                {
                    RegisterUnitSynthesis(upgrade.GetSourceSynthesisUnit(), upgrade);
                }
            }
            else
            {
                Trainworks.Log(LogLevel.Warning, "Attempted to register duplicate upgrade data with name: " + upgrade.name);
            }
        }

        public static void RegisterUnitSynthesis(CharacterData characterData, CardUpgradeData cardUpgrade)
        {
            if (UnitSynthesisMapping.ContainsKey(characterData))
            {
                Trainworks.Log(LogLevel.Warning, "Attempted to register duplicate synthesis data for character: " + characterData.name);
                return;
            }
            UnitSynthesisMapping.Add(characterData, cardUpgrade);
        }

        public static CardUpgradeData GetUpgradeData(CharacterData characterData)
        {
            if (UnitSynthesisMapping.ContainsKey(characterData))
            {
                return UnitSynthesisMapping[characterData];
            }
            return null;
        }
    }
}
