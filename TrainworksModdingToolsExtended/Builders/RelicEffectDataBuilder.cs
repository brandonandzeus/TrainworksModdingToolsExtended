﻿using HarmonyLib;
using System;
using System.Collections.Generic;

namespace Trainworks.BuildersV2
{
    public class RelicEffectDataBuilder
    {
        private Type relicEffectClassType;
        private string relicEffectClassName;

        /// <summary>
        /// Type of the relic effect class to instantiate.
        /// Implicitly sets RelicEffectClassName.
        /// </summary>
        public Type RelicEffectClassType
        {
            get { return relicEffectClassType; }
            set
            {
                relicEffectClassType = value;
                RelicEffectClassName = relicEffectClassType.AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// Name of the effect class to instantiate.
        /// Either pass an assembly qualified type name or use EffectStateType instead.
        /// Its generally better to use RelicEffectClassType over RelicEffectClassName, especially if
        /// RelicEffectClassName is referring to an effect in another Assembly or multiple classes
        /// with the same name exists for some reason.
        /// </summary>
        public string RelicEffectClassName
        {
            get { return relicEffectClassName; }
            set
            {
                relicEffectClassName = value;
                if (TooltipBodyKey == null)
                {
                    TooltipBodyKey = relicEffectClassName + "_RelicEffectData_TooltipBodyKey";
                }
                if (TooltipTitleKey == null)
                {
                    TooltipTitleKey = relicEffectClassName + "_RelicEffectData_TooltipTitleKey";
                }
            }
        }

        public List<CardTriggerType> CardTriggers { get; set; }
        public List<CardTraitData> Traits { get; set; }
        public List<CardTraitDataBuilder> TraitBuilders { get; set; }
        public List<CharacterTriggerData> Triggers { get; set; }
        public List<CharacterTriggerDataBuilder> TriggerBuilders { get; set; }
        public List<RelicEffectCondition> EffectConditions { get; set; }
        public List<RelicEffectConditionBuilder> EffectConditionBuilders { get; set; }
        public bool ParamBool { get; set; }
        public List<CardEffectData> ParamCardEffects { get; set; }
        public List<CardEffectDataBuilder> ParamCardEffectBuilders { get; set; }
        public CardUpgradeMaskData ParamCardFilter { get; set; }
        public CardUpgradeMaskDataBuilder ParamCardFilterBuilder { get; set; }
        public CardPool ParamCardPool { get; set; }
        public CardPoolBuilder ParamCardPoolBuilder { get; set; }
        public CardSetBuilder ParamCardSetBuilder { get; set; }
        public CardType ParamCardType { get; set; }
        public CardUpgradeData ParamCardUpgradeData { get; set; }
        public CardUpgradeDataBuilder ParamCardUpgradeDataBuilder { get; set; }
        public List<CharacterData> ParamCharacters { get; set; }
        public string ParamCharacterSubtype { get; set; }
        public EnhancerPool ParamEnhancerPool { get; set; }
        public string[] ParamExcludeCharacterSubtypes { get; set; }
        public float ParamFloat { get; set; }
        public int ParamInt { get; set; }
        public int ParamMaxInt { get; set; }
        public int ParamMinInt { get; set; }
        public bool ParamUseIntRange { get; set; }
        public RandomChampionPool ParamRandomChampionPool { get; set; }
        public CollectableRelicData ParamRelic { get; set; }
        public RewardData ParamReward { get; set; }
        public RoomData ParamRoomData { get; set; }
        public Team.Type ParamSourceTeam { get; set; }
        public SpecialCharacterType ParamSpecialCharacterType { get; set; }
        public StatusEffectStackData[] ParamStatusEffects { get; set; }
        public string ParamString { get; set; }
        public TargetMode ParamTargetMode { get; set; }
        public CharacterTriggerData.Trigger ParamTrigger { get; set; }
        public string SourceCardTraitParam { get; set; }
        public string TargetCardTraitParam { get; set; }
        public List<CardTraitData> ExcludedTraits { get; set; }
        public List<CardTraitDataBuilder> ExcludedTraitBuilders { get; set; }
        /// <summary>
        /// Note providing this value will set the localization for all languages
        /// </summary>
        public string TooltipBody { get; set; }
        /// <summary>
        /// Note providing this value will set the localization for all languages
        /// </summary>
        public string TooltipTitle { get; set; }
        /// <summary>
        /// Localization key for tooltip body. Default value is [RelicEffectClassName]_RelicEffectData_TooltipBodyKey.
        /// Note you shouldn't need to set this as its pre-set when setting the Type to instantiate.
        /// </summary>
        public string TooltipBodyKey { get; set; }
        /// <summary>
        /// Localization key for tooltip tody. Default value is [RelicEffectClassName]_RelicEffectData_TooltipTitleKey.
        /// Note you shouldn't need to set this as its pre-set when setting the Type to instantiate.
        /// </summary>
        public string TooltipTitleKey { get; set; }
        public bool TriggerTooltipsSuppressed { get; set; }
        public AdditionalTooltipData[] AdditionalTooltips { get; set; }
        public VfxAtLoc AppliedVfx { get; set; }

        public RelicEffectDataBuilder()
        {
            EffectConditions = new List<RelicEffectCondition>();
            Traits = new List<CardTraitData>();
            Triggers = new List<CharacterTriggerData>();
            ParamCardEffects = new List<CardEffectData>();
            ParamCharacters = new List<CharacterData>();
            ParamExcludeCharacterSubtypes = Array.Empty<string>();
            ExcludedTraits = new List<CardTraitData>();
            ParamStatusEffects = Array.Empty<StatusEffectStackData>();
            AdditionalTooltips = Array.Empty<AdditionalTooltipData>();
            EffectConditionBuilders = new List<RelicEffectConditionBuilder>();
            TraitBuilders = new List<CardTraitDataBuilder>();
            TriggerBuilders = new List<CharacterTriggerDataBuilder>();
            ParamCardEffectBuilders = new List<CardEffectDataBuilder>();
            CardTriggers = new List<CardTriggerType>();
            ExcludedTraitBuilders = new List<CardTraitDataBuilder>();
        }

        /// <summary>
        /// Builds the RelicEffectData represented by this builder's parameters recursively;
        /// all Builders represented in this class's various fields will also be built.
        /// </summary>
        /// <returns>The newly created RelicEffectData</returns>
        public RelicEffectData Build()
        {
            RelicEffectData relicEffectData = new RelicEffectData();

            // Saving an allocation by reusing the list that was allocated on initialization.
            var traits = relicEffectData.GetTraits();
            traits.AddRange(Traits);
            foreach (var builder in TraitBuilders)
            {
                traits.Add(builder.Build());
            }

            var triggers = relicEffectData.GetTriggers();
            triggers.AddRange(Triggers);
            foreach (var builder in TriggerBuilders)
            {
                triggers.Add(builder.Build());
            }

            var cardEffects = relicEffectData.GetParamCardEffectData();
            cardEffects.AddRange(ParamCardEffects);
            foreach (var builder in ParamCardEffectBuilders)
            {
                cardEffects.Add(builder.Build());
            }

            var cardTriggers = relicEffectData.GetCardTriggers();
            cardTriggers.AddRange(CardTriggers);

            var excludedTraits = relicEffectData.GetExcludedTraits();
            excludedTraits.AddRange(ExcludedTraits);
            foreach (var builder in ExcludedTraitBuilders)
            {
                excludedTraits.Add(builder.Build());
            }

            AccessTools.Field(typeof(RelicEffectData), "additionalTooltips").SetValue(relicEffectData, AdditionalTooltips);
            AccessTools.Field(typeof(RelicEffectData), "appliedVfx").SetValue(relicEffectData, AppliedVfx);
            AccessTools.Field(typeof(RelicEffectData), "paramBool").SetValue(relicEffectData, ParamBool);
            AccessTools.Field(typeof(RelicEffectData), "paramCardSetBuilder").SetValue(relicEffectData, ParamCardSetBuilder);
            AccessTools.Field(typeof(RelicEffectData), "paramCardType").SetValue(relicEffectData, ParamCardType);
            AccessTools.Field(typeof(RelicEffectData), "paramCharacters").SetValue(relicEffectData, ParamCharacters);
            AccessTools.Field(typeof(RelicEffectData), "paramCharacterSubtype").SetValue(relicEffectData, ParamCharacterSubtype);
            AccessTools.Field(typeof(RelicEffectData), "paramEnhancerPool").SetValue(relicEffectData, ParamEnhancerPool);
            AccessTools.Field(typeof(RelicEffectData), "paramExcludeCharacterSubtypes").SetValue(relicEffectData, ParamExcludeCharacterSubtypes);
            AccessTools.Field(typeof(RelicEffectData), "paramFloat").SetValue(relicEffectData, ParamFloat);
            AccessTools.Field(typeof(RelicEffectData), "paramInt").SetValue(relicEffectData, ParamInt);
            AccessTools.Field(typeof(RelicEffectData), "paramMaxInt").SetValue(relicEffectData, ParamMaxInt);
            AccessTools.Field(typeof(RelicEffectData), "paramMinInt").SetValue(relicEffectData, ParamMinInt);
            AccessTools.Field(typeof(RelicEffectData), "paramRandomChampionPool").SetValue(relicEffectData, ParamRandomChampionPool);
            AccessTools.Field(typeof(RelicEffectData), "paramRelic").SetValue(relicEffectData, ParamRelic);
            AccessTools.Field(typeof(RelicEffectData), "paramReward").SetValue(relicEffectData, ParamReward);
            AccessTools.Field(typeof(RelicEffectData), "paramRoomData").SetValue(relicEffectData, ParamRoomData);
            AccessTools.Field(typeof(RelicEffectData), "paramSourceTeam").SetValue(relicEffectData, ParamSourceTeam);
            AccessTools.Field(typeof(RelicEffectData), "paramSpecialCharacterType").SetValue(relicEffectData, ParamSpecialCharacterType);
            AccessTools.Field(typeof(RelicEffectData), "paramStatusEffects").SetValue(relicEffectData, ParamStatusEffects);
            AccessTools.Field(typeof(RelicEffectData), "paramString").SetValue(relicEffectData, ParamString);
            AccessTools.Field(typeof(RelicEffectData), "paramTargetMode").SetValue(relicEffectData, ParamTargetMode);
            AccessTools.Field(typeof(RelicEffectData), "paramTrigger").SetValue(relicEffectData, ParamTrigger);
            AccessTools.Field(typeof(RelicEffectData), "paramUseIntRange").SetValue(relicEffectData, ParamUseIntRange);
            AccessTools.Field(typeof(RelicEffectData), "relicEffectClassName").SetValue(relicEffectData, RelicEffectClassName);
            AccessTools.Field(typeof(RelicEffectData), "sourceCardTraitParam").SetValue(relicEffectData, SourceCardTraitParam);
            AccessTools.Field(typeof(RelicEffectData), "targetCardTraitParam").SetValue(relicEffectData, TargetCardTraitParam);
            AccessTools.Field(typeof(RelicEffectData), "tooltipBodyKey").SetValue(relicEffectData, TooltipBodyKey);
            AccessTools.Field(typeof(RelicEffectData), "tooltipTitleKey").SetValue(relicEffectData, TooltipTitleKey);
            AccessTools.Field(typeof(RelicEffectData), "triggerTooltipsSuppressed").SetValue(relicEffectData, TriggerTooltipsSuppressed);

            // Non allocated field
            var effectConditions = new List<RelicEffectCondition>();
            effectConditions.AddRange(EffectConditions);
            foreach (var builder in EffectConditionBuilders)
            {
                effectConditions.Add(builder.Build());
            }
            AccessTools.Field(typeof(RelicEffectData), "effectConditions").SetValue(relicEffectData, effectConditions);

            var upgrade = ParamCardUpgradeData;
            if (ParamCardUpgradeDataBuilder != null)
            {
                upgrade = ParamCardUpgradeDataBuilder.Build();
            }

            var filter = ParamCardFilter;
            if (ParamCardFilterBuilder != null)
            {
                filter = ParamCardFilterBuilder.Build();
            }

            var cardPool = ParamCardPool;
            if (ParamCardPoolBuilder != null)
            {
                cardPool = ParamCardPoolBuilder.BuildAndRegister();
            }

            AccessTools.Field(typeof(RelicEffectData), "paramCardUpgradeData").SetValue(relicEffectData, upgrade);
            AccessTools.Field(typeof(RelicEffectData), "paramCardFilter").SetValue(relicEffectData, filter);
            AccessTools.Field(typeof(RelicEffectData), "paramCardPool").SetValue(relicEffectData, cardPool);

            BuilderUtils.ImportStandardLocalization(TooltipBodyKey, TooltipBody);
            BuilderUtils.ImportStandardLocalization(TooltipTitleKey, TooltipTitle);

            return relicEffectData;
        }
    }
}
