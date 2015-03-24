﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteeperSlopes
{
    class CustomRoadAI : RoadAI
    {
        public override void GetElevationLimits(out int min, out int max)
        {
            min = 0;
            max = 20; // 5
        }
    }
}
