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
            get { return roomStateModifierClassType; }
            set
            {
                roomStateModifierClassType = value;
                RoomStateModifierClassName = roomStateModifierClassType.AssemblyQualifiedName;
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
            get { return roomStateModifierClassName; }
            set
            {
                roomStateModifierClassName = value;
                if (DescriptionKey == null)
                {
                    DescriptionKey = roomStateModifierClassName + "_RoomModifierData_DescriptionKey";
                }
                if (DescriptionKeyInPlay == null)
                {
                    DescriptionKeyInPlay = roomStateModifierClassName + "_RoomModifierData_DescriptionKeyInPlay";
                }
                if (ExtraTooltipBodyKey == null)
                {
                    ExtraTooltipBodyKey = roomStateModifierClassName + "_RoomModifierData_ExtraTooltipBodyKey";
                }
                if (ExtraTooltipTitleKey == null)
                {
                    ExtraTooltipTitleKey = roomStateModifierClassName + "_RoomModifierData_ExtraTooltipTitleKey";
                }
            }
        }

        /// <summary>
        /// This description.
        /// Note if this is set this will set the localized text across all languages
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// This description in play.
        /// Note if this is set this will set the localized text across all languages
        /// </summary>
        public string DescriptionInPlay { get; set; }
        /// <summary>
        /// This is the Extra Tooltip Body text.
        /// Note if this is set this will set the localized text across all languages
        /// </summary>
        public string ExtraTooltipBody { get; set; }
        /// <summary>
        /// This is the Extra Tooltip Title.
        /// Note if this is set this will set the localized text across all languages
        /// </summary>
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
            DescriptionKey = "";
            DescriptionKeyInPlay = "";
            ParamSubtype = "SubtypesData_None";
            ParamStatusEffects = new StatusEffectStackData[0];
            ExtraTooltipBodyKey = "";
            ExtraTooltipTitleKey = "";

            var assembly = Assembly.GetCallingAssembly();
            BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
        }

        /// <summary>
        /// Builds the RoomModifierData represented by this builders's parameters recursively;
        /// </summary>
        /// <returns>The newly created RoomModifierData</returns>
        public RoomModifierData Build()
        {
            RoomModifierData roomModifierData = new RoomModifierData();

            AccessTools.Field(typeof(RoomModifierData), "descriptionKey").SetValue(roomModifierData, DescriptionKey);
            AccessTools.Field(typeof(RoomModifierData), "descriptionKeyInPlay").SetValue(roomModifierData, DescriptionKeyInPlay);
            AccessTools.Field(typeof(RoomModifierData), "extraTooltipBodyKey").SetValue(roomModifierData, ExtraTooltipBodyKey);
            AccessTools.Field(typeof(RoomModifierData), "extraTooltipTitleKey").SetValue(roomModifierData, ExtraTooltipTitleKey);
            AccessTools.Field(typeof(RoomModifierData), "paramInt").SetValue(roomModifierData, ParamInt);
            AccessTools.Field(typeof(RoomModifierData), "paramStatusEffects").SetValue(roomModifierData, ParamStatusEffects);
            AccessTools.Field(typeof(RoomModifierData), "paramSubtype").SetValue(roomModifierData, ParamSubtype);
            if (ParamCardUpgradeDataBuilder == null)
            {
                AccessTools.Field(typeof(RoomModifierData), "paramCardUpgardeData" /* sic */).SetValue(roomModifierData, ParamCardUpgradeData);
            }
            else
            {
                AccessTools.Field(typeof(RoomModifierData), "paramCardUpgardeData" /* sic */).SetValue(roomModifierData, ParamCardUpgradeDataBuilder.Build());
            }

            AccessTools.Field(typeof(RoomModifierData), "roomStateModifierClassName").SetValue(roomModifierData, RoomStateModifierClassName);

            BuilderUtils.ImportStandardLocalization(DescriptionKey, Description);
            BuilderUtils.ImportStandardLocalization(DescriptionKeyInPlay, DescriptionInPlay);
            BuilderUtils.ImportStandardLocalization(ExtraTooltipBodyKey, ExtraTooltipBody);
            BuilderUtils.ImportStandardLocalization(ExtraTooltipTitleKey, ExtraTooltipTitle);


            if (IconPath != null)
            {
                Sprite sprite = CustomAssetManager.LoadSpriteFromPath(BaseAssetPath + "/" + IconPath);
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
            ParamStatusEffects = BuilderUtils.AddStatusEffect(statusEffectID, stackCount, ParamStatusEffects);
        }
    }
}
