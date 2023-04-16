﻿using System.Reflection;
using Trainworks.ConstantsV2;
using Trainworks.Managers;
using UnityEngine;

namespace Trainworks.BuildersV2
{
    public class ChampionCardDataBuilder : CardDataBuilder
    {
        public CharacterDataBuilder Champion { get; set; }
        public CardData StarterCardData { get; set; }
        public CardUpgradeTreeDataBuilder UpgradeTree { get; set; }
        public string ChampionIconPath { get; set; }
        public string ChampionSelectedCue { get; set; }

        public ChampionCardDataBuilder() : base()
        {
            Rarity = CollectableRarity.Champion;

            EffectBuilders.Add(new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectSpawnMonster),
                TargetMode = TargetMode.DropTargetCharacter,
            });

            CardType = CardType.Monster;
            TargetsRoom = true;

            var assembly = Assembly.GetCallingAssembly();
            BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
        }

        /// <summary>
        /// Builds the CardData represented by this builder's parameters
        /// and registers it and its components with the appropriate managers.
        /// </summary>
        /// <returns>The newly registered CardData</returns>
        public CardData BuildAndRegister(int ChampionIndex = 0)
        {
            var cardData = Build();
            CustomCardManager.RegisterCustomCard(cardData, CardPoolIDs);

            var Clan = cardData.GetLinkedClass();

            ChampionData ClanChamp = Clan.GetChampionData(ChampionIndex);
            ClanChamp.championCardData = cardData;
            if (ChampionIconPath != null)
            {
                Sprite championIconSprite = CustomAssetManager.LoadSpriteFromPath(BaseAssetPath + "/" + ChampionIconPath);
                ClanChamp.championIcon = championIconSprite;
            }
            ClanChamp.starterCardData = StarterCardData;
            if (UpgradeTree != null)
            {
                ClanChamp.upgradeTree = UpgradeTree.Build();
            }
            ClanChamp.championSelectedCue = ChampionSelectedCue;

            return cardData;
        }

        public new CardData BuildAndRegister()
        {
            return BuildAndRegister(0);
        }

        /// <summary>
        /// Builds the CardData represented by this builder's parameters
        /// i.e. all CardEffectBuilders in EffectBuilders will also be built.
        /// </summary>
        /// <returns>The newly created CardData</returns>
        public new CardData Build()
        {
            Champion.SubtypeKeys.Add(VanillaSubtypeIDs.Champion);
            EffectBuilders[0].ParamCharacterDataBuilder = Champion;

            CardData cardData = base.Build();

            return cardData;
        }
    }
}
