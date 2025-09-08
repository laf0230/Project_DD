using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct CharacterSpriteData
{
    public string name;
    public List<Sprite> spriteList;

    public Sprite this[string emotion]
    {
        get
        {
            var sprite = spriteList.Find(s => s.name == emotion);
            if (sprite == null)
            {
                Debug.LogWarning($"[SpriteTable] 감정 '{emotion}'에 해당하는 스프라이트를 찾을 수 없습니다.");
                return null;
            }
            return sprite;
        }
    }
}

[CreateAssetMenu(fileName = "CharacterInfoSO", menuName = "Scriptable Objects/CharacterInfoSO")]
public class CharacterInfoSO : ScriptableObject
{
    public List<CharacterSpriteData> characterSpriteList;
    private Dictionary<string, CharacterSpriteData[]> characterEmotionDictionary = new();

    public List<Sprite> this[string name]
    {
        get => characterSpriteList.Find(c => c.name == name).spriteList;
    }
}