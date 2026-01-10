using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Assets.script.Talk_System;

[System.Serializable]
public struct CharacterSpriteData
{
    public string id;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "CharacterInfoSO", menuName = "Scriptable Objects/CharacterInfoSO")]
public class CharacterInfoSO : ScriptableObject
{
    public List<CharacterSpriteData> characterSpriteList;
    private Dictionary<string, CharacterSpriteData[]> characterEmotionDictionary = new();

    public Sprite GetCharacterSprite(string id)
    {
        Sprite result = null;
        foreach (var character in characterSpriteList)
        {
            if (character.id == id)
                result = character.sprite;
        }

        if (result == null)
            Debug.LogWarning($"[CharacterInfoSO] {id} sprite not exist");

        return result;
    }
}

[CustomEditor(typeof(CharacterInfoSO))]
public class CharawcterInfoSOEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}