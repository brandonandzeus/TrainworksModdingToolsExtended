﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using Trainworks.Managers;
using UnityEngine;
using static StatusEffectData;

namespace Trainworks.BuildersV2
{
    public class StatusEffectDataBuilder
    {
        private string statusId;

        /// <summary>
        /// ID of the status effect.
        /// Implicitly sets StatusIdKey.
        /// </summary>
        public string StatusId
        {
            get { return statusId; }
            set
            {
                statusId = value;
                if (StatusIdKey == null)
                {
                    if (statusId.Length == 1)
                    {
                        StatusIdKey = "StatusEffect_" + char.ToUpper(statusId[0]);
                    }
                    else if (statusId.Length > 1)
                    {
                        StatusIdKey = "StatusEffect_" + char.ToUpper(statusId[0]) + statusId.Substring(1);
                    }
                }
            }
        }
        /// <summary>
        /// Type of the status effect class to instantiate.
        /// This should be a class inheriting from StatusEffectState.
        /// </summary>
        public Type StatusEffectStateType { get; set; }
        /// <summary>
        /// StatusEffectState class to instantiate.
        /// Note that this isn't a simple string name of the class it is the class name plus the Assembly info.
        /// </summary>
        public string StatusEffectStateName => StatusEffectStateType.AssemblyQualifiedName;
        /// <summary>
        /// Base localization key for the status effect.
        /// There's not much reason to directly set this as its set by StatusIdKey
        /// It is set to StatusEffect_[StatusId] where StatusId is the capitalized string.
        /// </summary>
        public string StatusIdKey { get; set; }
        /// <summary>
        /// Path relative to the plugin's file path for the SFX.
        /// </summary>
        public string AppliedSFXName { get; set; }
        /// <summary>
        /// Path relative to the plugin's file path for the SFX.
        /// </summary>
        public string TriggeredSFXName { get; set; }
        /// <summary>
        /// The display category for the status effect. Positive, Negative, or Persistent.
        /// </summary>
        public StatusEffectData.DisplayCategory DisplayCategory { get; set; }
        /// <summary>
        /// The VFX to display on the character when the status effect is added.
        /// </summary>
        public VfxAtLoc AddedVFX { get; set; }
        public VfxAtLocList MoreAddedVFX { get; set; }
        /// <summary>
        /// The VFX to display on the character while this status effect is active.
        /// </summary>
        public VfxAtLoc PersistentVFX { get; set; }
        public VfxAtLocList MorePersistentVFX { get; set; }
        /// <summary>
        /// The VFX to display on the character when the effect is triggered.
        /// </summary>
        public VfxAtLoc TriggeredVFX { get; set; }
        public VfxAtLocList MoreTriggeredVFX { get; set; }
        /// <summary>
        /// The VFX to display on the character when the status effect is removed.
        /// </summary>
        public VfxAtLoc RemovedVFX { get; set; }
        public VfxAtLocList MoreRemovedVFX { get; set; }
        /// <summary>
        /// The VFX to display on a character when it is damaged/affected by this effect.
        /// </summary>
        public VfxAtLoc AffectedVFX { get; set; }
        /// <summary>
        /// Controls when OnTriggered/TestTrigger is called for custom status effects.
        /// </summary>
        public TriggerStage TriggerStage { get; set; }
        /// <summary>
        /// Controls when OnTriggered/TestTrigger is called for custom status effects.
        /// </summary>
        public List<TriggerStage> AdditionalTriggerStages { get; set; }
        public bool RemoveStackAtEndOfTurn { get; set; }
        public bool RemoveAtEndOfTurnIfTriggered { get; set; }
        public bool RemoveAtEndOfTurn { get; set; }
        public bool RemoveWhenTriggered { get; set; }
        /// <summary>
        /// This is the same as Remove When Triggered except it will be removed only after the card currently being played finishes playing\n\nNOTE: This should only be used for status effects that are triggered by a card being played.
        /// </summary>
        public bool RemoveWhenTriggeredAfterCardPlayed { get; set; }
        /// <summary>
        /// Whether or not the status effect is stackable. Defaults to true.
        /// </summary>
		public bool IsStackable { get; set; }
        /// <summary>
        /// Whether or not the status effect should show stacks in the card text. Defaults to true.
        /// </summary>
		public bool ShowStackCount { get; set; }
        /// <summary>
        /// Whether the status can be shown on the pyre.
        /// </summary>
        public bool ShowOnPyreHeart { get; set; }
        /// <summary>
        /// Shows a popup text notification when the stacks of the status effect are removed.
        /// </summary>
        public bool ShowNotificationsOnRemoval { get; set; }
        public string ParamStr { get; set; }
        public int ParamInt { get; set; }
        public int ParamSecondaryInt { get; set; }
        public float ParamFloat { get; set; }
        public VFXDisplayType VFXDisplayType { get; set; }
        /// <summary>
        /// The full, absolute path to the asset.
        /// </summary>
        public string FullAssetPath => BaseAssetPath + "/" + IconPath;
        /// <summary>
        /// Set automatically in the constructor. Base asset path, usually the plugin directory.
        /// </summary>
        public string BaseAssetPath { get; private set; }
        /// <summary>
        /// Path relative to the plugin's file path for the icon.
        /// Note the icon should be a black and white image sized 24x24.
        /// </summary>
        public string IconPath { get; set; }

