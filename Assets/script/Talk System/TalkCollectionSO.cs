using Assets.script.Talk_System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TalkCollectionSO", menuName = "Scriptable Objects/TalkCollectionSO")]
public class TalkCollectionSO : ScriptableObject
{
    [SerializeField] private List<TalkNode> talkList = new List<TalkNode>();
    public List<TalkNode> TalkNodes { get => talkList; set => talkList = value; }
}
