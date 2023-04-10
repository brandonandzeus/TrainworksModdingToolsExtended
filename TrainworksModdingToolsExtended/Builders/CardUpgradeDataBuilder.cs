using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trainworks.Managers;
using Trainworks.Utilities;
using UnityEngine;

namespace Trainworks.BuildersV2
{
    public class CardUpgradeDataBuilder
    {
        private string upgradeTitle;

        /// <summary>
        /// UpgradeTitle
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
        /// <summary>
        /// Upgrade Title Key for localization.
        /// No need to set this as its automatically set by UpgradeTitle
        /// </summary>
        public string UpgradeTitleKey { get; set; }
        /// <summary>
        /// Upgrade Description Key for localization.
        /// No need to set this as its automatically set by UpgradeTitle
        /// </summary>
        public string UpgradeDescriptionKey { get; set; }
        /// <summary>
        /// Upgrade Notification Key for localization.
        /// No need to set this as its automatically set by UpgradeTitle
        /// </summary>
        public string UpgradeNotificationKey { get; set; }
        public bool HideUpgradeIconOnCard { get; set; }
        public bool UseUpgradeHighlightTextTags { get; set; }
        /// <summary>
        /// Bonus Damage for a Card or Unit. Note that this doesn't increase Heal amounts.
        /// </summary>
        public int BonusDamage { get; set; }
        /// <summary>
        /// Bonus HP for a unit.
        /// </summary>
        public int BonusHP { get; set; }
        /// <summary>
        /// Reduce ember to play card. Can be negative to increase Ember.
        /// </summary>
        public int CostReduction { get; set; }
        /// <summary>
        /// Reduce ember to play X Cost card. Can be negative.
        /// </summary>
        public int XCostReduction { get; set; }
        /// <summary>
        /// Bonus Healing
        /// </summary>
        public int BonusHeal { get; set; }
        /// <summary>
        /// Increases size of a unit. Can be negative to reduce size.
        /// </summary>
        public int BonusSize { get; set; }

        /// <summary>
        /// Convenience Builders for TraitDataUpgrade. Will be appended to the parameter.
        /// </summary>
        public List<CardTraitDataBuilder> TraitDataUpgradeBuilders { get; set; }
        /// <summary>
        /// Convenience Builders for TriggerUpgrade. Will be appended to the parameter.
        /// </summary>
        public List<CharacterTriggerDataBuilder> TriggerUpgradeBuilders { get; set; }
        /// <summary>
        /// Convenience Builders for CardTriggerUpgrade. Will be appended to the parameter.
        /// </summary>
        public List<CardTriggerEffectDataBuilder> CardTriggerUpgradeBuilders { get; set; }
        /// <summary>
        /// Convenience Builders for RoomModifierUpgrade. Will be appended to the parameter.
        /// </summary>
        public List<RoomModifierDataBuilder> RoomModifierUpgradeBuilders { get; set; }
        /// <summary>
        /// Convenience Builders for Filters. Will be appended to the parameter.
        /// </summary>
        public List<CardUpgradeMaskDataBuilder> FiltersBuilders { get; set; }
        /// <summary>
        /// Convenience Builders for UpgradesToRemove. Will be appended to the parameter.
        /// </summary>
        public List<CardUpgradeDataBuilder> UpgradesToRemoveBuilders { get; set; }

        /// <summary>
        /// Status Effects to apply to the unit.
        /// </summary>
        public List<StatusEffectStackData> StatusEffectUpgrades { get; set; }
        /// <summary>
        /// Card Traits to add/
        /// </summary>
        public List<CardTraitData> TraitDataUpgrades { get; set; }
        /// <summary>
        /// Traits to Remove when applied to a card.
        /// </summary>
        public List<string> RemoveTraitUpgrades { get; set; }
        /// <summary>
        /// When applied to a unit, additional Triggers to add.
        /// </summary>
        public List<CharacterTriggerData> TriggerUpgrades { get; set; }
        /// <summary>
        /// Card trigger upgrades to add to card.
        /// </summary>
        public List<CardTriggerEffectData> CardTriggerUpgrades { get; set; }
        /// <summary>
        /// Room Modifiers to add to a unit.
        /// </summary>
        public List<RoomModifierData> RoomModifierUpgrades { get; set; }
        /// <summary>
        /// Filters which can filter out cards to that the upgrade can be applied to.
        /// </summary>
        public List<CardUpgradeMaskData> Filters { get; set; }
        /// <summary>
        /// Upgrades to remove when this upgrade is applied.
        /// </summary>
        public List<CardUpgradeData> UpgradesToRemove { get; set; }
        /// <summary>
        /// Indicate that this CardUpgrade is a Synthesis by setting the CharacterData for it.
        /// </summary>
        public CharacterData SourceSynthesisUnit { get; set; }
        public bool IsUnitSynthesisUpgrade { get => SourceSynthesisUnit != null; }
        /// <summary>
        /// Indicates that the upgrade can only be applied to a card/unit once.
        /// </summary>
        public bool IsUnique { get; set; }
        /// <summary>
        /// Specifies how much pact shards should be gained for duplicating the card.
        /// </summary>
        public CollectableRarity LinkedPactDuplicateRarity { get; set; }

        /// <summary>
        /// The full, absolute path to the asset.
        /// </summary>
        public string FullAssetPath => BaseAssetPath + "/" + AssetPath;
        /// <summary>
        /// Set automatically in the constructor. Base asset path, usually the plugin directory.
        /// </summary>
        public string BaseAssetPath { get; private set; }
        /// <summary>
        /// Custom asset path to load from relative to the plugin's path
        /// </summary>
        public string AssetPath { get; set; }

