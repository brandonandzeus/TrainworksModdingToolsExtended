using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trainworks.Managers;
using UnityEngine;

namespace Trainworks.BuildersV2
{
    public class CardUpgradeDataBuilder
    {
        private string upgradeTitle;

        /// <summary>
        /// Overrides UpgradeTitleKey
        /// Implicitly sets UpgradeTitleKey, UpgradeDescriptionKey, and UpgradeNotificationKey if null
        /// </summary>
        public string UpgradeTitle
        {
            get { return upgradeTitle; }
            set
            {
                upgradeTitle = value;
                if (UpgradeTitleKey == null)
                {
                    UpgradeTitleKey = upgradeTitle + "_CardUpgradeData_UpgradeTitleKey";
                }
                if (UpgradeDescriptionKey == null)
                {
                    UpgradeDescriptionKey = upgradeTitle + "_CardUpgradeData_UpgradeDescriptionKey";
                }
                if (UpgradeNotificationKey == null)
                {
                    UpgradeNotificationKey = upgradeTitle + "_CardUpgradeData_UpgradeNotificationKey";
                }
            }
        }
        /// <summary>
        /// Note that setting this property will set the localization for all languages.
        /// </summary>
        public string UpgradeDescription { get; set; }
        /// <summary>
        /// Note that setting this property will set the localization for all languages.
        /// </summary>
        public string UpgradeNotification { get; set; }
        public string UpgradeTitleKey { get; set; }
        public string UpgradeDescriptionKey { get; set; }
        public string UpgradeNotificationKey { get; set; }
        public string UpgradeIconPath { get; set; }
        public bool HideUpgradeIconOnCard { get; set; }
        public bool UseUpgradeHighlightTextTags { get; set; }
        public int BonusDamage { get; set; }
        public int BonusHP { get; set; }
        public int CostReduction { get; set; }
        public int XCostReduction { get; set; }
        public int BonusHeal { get; set; }
        public int BonusSize { get; set; }

        public List<CardTraitDataBuilder> TraitDataUpgradeBuilders { get; set; }
        public List<CharacterTriggerDataBuilder> TriggerUpgradeBuilders { get; set; }
        public List<CardTriggerEffectDataBuilder> CardTriggerUpgradeBuilders { get; set; }
        public List<RoomModifierDataBuilder> RoomModifierUpgradeBuilders { get; set; }
        public List<CardUpgradeMaskDataBuilder> FiltersBuilders { get; set; }
        public List<CardUpgradeDataBuilder> UpgradesToRemoveBuilders { get; set; }

        /// <summary>
        /// To add a status effect, no need for a builder. new StatusEffectStackData with properties statusId (string) and count (int) are sufficient.
        /// Get the string with -> statusEffectID = VanillaStatusEffectIDs.GetIDForType(statusEffectType);
        /// </summary>
        public List<StatusEffectStackData> StatusEffectUpgrades { get; set; }
        public List<CardTraitData> TraitDataUpgrades { get; set; }
        public List<string> RemoveTraitUpgrades { get; set; }
        public List<CharacterTriggerData> TriggerUpgrades { get; set; }
        public List<CardTriggerEffectData> CardTriggerUpgrades { get; set; }
        public List<RoomModifierData> RoomModifierUpgrades { get; set; }
        public List<CardUpgradeMaskData> Filters { get; set; }
        public List<CardUpgradeData> UpgradesToRemove { get; set; }

        public CharacterData SourceSynthesisUnit { get; set; }
        public bool IsUnitSynthesisUpgrade { get => SourceSynthesisUnit != null; }

        public string BaseAssetPath { get; set; }
        public object IsUnique { get; set; }
        public object LinkedPactDuplicateRarity { get; set; }

        public CardUpgradeDataBuilder()
        {
            UpgradeNotificationKey = null;
            UseUpgradeHighlightTextTags = true;

            TraitDataUpgradeBuilders = new List<CardTraitDataBuilder>();
            TriggerUpgradeBuilders = new List<CharacterTriggerDataBuilder>();
            CardTriggerUpgradeBuilders = new List<CardTriggerEffectDataBuilder>();
            RoomModifierUpgradeBuilders = new List<RoomModifierDataBuilder>();
            FiltersBuilders = new List<CardUpgradeMaskDataBuilder>();
            UpgradesToRemoveBuilders = new List<CardUpgradeDataBuilder>();

            StatusEffectUpgrades = new List<StatusEffectStackData>();
            TraitDataUpgrades = new List<CardTraitData>();
            RemoveTraitUpgrades = new List<string>();
            TriggerUpgrades = new List<CharacterTriggerData>();
            CardTriggerUpgrades = new List<CardTriggerEffectData>();
            RoomModifierUpgrades = new List<RoomModifierData>();
            Filters = new List<CardUpgradeMaskData>();
            UpgradesToRemove = new List<CardUpgradeData>();

            var assembly = Assembly.GetCallingAssembly();
            BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
        }

