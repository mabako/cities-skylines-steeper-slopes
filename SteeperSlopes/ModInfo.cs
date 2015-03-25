using CitiesSkylinesDetour;
using ColossalFramework.Plugins;
using ICities;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SteeperSlopes
{
    public class ModInfo : IUserMod
    {
        public static bool IsEnabled;

        public string Description
        {
            get
            {
                return "Build better roads";
            }
        }

        public string Name
        {
            get
            {
                Setup();
                return "Steeper Slopes & Higher Bridges";
            }
        }

        private void Setup()
        {
            try
            {
                RedirectCalls(typeof(RoadAI), typeof(CustomRoadAI), "GetElevationLimits");
                RedirectCalls(typeof(TrainTrackAI), typeof(CustomTrainTrackAI), "GetElevationLimits");
                RedirectCalls(typeof(PedestrianPathAI), typeof(CustomPedestrianPathAI), "GetElevationLimits");

                // We should probably check if we're enabled
                PluginsChanged();
                PluginManager.instance.eventPluginsChanged += PluginsChanged;
                PluginManager.instance.eventPluginsStateChanged += PluginsChanged;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, "[SteeperSlopes] " + e.GetType() + ": " + e.Message);
            }
        }

        private void RedirectCalls(Type type1, Type type2, string p)
        {
            //Debug.LogFormat("{0}/{1}/{2}", type1, type2, p);
            RedirectionHelper.RedirectCalls(type1.GetMethod(p, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static), type2.GetMethod(p, BindingFlags.NonPublic | BindingFlags.Static));
        }

        private void PluginsChanged()
        {
            try
            {
                PluginManager.PluginInfo pi = PluginManager.instance.GetPluginsInfo().Where(p => p.publishedFileID.AsUInt64 == 412920038L).FirstOrDefault();
                if (pi != null)
                {
                    IsEnabled = pi.isEnabled;
                }
                else
                {
                    IsEnabled = false;
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "[SteeperSlopes] Can't find self. No idea if this mod is enabled.");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, "[SteeperSlopes] " + e.GetType() + ": " + e.Message);
            }
        }
    }


    public class Initialization : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
                return;


            int num = PrefabCollection<NetInfo>.PrefabCount();
            for (int index = 0; index < PrefabCollection<NetInfo>.LoadedCount(); ++index)
            {
                NetInfo ni = PrefabCollection<NetInfo>.GetLoaded((uint)index);

                if (ni == null)
                    continue;

                ni.m_maxSlope = ni.m_maxSlope > 0.5f ? ni.m_maxSlope : Math.Min(0.5f, ni.m_maxSlope * 2);
            }
        }
    }
}
