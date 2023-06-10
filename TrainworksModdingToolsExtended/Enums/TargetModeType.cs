using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.Enums;

namespace Trainworks.Enums
{
    public class TargetModeType : ExtendedByteEnum<TargetModeType, TargetMode>
    {
        private static byte InitialID = 128;

        public TargetModeType(string Name, byte? ID = null) : base(Name, ID ?? GetNewGUID())
        {

        }

        public static byte GetNewGUID()
        {
            InitialID++;
            return InitialID;
        }
    }
}
