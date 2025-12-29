using ServiceLocator;
using System.Collections;
using UnityEngine;

public class BossAnimatorWrapper : MonoBehaviour
{
    Animator animator;
    public bool isActionFinished { get; private set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Locator.Subscribe(this);
    }

    public void PlayAction(string triggerName, int id)
    {
        isActionFinished = false;
        animator.SetInteger("ActionID", id);
        animator.SetTrigger(triggerName);
    }

    // Place End of Animation Clip
    public void OnAnimationComplate()
    {
        isActionFinished = true;
    }
}
