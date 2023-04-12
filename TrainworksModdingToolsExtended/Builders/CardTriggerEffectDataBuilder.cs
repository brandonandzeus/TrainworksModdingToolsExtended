﻿using HarmonyLib;
using System.Collections.Generic;

namespace Trainworks.BuildersV2
{
    public class CardTriggerEffectDataBuilder
    {
        private string triggerID;

        /// <summary>
        /// Unique ID for CardTriggerEffect. It should be unique.
        /// Implicitly sets DescriptionKey if null.
        /// </summary>
        public string TriggerID
        {
            get { return triggerID; }
            set
            {
                triggerID = value;
                if (DescriptionKey == null)
                {
                    DescriptionKey = triggerID + "_CardTriggerEffectData_DescriptionKey";
                }
            }
        }

        /// <summary>
        /// Card Trigger Type.
        /// </summary>
        private CardTriggerType Trigger { get; set; }
        /// <summary>
        /// Custom description for the trigger effect.
        /// Note that setting this property will set the localization for all languages.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Use an existing base game trigger's description key to copy the format of its description.
        /// *HIGHLY* Recommended to set this property if using a custom card trigger effect as the localization key is not unique.
        /// </summary>
        public string DescriptionKey { get; set; }
        /// <summary>
        /// List of CardTrigger Effects.
        /// Note no builder since CardTriggerData is mutable.
        /// </summary>
        public List<CardTriggerData> CardTriggerEffects { get; set; }
        /// <summary>
        /// List of CardEffects.
        /// </summary>
        public List<CardEffectData> CardEffects { get; set; }
        /// <summary>
        /// Convenience Builders to append to CardEffects.
        /// </summary>
        public List<CardEffectDataBuilder> CardEffectBuilders { get; set; }

        public CardTriggerEffectDataBuilder()
        {
            CardTriggerEffects = new List<CardTriggerData>();
            CardEffectBuilders = new List<CardEffectDataBuilder>();
            CardEffects = new List<CardEffectData>();
        }

        /// <summary>
        /// Builds the CardTriggerEffectData represented by this builder's parameters.
        /// all Builders represented in this class's various fields will also be built.
        /// </summary>
        /// <returns>The newly created CardTriggerEffectData</returns>
        public CardTriggerEffectData Build()
        {
            // Doesn't inherit from ScriptableObject
            CardTriggerEffectData cardTriggerEffectData = new CardTriggerEffectData();

            AccessTools.Field(typeof(CardTriggerEffectData), "descriptionKey").SetValue(cardTriggerEffectData, DescriptionKey);
            AccessTools.Field(typeof(CardTriggerEffectData), "trigger").SetValue(cardTriggerEffectData, Trigger);

            // Saving allocations by adding to the list that was already allocated.
            var cardEffects = cardTriggerEffectData.GetCardEffects();
            cardEffects.AddRange(CardEffects);
            foreach (var builder in CardEffectBuilders)
            {
                cardEffects.Add(builder.Build());
            }

            var cardTriggerEffects = cardTriggerEffectData.GetTriggerEffects();
            cardTriggerEffects.AddRange(CardTriggerEffects);

            BuilderUtils.ImportStandardLocalization(DescriptionKey, Description);

            return cardTriggerEffectData;
        }

        /// <summary>
        /// Adds a new CardTrigger to the list.
        /// </summary>
        /// <param name="persistenceMode">SingleRun, or SingleBattle</param>
        /// <param name="cardTriggerEffect"></param>
        /// <param name="buffEffectType"></param>
        /// <param name="paramInt"></param>
        /// <returns></returns>
        public CardTriggerData AddCardTrigger(PersistenceMode persistenceMode, string cardTriggerEffect, string buffEffectType, int paramInt)
        {
            CardTriggerData trigger = new CardTriggerData();

            trigger.persistenceMode = persistenceMode;
            trigger.cardTriggerEffect = cardTriggerEffect;
            trigger.buffEffectType = buffEffectType;
            trigger.paramInt = paramInt;

            CardTriggerEffects.Add(trigger);
            return trigger;
        }

        /// <summary>
        /// Makes a new card trigger.
        /// </summary>
        /// <param name="persistenceMode">SingleRun, or SingleBattle</param>
        /// <param name="cardTriggerEffect"></param>
        /// <param name="buffEffectType"></param>
        /// <param name="paramInt"></param>
        /// <returns></returns>
        public static CardTriggerData MakeCardTrigger(PersistenceMode persistenceMode, string cardTriggerEffect, string buffEffectType, int paramInt)
        {
            CardTriggerData trigger = new CardTriggerData();

            trigger.persistenceMode = persistenceMode;
            trigger.cardTriggerEffect = cardTriggerEffect;
            trigger.buffEffectType = buffEffectType;
            trigger.paramInt = paramInt;

            return trigger;
        }
    }
}