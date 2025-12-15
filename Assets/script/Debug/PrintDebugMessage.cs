using System;
using UnityEngine;

[Serializable]
public class PrintDebugMessage
{
    public void PrintDebug(string message)
    {
        Debug.Log(message);
    }    
}
