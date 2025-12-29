using UnityEngine;

public class AnimatorWrapper : MonoBehaviour
{
    Animator animator;
    public AnimationClip currentClip { get; private set; }
    public bool isActionFinished { get; private set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Used from Animation clip
    public void SetCurrentAnimationClip(AnimationClip clip)
    {
        currentClip = clip;
        isActionFinished = false;
    }

    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
        isActionFinished = false;
    }

    // Place End of Animation Clip
    public void OnAnimationComplate()
    {
        isActionFinished = true;
    }
}