        public StatusEffectDataBuilder()
        {
            IsStackable = true;
            ShowNotificationsOnRemoval = true;
            ShowStackCount = true;
            AdditionalTriggerStages = new List<TriggerStage>();
            VFXDisplayType = StatusEffectData.VFXDisplayType.Default;

            var assembly = Assembly.GetCallingAssembly();
            BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
        }

        public StatusEffectData Build()
        {
            StatusEffectData statusEffect = new StatusEffectData();
            AccessTools.Field(typeof(StatusEffectData), "addedVFX").SetValue(statusEffect, AddedVFX);
            AccessTools.Field(typeof(StatusEffectData), "additionalTriggerStages").SetValue(statusEffect, AdditionalTriggerStages);
            AccessTools.Field(typeof(StatusEffectData), "affectedVFX").SetValue(statusEffect, AffectedVFX);
            AccessTools.Field(typeof(StatusEffectData), "appliedSFXName").SetValue(statusEffect, AppliedSFXName);
            AccessTools.Field(typeof(StatusEffectData), "displayCategory").SetValue(statusEffect, DisplayCategory);
            AccessTools.Field(typeof(StatusEffectData), "isStackable").SetValue(statusEffect, IsStackable);
            AccessTools.Field(typeof(StatusEffectData), "moreAddedVFX").SetValue(statusEffect, MoreAddedVFX);
            AccessTools.Field(typeof(StatusEffectData), "morePersistentVFX").SetValue(statusEffect, MorePersistentVFX);
            AccessTools.Field(typeof(StatusEffectData), "moreRemovedVFX").SetValue(statusEffect, MoreRemovedVFX);
            AccessTools.Field(typeof(StatusEffectData), "moreTriggeredVFX").SetValue(statusEffect, MoreTriggeredVFX);
            AccessTools.Field(typeof(StatusEffectData), "paramFloat").SetValue(statusEffect, ParamFloat);
            AccessTools.Field(typeof(StatusEffectData), "paramInt").SetValue(statusEffect, ParamInt);
            AccessTools.Field(typeof(StatusEffectData), "paramSecondaryInt").SetValue(statusEffect, ParamSecondaryInt);
            AccessTools.Field(typeof(StatusEffectData), "paramStr").SetValue(statusEffect, ParamStr);
            AccessTools.Field(typeof(StatusEffectData), "persistentVFX").SetValue(statusEffect, PersistentVFX);
            AccessTools.Field(typeof(StatusEffectData), "removeAtEndOfTurn").SetValue(statusEffect, RemoveAtEndOfTurn);
            AccessTools.Field(typeof(StatusEffectData), "removeAtEndOfTurnIfTriggered").SetValue(statusEffect, RemoveAtEndOfTurnIfTriggered);
            AccessTools.Field(typeof(StatusEffectData), "removedVFX").SetValue(statusEffect, RemovedVFX);
            AccessTools.Field(typeof(StatusEffectData), "removeStackAtEndOfTurn").SetValue(statusEffect, RemoveStackAtEndOfTurn);
            AccessTools.Field(typeof(StatusEffectData), "removeWhenTriggered").SetValue(statusEffect, RemoveWhenTriggered);
            AccessTools.Field(typeof(StatusEffectData), "removeWhenTriggeredAfterCardPlayed").SetValue(statusEffect, RemoveWhenTriggeredAfterCardPlayed);
            AccessTools.Field(typeof(StatusEffectData), "showNotificationsOnRemoval").SetValue(statusEffect, ShowNotificationsOnRemoval);
            AccessTools.Field(typeof(StatusEffectData), "showOnPyreHeart").SetValue(statusEffect, ShowOnPyreHeart);
            AccessTools.Field(typeof(StatusEffectData), "showStackCount").SetValue(statusEffect, ShowStackCount);
            AccessTools.Field(typeof(StatusEffectData), "statusEffectStateName").SetValue(statusEffect, StatusEffectStateName);
            AccessTools.Field(typeof(StatusEffectData), "statusId").SetValue(statusEffect, StatusId);
            AccessTools.Field(typeof(StatusEffectData), "triggeredSFXName").SetValue(statusEffect, TriggeredSFXName);
            AccessTools.Field(typeof(StatusEffectData), "triggeredVFX").SetValue(statusEffect, TriggeredVFX);
            AccessTools.Field(typeof(StatusEffectData), "triggerStage").SetValue(statusEffect, TriggerStage);
            AccessTools.Field(typeof(StatusEffectData), "vfxDisplayType").SetValue(statusEffect, VFXDisplayType);

            if (IconPath != null)
            {
                Sprite sprite = CustomAssetManager.LoadSpriteFromPath(FullAssetPath);
                AccessTools.Field(typeof(StatusEffectData), "icon").SetValue(statusEffect, sprite);
            }

            StatusEffectManager manager = GameObject.FindObjectOfType<StatusEffectManager>() as StatusEffectManager;
            manager.GetAllStatusEffectsData().GetStatusEffectData().Add(statusEffect);
            StatusEffectManager.StatusIdToLocalizationExpression.Add(StatusId, StatusIdKey);

            return statusEffect;
        }
    }
}