using BehaviourTrees;
using UnityEngine;

public class SampleCharacter : MonoBehaviour
{
    /*
    BehaviourTree BehaviourTree;

    private void Awake()
    {
        BehaviourTree = new("BehaviourTree");
        BehaviourTree.AddChild(new Sequence("Sequence"));
    }
    */

    public float moveSpeed = 0.5f;
    public float moveTime = 5f;
    float currentTime = 0;

    public void MoveToUp()
    {
        while (currentTime < moveSpeed)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            currentTime += Time.deltaTime;
        }
        currentTime = 0;
    }
}