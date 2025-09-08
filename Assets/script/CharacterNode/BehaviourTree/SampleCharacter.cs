using BehaviourTrees;
using UnityEngine;

public class SampleCharacter : MonoBehaviour
{
    BehaviourTree BehaviourTree;

    private void Awake()
    {
        BehaviourTree = new("BehaviourTree");
        BehaviourTree.AddChild(new Sequence("Sequence"));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
