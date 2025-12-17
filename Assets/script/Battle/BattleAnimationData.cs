using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController  : MonoBehaviour
{
    [SerializeField] List<BattleAnimationData> battleAnimations;
    Dictionary<BattleAnimationData.BattleAnimType, List<AnimationClip>> battleAnimPair = new();

    void ApplyAnimation()
    {
        foreach(var data in battleAnimations)
        {
            if(!battleAnimPair.TryGetValue(data.animType, out List<AnimationClip> clips))
            {
                clips = new List<AnimationClip>();
            }

            clips.Add(data.clip);
        }
    }

    static void ChangeAnimationClip(Animator anim)
    {
        

        //AnimatorOverrideController overrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        //var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        //foreach (var a in overrideController.animationClips)
        //{
        //    overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, clip));

        //    overrideController.ApplyOverrides(overrides);
        //    anim.runtimeAnimatorController = overrideController;
        //}
    }
}

[CreateAssetMenu(menuName = "new Aniamtion Data")]
public class BattleAnimationData : ScriptableObject
{
    [SerializeField] public AnimationClip clip;
    [SerializeField] public BattleAnimType animType;

    public enum BattleAnimType
    {
        Idle,
        Charge,
        StepDown,
        LongDistance,
        ShortDistance,
        HealthLess,
        Backward,
    }
}

public class AnimationTracker
{
    Animator anim;

    public void GetAnimationInfo()
    {
        //anim.GetCurrentAnimatorStateInfo(0)
    }
}