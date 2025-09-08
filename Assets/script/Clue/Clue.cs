using System;
using System.Collections.Generic;
using UnityEngine;

namespace Detectivej
{
    [Serializable]
    public class Tag
    {
        readonly string tag;

        public Tag(string tag)
        {
            this.tag = tag;
        }
    }

    [Serializable]
    public class Clue
    {
        readonly string id;
        public List<Tag> tags;
        public string name;
        public string description;

        public Clue(string id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }

        public void AddTag(string tag) => tags.Add(new Tag(tag));
    }

    public class ClueData : ScriptableObject
    {
        public List<Clue> clues;
    }

    public class DetectiveNote
    {
        Dictionary<string, Clue> clueDictionary = new();
        List<Clue> hasClues;

    }
}
