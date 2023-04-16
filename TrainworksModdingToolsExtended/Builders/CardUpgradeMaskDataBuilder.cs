using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Trainworks.BuildersV2
{
    // Adopted from ThreeFishes's Equestrian Clan.
    public class CardUpgradeMaskDataBuilder
    {
        public string CardUpgradeMaskDataID { get; set; }
        public CardType CardType { get; set; } = CardType.Invalid;
        /// <summary>
        /// Note that these are subtype keys required, not the localized name.
        /// </summary>
        public List<string> RequiredSubtypes { get; set; } = new List<string>();
        /// <summary>
        /// Note that these are subtype keys required, not the localized name.
        /// </summary>
        public List<string> ExcludedSubtypes { get; set; } = new List<string>();
        public List<StatusEffectStackData> RequiredStatusEffects { get; set; } = new List<StatusEffectStackData>();
        public List<StatusEffectStackData> ExcludedStatusEffects { get; set; } = new List<StatusEffectStackData>();
        /// <summary>
        /// Note if you are checking for a CardTrait in the base game feel free to use the exact class name of it.
        /// However if it is a custom CardTrait you will need to pass typeof(Class).AssemblyQualifiedName.
        /// </summary>
        public List<string> RequiredCardTraits { get; set; } = new List<string>();
        /// <summary>
        /// Note if you are checking for a CardTrait in the base game feel free to use the exact class name of it.
        /// However if it is a custom CardTrait you will need to pass typeof(Class).AssemblyQualifiedName.
        /// </summary>
        public List<string> ExcludedCardTraits { get; set; } = new List<string>();
        /// <summary>
        /// Note if you are checking for a CardEffect in the base game feel free to use the exact class name of it.
        /// However if it is a custom CardEffect you will need to pass typeof(Class).AssemblyQualifiedName.
        /// </summary>
        public List<string> RequiredCardEffects { get; set; } = new List<string>();
        /// <summary>
        /// Note if you are checking for a CardEffect in the base game feel free to use the exact class name of it.
        /// However if it is a custom CardEffect you will need to pass typeof(Class).AssemblyQualifiedName.
        /// </summary>
        public List<string> ExcludedCardEffects { get; set; } = new List<string>();
        /// <summary>
        /// If there are any cards in this pool, then only the cards in this pool will be allowed
        /// </summary>
        public List<CardPool> AllowedCardPools { get; set; } = new List<CardPool>();
        /// <summary>
        /// No cards in this pool will be allowed
        /// </summary>
        public List<CardPool> DisallowedCardPools { get; set; } = new List<CardPool>();
        public List<int> RequiredSizes { get; set; } = new List<int>();
        public List<int> ExcludedSizes { get; set; } = new List<int>();
        public Vector2 CostRange { get; set; } = new Vector2(0f, 99f);
        public bool ExcludeNonAttackingMonsters { get; set; }

        /// <summary>
        /// Operator determines if we require all or at least one
        /// </summary>
        public CardUpgradeMaskDataBuilder.CompareOperator RequiredSubtypesOperator { get; set; }
        public CardUpgradeMaskDataBuilder.CompareOperator ExcludedSubtypesOperator { get; set; }
        public CardUpgradeMaskDataBuilder.CompareOperator RequiredStatusEffectsOperator { get; set; }
        public CardUpgradeMaskDataBuilder.CompareOperator ExcludedStatusEffectsOperator { get; set; }
        public CardUpgradeMaskDataBuilder.CompareOperator RequiredCardTraitsOperator { get; set; }
        public CardUpgradeMaskDataBuilder.CompareOperator ExcludedCardTraitsOperator { get; set; }
        public CardUpgradeMaskDataBuilder.CompareOperator RequiredCardEffectsOperator { get; set; }
        public CardUpgradeMaskDataBuilder.CompareOperator ExcludedCardEffectsOperator { get; set; }
        public bool RequireXCost { get; set; }
        public bool ExcludeXCost { get; set; }

        /// <summary>
        /// This is the reason why a card is filtered away from having this upgrade applied to it
        /// </summary>
        public CardState.UpgradeDisabledReason UpgradeDisabledReason { get; set; }

        /// <summary>
        /// Builds the CardUpgradeMaskData represented by this builders's parameters recursively;
        /// </summary>
        /// <returns>The newly created CardUpgradeMaskData</returns>
        public CardUpgradeMaskData Build()
        {
            // Not catastrophic enough to pop an error message, this should be provided though.
            if (CardUpgradeMaskDataID == null)
            {
                Trainworks.Log(BepInEx.Logging.LogLevel.Error, "Error should provide a CardUpgradeMaskDataID.");
            }

            CardUpgradeMaskData cardUpgradeMaskData = ScriptableObject.CreateInstance<CardUpgradeMaskData>();
            cardUpgradeMaskData.name = CardUpgradeMaskDataID;
            Type realEnumType = AccessTools.Inner(typeof(CardUpgradeMaskData), "CompareOperator");
            AccessTools.Field(typeof(CardUpgradeMaskData), "allowedCardPools").SetValue(cardUpgradeMaskData, AllowedCardPools);
            AccessTools.Field(typeof(CardUpgradeMaskData), "cardType").SetValue(cardUpgradeMaskData, CardType);
            AccessTools.Field(typeof(CardUpgradeMaskData), "costRange").SetValue(cardUpgradeMaskData, CostRange);
            AccessTools.Field(typeof(CardUpgradeMaskData), "disallowedCardPools").SetValue(cardUpgradeMaskData, DisallowedCardPools);
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludedCardEffects").SetValue(cardUpgradeMaskData, ExcludedCardEffects);
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludedCardEffectsOperator").SetValue(cardUpgradeMaskData, Enum.ToObject(realEnumType, ExcludedCardEffectsOperator));
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludedCardTraits").SetValue(cardUpgradeMaskData, ExcludedCardTraits);
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludedCardTraitsOperator").SetValue(cardUpgradeMaskData, Enum.ToObject(realEnumType, ExcludedCardTraitsOperator));
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludedSizes").SetValue(cardUpgradeMaskData, ExcludedSizes);
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludedStatusEffects").SetValue(cardUpgradeMaskData, ExcludedStatusEffects);
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludedStatusEffectsOperator").SetValue(cardUpgradeMaskData, Enum.ToObject(realEnumType, ExcludedStatusEffectsOperator));
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludedSubtypes").SetValue(cardUpgradeMaskData, ExcludedSubtypes);
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludedSubtypesOperator").SetValue(cardUpgradeMaskData, Enum.ToObject(realEnumType, ExcludedSubtypesOperator));
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludeNonAttackingMonsters").SetValue(cardUpgradeMaskData, ExcludeNonAttackingMonsters);
            AccessTools.Field(typeof(CardUpgradeMaskData), "excludeXCost").SetValue(cardUpgradeMaskData, ExcludeXCost);
            AccessTools.Field(typeof(CardUpgradeMaskData), "requiredCardEffects").SetValue(cardUpgradeMaskData, RequiredCardEffects);
            AccessTools.Field(typeof(CardUpgradeMaskData), "requiredCardEffectsOperator").SetValue(cardUpgradeMaskData, Enum.ToObject(realEnumType, RequiredCardEffectsOperator));
            AccessTools.Field(typeof(CardUpgradeMaskData), "requiredCardTraits").SetValue(cardUpgradeMaskData, RequiredCardTraits);
            AccessTools.Field(typeof(CardUpgradeMaskData), "requiredCardTraitsOperator").SetValue(cardUpgradeMaskData, Enum.ToObject(realEnumType, RequiredCardTraitsOperator));
            AccessTools.Field(typeof(CardUpgradeMaskData), "requiredSizes").SetValue(cardUpgradeMaskData, RequiredSizes);
            AccessTools.Field(typeof(CardUpgradeMaskData), "requiredStatusEffects").SetValue(cardUpgradeMaskData, RequiredStatusEffects);
            AccessTools.Field(typeof(CardUpgradeMaskData), "requiredStatusEffectsOperator").SetValue(cardUpgradeMaskData, Enum.ToObject(realEnumType, RequiredStatusEffectsOperator));
            AccessTools.Field(typeof(CardUpgradeMaskData), "requiredSubtypes").SetValue(cardUpgradeMaskData, RequiredSubtypes);
            AccessTools.Field(typeof(CardUpgradeMaskData), "requiredSubtypesOperator").SetValue(cardUpgradeMaskData, Enum.ToObject(realEnumType, RequiredSubtypesOperator));
            AccessTools.Field(typeof(CardUpgradeMaskData), "requireXCost").SetValue(cardUpgradeMaskData, RequireXCost);
            AccessTools.Field(typeof(CardUpgradeMaskData), "upgradeDisabledReason").SetValue(cardUpgradeMaskData, UpgradeDisabledReason);
            return cardUpgradeMaskData;
        }

        public enum CompareOperator
        {
            And,
            Or
        }
    }
}