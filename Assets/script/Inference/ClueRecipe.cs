using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ClueRecipe
{
    public List<string> clueIds = new();

    ClueRecipe(List<string> clueIds)
    {
        this.clueIds = clueIds;
    }

    public static ClueRecipe Combine(List<string> ids) => new ClueRecipe(ids);
}
