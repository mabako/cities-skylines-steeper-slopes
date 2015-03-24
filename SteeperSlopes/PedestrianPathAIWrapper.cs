using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SteeperSlopes
{
    class PedestrianPathAIWrapper : PedestrianPathAI
    {
        public override void GetElevationLimits(out int min, out int max)
        {
            min = 0;
            max = 8; // 2
        }
    }
}
