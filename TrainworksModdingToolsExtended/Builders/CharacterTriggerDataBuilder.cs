using HarmonyLib;
using System.Collections.Generic;

namespace Trainworks.BuildersV2
{
    public class CharacterTriggerDataBuilder
    {
        private CharacterTriggerData.Trigger trigger;
        /// <summary>
        /// Implicitly sets DescriptionKey and AdditionalTextOnTriggerKey if null.
        /// It is highly recommended to set DescriptionKey and AdditionalTextOnTriggerKey as they aren't unique
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
        /// Append to this list to add new card effects.
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
            HideTriggerTooltip = false;
            DisplayEffectHintText = false;
        }

        /// <summary>
        /// Builds the CharacterTriggerData represented by this builders's parameters
        /// all Builders represented in this class's various fields will also be built.
        /// </summary>
        /// <returns>The newly created CardTraitData</returns>
        public CharacterTriggerData Build()
        {
            CharacterTriggerData characterTriggerData = new CharacterTriggerData(Trigger, /* cardEffectData = */null);

            var effects = characterTriggerData.GetEffects();
            effects.Clear(); // Contains a null from constructor above.
            effects.AddRange(Effects);
            foreach (var builder in EffectBuilders)
                effects.Add(builder.Build());

            AccessTools.Field(typeof(CharacterTriggerData), "additionalTextOnTriggerKey").SetValue(characterTriggerData, AdditionalTextOnTriggerKey);
            AccessTools.Field(typeof(CharacterTriggerData), "descriptionKey").SetValue(characterTriggerData, DescriptionKey);
            AccessTools.Field(typeof(CharacterTriggerData), "displayEffectHintText").SetValue(characterTriggerData, DisplayEffectHintText);
            AccessTools.Field(typeof(CharacterTriggerData), "hideTriggerTooltip").SetValue(characterTriggerData, HideTriggerTooltip);
            AccessTools.Field(typeof(CharacterTriggerData), "trigger").SetValue(characterTriggerData, Trigger);

            BuilderUtils.ImportStandardLocalization(DescriptionKey, Description);
            BuilderUtils.ImportStandardLocalization(AdditionalTextOnTriggerKey, AdditionalTextOnTrigger);

            return characterTriggerData;
        }
    }
}