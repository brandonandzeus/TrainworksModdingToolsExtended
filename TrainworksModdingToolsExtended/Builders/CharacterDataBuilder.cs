using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using BepInEx;
using BepInEx.Harmony;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ShinyShoe;
using Trainworks.Managers;
using Trainworks.Utilities;

namespace Trainworks.BuildersV2
{
    public class CharacterDataBuilder
    {
        /// <summary>
        /// Don't set directly; use CharacterID instead.
        /// Unique string used to store and retrieve the character data.
        /// </summary>
        public string characterID;

        /// <summary>
        /// Unique string used to store and retrieve the character data.
        /// Implictly sets NameKey if null.
        /// </summary>
        public string CharacterID
        {
            get { return this.characterID; }
            set
            {
                this.characterID = value;
                if (this.NameKey == null)
                {
                    this.NameKey = this.characterID + "_CharacterData_NameKey";
                }
            }
        }

        /// <summary>
        /// Name displayed for the character.
        /// Note that setting this property will set the localization for all languages.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Localization key for the character's name.
        /// Note that setting CharacterID sets this field to [CharacterID]_CharacterData_NameKey.
        /// </summary>
        public string NameKey { get; set; }

        /// <summary>
        /// The character's attack stat.
        /// </summary>
        public int AttackDamage { get; set; }
        /// <summary>
        /// The character's health stat.
        /// </summary>
        public int Health { get; set; }
        /// <summary>
        /// The amount of capacity the character uses up on the floor.
        /// </summary>
        public int Size { get; set; }

        public List<CharacterTriggerDataBuilder> TriggerBuilders { get; set; }
        public List<CharacterTriggerData> Triggers { get; set; }

        /// <summary>
        /// Status effects the character starts with when spawned in.
        /// </summary>
        public StatusEffectStackData[] StartingStatusEffects { get; set; }
        public string[] StatusEffectImmunities { get; set; }

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
        /// Loading Info for loading a character's art
        /// </summary>
        public BundleAssetLoadingInfo BundleLoadingInfo { get; set; }
        /// <summary>
        /// Use an existing base game character's art by filling this in with the appropriate character's asset reference information.
        /// </summary>
        public AssetReferenceGameObject CharacterPrefabVariantRef { get; set; }
        /// <summary>
        /// Set CardArtPrefabVariantRef without reflection. The Build() method recursively builds all nested builders.
        /// </summary>
        public Builders.AssetRefBuilder CharacterPrefabVariantRefBuilder { get; set; }

        /// <summary>
        /// Whether or not the character is able to attack.
        /// </summary>
        public bool CanAttack { get; set; }
        /// <summary>
        /// Whether or not the character is able to be healed.
        /// </summary>
        public bool CanBeHealed { get; set; }

        /// <summary>
        /// Whether or not the character is a miniboss (a non-flying boss).
        /// </summary>
        public bool IsMiniboss { get; set; }
        /// <summary>
        /// Whether or not the character is a flying boss.
        /// </summary>
        public bool IsOuterTrainBoss { get; set; }

        public bool DeathSlidesBackwards { get; set; }

        public List<ActionGroupData> BossActionGroups { get; set; }

        /// <summary>
        /// Use an existing base game character's lore tooltip by adding its key to this list.
        /// </summary>
        public List<string> CharacterLoreTooltipKeys { get; set; }

        public List<RoomModifierDataBuilder> RoomModifierBuilders { get; set; }
        public List<RoomModifierData> RoomModifiers { get; set; }

        public bool AscendsTrainAutomatically { get; set; }

        /// <summary>
        /// "When attacking, this character moves next to its target before hitting it." - base game comment
        /// </summary>
        public bool AttackTeleportsToDefender { get; set; }

        public List<string> SubtypeKeys { get; set; }

        public CharacterSoundData CharacterSoundData { get; set; }

        public CharacterChatterData CharacterChatterData { get; set; }

