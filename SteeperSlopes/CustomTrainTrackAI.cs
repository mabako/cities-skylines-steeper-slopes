namespace SteeperSlopes
{
    internal class CustomTrainTrackAI
    {
        private static void GetElevationLimits(TrainTrackAI ai, out int min, out int max)
        {
            min = 0;
            max = ModInfo.IsEnabled ? 20 : 5;
        }
    }
}
