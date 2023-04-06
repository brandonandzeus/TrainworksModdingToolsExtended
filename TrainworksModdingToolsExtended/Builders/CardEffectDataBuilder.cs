﻿using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Harmony;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ShinyShoe;
using Trainworks.Managers;
using System.Runtime.CompilerServices;

namespace Trainworks.BuildersV2
{
    public class CardEffectDataBuilder
    {
        /// <summary>
        /// Don't set directly; use EffectStateType instead.
        /// Type of the effect class to instantiate.
        /// </summary>
        public Type effectStateType;

        /// <summary>
        /// Type of the effect class to instantiate.
        /// Implicitly sets EffectStateName.
        /// </summary>
        public Type EffectStateType
        {
            get { return this.effectStateType; }
            set
            {
                this.effectStateType = value;
                this.EffectStateName = this.effectStateType.AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// Name of the effect class to instantiate.
        /// Either pass an assembly qualified type name or use EffectStateType instead.
        /// Its generally better to use EffectStateType over EffectStateName, especially since
        /// EffectStateName is referring to an effect in another Assembly or multiple classes
        /// with the same name exists for some reason.
        /// </summary>
        public string EffectStateName { get; set; }

        /// <summary>
        /// Int parameter.
        /// </summary>
        public int ParamInt { get; set; }
        /// <summary>
        /// Int parameter.
        /// </summary>
        public int AdditionalParamInt { get; set; }
        /// <summary>
        /// Int parameter
        /// </summary>
        public int ParamMaxInt { get; set; }
        /// <summary>
        /// Int parameter.
        /// </summary>
        public int ParamMinInt { get; set; }
        /// <summary>
        /// Float parameter.
        /// </summary>
        public float ParamMultiplier { get; set; }
        /// <summary>
        /// Boolean parameter.
        /// </summary>
        public bool ParamBool { get; set; }
        /// <summary>
        /// String parameter.
        /// </summary>
        public string ParamStr { get; set; }
        /// <summary>
        /// Subtype parameter.
        /// </summary>
        public string ParamSubtype { get; set; }


        /// <summary>
        /// CharacterData parameter.
        /// </summary>
        public CharacterData ParamAdditionalCharacterData { get; set; }
        /// <summary>
        /// CardUpgradeMaskData parameter.
        /// </summary>
        public CardUpgradeMaskData ParamCardFilter { get; set; }
        /// <summary>
        /// CardPool parameter.
        /// </summary>
        public CardPool ParamCardPool { get; set; }
        /// <summary>
        /// CardUpgradeData parameter.
        /// </summary>
        public CardUpgradeData ParamCardUpgradeData { get; set; }
        /// <summary>
        /// CardUpgradeData parameter.
        /// If this is set overrides ParamCardUpgradeData
        /// </summary>
        public CardUpgradeDataBuilder ParamCardUpgradeDataBuilder { get; set; }
        /// <summary>
        /// CharacterData parameter.
        /// </summary>
        public CharacterData ParamCharacterData { get; set; }
        /// <summary>
        /// Builder for CharacterData parameter.
        /// Calling Build() will also build this parameter recursively.
        /// </summary>
        public CharacterDataBuilder ParamCharacterDataBuilder { get; set; }
        /// <summary>
        /// CharacterDataPool parameter.
        /// </summary>
        public List<CharacterData> ParamCharacterDataPool { get; set; }
        /// <summary>
        /// RoomData parameter.
        /// </summary>
        public RoomData ParamRoomData { get; set; }
        /// <summary>
        /// Status effect array parameter.
        /// </summary>
        public StatusEffectStackData[] ParamStatusEffects { get; set; }
        /// <summary>
        /// Vector3 parameter.
        /// </summary>
        public Vector3 ParamTimingDelays { get; set; }
        /// <summary>
        /// Trigger parameter.
        /// </summary>
        public CharacterTriggerData.Trigger ParamTrigger { get; set; }

        public string StatusEffectStackMultiplier { get; set; }

        /// <summary>
        /// Tooltips displayed when hovering over any game entity this effect is applied to.
        /// </summary>
        public AdditionalTooltipData[] AdditionalTooltips { get; set; }

        public CharacterUI.Anim AnimToPlay { get; set; }
        public VfxAtLoc AppliedToSelfVFX { get; set; }
        public VfxAtLoc AppliedVFX { get; set; }

        public bool UseIntRange { get; set; }
        public bool UseStatusEffectStackMultiplier { get; set; }
        public bool CopyModifiersFromSource { get; set; }
        public bool FilterBasedOnMainSubClass { get; set; }
        public bool HideTooltip { get; set; }
        public bool IgnoreTemporaryModifiersFromSource { get; set; }
        public bool ShouldTest { get; set; }

        public CardEffectData.CardSelectionMode TargetCardSelectionMode { get; set; }
        public CardType TargetCardType { get; set; }
        public string TargetCharacterSubtype { get; set; }
        public bool TargetIgnoreBosses { get; set; }
        public bool TargetIgnorePyre { get; set; }
        public TargetMode TargetMode { get; set; }
        public CardEffectData.HealthFilter TargetModeHealthFilter { get; set; }
        public string[] TargetModeStatusEffectsFilter { get; set; }
        public Team.Type TargetTeamType { get; set; }

        public CardEffectDataBuilder()
        {
            this.TargetTeamType = Team.Type.Heroes;
            this.ShouldTest = true;
            this.ParamMultiplier = 1f;
            this.ParamStatusEffects = new StatusEffectStackData[0];
            this.ParamTimingDelays = Vector3.zero;
            this.AdditionalTooltips = new AdditionalTooltipData[0];
            this.TargetModeStatusEffectsFilter = new string[0];
        }

        /// <summary>
        /// Builds the CardEffectData represented by this builder's parameters recursively;
        /// all Builders represented in this class's various fields will also be built.
        /// </summary>
        /// <returns>The newly created CardEffectData</returns>
        public CardEffectData Build()
        {
            if (this.ParamCharacterDataBuilder != null)
            {
                this.ParamCharacterData = this.ParamCharacterDataBuilder.BuildAndRegister();
            }
            CardEffectData cardEffectData = new CardEffectData();
            AccessTools.Field(typeof(CardEffectData), "additionalParamInt").SetValue(cardEffectData, this.AdditionalParamInt);
            AccessTools.Field(typeof(CardEffectData), "additionalTooltips").SetValue(cardEffectData, this.AdditionalTooltips);
            AccessTools.Field(typeof(CardEffectData), "animToPlay").SetValue(cardEffectData, this.AnimToPlay);
            AccessTools.Field(typeof(CardEffectData), "appliedToSelfVFX").SetValue(cardEffectData, this.AppliedToSelfVFX);
            AccessTools.Field(typeof(CardEffectData), "appliedVFX").SetValue(cardEffectData, this.AppliedVFX);
            AccessTools.Field(typeof(CardEffectData), "copyModifiersFromSource").SetValue(cardEffectData, this.CopyModifiersFromSource);
            AccessTools.Field(typeof(CardEffectData), "effectStateName").SetValue(cardEffectData, this.EffectStateName);
            AccessTools.Field(typeof(CardEffectData), "filterBasedOnMainSubClass").SetValue(cardEffectData, this.FilterBasedOnMainSubClass);
            AccessTools.Field(typeof(CardEffectData), "hideTooltip").SetValue(cardEffectData, this.HideTooltip);
            AccessTools.Field(typeof(CardEffectData), "ignoreTemporaryModifiersFromSource").SetValue(cardEffectData, this.IgnoreTemporaryModifiersFromSource);
            AccessTools.Field(typeof(CardEffectData), "paramAdditionalCharacterData").SetValue(cardEffectData, this.ParamAdditionalCharacterData);
            AccessTools.Field(typeof(CardEffectData), "paramBool").SetValue(cardEffectData, this.ParamBool);
            AccessTools.Field(typeof(CardEffectData), "paramCardFilter").SetValue(cardEffectData, this.ParamCardFilter);
            AccessTools.Field(typeof(CardEffectData), "paramCardPool").SetValue(cardEffectData, this.ParamCardPool);
            if (ParamCardUpgradeDataBuilder == null)
                AccessTools.Field(typeof(CardEffectData), "paramCardUpgradeData").SetValue(cardEffectData, this.ParamCardUpgradeData);
            else
                AccessTools.Field(typeof(CardEffectData), "paramCardUpgradeData").SetValue(cardEffectData, this.ParamCardUpgradeDataBuilder.Build());
            AccessTools.Field(typeof(CardEffectData), "paramCharacterData").SetValue(cardEffectData, this.ParamCharacterData);
            AccessTools.Field(typeof(CardEffectData), "paramCharacterDataPool").SetValue(cardEffectData, this.ParamCharacterDataPool);
            AccessTools.Field(typeof(CardEffectData), "paramInt").SetValue(cardEffectData, this.ParamInt);
            AccessTools.Field(typeof(CardEffectData), "paramMaxInt").SetValue(cardEffectData, this.ParamMaxInt);
            AccessTools.Field(typeof(CardEffectData), "paramMinInt").SetValue(cardEffectData, this.ParamMinInt);
            AccessTools.Field(typeof(CardEffectData), "paramMultiplier").SetValue(cardEffectData, this.ParamMultiplier);
            AccessTools.Field(typeof(CardEffectData), "paramRoomData").SetValue(cardEffectData, this.ParamRoomData);
            AccessTools.Field(typeof(CardEffectData), "paramStatusEffects").SetValue(cardEffectData, this.ParamStatusEffects);
            AccessTools.Field(typeof(CardEffectData), "paramStr").SetValue(cardEffectData, this.ParamStr);
            AccessTools.Field(typeof(CardEffectData), "paramSubtype").SetValue(cardEffectData, this.ParamSubtype);
            AccessTools.Field(typeof(CardEffectData), "paramTimingDelays").SetValue(cardEffectData, this.ParamTimingDelays);
            AccessTools.Field(typeof(CardEffectData), "paramTrigger").SetValue(cardEffectData, this.ParamTrigger);
            AccessTools.Field(typeof(CardEffectData), "shouldTest").SetValue(cardEffectData, this.ShouldTest);
            AccessTools.Field(typeof(CardEffectData), "statusEffectStackMultiplier").SetValue(cardEffectData, this.StatusEffectStackMultiplier);
            AccessTools.Field(typeof(CardEffectData), "targetCardSelectionMode").SetValue(cardEffectData, this.TargetCardSelectionMode);
            AccessTools.Field(typeof(CardEffectData), "targetCardType").SetValue(cardEffectData, this.TargetCardType);
            AccessTools.Field(typeof(CardEffectData), "targetCharacterSubtype").SetValue(cardEffectData, this.TargetCharacterSubtype);
            AccessTools.Field(typeof(CardEffectData), "targetIgnoreBosses").SetValue(cardEffectData, this.TargetIgnoreBosses);
            AccessTools.Field(typeof(CardEffectData), "targetMode").SetValue(cardEffectData, this.TargetMode);
            AccessTools.Field(typeof(CardEffectData), "targetModeHealthFilter").SetValue(cardEffectData, this.TargetModeHealthFilter);
            AccessTools.Field(typeof(CardEffectData), "targetModeStatusEffectsFilter").SetValue(cardEffectData, this.TargetModeStatusEffectsFilter);
            AccessTools.Field(typeof(CardEffectData), "targetTeamType").SetValue(cardEffectData, this.TargetTeamType);
            AccessTools.Field(typeof(CardEffectData), "useIntRange").SetValue(cardEffectData, this.UseIntRange);
            AccessTools.Field(typeof(CardEffectData), "useStatusEffectStackMultiplier").SetValue(cardEffectData, this.UseStatusEffectStackMultiplier);
            return cardEffectData;
        }

        /// <summary>
        /// Add a status effect to this effect's status effect array.
        /// </summary>
        /// <param name="statusEffectID">ID of the status effect, most easily retrieved using the helper class "MTStatusEffectIDs"</param>
        /// <param name="stackCount">Number of stacks to apply</param>
        public CardEffectDataBuilder AddStatusEffect(string statusEffectID, int stackCount)
        {
            this.ParamStatusEffects = BuilderUtils.AddStatusEffect(statusEffectID, stackCount, this.ParamStatusEffects);
            return this;
        }
    }
}