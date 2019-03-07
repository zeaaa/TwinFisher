using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFUtility{

    public static float GetLengthByName(Animator animator,string name)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(name))
            {
                return clip.length;
            }
        }
        return 0;
    }
}
