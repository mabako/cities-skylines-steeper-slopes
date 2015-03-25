namespace SteeperSlopes
{
    internal class CustomRoadAI
    {
        private static void GetElevationLimits(RoadAI ai, out int min, out int max)
        {
            min = 0;
            max = ModInfo.IsEnabled ? 20 : 5;
        }
    }
}
