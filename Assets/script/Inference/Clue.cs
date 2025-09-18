using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inference
{
    [Serializable]
    public class Clue
    {
        public string id;
        public string imagePath;
        public string name;
        public string[] tags;
        public string[] description;
        public int[] descriptionIndex => new int[description.Length];
        public int descriptionLength => description.Length;
        public string[] linkedClueId;

        Clue(string id, string name, string[] tags, string[] linkedClueId, int descriptionLength)
        {
            this.id = id;
            this.name = name;
            this.tags = tags;
            this.linkedClueId = linkedClueId;
            this.description = new string[descriptionLength];
        }

        public static Clue Instantiate(string id, string name, string[] tags, string[] linkedClueId, int descriptionLength) => new Clue(id, name, tags, linkedClueId, descriptionLength);
    }

    [Serializable]
    public class ClueDescription
    {
        public string description;
        public int index;

        public ClueDescription(string description, int index)
        {
            this.description = description;
            this.index = index;
        }
    }
}
