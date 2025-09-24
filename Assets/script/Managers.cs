using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    [SerializeField] private Object[] objects;

    private List<GameObject> managers;

    static Managers instance;
}