        public RuntimeAnimatorController AnimationController { get; set; }
        public CharacterDeathVFX.Type DeathType { get; set; }
        public VfxAtLoc DeathVFX { get; set; }
        /// <summary>
        /// "The VFX to put on the target when attacking to override CombatManager CombatReactVFXPrefab" - base game comment
        /// </summary>
        public VfxAtLoc ImpactVFX { get; set; }
        /// <summary>
        /// The projectile to fire when attacking - base game comment
        /// </summary>
        public VfxAtLoc ProjectilePrefab { get; set; }
        public VfxAtLoc BossRoomSpellCastVFX { get; set; }
        public VfxAtLoc BossSpellCastVFX { get; set; }

        /// <summary>
        /// A cache for the character's sprite so it doesn't have to be reloaded repeatedly.
        /// </summary>
        public Sprite CharacterSpriteCache { get; set; }
        /// <summary>
        /// "The default character prefab to use if one isn't found.  (Which should never happen in the shpped game)" - base game comment
        /// </summary>
        public FallbackData FallBackData { get; set; }
        /// <summary>
        /// Sets whether or not this unit should be considered for being automatically drawn as part of the priority unit draw system for hands. Defaults to true based 
        /// </summary>
        public bool PriorityDraw { get; set; }

        /// <summary>
        /// Holds the builder for a unit's synthesis ability.
        /// </summary>
        public CardUpgradeDataBuilder UnitSynthesisBuilder { get; set; }

        public bool BlockVisualSizeIncrease { get; set; }
        public CharacterData.ReorderableCharacterShardUpgradeList BypassPactCrystalsUpgradeDataList { get; set; }
        public int PactCrystalsRequiredCount { get; set; }
        public CharacterData PactCrystalsVariantData { get; set; }
        public bool RemoveTriggersOnRelentlessChange { get; set; }
        public BossState.AttackPhase ValidBossAttackPhase { get; set; }

        public CharacterDataBuilder()
        {
            AttackDamage = 10;
            Size = 2;
            AttackTeleportsToDefender = true;
            CanAttack = true;
            CanBeHealed = true;
            DeathSlidesBackwards = true;
            Triggers = new List<CharacterTriggerData>();
            SubtypeKeys = new List<string>();
            BossActionGroups = new List<ActionGroupData>();
            RoomModifiers = new List<RoomModifierData>();
            CharacterLoreTooltipKeys = new List<string>();
            StartingStatusEffects = new StatusEffectStackData[0];
            StatusEffectImmunities = new string[0];
            ImpactVFX = (VfxAtLoc)FormatterServices.GetUninitializedObject(typeof(VfxAtLoc));
            TriggerBuilders = new List<CharacterTriggerDataBuilder>();
            RoomModifierBuilders = new List<RoomModifierDataBuilder>();
            PriorityDraw = true;
            ValidBossAttackPhase = BossState.AttackPhase.Relentless;
            PactCrystalsRequiredCount = -1;

            var assembly = Assembly.GetCallingAssembly();
            BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
        }

        /// <summary>
        /// Builds the CharacterData represented by this builder's parameters recursively
        /// and registers it and its components with the appropriate managers.
        /// </summary>
        /// <returns>The newly registered CharacterData</returns>
        public CharacterData BuildAndRegister()
        {
            var characterData = this.Build();
            CustomCharacterManager.RegisterCustomCharacter(characterData);

            // Build the unit's synthesis ability
            if (UnitSynthesisBuilder == null)
            {
                // If none was provided, build a dummy synthesis ability
                if (!characterData.IsChampion())
                {
                    BuildDummyUnitSynthesis(characterData);
                }
            }
            else
            {
                UnitSynthesisBuilder.Build();
            }

            return characterData;
        }

        private void BuildDummyUnitSynthesis(CharacterData characterData)
        {
            new CardUpgradeDataBuilder()
            {
                UpgradeTitle = $"Dummy_synth_{characterData.name}",
                SourceSynthesisUnit = characterData,
                UpgradeDescription = "<DUMMY>",
                UpgradeDescriptionKey = "Default_dummy_synthesis_description",
                BonusDamage = 1
            }.Build();
        }

