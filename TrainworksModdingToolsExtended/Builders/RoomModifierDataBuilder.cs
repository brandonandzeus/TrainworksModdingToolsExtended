using HarmonyLib;
using System;
using System.Reflection;
using Trainworks.Managers;
using UnityEngine;

namespace Trainworks.BuildersV2
{
    // Changed Icon -> IconPath to be consistent across DataBuilders.
    public class RoomModifierDataBuilder
    {
        private Type roomStateModifierClassType;
        private string roomStateModifierClassName;

        /// <summary>
        /// Type of the room state modifier class to instantiate.
        /// Implicitly sets RoomStateModifierClassName.
        /// </summary>
        public Type RoomStateModifierClassType
        {
            get { return this.roomStateModifierClassType; }
            set
            {
                this.roomStateModifierClassType = value;
                this.RoomStateModifierClassName = this.roomStateModifierClassType.AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// Implicitly sets DescriptionKey, DescriptionKeyInPlay, ExtraTooltipBodyKey, and ExtraTooltipTitleKey if null.
        /// Note that RoomStateModifierClassType is preferred especially if you are using
        /// a custom room state modifier class, since it will include in which assembly the
        /// class is.
        /// </summary>
        public string RoomStateModifierClassName
        {
            get { return this.roomStateModifierClassName; }
            set
            {
                this.roomStateModifierClassName = value;
                if (this.DescriptionKey == null)
                {
                    this.DescriptionKey = this.roomStateModifierClassName + "_RoomModifierData_DescriptionKey";
                }
                if (this.DescriptionKeyInPlay == null)
                {
                    this.DescriptionKeyInPlay = this.roomStateModifierClassName + "_RoomModifierData_DescriptionKeyInPlay";
                }
                if (this.ExtraTooltipBodyKey == null)
                {
                    this.ExtraTooltipBodyKey = this.roomStateModifierClassName + "_RoomModifierData_ExtraTooltipBodyKey";
                }
                if (this.ExtraTooltipTitleKey == null)
                {
                    this.ExtraTooltipTitleKey = this.roomStateModifierClassName + "_RoomModifierData_ExtraTooltipTitleKey";
                }
            }
        }

        public string Description { get; set; }
        public string DescriptionInPlay { get; set; }
        public string ExtraTooltipBody { get; set; }
        public string ExtraTooltipTitle { get; set; }
        /// <summary>
        /// Localization key for description. Default value is [RoomModifierClassName]_RoomModifierData_DescriptionKey.
        /// Note you shouldn't need to set this as its pre-set when setting the Type to instantiate.
        /// </summary>
        public string DescriptionKey { get; set; }
        /// <summary>
        /// Localization key for descriptionInPlay. Default value is [RoomModifierClassName]_RoomModifierData_DescriptionKeyInPlay.
        /// Note you shouldn't need to set this as its pre-set when setting the Type to instantiate.
        /// </summary>
        public string DescriptionKeyInPlay { get; set; }
        /// <summary>
        /// Localization key for extra tooltip title. Default value is [RoomModifierClassName]_RoomModifierData_ExtraTooltiptitleKey.
        /// Note you shouldn't need to set this as its pre-set when setting the Type to instantiate.
        /// </summary>
        public string ExtraTooltipTitleKey { get; set; }
        /// <summary>
        /// Localization key for extra tooltip boy. Default value is [RoomModifierClassName]_RoomModifierData_ExtraTooltipBodyKey.
        /// Note you shouldn't need to set this as its pre-set when setting the Type to instantiate.
        /// </summary>
        public string ExtraTooltipBodyKey { get; set; }
        /// <summary>
        /// Path relative to the plugin's file path for the icon.
        /// Note the icon should be a black and white image sized 24x24.
        /// </summary>
        public string IconPath { get; set; }
        public int ParamInt { get; set; }
        public string ParamSubtype { get; set; }
        public CardUpgradeData ParamCardUpgradeData { get; set; }
        /// <summary>
        /// CardUpgradeData Parameter.
        /// if set overrides ParamCardUpgradeData.
        /// </summary>
        public CardUpgradeDataBuilder ParamCardUpgradeDataBuilder { get; set; }
        public StatusEffectStackData[] ParamStatusEffects { get; set; }
        public string BaseAssetPath { get; private set; }

        public RoomModifierDataBuilder()
        {
            this.DescriptionKey = "";
            this.DescriptionKeyInPlay = "";
            this.ParamSubtype = "SubtypesData_None";
            this.ParamStatusEffects = new StatusEffectStackData[0];
            this.ExtraTooltipBodyKey = "";
            this.ExtraTooltipTitleKey = "";

            var assembly = Assembly.GetCallingAssembly();
            this.BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
        }

        /// <summary>
        /// Builds the RoomModifierData represented by this builders's parameters recursively;
        /// </summary>
        /// <returns>The newly created RoomModifierData</returns>
        public RoomModifierData Build()
        {
            RoomModifierData roomModifierData = new RoomModifierData();

            AccessTools.Field(typeof(RoomModifierData), "descriptionKey").SetValue(roomModifierData, this.DescriptionKey);
            AccessTools.Field(typeof(RoomModifierData), "descriptionKeyInPlay").SetValue(roomModifierData, this.DescriptionKeyInPlay);
            AccessTools.Field(typeof(RoomModifierData), "extraTooltipBodyKey").SetValue(roomModifierData, this.ExtraTooltipBodyKey);
            AccessTools.Field(typeof(RoomModifierData), "extraTooltipTitleKey").SetValue(roomModifierData, this.ExtraTooltipTitleKey);
            AccessTools.Field(typeof(RoomModifierData), "paramInt").SetValue(roomModifierData, this.ParamInt);
            AccessTools.Field(typeof(RoomModifierData), "paramStatusEffects").SetValue(roomModifierData, this.ParamStatusEffects);
            AccessTools.Field(typeof(RoomModifierData), "paramSubtype").SetValue(roomModifierData, this.ParamSubtype);
            if (ParamCardUpgradeDataBuilder == null)
                AccessTools.Field(typeof(RoomModifierData), "paramCardUpgardeData" /* sic */).SetValue(roomModifierData, this.ParamCardUpgradeData);
            else
                AccessTools.Field(typeof(RoomModifierData), "paramCardUpgardeData" /* sic */).SetValue(roomModifierData, this.ParamCardUpgradeDataBuilder.Build());
            AccessTools.Field(typeof(RoomModifierData), "roomStateModifierClassName").SetValue(roomModifierData, this.RoomStateModifierClassName);

            BuilderUtils.ImportStandardLocalization(this.DescriptionKey, this.Description);
            BuilderUtils.ImportStandardLocalization(this.DescriptionKeyInPlay, this.DescriptionInPlay);
            BuilderUtils.ImportStandardLocalization(this.ExtraTooltipBodyKey, this.ExtraTooltipBody);
            BuilderUtils.ImportStandardLocalization(this.ExtraTooltipTitleKey, this.ExtraTooltipTitle);


            if (this.IconPath != null)
            {
                Sprite sprite = CustomAssetManager.LoadSpriteFromPath(this.BaseAssetPath + "/" + this.IconPath);
                AccessTools.Field(typeof(RoomModifierData), "icon").SetValue(roomModifierData, sprite);
            }

            return roomModifierData;
        }

        /// <summary>
        /// Add a status effect to this room's params.
        /// </summary>
        /// <param name="statusEffectID">ID of the status effect, most easily retrieved using the helper class "VanillaStatusEffectIDs"</param>
        /// <param name="stackCount">Number of stacks to apply</param>
        public void AddStartingStatusEffect(string statusEffectID, int stackCount)
        {
            this.ParamStatusEffects = BuilderUtils.AddStatusEffect(statusEffectID, stackCount, this.ParamStatusEffects);
        }
    }
}
