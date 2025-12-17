

using BlackboardSystem;
using ServiceLocator;
using UnityEngine;

public class Scout : MonoBehaviour, IExpert
{
    Blackboard blackboard;
    BlackboardKey isSafeKey;

    [SerializeField] bool dangerSensor;

    private void Start()
    {
        blackboard = Locator.Get<BlackboardController>().GetBlackboard();
        Locator.Get<BlackboardController>().RegisterExpert(this);
    }

    public int GetInsistence(Blackboard blackboard)
    {
        return dangerSensor ? 50 : 0;
    }

    public void Execute(Blackboard blackboard)
    {
        blackboard.AddAction(() =>
        {
            if (blackboard.TrygetValue(isSafeKey, out bool isSafe))
            {
                blackboard.SetValue(isSafeKey, !isSafe);
            }
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (blackboard.TrygetValue(isSafeKey, out bool isSafe))
            {
                blackboard.SetValue(isSafeKey, !isSafe);
                Debug.Log($"IsSafe: {isSafe}");
            }
        }
    }
}