using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ClueRecipe : IClue
{
    [SerializeField] private string id;
    [SerializeField] private string name;
    [SerializeField] private string[] description;
    [SerializeField] private string[] tags;
    public List<string> clueIds = new();
    public string Id { get => id; set => id = value; }
    public string Name {get => name; set => name = value; }
    public string[] Description { get => description; set => description = value; }
    public string[] Tags { get => tags; set => tags = value; }
    public int DescriptionLength { get => description.Length; }

    ClueRecipe(string id, string name, string[] tags, List<string> clueIds, int descriptionLength)
    {
        this.id = id;
        this.name = name;
        this.tags = tags;
        this.clueIds = clueIds;
        this.description = new string[descriptionLength];
    }

    public static ClueRecipe CreateRecipe(string id, string name, string[] tags, List<string> ids, int descriptionLength) => new ClueRecipe(id, name, tags, ids, descriptionLength);
}
