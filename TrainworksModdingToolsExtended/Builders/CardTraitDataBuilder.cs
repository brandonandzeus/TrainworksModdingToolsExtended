﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using Trainworks.ConstantsV2;

namespace Trainworks.BuildersV2
{
    public class CardTraitDataBuilder
    {
        /// <summary>
        /// Type of the trait class to instantiate.
        /// This should be a class inheriting from CardTraitState
        /// </summary>
        public Type TraitStateType { get; set; }
        /// <summary>
        /// CardTraitState class to instantiate.
        /// Note that this isn't a simple string name of the class it is the class name plus the Assembly info.
        /// </summary>
        public string TraitStateName => TraitStateType.AssemblyQualifiedName;
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
        public List<StatusEffectStackData> ParamStatusEffects { get; set; }
        public CardStatistics.EntryDuration ParamEntryDuration { get; set; }
        public CardStatistics.TrackedValueType ParamTrackedValue { get; set; }
        public bool ParamUseScalingParams { get; set; }
        /// <summary>
        /// Card Upgrade Data Parameter.
        /// </summary>
        public CardUpgradeData ParamCardUpgradeData { get; set; }
        /// <summary>
        /// Convenience Card Upgrade Data Builder. Overrides ParamCardUpgradeData if set.
        /// </summary>
        public CardUpgradeDataBuilder ParamCardUpgradeDataBuilder { get; set; }
        public bool TraitIsRemovable { get; set; }
        public CardTraitData.StackMode StackMode { get; set; }

        public CardTraitDataBuilder()
        {
            ParamStatusEffects = new List<StatusEffectStackData>();
            ParamFloat = 1f;
            TraitIsRemovable = true;
            ParamTeamType = Team.Type.Heroes;
            ParamSubtype = VanillaSubtypeIDs.None;
        }

        /// <summary>
        /// Builds the CardTraitData represented by this builder's parameters
        /// all Builders represented in this class's various fields will also be built.
        /// </summary>
        /// <returns>The newly created CardTraitData</returns>
        public CardTraitData Build()
        {
            if (TraitStateType == null)
                throw new BuilderException("TraitStateType is required");

            // Doesn't inherit from ScriptableObject
            CardTraitData cardTraitData = new CardTraitData();
            AccessTools.Field(typeof(CardTraitData), "paramCardData").SetValue(cardTraitData, ParamCardData);
            AccessTools.Field(typeof(CardTraitData), "paramCardType").SetValue(cardTraitData, ParamCardType);
            AccessTools.Field(typeof(CardTraitData), "paramEntryDuration").SetValue(cardTraitData, ParamEntryDuration);
            AccessTools.Field(typeof(CardTraitData), "paramFloat").SetValue(cardTraitData, ParamFloat);
            AccessTools.Field(typeof(CardTraitData), "paramInt").SetValue(cardTraitData, ParamInt);
            AccessTools.Field(typeof(CardTraitData), "paramStatusEffects").SetValue(cardTraitData, ParamStatusEffects.ToArray());
            AccessTools.Field(typeof(CardTraitData), "paramStr").SetValue(cardTraitData, ParamStr);
            AccessTools.Field(typeof(CardTraitData), "paramSubtype").SetValue(cardTraitData, ParamSubtype);
            AccessTools.Field(typeof(CardTraitData), "paramTeamType").SetValue(cardTraitData, ParamTeamType);
            AccessTools.Field(typeof(CardTraitData), "paramTrackedValue").SetValue(cardTraitData, ParamTrackedValue);
            AccessTools.Field(typeof(CardTraitData), "paramUseScalingParams").SetValue(cardTraitData, ParamUseScalingParams);
            AccessTools.Field(typeof(CardTraitData), "traitIsRemovable").SetValue(cardTraitData, TraitIsRemovable);
            AccessTools.Field(typeof(CardTraitData), "traitStateName").SetValue(cardTraitData, TraitStateName);
            AccessTools.Field(typeof(CardTraitData), "stackMode").SetValue(cardTraitData, StackMode);
            var upgrade = ParamCardUpgradeData;
            if (ParamCardUpgradeDataBuilder != null)
            {
                upgrade = ParamCardUpgradeDataBuilder.Build();
            }

            AccessTools.Field(typeof(CardTraitData), "paramCardUpgradeData").SetValue(cardTraitData, upgrade);
            return cardTraitData;
        }
    }
}