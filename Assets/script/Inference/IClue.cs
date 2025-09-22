using UnityEngine;

public interface IClue
{
    string Id { get; set; }
    string Name { get; set; }
    string[] Tags { get; set; }
    string[] Description { get; set; }
    int DescriptionLength { get; }
}
