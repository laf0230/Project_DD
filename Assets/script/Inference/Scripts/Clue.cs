using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inference
{
    [Serializable]
    public class Clue: IClue
    {
        [SerializeField] private string id;
        public string imagePath;
        [SerializeField] private string name;
        [SerializeField] private string[] tags;
        [SerializeField] private ClueDescription[] description;
        public int[] descriptionIndex => new int[description.Length];
        public string[] linkedClueId;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string[] Tags { get => tags; set => tags = value; }
        public ClueDescription[] Description { get => description; set => description = value; }
        public int DescriptionLength { get => description.Length; }

        public Clue() { }

        Clue(string id, string name, string[] tags, string[] linkedClueId, int descriptionLength)
        {
            this.id = id;
            this.name = name;
            this.tags = tags;
            this.linkedClueId = linkedClueId;
            this.description = new ClueDescription[descriptionLength];
        }

        public static Clue Instantiate(string id, string name, string[] tags, string[] linkedClueId, int descriptionLength) => new Clue(id, name, tags, linkedClueId, descriptionLength);
    }

    [Serializable]
    public class ClueDescription
    {
        public string description = "Nothing";
        public int index;

        public ClueDescription(string description, int index)
        {
            this.description = description;
            this.index = index;
        }
    }
}
