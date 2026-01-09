using ServiceLocator;
using System.Collections;
using UnityEditor.VFX.UI;
using UnityEngine;

public enum AnimationParameterType
{
    Trigger,
    Bool,
    Rational, // 유리수
}

public class BossAnimatorWrapper : MonoBehaviour
{
    Animator animator;
    public bool isActionFinished { get; private set; } = false;

    public bool isHiting = true;

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

    public void PlayAction(string keyName, float value)
    {
        animator.SetFloat(keyName, value);
    }

    public void PlayAction(string keyName, float value, bool hasExitTime = false)
    {
        isActionFinished = hasExitTime;
        animator.SetFloat(keyName, value);
    }

    public void HitStart() => isHiting = true;

    public void HitEnd() => isHiting = false;

    // Place End of Animation Clip
    public void OnAnimationComplate()
    {
        isActionFinished = true;
    }
}
