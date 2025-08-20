using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialPair
{
    public string name;
    public Material material;
}

[CreateAssetMenu(fileName = "Metrials", menuName = "Scriptable Objects/Metrials")]
public  class Materials : ScriptableObject
{
    public Material[] material;


    public Material GetMaterial(string name)
    {
        for (int i = 0; i < material.Length; i++)
        {
            if(material[i].name == name)
            {
                return material[i];
            }
        }
        Debug.LogError("Load Error: Not Correct material name or not exist \n Check the material collection");
        return null;
    }

    public bool TryGetMaterial(string name, out Material oMat)
    {
        for (int i = 0; i < material.Length; i++)
        {
            if(material[i].name == name)
            {
                oMat = material[i];
                return true;
            }
        }
        oMat = null;

        return false;
    }

}
