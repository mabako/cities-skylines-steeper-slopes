using ColossalFramework.Plugins;
using ICities;
using System;
using System.Collections.Generic;

namespace SteeperSlopes
{
    public class ModInfo : IUserMod
    {
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
                return "Steeper Slopes & Higher Bridges";
            }
        }
    }


    // HUGE Thanks to sschoener for https://github.com/sschoener/cities-skylines-no-despawn
    public class Initialization : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
                return;
            var mapping = new Dictionary<Type, Type>
            {
                {typeof (RoadAI), typeof (CustomRoadAI)},
                {typeof (TrainTrackAI), typeof (CustomTrainTrackAI)},
                {typeof (PedestrianPathAI), typeof (PedestrianPathAIWrapper)}
            };

            int num = PrefabCollection<VehicleInfo>.PrefabCount();
            for (int index = 0; index < PrefabCollection<NetInfo>.LoadedCount(); ++index)
            {
                NetInfo ni = PrefabCollection<NetInfo>.GetLoaded((uint)index);
                
                ReplaceAI(ni, mapping);
            }
        }

        private void ReplaceAI(NetInfo ni, Dictionary<Type, Type> componentRemap)
        {
            var oldAI = ni.GetComponent<NetAI>();
            if (oldAI == null)
                return;

            var compType = oldAI.GetType();
            Type newCompType;
            if (!componentRemap.TryGetValue(compType, out newCompType))
                return;

            var fields = ExtractFields(oldAI);

            UnityEngine.Object.DestroyImmediate(oldAI);

            NetAI newAI = ni.gameObject.AddComponent(newCompType) as NetAI;
            SetFields(newAI, fields);

            ni.m_maxSlope = ni.m_maxSlope > 0.5f ? ni.m_maxSlope : Math.Min(0.5f, ni.m_maxSlope * 2);

            newAI.m_info = ni;
            ni.m_netAI = newAI;
        }

        private Dictionary<string, object> ExtractFields(object a)
        {
            var fields = a.GetType().GetFields();
            var dict = new Dictionary<string, object>(fields.Length);
            for (int i = 0; i < fields.Length; i++)
            {
                var af = fields[i];
                dict[af.Name] = af.GetValue(a);
            }
            return dict;
        }

        private void SetFields(object b, Dictionary<string, object> fieldValues)
        {
            var bType = b.GetType();
            foreach (var kvp in fieldValues)
            {
                var bf = bType.GetField(kvp.Key);
                if (bf == null)
                    continue;

                bf.SetValue(b, kvp.Value);
            }
        }
    }
}
