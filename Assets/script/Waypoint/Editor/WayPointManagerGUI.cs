using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WaypointSystem
{
    [CustomEditor(typeof(WaypointManager))]
    [CanEditMultipleObjects]
    public class WayPointGeneratorEditor : Editor
    {
        WaypointManager manager;

        private void OnEnable()
        {
            manager = (WaypointManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Generator ¹öÆ°
            try
            {
                if (GUILayout.Button("Generate"))
                {
                    manager.ExportToJson();
                }

                if (GUILayout.Button("Import Data"))
                {
                    manager.ImportFromJson();
                }
            }
            catch
            {

            }
        }
    }
}
