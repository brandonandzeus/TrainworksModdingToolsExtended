using BepInEx;
using HarmonyLib;
using Trainworks.Interfaces;

namespace Trainworks
{
    [BepInPlugin(MODGUID, MODNAME, VERSION)]
    [BepInProcess("MonsterTrain.exe")]
    [BepInProcess("MtLinkHandler.exe")]
    [BepInDependency("tools.modding.trainworks")]
    public class TrainworksExtended : BaseUnityPlugin, IInitializable
    {
        public const string MODGUID = "tools.modding.trainworks.extended";
        public const string MODNAME = "Trainworks Modding Tools Extended";
        public const string VERSION = "1.0.1";

        private void Awake()
        {
            var harmony = new Harmony(MODGUID);
            harmony.PatchAll();
        }

        public void Initialize()
        {
        }
    }
}
