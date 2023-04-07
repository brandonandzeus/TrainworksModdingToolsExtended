using HarmonyLib;
using System;

namespace Trainworks.BuildersV2
{
    public class CardTraitDataBuilder
    {
        private Type traitStateType;

        /// <summary>
        /// Type of the trait class to instantiate.
        /// Implicitly sets TraitStateName.
        /// </summary>
        public Type TraitStateType
        {
            get { return traitStateType; }
            set
            {
                traitStateType = value;
                TraitStateName = traitStateType.AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// Name of the trait class to instantiate.
        /// Its generally better to use TraitStateType over TraitStateName, especially if
        /// TraitStateName is referring to an type in another Assembly or multiple types
        /// with the same name exists for some reason.
        /// </summary>
        public string TraitStateName { get; set; }

        /// <summary>
        /// CardData parameter.
        /// </summary>
        public CardData ParamCardData { get; set; }
        /// <summary>
        /// Type of card to target.
        /// </summary>
        public CardStatistics.CardTypeTarget ParamCardType { get; set; }
        /// <summary>
        /// Team to target.
        /// </summary>
        public Team.Type ParamTeamType { get; set; }

        /// <summary>
        /// Float parameter.
        /// </summary>
        public float ParamFloat { get; set; }
        /// <summary>
        /// Int parameter.
        /// </summary>
        public int ParamInt { get; set; }
        /// <summary>
        /// String parameter.
        /// </summary>
        public string ParamStr { get; set; }
        /// <summary>
        /// Subtype parameter.
        /// </summary>
        public string ParamSubtype { get; set; }

        /// <summary>
        /// Status effect array parameter.
        /// </summary>
        public StatusEffectStackData[] ParamStatusEffects { get; set; }

        public CardStatistics.EntryDuration ParamEntryDuration { get; set; }
        public CardStatistics.TrackedValueType ParamTrackedValue { get; set; }
        public bool ParamUseScalingParams { get; set; }
        /// <summary>
        /// Card Upgrade Data Parameter.
        /// </summary>
        public CardUpgradeData ParamCardUpgradeData { get; set; }
        /// <summary>
        /// Card Upgrade Data Builder. Overrides ParamCardUpgradeData if set.
        /// </summary>
        public CardUpgradeDataBuilder ParamCardUpgradeDataBuilder { get; set; }
        public bool TraitIsRemovable { get; set; }
        public CardTraitData.StackMode StackMode { get; set; }

        public CardTraitDataBuilder()
        {
            ParamStatusEffects = new StatusEffectStackData[0];
        }

        /// <summary>
        /// Builds the CardTraitData represented by this builder's parameters recursively;
        /// all Builders represented in this class's various fields will also be built.
        /// </summary>
        /// <returns>The newly created CardTraitData</returns>
        public CardTraitData Build()
        {
            CardTraitData cardTraitData = new CardTraitData();
            AccessTools.Field(typeof(CardTraitData), "paramCardData").SetValue(cardTraitData, ParamCardData);
            AccessTools.Field(typeof(CardTraitData), "paramCardType").SetValue(cardTraitData, ParamCardType);
            if (ParamCardUpgradeDataBuilder == null)
            {
                AccessTools.Field(typeof(CardTraitData), "paramCardUpgradeData").SetValue(cardTraitData, ParamCardUpgradeData);
            }
            else
            {
                AccessTools.Field(typeof(CardTraitData), "paramCardUpgradeData").SetValue(cardTraitData, ParamCardUpgradeDataBuilder.Build());
            }

            AccessTools.Field(typeof(CardTraitData), "paramEntryDuration").SetValue(cardTraitData, ParamEntryDuration);
            AccessTools.Field(typeof(CardTraitData), "paramFloat").SetValue(cardTraitData, ParamFloat);
            AccessTools.Field(typeof(CardTraitData), "paramInt").SetValue(cardTraitData, ParamInt);
            AccessTools.Field(typeof(CardTraitData), "paramStatusEffects").SetValue(cardTraitData, ParamStatusEffects);
            AccessTools.Field(typeof(CardTraitData), "paramStr").SetValue(cardTraitData, ParamStr);
            AccessTools.Field(typeof(CardTraitData), "paramSubtype").SetValue(cardTraitData, ParamSubtype);
            AccessTools.Field(typeof(CardTraitData), "paramTeamType").SetValue(cardTraitData, ParamTeamType);
            AccessTools.Field(typeof(CardTraitData), "paramTrackedValue").SetValue(cardTraitData, ParamTrackedValue);
            AccessTools.Field(typeof(CardTraitData), "paramUseScalingParams").SetValue(cardTraitData, ParamUseScalingParams);
            AccessTools.Field(typeof(CardTraitData), "traitIsRemovable").SetValue(cardTraitData, TraitIsRemovable);
            AccessTools.Field(typeof(CardTraitData), "traitStateName").SetValue(cardTraitData, TraitStateName);
            AccessTools.Field(typeof(CardTraitData), "stackMode").SetValue(cardTraitData, StackMode);
            return cardTraitData;
        }

        /// <summary>
        /// Add a status effect to this effect's status effect array.
        /// </summary>
        /// <param name="statusEffectID">ID of the status effect, most easily retrieved using the helper class "MTStatusEffectIDs"</param>
        /// <param name="stackCount">Number of stacks to apply</param>
        public void AddStatusEffect(string statusEffectID, int stackCount)
        {
            ParamStatusEffects = BuilderUtils.AddStatusEffect(statusEffectID, stackCount, ParamStatusEffects);
        }
    }
}