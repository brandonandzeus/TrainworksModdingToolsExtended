using HarmonyLib;
using Trainworks.Managers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using static CharacterTriggerData;
using static StatusEffectData;

namespace Trainworks.BuildersV2
{
    public class StatusEffectDataBuilder
    {
        /// <summary>
        /// Don't set directly; use StatusEffectStateType instead.
        /// Type of the status effect class to instantiate.
        /// </summary>
        public Type statusEffectStateType;

        /// <summary>
        /// Type of the status effect class to instantiate.
        /// Implicitly sets StatusEffectStateName.
        /// </summary>
        public Type StatusEffectStateType
        {
            get { return this.statusEffectStateType; }
            set
            {
                this.statusEffectStateType = value;
                this.StatusEffectStateName = this.statusEffectStateType.AssemblyQualifiedName;
            }
        }


        public string StatusEffectStateName { get; set; }

        /// <summary>
        /// Don't set directly; use StatusId instead.
        /// ID of the status effect.
        /// </summary>
        public string statusId;

        /// <summary>
        /// ID of the status effect.
        /// Implicitly sets StatusIdKey.
        /// </summary>
        public string StatusId 
        {
            get { return this.statusId; } 
            set
            {
                this.statusId = value;
                if (this.StatusIdKey == null)
                {
                    if (this.statusId.Length == 1)
                        this.StatusIdKey = "StatusEffect_" + char.ToUpper(this.statusId[0]);
                    else if (this.statusId.Length > 1)
                        this.StatusIdKey = "StatusEffect_" + char.ToUpper(this.statusId[0]) + this.statusId.Substring(1);
                }
            }
        }
        /// <summary>
        /// Base localization key for the status effect.
        /// If unset is set to StatusEffect_[StatusId] where [StatusId] is the capitalized status id.
        /// </summary>
        public string StatusIdKey { get; set; }
        public string IconPath { get; set; }
        public string AppliedSFXName { get; set; }
        public string TriggeredSFXName { get; set; }

        //[Tooltip("This category determines the color used for the icon.")]
        public StatusEffectData.DisplayCategory DisplayCategory { get; set; }

        //[Tooltip("The VFX to display on the character when the status effect is added.")]
        public VfxAtLoc AddedVFX { get; set; }
        public VfxAtLocList MoreAddedVFX { get; set; }

        //[Tooltip("The VFX to display on the character while this status effect is active")]
        public VfxAtLoc PersistentVFX { get; set; }
        public VfxAtLocList MorePersistentVFX { get; set; }

        //[Tooltip("The VFX to display on the character when the effect is triggered.")]
        public VfxAtLoc TriggeredVFX { get; set; }
        public VfxAtLocList MoreTriggeredVFX { get; set; }

        //[Tooltip("The VFX to display on the character when the status effect is removed.")]
        public VfxAtLoc RemovedVFX { get; set; }
        public VfxAtLocList MoreRemovedVFX { get; set; }

        //[Tooltip("The VFX to display on a character when it is damaged/affected by this effect.")]
        public VfxAtLoc AffectedVFX { get; set; }

        public TriggerStage TriggerStage { get; set; }
        public List<TriggerStage> AdditionalTriggerStages { get; set; }
        public bool RemoveStackAtEndOfTurn { get; set; }
        public bool RemoveAtEndOfTurnIfTriggered { get; set; }
        public bool RemoveAtEndOfTurn { get; set; }
        public bool RemoveWhenTriggered { get; set; }

        //[Tooltip("This is the same as Remove When Triggered except it will be removed only after the card currently being played finishes playing\n\nNOTE: This should only be used for status effects that are triggered by a card being played.")]
        public bool RemoveWhenTriggeredAfterCardPlayed { get; set; }
        /// <summary>
        /// Whether or not the status effect is stackable. Defaults to true.
        /// </summary>
		public bool IsStackable { get; set; }
        /// <summary>
        /// Whether or not the status effect should show stacks in the card text. Defaults to true.
        /// </summary>
		public bool ShowStackCount { get; set; }
        public bool ShowOnPyreHeart { get; set; }
        /// <summary>
        /// Defaults to true.
        /// </summary>
        public bool ShowNotificationsOnRemoval { get; set; }
        public string ParamStr { get; set; }
        public int ParamInt { get; set; }
        public int ParamSecondaryInt { get; set; }
        public float ParamFloat { get; set; }
        public string BaseAssetPath { get; private set; }
        public VFXDisplayType VFXDisplayType { get; set; }

        public StatusEffectDataBuilder()
        {
            IsStackable = true;
            ShowNotificationsOnRemoval = true;
            ShowStackCount = true;
            AdditionalTriggerStages = new List<TriggerStage>();
            VFXDisplayType = StatusEffectData.VFXDisplayType.Default;

            var assembly = Assembly.GetCallingAssembly();
            this.BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
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
            AccessTools.Field(typeof(StatusEffectData), "vfxDisplayType").SetValue(statusEffect, this.VFXDisplayType);

            if (this.IconPath != null)
            {
                Sprite draftIconSprite = CustomAssetManager.LoadSpriteFromPath(this.BaseAssetPath + "/" + this.IconPath);
                AccessTools.Field(typeof(StatusEffectData), "icon").SetValue(statusEffect, draftIconSprite);
            }

            StatusEffectManager manager = GameObject.FindObjectOfType<StatusEffectManager>() as StatusEffectManager;
            manager.GetAllStatusEffectsData().GetStatusEffectData().Add(statusEffect);
            StatusEffectManager.StatusIdToLocalizationExpression.Add(StatusId, StatusIdKey);

            return statusEffect;
        }
    }
}