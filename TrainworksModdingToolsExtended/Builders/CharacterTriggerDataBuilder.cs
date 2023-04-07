using HarmonyLib;
using System.Collections.Generic;

namespace Trainworks.BuildersV2
{
    // Note code is unchanged from Trainworks Modding Tools.
    public class CharacterTriggerDataBuilder
    {
        private CharacterTriggerData.Trigger trigger;

        /// <summary>
        /// Implicitly sets DescriptionKey and AdditionalTextOnTriggerKey if null.
        /// </summary>
        public CharacterTriggerData.Trigger Trigger
        {
            get { return trigger; }
            set
            {
                trigger = value;
                if (DescriptionKey == null)
                {
                    DescriptionKey = trigger + "_CharacterTriggerData_DescriptionKey";
                }
                if (AdditionalTextOnTriggerKey == null)
                {
                    AdditionalTextOnTriggerKey = trigger + "_CharacterTriggerData_BonusTriggerKey";
                }
            }
        }

        /// <summary>
        /// Append to this list to add new card effects. The Build() method recursively builds all nested builders.
        /// </summary>
        public List<CardEffectDataBuilder> EffectBuilders { get; set; }
        /// <summary>
        /// List of pre-built card effects.
        /// </summary>
        public List<CardEffectData> Effects { get; set; }

        /// <summary>
        /// Description displayed for the relic.
        /// Note that setting this field will set the localization for all langauges.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Additional Text on Trigger
        /// Note that setting this field will set the localization for all langauges.
        /// </summary>
        public string AdditionalTextOnTrigger { get; set; }

        /// <summary>
        /// Localization key for the text's localization.
        /// *HIGHLY* Recommended to set this as the description key is not a unique key.
        /// </summary>
        public string DescriptionKey { get; set; }
        /// <summary>
        /// Localization key for the text's localization.
        /// *HIGHLY* Recommended to set this as the localization key is not a unique key.
        /// </summary>
        public string AdditionalTextOnTriggerKey { get; set; }

        public bool DisplayEffectHintText { get; set; }
        public bool HideTriggerTooltip { get; set; }

        public CharacterTriggerDataBuilder()
        {
            EffectBuilders = new List<CardEffectDataBuilder>();
            Effects = new List<CardEffectData>();
        }

        /// <summary>
        /// Builds the CharacterTriggerData represented by this builders's parameters recursively;
        /// all Builders represented in this class's various fields will also be built.
        /// </summary>
        /// <returns>The newly created CardTraitData</returns>
        public CharacterTriggerData Build()
        {
            foreach (var builder in EffectBuilders)
            {
                Effects.Add(builder.Build());
            }
            CharacterTriggerData characterTriggerData = new CharacterTriggerData(Trigger, null);
            BuilderUtils.ImportStandardLocalization(AdditionalTextOnTriggerKey, AdditionalTextOnTrigger);
            AccessTools.Field(typeof(CharacterTriggerData), "additionalTextOnTriggerKey").SetValue(characterTriggerData, AdditionalTextOnTriggerKey);
            BuilderUtils.ImportStandardLocalization(DescriptionKey, Description);
            AccessTools.Field(typeof(CharacterTriggerData), "descriptionKey").SetValue(characterTriggerData, DescriptionKey);
            AccessTools.Field(typeof(CharacterTriggerData), "displayEffectHintText").SetValue(characterTriggerData, DisplayEffectHintText);
            AccessTools.Field(typeof(CharacterTriggerData), "effects").SetValue(characterTriggerData, Effects);
            AccessTools.Field(typeof(CharacterTriggerData), "hideTriggerTooltip").SetValue(characterTriggerData, HideTriggerTooltip);
            AccessTools.Field(typeof(CharacterTriggerData), "trigger").SetValue(characterTriggerData, Trigger);
            return characterTriggerData;
        }
    }
}