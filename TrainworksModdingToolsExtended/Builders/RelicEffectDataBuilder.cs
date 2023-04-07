using HarmonyLib;
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

        public List<RelicEffectConditionBuilder> EffectConditionBuilders { get; set; }
        public List<CardTriggerType> CardTriggers { get; set; }
        public List<CardTraitData> Traits { get; set; }
        public List<CardTraitDataBuilder> TraitBuilders { get; set; }
        public List<CharacterTriggerData> Triggers { get; set; }
        public List<CharacterTriggerDataBuilder> TriggerBuilders { get; set; }
        public List<RelicEffectCondition> EffectConditions { get; set; }

        public bool ParamBool { get; set; }
        public List<CardEffectData> ParamCardEffects { get; set; }
        public List<CardEffectDataBuilder> ParamCardEffectBuilders { get; set; }
        public CardUpgradeMaskData ParamCardFilter { get; set; }
        public CardPool ParamCardPool { get; set; }
        public CardSetBuilder ParamCardSetBuilder { get; set; }
        public CardType ParamCardType { get; set; }
        public CardUpgradeData ParamCardUpgradeData { get; set; }
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
            ParamExcludeCharacterSubtypes = new string[0];
            ExcludedTraits = new List<CardTraitData>();
            ParamStatusEffects = new StatusEffectStackData[0];
            AdditionalTooltips = new AdditionalTooltipData[0];
            EffectConditionBuilders = new List<RelicEffectConditionBuilder>();
            TraitBuilders = new List<CardTraitDataBuilder>();
            TriggerBuilders = new List<CharacterTriggerDataBuilder>();
            ParamCardEffectBuilders = new List<CardEffectDataBuilder>();
            CardTriggers = new List<CardTriggerType>();
        }

        /// <summary>
        /// Builds the RelicEffectData represented by this builder's parameters recursively;
        /// all Builders represented in this class's various fields will also be built.
        /// </summary>
        /// <returns>The newly created RelicEffectData</returns>
        public RelicEffectData Build()
        {
            foreach (var builder in EffectConditionBuilders)
            {
                EffectConditions.Add(builder.Build());
            }
            foreach (var builder in TraitBuilders)
            {
                Traits.Add(builder.Build());
            }
            foreach (var builder in TriggerBuilders)
            {
                Triggers.Add(builder.Build());
            }
            foreach (var builder in ParamCardEffectBuilders)
            {
                ParamCardEffects.Add(builder.Build());
            }

            RelicEffectData relicEffectData = new RelicEffectData();
            AccessTools.Field(typeof(RelicEffectData), "additionalTooltips").SetValue(relicEffectData, AdditionalTooltips);
            AccessTools.Field(typeof(RelicEffectData), "appliedVfx").SetValue(relicEffectData, AppliedVfx);
            AccessTools.Field(typeof(RelicEffectData), "cardTriggers").SetValue(relicEffectData, CardTriggers);
            AccessTools.Field(typeof(RelicEffectData), "effectConditions").SetValue(relicEffectData, EffectConditions);
            AccessTools.Field(typeof(RelicEffectData), "excludedTraits").SetValue(relicEffectData, ExcludedTraits);
            AccessTools.Field(typeof(RelicEffectData), "paramBool").SetValue(relicEffectData, ParamBool);
            AccessTools.Field(typeof(RelicEffectData), "paramCardEffects").SetValue(relicEffectData, ParamCardEffects);
            AccessTools.Field(typeof(RelicEffectData), "paramCardFilter").SetValue(relicEffectData, ParamCardFilter);
            AccessTools.Field(typeof(RelicEffectData), "paramCardPool").SetValue(relicEffectData, ParamCardPool);
            AccessTools.Field(typeof(RelicEffectData), "paramCardSetBuilder").SetValue(relicEffectData, ParamCardSetBuilder);
            AccessTools.Field(typeof(RelicEffectData), "paramCardType").SetValue(relicEffectData, ParamCardType);
            AccessTools.Field(typeof(RelicEffectData), "paramCardUpgradeData").SetValue(relicEffectData, ParamCardUpgradeData);
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
            AccessTools.Field(typeof(RelicEffectData), "traits").SetValue(relicEffectData, Traits);
            AccessTools.Field(typeof(RelicEffectData), "triggers").SetValue(relicEffectData, Triggers);
            AccessTools.Field(typeof(RelicEffectData), "triggerTooltipsSuppressed").SetValue(relicEffectData, TriggerTooltipsSuppressed);


            BuilderUtils.ImportStandardLocalization(TooltipBodyKey, TooltipBody);
            BuilderUtils.ImportStandardLocalization(TooltipTitleKey, TooltipTitle);

            return relicEffectData;
        }

        /// <summary>
        /// Add a status effect to this effect's status effect array.
        /// </summary>
        /// <param name="statusEffectID">ID of the status effect, most easily retrieved using the helper class "VanillaStatusEffectIDs"</param>
        /// <param name="stackCount">Number of stacks to apply</param>
        public void AddStatusEffect(string statusEffectID, int stackCount)
        {
            ParamStatusEffects = BuilderUtils.AddStatusEffect(statusEffectID, stackCount, ParamStatusEffects);
        }
    }
}
