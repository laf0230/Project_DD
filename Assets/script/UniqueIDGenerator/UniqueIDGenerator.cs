using System;
using UnityEngine;

public class UniqueIDGenerator
{
    public static string GenerateUniqueId(string name)
    {
        return Guid.NewGuid().ToString();
    }
}