        /// <summary>
        /// Builds the CharacterData represented by this builder's parameters recursively;
        /// all Builders represented in this class's various fields will also be built.
        /// </summary>
        /// <returns>The newly created CharacterData</returns>
        public CharacterData Build()
        {
            CharacterData characterData = ScriptableObject.CreateInstance<CharacterData>();
            characterData.name = this.CharacterID;

            foreach (var builder in this.TriggerBuilders)
            {
                this.Triggers.Add(builder.Build());
            }
            foreach (var builder in this.RoomModifierBuilders)
            {
                this.RoomModifiers.Add(builder.Build());
            }

            var guid = GUIDGenerator.GenerateDeterministicGUID(this.CharacterID);

            if (this.CharacterPrefabVariantRef == null)
            {
                if (this.CharacterPrefabVariantRefBuilder == null)
                {
                    if (this.BundleLoadingInfo != null)
                    {
                        this.BundleLoadingInfo.PluginPath = this.BaseAssetPath;
                        this.CharacterPrefabVariantRefBuilder = new Builders.AssetRefBuilder
                        {
                            AssetLoadingInfo = this.BundleLoadingInfo
                        };
                    }
                    else
                    {
                        var assetLoadingInfo = new AssetLoadingInfo()
                        {
                            FilePath = this.AssetPath,
                            PluginPath = this.BaseAssetPath,
                            AssetType = Builders.AssetRefBuilder.AssetTypeEnum.Character
                        };
                        this.CharacterPrefabVariantRefBuilder = new Builders.AssetRefBuilder
                        {
                            AssetLoadingInfo = assetLoadingInfo
                        };
                    }
                }
                this.CharacterPrefabVariantRef = this.CharacterPrefabVariantRefBuilder.BuildAndRegister();
            }

            if (this.PriorityDraw)
            {
                this.SubtypeKeys.Add("SubtypesData_Chosen");
            }


            AccessTools.Field(typeof(CharacterData), "id").SetValue(characterData, guid);
            AccessTools.Field(typeof(CharacterData), "animationController").SetValue(characterData, this.AnimationController);
            AccessTools.Field(typeof(CharacterData), "ascendsTrainAutomatically").SetValue(characterData, this.AscendsTrainAutomatically);
            AccessTools.Field(typeof(CharacterData), "attackDamage").SetValue(characterData, this.AttackDamage);
            AccessTools.Field(typeof(CharacterData), "attackTeleportsToDefender").SetValue(characterData, this.AttackTeleportsToDefender);
            AccessTools.Field(typeof(CharacterData), "blockVisualSizeIncrease").SetValue(characterData, BlockVisualSizeIncrease);
            // TODO this needs a DataBuilder.
            AccessTools.Field(typeof(CharacterData), "bossActionGroups").SetValue(characterData, this.BossActionGroups);
            AccessTools.Field(typeof(CharacterData), "bossRoomSpellCastVFX").SetValue(characterData, this.BossRoomSpellCastVFX);
            AccessTools.Field(typeof(CharacterData), "bossSpellCastVFX").SetValue(characterData, this.BossSpellCastVFX);
            AccessTools.Field(typeof(CharacterData), "bypassPactCrystalsUpgradeDataList").SetValue(characterData, BypassPactCrystalsUpgradeDataList);
            AccessTools.Field(typeof(CharacterData), "canAttack").SetValue(characterData, this.CanAttack);
            AccessTools.Field(typeof(CharacterData), "canBeHealed").SetValue(characterData, this.CanBeHealed);
            // TODO this needs a DataBuilder.
            AccessTools.Field(typeof(CharacterData), "characterChatterData").SetValue(characterData, this.CharacterChatterData);
            AccessTools.Field(typeof(CharacterData), "characterLoreTooltipKeys").SetValue(characterData, this.CharacterLoreTooltipKeys);
            AccessTools.Field(typeof(CharacterData), "characterPrefabVariantRef").SetValue(characterData, this.CharacterPrefabVariantRef);
            AccessTools.Field(typeof(CharacterData), "characterSoundData").SetValue(characterData, this.CharacterSoundData);
            AccessTools.Field(typeof(CharacterData), "characterSpriteCache").SetValue(characterData, this.CharacterSpriteCache);
            AccessTools.Field(typeof(CharacterData), "deathSlidesBackwards").SetValue(characterData, this.DeathSlidesBackwards);
            AccessTools.Field(typeof(CharacterData), "deathType").SetValue(characterData, this.DeathType);
            AccessTools.Field(typeof(CharacterData), "deathVFX").SetValue(characterData, this.DeathVFX);
            AccessTools.Field(typeof(CharacterData), "fallbackData").SetValue(characterData, this.FallBackData);
            AccessTools.Field(typeof(CharacterData), "health").SetValue(characterData, this.Health);
            AccessTools.Field(typeof(CharacterData), "impactVFX").SetValue(characterData, this.ImpactVFX);
            AccessTools.Field(typeof(CharacterData), "isMiniboss").SetValue(characterData, this.IsMiniboss);
            AccessTools.Field(typeof(CharacterData), "isOuterTrainBoss").SetValue(characterData, this.IsOuterTrainBoss);
            AccessTools.Field(typeof(CharacterData), "nameKey").SetValue(characterData, this.NameKey);
            AccessTools.Field(typeof(CharacterData), "pactCrystalsRequiredCount").SetValue(characterData, PactCrystalsRequiredCount);
            AccessTools.Field(typeof(CharacterData), "pactCrystalsVariantData").SetValue(characterData, PactCrystalsVariantData);
            AccessTools.Field(typeof(CharacterData), "projectilePrefab").SetValue(characterData, this.ProjectilePrefab);
            AccessTools.Field(typeof(CharacterData), "removeTriggersOnRelentlessChange").SetValue(characterData, RemoveTriggersOnRelentlessChange);
            AccessTools.Field(typeof(CharacterData), "roomModifiers").SetValue(characterData, this.RoomModifiers);
            AccessTools.Field(typeof(CharacterData), "size").SetValue(characterData, this.Size);
            AccessTools.Field(typeof(CharacterData), "startingStatusEffects").SetValue(characterData, this.StartingStatusEffects);
            AccessTools.Field(typeof(CharacterData), "statusEffectImmunities").SetValue(characterData, this.StatusEffectImmunities);
            AccessTools.Field(typeof(CharacterData), "subtypeKeys").SetValue(characterData, this.SubtypeKeys);
            AccessTools.Field(typeof(CharacterData), "triggers").SetValue(characterData, this.Triggers);
            AccessTools.Field(typeof(CharacterData), "validBossAttackPhase").SetValue(characterData, ValidBossAttackPhase);

            BuilderUtils.ImportStandardLocalization(this.NameKey, this.Name);
            return characterData;
        }

