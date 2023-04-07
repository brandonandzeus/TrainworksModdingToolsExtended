using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Trainworks.Managers;
using Trainworks.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Trainworks.BuildersV2
{
    public class CardDataBuilder
    {
        private string cardID;

        /// <summary>
        /// Unique string used to store and retrieve the card data.
        /// Implicitly sets NameKey and OverrideDescriptionKey if null
        /// </summary>
        public string CardID
        {
            get { return cardID; }
            set
            {
                cardID = value;
                if (NameKey == null)
                {
                    NameKey = cardID + "_CardData_NameKey";
                }
                if (OverrideDescriptionKey == null)
                {
                    OverrideDescriptionKey = cardID + "_CardData_OverrideDescriptionKey";
                }
            }
        }

        /// <summary>
        /// The IDs of all card pools the card should be inserted into.
        /// </summary>
        public List<string> CardPoolIDs { get; set; }

        /// <summary>
        /// Ember cost of the card.
        /// </summary>
        public int Cost { get; set; }
        /// <summary>
        /// Determines whether the card has a normal ember cost or an X ember cost.
        /// </summary>
        public CardData.CostType CostType { get; set; }
        /// <summary>
        /// Name displayed on the card.
        /// Note this sets the localization for all languages.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Localization key for the card's name.
        /// </summary>
        public string NameKey { get; set; }
        /// <summary>
        /// Custom description text appended to the end of the card.
        /// Note this is set for all langauges if set.
        /// Overridden by the OverrideDescriptionKey field.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Localization key for the card's description.
        /// </summary>
        public string OverrideDescriptionKey { get; set; }

        /// <summary>
        /// ID of the clan the card is a part of. Leave null for clanless.
        /// Base game clan IDs should be retrieved via helper class "VanillaClanIDs".
        /// </summary>
        public string ClanID { get; set; }

        /// <summary>
        /// The full, absolute path to the asset. Concatenates BaseAssetPath and AssetPath.
        /// </summary>
        public string FullAssetPath => BaseAssetPath + "/" + AssetPath;
        /// <summary>
        /// Set automatically in the constructor. Base asset path, usually the plugin directory.
        /// </summary>
        public string BaseAssetPath { get; set; }
        /// <summary>
        /// Custom asset path to load from. Must be inside the BaseAssetPath.
        /// </summary>
        public string AssetPath { get; set; } = "";
        /// <summary>
        /// Loading Info for loading a card's sprite from an asset bundle.
        /// </summary>
        public BundleAssetLoadingInfo BundleLoadingInfo { get; set; }
        /// <summary>
        /// Use an existing base game card's art by filling this in with the appropriate card's asset reference information.
        /// </summary>
        public AssetReferenceGameObject CardArtPrefabVariantRef { get; set; }

        /// <summary>
        /// Append to this list to add new card effects. The Build() method recursively builds all nested builders.
        /// </summary>
        public List<CardEffectDataBuilder> EffectBuilders { get; set; }
        /// <summary>
        /// Append to this list to add new card traits. The Build() method recursively builds all nested builders.
        /// </summary>
        public List<CardTraitDataBuilder> TraitBuilders { get; set; }
        /// <summary>
        /// Append to this list to add new character triggers. The Build() method recursively builds all nested builders.
        /// </summary>
        public List<CharacterTriggerDataBuilder> EffectTriggerBuilders { get; set; }
        /// <summary>
        /// Append to this list to add new card triggers. The Build() method recursively builds all nested builders.
        /// </summary>
        public List<CardTriggerEffectDataBuilder> TriggerBuilders { get; set; }
        /// <summary>
        /// Set CardArtPrefabVariantRef without reflection. The Build() method recursively builds all nested builders.
        /// </summary>
        public Builders.AssetRefBuilder CardArtPrefabVariantRefBuilder { get; set; }


        /// <summary>
        /// List of pre-built card effects.
        /// </summary>
        public List<CardEffectData> Effects { get; set; }
        /// <summary>
        /// List of pre-built card traits.
        /// </summary>
        public List<CardTraitData> Traits { get; set; }
        /// <summary>
        /// List of pre-built character triggers.
        /// </summary>
        public List<CharacterTriggerData> EffectTriggers { get; set; }
        /// <summary>
        /// List of pre-built card triggers.
        /// </summary>
        public List<CardTriggerEffectData> Triggers { get; set; }

        /// <summary>
        /// These upgrades are applied to all new instances of this card by default.
        /// </summary>
        public List<CardUpgradeData> StartingUpgrades { get; set; }

        /// <summary>
        /// Use an existing base game card's lore tooltip by adding its key to this list.
        /// </summary>
        public List<string> CardLoreTooltipKeys { get; set; }

        /// <summary>
        /// Whether or not the card has a target.
        /// </summary>
        public bool Targetless { get; set; }
        /// <summary>
        /// Whether or not the card targets a room.
        /// </summary>
        public bool TargetsRoom { get; set; }

        /// <summary>
        /// The class associated with the card.
        /// </summary>
        public ClassData LinkedClass { get; set; }
        /// <summary>
        /// The type of card: Spell, Monster, Blight, Scourge, or Invalid.
        /// </summary>
        public CardType CardType { get; set; }
        /// <summary>
        /// The card's rarity: Common, Uncommon, Rare, Champion, Starter
        /// </summary>
        public CollectableRarity Rarity { get; set; }

        /// <summary>
        /// Level at which the card is unlocked.
        /// </summary>
        public int UnlockLevel { get; set; }
        public List<CardData> SharedMasteryCards { get; set; }
        public CardData LinkedMasteryCard { get; set; }
        /// <summary>
        /// Whether or not this card is displayed in the logbook when counting all of the player's mastered cards.
        /// </summary>
        public bool IgnoreWhenCountingMastery { get; set; }

        /// <summary>
        /// A cache for the card's sprite so it doesn't have to be reloaded repeatedly.
        /// </summary>
        public Sprite SpriteCache { get; set; }
        /// <summary>
        /// In the event that art assets cannot be found, the game will search this for backup assets.
        /// </summary>
        public FallbackData FallbackData { get; set; }

        public CardInitialKeyboardTarget InitialKeyboardTarget { get; set; }
        public ShinyShoe.DLC RequiredDLC { get; set; }

        public CardDataBuilder()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string assemblyPath = Path.GetDirectoryName(path);
            Name = "";
            Description = "";
            OverrideDescriptionKey = null;

            CardPoolIDs = new List<string>();
            EffectBuilders = new List<CardEffectDataBuilder>();
            TraitBuilders = new List<CardTraitDataBuilder>();
            EffectTriggerBuilders = new List<CharacterTriggerDataBuilder>();
            TriggerBuilders = new List<CardTriggerEffectDataBuilder>();
            Effects = new List<CardEffectData>();
            Traits = new List<CardTraitData>();
            EffectTriggers = new List<CharacterTriggerData>();
            Triggers = new List<CardTriggerEffectData>();
            SharedMasteryCards = new List<CardData>();
            StartingUpgrades = new List<CardUpgradeData>();
            CardLoreTooltipKeys = new List<string>();

            var assembly = Assembly.GetCallingAssembly();
            if (PluginManager.AssemblyNameToPluginGUID.ContainsKey(assembly.FullName))
            {
                BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
            }
        }

        /// <summary>
        /// Builds the CardData represented by this builder's parameters recursively
        /// and registers it and its components with the appropriate managers.
        /// </summary>
        /// <returns>The newly registered CardData</returns>
        public CardData BuildAndRegister()
        {
            var cardData = Build();
            Trainworks.Log(LogLevel.Debug, "Adding custom card: " + cardData.GetName());
            CustomCardManager.RegisterCustomCard(cardData, CardPoolIDs);

            return cardData;
        }

        /// <summary>
        /// Builds the CardData represented by this builder's parameters recursively;
        /// i.e. all CardEffectBuilders in EffectBuilders will also be built.
        /// </summary>
        /// <returns>The newly created CardData</returns>
        public CardData Build()
        {
            foreach (var builder in EffectBuilders)
            {
                Effects.Add(builder.Build());
            }
            foreach (var builder in TraitBuilders)
            {
                Traits.Add(builder.Build());
            }
            foreach (var builder in EffectTriggerBuilders)
            {
                EffectTriggers.Add(builder.Build());
            }
            foreach (var builder in TriggerBuilders)
            {
                Triggers.Add(builder.Build());
            }

            var allGameData = ProviderManager.SaveManager.GetAllGameData();
            if (LinkedClass == null)
            {
                LinkedClass = CustomClassManager.GetClassDataByID(ClanID);
            }
            CardData cardData = ScriptableObject.CreateInstance<CardData>();
            var guid = GUIDGenerator.GenerateDeterministicGUID(CardID);
            AccessTools.Field(typeof(CardData), "id").SetValue(cardData, guid);
            cardData.name = CardID;
            if (CardArtPrefabVariantRef == null)
            {
                if (CardArtPrefabVariantRefBuilder == null)
                {
                    if (BundleLoadingInfo != null)
                    {
                        BundleLoadingInfo.PluginPath = BaseAssetPath;
                        CardArtPrefabVariantRefBuilder = new Builders.AssetRefBuilder
                        {
                            AssetLoadingInfo = BundleLoadingInfo
                        };
                    }
                    else
                    {
                        var assetLoadingInfo = new AssetLoadingInfo()
                        {
                            FilePath = AssetPath,
                            PluginPath = BaseAssetPath,
                            AssetType = Builders.AssetRefBuilder.AssetTypeEnum.CardArt
                        };
                        CardArtPrefabVariantRefBuilder = new Builders.AssetRefBuilder
                        {
                            AssetLoadingInfo = assetLoadingInfo
                        };
                    }
                }
                CardArtPrefabVariantRef = CardArtPrefabVariantRefBuilder.BuildAndRegister();
            }
            AccessTools.Field(typeof(CardData), "cardArtPrefabVariantRef").SetValue(cardData, CardArtPrefabVariantRef);
            AccessTools.Field(typeof(CardData), "cardLoreTooltipKeys").SetValue(cardData, CardLoreTooltipKeys);
            AccessTools.Field(typeof(CardData), "cardType").SetValue(cardData, CardType);
            AccessTools.Field(typeof(CardData), "cost").SetValue(cardData, Cost);
            AccessTools.Field(typeof(CardData), "costType").SetValue(cardData, CostType);
            AccessTools.Field(typeof(CardData), "effects").SetValue(cardData, Effects);
            AccessTools.Field(typeof(CardData), "effectTriggers").SetValue(cardData, EffectTriggers);
            AccessTools.Field(typeof(CardData), "fallbackData").SetValue(cardData, FallbackData);
            AccessTools.Field(typeof(CardData), "ignoreWhenCountingMastery").SetValue(cardData, IgnoreWhenCountingMastery);
            AccessTools.Field(typeof(CardData), "initialKeyboardTarget").SetValue(cardData, InitialKeyboardTarget);
            AccessTools.Field(typeof(CardData), "linkedClass").SetValue(cardData, LinkedClass);
            AccessTools.Field(typeof(CardData), "linkedMasteryCard").SetValue(cardData, LinkedMasteryCard);
            AccessTools.Field(typeof(CardData), "nameKey").SetValue(cardData, NameKey);
            AccessTools.Field(typeof(CardData), "overrideDescriptionKey").SetValue(cardData, OverrideDescriptionKey);
            AccessTools.Field(typeof(CardData), "rarity").SetValue(cardData, Rarity);
            AccessTools.Field(typeof(CardData), "requiredDLC").SetValue(cardData, RequiredDLC);
            AccessTools.Field(typeof(CardData), "sharedMasteryCards").SetValue(cardData, SharedMasteryCards);
            if (SpriteCache != null)
            {
                AccessTools.Field(typeof(CardData), "spriteCache").SetValue(cardData, SpriteCache);
            }
            AccessTools.Field(typeof(CardData), "startingUpgrades").SetValue(cardData, StartingUpgrades);
            AccessTools.Field(typeof(CardData), "targetless").SetValue(cardData, Targetless);
            AccessTools.Field(typeof(CardData), "targetsRoom").SetValue(cardData, TargetsRoom);
            foreach (CardTraitData cardTraitData in Traits)
            {
                AccessTools.Field(typeof(CardTraitData), "paramCardData").SetValue(cardTraitData, cardData);
            }
            AccessTools.Field(typeof(CardData), "traits").SetValue(cardData, Traits);
            AccessTools.Field(typeof(CardData), "triggers").SetValue(cardData, Triggers);
            AccessTools.Field(typeof(CardData), "unlockLevel").SetValue(cardData, UnlockLevel);

            BuilderUtils.ImportStandardLocalization(NameKey, Name);
            BuilderUtils.ImportStandardLocalization(OverrideDescriptionKey, Description);

            return cardData;
        }

        /// <summary>
        /// Creates an asset reference to an existing game file.
        /// Primarily useful for reusing base game art.
        /// Cards with custom art should not use this method.
        /// </summary>
        /// <param name="m_debugName">The asset's debug name (usually the path to it)</param>
        /// <param name="m_AssetGUID">The asset's GUID</param>
        public void CreateAndSetCardArtPrefabVariantRef(string m_debugName, string m_AssetGUID)
        {
            var assetReferenceGameObject = new AssetReferenceGameObject();
            AccessTools.Field(typeof(AssetReferenceGameObject), "m_debugName")
                    .SetValue(assetReferenceGameObject, m_debugName);
            AccessTools.Field(typeof(AssetReferenceGameObject), "m_AssetGUID")
                .SetValue(assetReferenceGameObject, m_AssetGUID);
            CardArtPrefabVariantRef = assetReferenceGameObject;

            AssetPath = m_AssetGUID;
        }

        /// <summary>
        /// Sets this card's clan to the clan whose ID is passed in
        /// </summary>
        /// <param name="clanID">ID of the clan, most easily retrieved using the helper class "VanillaClanIDs"</param>
        public void SetClan(string clanID)
        {
            ClanID = clanID;
        }

        /// <summary>
        /// Adds this card to the cardpool whose ID is passed in
        /// </summary>
        /// <param name="cardPoolID">ID of the card pool, most easily retrieved using the helper class "VanillaCardPoolIDs"</param>
        public void AddToCardPool(string cardPoolID)
        {
            CardPoolIDs.Add(cardPoolID);
        }
    }
}