using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Trainworks.Interfaces;
using Trainworks.Managers;
using Trainworks.Utilities;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Trainworks.Constants;
using Trainworks.Builders;

namespace Trainworks
{
    /// <summary>
    /// The entry point for the framework.
    /// </summary>
    [BepInPlugin(MODGUID, MODNAME, VERSION)]
    [BepInProcess("MonsterTrain.exe")]
    [BepInProcess("MtLinkHandler.exe")]
    [BepInDependency("tools.modding.trainworks")]
    public class TrainworksExtended : BaseUnityPlugin, IInitializable
    {
        public const string MODGUID = "tools.modding.trainworks.extended";
        public const string MODNAME = "Trainworks Modding Tools Extended";
        public const string VERSION = "1.0";


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
