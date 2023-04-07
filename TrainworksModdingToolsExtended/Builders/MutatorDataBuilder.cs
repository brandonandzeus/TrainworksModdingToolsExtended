using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using Trainworks.Managers;
using UnityEngine;

namespace Trainworks.BuildersV2
{
    public class MutatorDataBuilder
    {
        /// <summary>
        /// Don't set directly; use MutatorID instead.
        /// Unique string used to store and retrieve the mutator data.
        /// </summary>
        public string mutatorID;

        /// <summary>
        /// Unique string used to store and retrieve the mutator data.
        /// Implicitly sets NameKey and DescriptionKey.
        /// </summary>
        public string MutatorID
        {
            get { return this.mutatorID; }
            set
            {
                this.mutatorID = value;
                if (this.NameKey == null)
                {
                    this.NameKey = this.mutatorID + "_MutatorData_NameKey";
                }
                if (this.DescriptionKey == null)
                {
                    this.DescriptionKey = this.mutatorID + "_MutatorData_DescriptionKey";
                }
            }
        }

        /// <summary>
        /// Name displayed for the mutator.
        /// Note that setting this property will set the localization for all languages.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Localization key for the mutator's name.
        /// Note that setting MutatorID sets this field to [mutatorID]_MutatorData_NameKey.
        /// </summary>
        public string NameKey { get; set; }
        /// <summary>
        /// Description displayed for the relic.
        /// Note that setting this property will set the localization for all languages.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Localization key for the relic's description.
        /// Note that setting MutatorID sets this field to [mutatorID]_MutatorData_DescriptionKey.
        /// </summary>
        public string DescriptionKey { get; set; }
        /// <summary>
        /// Custom asset path to load mutator art from. 76x76 image.
        /// </summary>
        public string IconPath { get; set; }

        // TODO port RelicEffectDataBuilder
        public List<Builders.RelicEffectDataBuilder> EffectBuilders { get; set; }
        public List<RelicEffectData> Effects { get; set; }

        public string RelicActivatedKey { get; set; }
        public List<string> RelicLoreTooltipKeys { get; set; }

        /// <summary>
        /// This typically ranges from -10 to +10.
        /// <br></br>
        /// Positive numbers indicate beneficial mutators, while negative numbers indicate harmful ones.
        /// <br></br>
        /// When a player clicks the 'randomize mutators' button, one positive, one negative, and one other mutator will be chosen. The total combined boon value will be -5 to -2.
        /// </summary>
        public int BoonValue { get; set; }
        /// <summary>
        /// Tags are metadata that can place the mutator into different categories.
        /// <br></br>
        /// They are mutually exclusive, and no two tags will ever be the same when mutators are selected randomly.
        /// <br></br>
        /// ex: 'friendlyeffect'
        /// </summary>
        public List<string> Tags { get; set; }
        /// <summary>
        /// This determines whether the player needs the DLC to be active to use this mutator.
        /// <br></br>
        /// Defaults to 'DLC.None'.
        /// </summary>
        public ShinyShoe.DLC RequiredDLC { get; set; }
        /// <summary>
        /// When set to 'true', this mutator will never be picked randomly.
        /// </summary>
        public bool DisableInDailyChallenges { get; set; }

        /// <summary>
        /// The full, absolute path to the asset. Concatenates BaseAssetPath and IconPath.
        /// </summary>
        public string FullAssetPath => BaseAssetPath + "/" + IconPath;
        /// <summary>
        /// Set automatically in the constructor. Base asset path, usually the plugin directory.
        /// </summary>
        public string BaseAssetPath { get; private set; }

        public MutatorDataBuilder()
        {

            this.Name = "";
            this.Description = null;
            this.Effects = new List<RelicEffectData>();
            this.EffectBuilders = new List<Builders.RelicEffectDataBuilder>();
            this.RelicActivatedKey = "";
            this.RelicLoreTooltipKeys = new List<string>();
            this.BoonValue = 0;
            this.DisableInDailyChallenges = false;
            this.RequiredDLC = ShinyShoe.DLC.None;
            this.Tags = new List<string>();

            var assembly = Assembly.GetCallingAssembly();
            this.BaseAssetPath = PluginManager.PluginGUIDToPath[PluginManager.AssemblyNameToPluginGUID[assembly.FullName]];
        }

        /// <summary>
        /// Builds the RelicData represented by this builder's parameters recursively
        /// and registers it and its components with the appropriate managers.
        /// </summary>
        /// <returns>The newly registered RelicData</returns>
        public MutatorData BuildAndRegister()
        {
            var mutatorData = this.Build();
            CustomMutatorManager.RegisterCustomMutator(mutatorData);
            return mutatorData;
        }

        /// <summary>
        /// Builds the RelicData represented by this builder's parameters recursively;
        /// all Builders represented in this class's various fields will also be built.
        /// </summary>
        /// <returns>The newly created RelicData</returns>
        public MutatorData Build()
        {
            foreach (var builder in this.EffectBuilders)
            {
                this.Effects.Add(builder.Build());
            }

            var relicData = ScriptableObject.CreateInstance<MutatorData>();

            AccessTools.Field(typeof(GameData), "id").SetValue(relicData, this.MutatorID);
            relicData.name = this.MutatorID;
            // RelicData fields
            BuilderUtils.ImportStandardLocalization(this.DescriptionKey, this.Description);
            AccessTools.Field(typeof(RelicData), "descriptionKey").SetValue(relicData, this.DescriptionKey);
            AccessTools.Field(typeof(RelicData), "effects").SetValue(relicData, this.Effects);
            if (this.IconPath != null)
            {
                Sprite iconSprite = CustomAssetManager.LoadSpriteFromPath(this.FullAssetPath);
                AccessTools.Field(typeof(RelicData), "icon").SetValue(relicData, iconSprite);
            }
            BuilderUtils.ImportStandardLocalization(this.NameKey, this.Name);
            AccessTools.Field(typeof(RelicData), "nameKey").SetValue(relicData, this.NameKey);
            AccessTools.Field(typeof(RelicData), "relicActivatedKey").SetValue(relicData, this.RelicActivatedKey);
            AccessTools.Field(typeof(RelicData), "relicLoreTooltipKeys").SetValue(relicData, this.RelicLoreTooltipKeys);

            // MutatorData fields
            AccessTools.Field(typeof(MutatorData), "boonValue").SetValue(relicData, this.BoonValue);
            AccessTools.Field(typeof(MutatorData), "tags").SetValue(relicData, this.Tags);
            AccessTools.Field(typeof(MutatorData), "disableInDailyChallenges").SetValue(relicData, this.DisableInDailyChallenges);
            AccessTools.Field(typeof(MutatorData), "requiredDLC").SetValue(relicData, this.RequiredDLC);
            return relicData;
        }
    }
}