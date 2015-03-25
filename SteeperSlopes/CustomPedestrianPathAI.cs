namespace SteeperSlopes
{
    internal class CustomPedestrianPathAI
    {
        private static void GetElevationLimits(PedestrianPathAI ai, int min, out int max)
        {
            min = 0;
            max = ModInfo.IsEnabled ? 8 : 2;
        }
    }
}
