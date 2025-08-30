using ChainAction;
using UnityEngine;

public class NodeBasedController : MonoBehaviour
{
    [SerializeField] ChainAction.SequenceNode node;

    private void Update()
    {
        node?.Process(gameObject);
    }
}