        public CardUpgradeDataBuilder()
        {
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
            LinkedPactDuplicateRarity = CollectableRarity.Starter;

            var assembly = Assembly.GetCallingAssembly();
            BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
        }

        /// <summary>
        /// Builds and automatically registers the CardUpgradeData.
        /// </summary>
        /// <returns>CardUpgradeData</returns>
        public CardUpgradeData Build()
        {
            CardUpgradeData cardUpgradeData = ScriptableObject.CreateInstance<CardUpgradeData>();
            cardUpgradeData.name = UpgradeTitleKey;
            var guid = GUIDGenerator.GenerateDeterministicGUID(UpgradeTitleKey);
            AccessTools.Field(typeof(GameData), "id").SetValue(cardUpgradeData, guid);
            AccessTools.Field(typeof(CardUpgradeData), "bonusDamage").SetValue(cardUpgradeData, BonusDamage);
            AccessTools.Field(typeof(CardUpgradeData), "bonusHeal").SetValue(cardUpgradeData, BonusHeal);
            AccessTools.Field(typeof(CardUpgradeData), "bonusHP").SetValue(cardUpgradeData, BonusHP);
            AccessTools.Field(typeof(CardUpgradeData), "bonusSize").SetValue(cardUpgradeData, BonusSize);
            AccessTools.Field(typeof(CardUpgradeData), "costReduction").SetValue(cardUpgradeData, CostReduction);
            AccessTools.Field(typeof(CardUpgradeData), "hideUpgradeIconOnCard").SetValue(cardUpgradeData, HideUpgradeIconOnCard);
            AccessTools.Field(typeof(CardUpgradeData), "upgradeDescriptionKey").SetValue(cardUpgradeData, UpgradeDescriptionKey);
            AccessTools.Field(typeof(CardUpgradeData), "upgradeNotificationKey").SetValue(cardUpgradeData, UpgradeNotificationKey);
            AccessTools.Field(typeof(CardUpgradeData), "upgradeTitleKey").SetValue(cardUpgradeData, UpgradeTitleKey);
            AccessTools.Field(typeof(CardUpgradeData), "useUpgradeHighlightTextTags").SetValue(cardUpgradeData, UseUpgradeHighlightTextTags);
            AccessTools.Field(typeof(CardUpgradeData), "xCostReduction").SetValue(cardUpgradeData, XCostReduction);
            AccessTools.Field(typeof(CardUpgradeData), "isUnitSynthesisUpgrade").SetValue(cardUpgradeData, IsUnitSynthesisUpgrade);
            AccessTools.Field(typeof(CardUpgradeData), "sourceSynthesisUnit").SetValue(cardUpgradeData, SourceSynthesisUnit);
            AccessTools.Field(typeof(CardUpgradeData), "isUnique").SetValue(cardUpgradeData, IsUnique);
            AccessTools.Field(typeof(CardUpgradeData), "linkedPactDuplicateRarity").SetValue(cardUpgradeData, LinkedPactDuplicateRarity);

            var cardTriggers = cardUpgradeData.GetCardTriggerUpgrades();
            cardTriggers.AddRange(CardTriggerUpgrades);
            foreach (var builder in CardTriggerUpgradeBuilders)
                cardTriggers.Add(builder.Build());

            var filters = cardUpgradeData.GetFilters();
            filters.AddRange(Filters);
            foreach (var builder in FiltersBuilders)
                filters.Add(builder.Build());

            cardUpgradeData.GetRemoveTraitUpgrades().AddRange(RemoveTraitUpgrades);

            var roomModifiers = cardUpgradeData.GetRoomModifierUpgrades();
            roomModifiers.AddRange(RoomModifierUpgrades);
            foreach (var builder in RoomModifierUpgradeBuilders)
                roomModifiers.Add(builder.Build());

            cardUpgradeData.GetStatusEffectUpgrades().AddRange(StatusEffectUpgrades);

            var traitDatas = cardUpgradeData.GetTraitDataUpgrades();
            traitDatas.AddRange(TraitDataUpgrades);
            foreach (var builder in TraitDataUpgradeBuilders)
                traitDatas.Add(builder.Build());

            var triggers = cardUpgradeData.GetTriggerUpgrades();
            triggers.AddRange(TriggerUpgrades);
            foreach (var builder in TriggerUpgradeBuilders)
                triggers.Add(builder.Build());

            var upgradesToRemove = cardUpgradeData.GetUpgradesToRemove();
            upgradesToRemove.AddRange(UpgradesToRemove);
            foreach (var builder in UpgradesToRemoveBuilders)
                upgradesToRemove.Add(builder.Build());

            if (AssetPath != null)
            {
                AccessTools.Field(typeof(CardUpgradeData), "upgradeIcon").SetValue(cardUpgradeData, CustomAssetManager.LoadSpriteFromPath(FullAssetPath));
            }

            // TODO Make a CardUpgradeManager.
            var upgradeList = ProviderManager.SaveManager.GetAllGameData().GetAllCardUpgradeData();

            // If upgrade already exists, update it by removing the previously added version
            // This might happen if Build() is called twice (e.g. when defining a synthesis for a unit and calling Build())
            var existingEntry = upgradeList
                .Where(u => UpgradeTitleKey == u.GetUpgradeTitleKey())
                .FirstOrDefault();

            if (existingEntry != null)
            {
                upgradeList.Remove(existingEntry);
            }

            upgradeList.Add(cardUpgradeData);

            BuilderUtils.ImportStandardLocalization(UpgradeDescriptionKey, UpgradeDescription);
            BuilderUtils.ImportStandardLocalization(UpgradeNotificationKey, UpgradeNotification);
            BuilderUtils.ImportStandardLocalization(UpgradeTitleKey, UpgradeTitle);

            return cardUpgradeData;
        }
    }
}