        public CardUpgradeData Build()
        {
            CardUpgradeData cardUpgradeData = ScriptableObject.CreateInstance<CardUpgradeData>();

            foreach (var builder in TraitDataUpgradeBuilders)
            {
                TraitDataUpgrades.Add(builder.Build());
            }
            foreach (var builder in TriggerUpgradeBuilders)
            {
                TriggerUpgrades.Add(builder.Build());
            }
            foreach (var builder in CardTriggerUpgradeBuilders)
            {
                CardTriggerUpgrades.Add(builder.Build());
            }
            foreach (var builder in RoomModifierUpgradeBuilders)
            {
                RoomModifierUpgrades.Add(builder.Build());
            }
            foreach (var builder in FiltersBuilders)
            {
                Filters.Add(builder.Build());
            }
            foreach (var builder in UpgradesToRemoveBuilders)
            {
                UpgradesToRemove.Add(builder.Build());
            }

            AccessTools.Field(typeof(CardUpgradeData), "bonusDamage").SetValue(cardUpgradeData, BonusDamage);
            AccessTools.Field(typeof(CardUpgradeData), "bonusHeal").SetValue(cardUpgradeData, BonusHeal);
            AccessTools.Field(typeof(CardUpgradeData), "bonusHP").SetValue(cardUpgradeData, BonusHP);
            AccessTools.Field(typeof(CardUpgradeData), "bonusSize").SetValue(cardUpgradeData, BonusSize);
            AccessTools.Field(typeof(CardUpgradeData), "cardTriggerUpgrades").SetValue(cardUpgradeData, CardTriggerUpgrades);
            AccessTools.Field(typeof(CardUpgradeData), "costReduction").SetValue(cardUpgradeData, CostReduction);
            AccessTools.Field(typeof(CardUpgradeData), "filters").SetValue(cardUpgradeData, Filters);
            AccessTools.Field(typeof(CardUpgradeData), "hideUpgradeIconOnCard").SetValue(cardUpgradeData, HideUpgradeIconOnCard);
            AccessTools.Field(typeof(CardUpgradeData), "removeTraitUpgrades").SetValue(cardUpgradeData, RemoveTraitUpgrades);
            AccessTools.Field(typeof(CardUpgradeData), "roomModifierUpgrades").SetValue(cardUpgradeData, RoomModifierUpgrades);
            AccessTools.Field(typeof(CardUpgradeData), "statusEffectUpgrades").SetValue(cardUpgradeData, StatusEffectUpgrades);
            AccessTools.Field(typeof(CardUpgradeData), "traitDataUpgrades").SetValue(cardUpgradeData, TraitDataUpgrades);
            AccessTools.Field(typeof(CardUpgradeData), "triggerUpgrades").SetValue(cardUpgradeData, TriggerUpgrades);
            BuilderUtils.ImportStandardLocalization(UpgradeDescriptionKey, UpgradeDescription);
            AccessTools.Field(typeof(CardUpgradeData), "upgradeDescriptionKey").SetValue(cardUpgradeData, UpgradeDescriptionKey);
            if (UpgradeIconPath != null && UpgradeIconPath != "")
            {
                AccessTools.Field(typeof(CardUpgradeData), "upgradeIcon").SetValue(cardUpgradeData, CustomAssetManager.LoadSpriteFromPath(BaseAssetPath + "/" + UpgradeIconPath));
            }

            BuilderUtils.ImportStandardLocalization(UpgradeNotificationKey, UpgradeNotification);
            AccessTools.Field(typeof(CardUpgradeData), "upgradeNotificationKey").SetValue(cardUpgradeData, UpgradeNotificationKey);
            AccessTools.Field(typeof(CardUpgradeData), "upgradesToRemove").SetValue(cardUpgradeData, UpgradesToRemove);
            BuilderUtils.ImportStandardLocalization(UpgradeTitleKey, UpgradeTitle);
            AccessTools.Field(typeof(CardUpgradeData), "upgradeTitleKey").SetValue(cardUpgradeData, UpgradeTitleKey);
            AccessTools.Field(typeof(CardUpgradeData), "useUpgradeHighlightTextTags").SetValue(cardUpgradeData, UseUpgradeHighlightTextTags);
            AccessTools.Field(typeof(CardUpgradeData), "xCostReduction").SetValue(cardUpgradeData, XCostReduction);

            AccessTools.Field(typeof(CardUpgradeData), "isUnitSynthesisUpgrade").SetValue(cardUpgradeData, IsUnitSynthesisUpgrade);
            AccessTools.Field(typeof(CardUpgradeData), "sourceSynthesisUnit").SetValue(cardUpgradeData, SourceSynthesisUnit);
            AccessTools.Field(typeof(CardUpgradeData), "isUnique").SetValue(cardUpgradeData, IsUnique);
            AccessTools.Field(typeof(CardUpgradeData), "linkedPactDuplicateRarity").SetValue(cardUpgradeData, LinkedPactDuplicateRarity);

            cardUpgradeData.name = UpgradeTitleKey;
            Traverse.Create(cardUpgradeData).Field("id").SetValue(UpgradeTitleKey);

            // If CardUpgrades are not added to allGameData, there are many troubles.
            var field = Traverse.Create(ProviderManager.SaveManager.GetAllGameData()).Field("cardUpgradeDatas");
            var upgradeList = field.GetValue<List<CardUpgradeData>>();

            // If upgrade already exists, update it by removing the previously added version
            // This might happen if Build() is called twice (e.g. when defining a synthesis for a unit and calling Build())
            var existingEntry = upgradeList
                .Where(u => UpgradeTitleKey == (string)AccessTools.Field(typeof(CardUpgradeData), "upgradeTitleKey").GetValue(u))
                .FirstOrDefault();

            upgradeList.Remove(existingEntry);

            upgradeList.Add(cardUpgradeData);

            return cardUpgradeData;
        }
    }
}