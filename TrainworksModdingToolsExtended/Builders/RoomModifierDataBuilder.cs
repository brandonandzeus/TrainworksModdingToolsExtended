using HarmonyLib;
using System;
using System.Reflection;
using Trainworks.ConstantsV2;
using Trainworks.Managers;
using UnityEngine;

namespace Trainworks.BuildersV2
{
    // Changed Icon -> IconPath to be consistent across DataBuilders.
    public class RoomModifierDataBuilder
    {
        private Type roomModifierClassType;
        private string roomModifierID;

        /// <summary>
        /// Unique ID
        /// Implicitly sets DescriptionKey and DescriptionKeyInPlay if null.
        /// </summary>
        public string RoomModifierID
        {
            get { return roomModifierID; }
            set
            {
                roomModifierID = value;
                if (DescriptionKey == null)
                {
                    DescriptionKey = roomModifierID + "_RoomModifierData_DescriptionKey";
                }
                if (DescriptionKeyInPlay == null)
                {
                    DescriptionKeyInPlay = roomModifierID + "_RoomModifierData_DescriptionKeyInPlay";
                }
            }
        }

        /// <summary>
        /// Type of the room state modifier class to instantiate.
        /// Implicitly sets RoomStateModifierClassName.
        /// </summary>
        public Type RoomModifierClassType
        {
            get { return roomModifierClassType; }
            set
            {
                roomModifierClassType = value;
                RoomModifierClassName = roomModifierClassType.AssemblyQualifiedName;
            }
        }
        /// <summary>
        /// RoomStateModifierClass Name to instantiate.
        /// </summary>
        public string RoomModifierClassName { get; set; }

        /// <summary>
        /// This description.
        /// Note if this is set this will set the localized text across all languages.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// This description in play.
        /// Note if this is set this will set the localized text across all languages.
        /// </summary>
        public string DescriptionInPlay { get; set; }
        /// <summary>
        /// This is the Extra Tooltip Body text.
        /// Note if this is set this will set the localized text across all languages.
        /// You must set ExtraTooltipTitleKey if this is set.
        /// </summary>
        public string ExtraTooltipBody { get; set; }
        /// <summary>
        /// This is the Extra Tooltip Title.
        /// Note if this is set this will set the localized text across all languages.
        /// You must set ExtraTooltipTitleKey if this is set.
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
        /// Localization key for extra tooltip title.
        /// Note this isn't set automatically.
        /// </summary>
        public string ExtraTooltipTitleKey { get; set; }
        /// <summary>
        /// Localization key for extra tooltip boy.
        /// Note this isn't set automatically/
        /// </summary>
        public string ExtraTooltipBodyKey { get; set; }

        public int ParamInt { get; set; }
        public string ParamSubtype { get; set; }
        public CardUpgradeData ParamCardUpgradeData { get; set; }
        /// <summary>
        /// Convenience Builder for CardUpgradeData Parameter.
        /// if set overrides ParamCardUpgradeData.
        /// </summary>
        public CardUpgradeDataBuilder ParamCardUpgradeDataBuilder { get; set; }
        public StatusEffectStackData[] ParamStatusEffects { get; set; }
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

        public RoomModifierDataBuilder()
        {
            ParamSubtype = VanillaSubtypeIDs.None;
            ParamStatusEffects = Array.Empty<StatusEffectStackData>();
            ExtraTooltipBodyKey = "";
            ExtraTooltipTitleKey = "";

            var assembly = Assembly.GetCallingAssembly();
            BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
        }

        /// <summary>
        /// Builds the RoomModifierData represented by this builders's parameters
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
            AccessTools.Field(typeof(RoomModifierData), "roomStateModifierClassName").SetValue(roomModifierData, RoomModifierClassName);

            var upgrade = ParamCardUpgradeData;
            if (ParamCardUpgradeDataBuilder != null)
            {
                upgrade = ParamCardUpgradeDataBuilder.Build();
            }

            AccessTools.Field(typeof(RoomModifierData), "paramCardUpgardeData" /* sic */).SetValue(roomModifierData, upgrade);
            if (IconPath != null)
            {
                Sprite sprite = CustomAssetManager.LoadSpriteFromPath(FullAssetPath);
                AccessTools.Field(typeof(RoomModifierData), "icon").SetValue(roomModifierData, sprite);
            }

            BuilderUtils.ImportStandardLocalization(DescriptionKey, Description);
            BuilderUtils.ImportStandardLocalization(DescriptionKeyInPlay, DescriptionInPlay);
            BuilderUtils.ImportStandardLocalization(ExtraTooltipBodyKey, ExtraTooltipBody);
            BuilderUtils.ImportStandardLocalization(ExtraTooltipTitleKey, ExtraTooltipTitle);

            return roomModifierData;
        }
    }
}
