using System;
using System.Collections.Generic;
using System.Text;

namespace Trainworks.Enums
{
    public class StatusEffectTriggerStage : ExtendedByteEnum<StatusEffectTriggerStage, StatusEffectData.TriggerStage>
    {
        private static byte InitialID = 128;

        public StatusEffectTriggerStage(string Name, byte? ID = null) : base(Name, ID ?? GetNewGUID())
        {

        }

        public static byte GetNewGUID()
        {
            InitialID++;
            return InitialID;
        }
    }

}
