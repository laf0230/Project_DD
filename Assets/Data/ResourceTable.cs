using UnityEngine;

namespace ResourceManage
{
    [CreateAssetMenu(fileName = "ResourceTable", menuName = "Scriptable Objects/ResourceTable")]
    public class ResourceTable : ScriptableObject
    {
        public Resource[] resources;

        public Object this[string id]
        {
            get
            {
                for (int i = 0; i < resources.Length; i++)
                {
                    if (resources[i].id == id)
                    {
                        return resources[i].file;
                    }
                }
                return null;
            }
        }
    }

    [System.Serializable]
    public class Resource
    {
        public string id;
        public Object file;
    }
}
