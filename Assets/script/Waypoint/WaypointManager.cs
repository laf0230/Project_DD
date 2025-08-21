using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WaypointSystem
{

    [System.Serializable]
    public class WaypointGroup
    {
        public string groupName;
        public Transform[] waypoints;
    }

    [System.Serializable]
    public class SerializableWaypointGroup
    {
        public string groupName;
        public Vector3[] waypointPositions;

        public SerializableWaypointGroup() { }

        public SerializableWaypointGroup(WaypointGroup group)
        {
            groupName = group.groupName;
            waypointPositions = new Vector3[group.waypoints.Length];
            for (int i = 0; i < group.waypoints.Length; i++)
            {
                waypointPositions[i] = group.waypoints[i].position;
            }
        }
    }

    [System.Serializable]
    public class SerializableWaypointManager
    {
        public List<SerializableWaypointGroup> groups = new List<SerializableWaypointGroup>();
    }

    public class WaypointManager : MonoBehaviour
    {
        public List<WaypointGroup> waypointGroups = new List<WaypointGroup>();
        public bool isLocalPath = false;
        public string filePath;

        public bool GetWaypoint(string id, out Transform[] waypoints)
        {
            foreach (var group in waypointGroups)
            {
                if(id == group.groupName)
                {
                    waypoints = group.waypoints;
                    return true;
                }
            }
            waypoints = null;
            return false;
        }
        // JSON Export
        public void ExportToJson()
        {
            SerializableWaypointManager serializableManager = new SerializableWaypointManager();
            foreach (var group in waypointGroups)
            {
                serializableManager.groups.Add(new SerializableWaypointGroup(group));
            }

            string json = JsonUtility.ToJson(serializableManager, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"Waypoints exported to {filePath}");
        }

        // JSON Import
        public void ImportFromJson()
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("File not found: " + filePath);
                return;
            }

            string json = File.ReadAllText(filePath);
            SerializableWaypointManager serializableManager = JsonUtility.FromJson<SerializableWaypointManager>(json);

            waypointGroups.Clear();

            foreach (var sGroup in serializableManager.groups)
            {
                WaypointGroup newGroup = new WaypointGroup();
                newGroup.groupName = sGroup.groupName;
                newGroup.waypoints = new Transform[sGroup.waypointPositions.Length];

                GameObject parent = new GameObject(sGroup.groupName);

                // 실제 씬에 존재하는 Transform에 매핑하거나, 필요에 따라 임시 오브젝트 생성
                for (int i = 0; i < sGroup.waypointPositions.Length; i++)
                {
                    GameObject temp = new GameObject($"{sGroup.groupName}_Waypoint_{i}");
                    temp.transform.SetParent(parent.transform);
                    temp.transform.position = sGroup.waypointPositions[i];
                    newGroup.waypoints[i] = temp.transform;
                }

                waypointGroups.Add(newGroup);
            }

            Debug.Log($"Waypoints imported from {filePath}");
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (waypointGroups == null) return;

            Gizmos.color = Color.green; // 선 색상 지정

            foreach (var group in waypointGroups)
            {
                if (group.waypoints == null || group.waypoints.Length < 2) continue;

                for (int i = 0; i < group.waypoints.Length - 1; i++)
                {
                    if (group.waypoints[i] != null && group.waypoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(group.waypoints[i].position, group.waypoints[i + 1].position);
                    }
                }
            }
        }
#endif
    }
}
