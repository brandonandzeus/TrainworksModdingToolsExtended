using BepInEx.Logging;
using System.Collections.Generic;
using Trainworks.Utilities;

namespace Trainworks.Managers
{
    class CustomChallengeManager
    {
        private static Dictionary<string, SpChallengeData> CustomChallengeData = new Dictionary<string, SpChallengeData>();

        public static void RegisterCustomChallenge(SpChallengeData challengeData)
        {
            if (!CustomChallengeData.ContainsKey(challengeData.GetID()))
            {
                CustomChallengeData.Add(challengeData.GetID(), challengeData);
                ProviderManager.SaveManager.GetAllGameData().GetBalanceData().GetSpChallenges().Add(challengeData);
            }
            else
            {
                Trainworks.Log(LogLevel.Warning, "Attempted to register duplicate challenge data with name: " + challengeData.name);
            }
        }

        public static SpChallengeData FindChallengeByID(string id)
        {
            var guid = GUIDGenerator.GenerateDeterministicGUID(id);
            if (CustomChallengeData.ContainsKey(guid))
            {
                return CustomChallengeData[guid];
            }

            foreach (var challenge in ProviderManager.SaveManager.GetBalanceData().GetSpChallenges())
            {
                if (challenge.GetID() == id)
                {
                    return challenge;
                }
            }

            Trainworks.Log(LogLevel.All, "Couldn't find challenge: " + id + " - This will cause crashes.");
            return null;

        }
    }
}
