﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Trainworks.BuildersV2
{
    public class RandomChampionPoolBuilder
    {
        /// <summary>
        /// This is both its ID and its name. Must be unique.
        /// </summary>
        public string ChampionPoolID { get; set; }

        /// <summary>
        /// Champion card along with a set of upgrades to apply to the card when picked.
        /// </summary>
        public List<RandomChampionPool.GrantedChampionInfo> ChampionInfoList { get; set; }

        public RandomChampionPoolBuilder()
        {
            ChampionInfoList = new List<RandomChampionPool.GrantedChampionInfo>();
        }

        /// <summary>
        /// Builds the RandomChampionPool represented by this builder's parameters
        /// </summary>
        /// <returns>The newly created RandomChampionPool</returns>
        public RandomChampionPool Build()
        {
            // Not catastrophic enough to pop an error message, this should be provided though.
            if (ChampionPoolID == null)
                Trainworks.Log(BepInEx.Logging.LogLevel.Error, "Error should provide a ChampionPoolID.");

            RandomChampionPool championPool = ScriptableObject.CreateInstance<RandomChampionPool>();
            championPool.name = ChampionPoolID;

            var championInfoList = (Malee.ReorderableArray<RandomChampionPool.GrantedChampionInfo>)AccessTools.Field(typeof(RandomChampionPool), "championInfoList").GetValue(championPool);
            championInfoList.CopyFrom(ChampionInfoList);

            return championPool;
        }
    }
}