        /// <summary>
        /// Creates an asset reference to an existing game file.
        /// Primarily useful for reusing base game art.
        /// Cards with custom art should not use this method.
        /// </summary>
        /// <param name="m_debugName">The asset's debug name (usually the path to it)</param>
        /// <param name="m_AssetGUID">The asset's GUID</param>
        public void CreateAndSetCharacterArtPrefabVariantRef(string m_debugName, string m_AssetGUID)
        {
            var assetReferenceGameObject = new AssetReferenceGameObject();
            AccessTools.Field(typeof(AssetReferenceGameObject), "m_debugName")
                    .SetValue(assetReferenceGameObject, m_debugName);
            AccessTools.Field(typeof(AssetReferenceGameObject), "m_AssetGUID")
                .SetValue(assetReferenceGameObject, m_AssetGUID);
            this.CharacterPrefabVariantRef = assetReferenceGameObject;

            this.AssetPath = m_AssetGUID;
        }

        /// <summary>
        /// Add a status effect to this character's starting status effect array.
        /// </summary>
        /// <param name="statusEffectID">ID of the status effect, most easily retrieved using the helper class "MTStatusEffectIDs"</param>
        /// <param name="stackCount">Number of stacks to apply</param>
        public void AddStartingStatusEffect(string statusEffectID, int stackCount)
        {
            this.StartingStatusEffects = BuilderUtils.AddStatusEffect(statusEffectID, stackCount, this.StartingStatusEffects);
        }
    }
}