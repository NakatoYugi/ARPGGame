using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager :MonoBehaviour
{
    public Animator anim;
    public int currentAnimHash = -1;
    public bool canInterruptCurrentAnim = false;
    private Dictionary<string, int> stringToPriority;

    public void RegisterAnim(string animName, int priority)
    {
        if (stringToPriority == null)
            stringToPriority = new Dictionary<string, int>();

        try
        {
            stringToPriority.Add(animName, priority);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        //if (stringToPriority.ContainsKey(targetAnim))
        //{
        //    Debug.LogWarning("There is not containskey:" + targetAnim + "in hashtable. Can't play this anim");
        //    return;
        //}

        //if (!canInterruptCurrentAnim) return;

        //Debug.Log("Before Updated AnimHash:" + currentAnimHash);
        //currentAnimHash = Animator.StringToHash(targetAnim);
        //Debug.Log("After Updated AnimHash:" + currentAnimHash);

        anim.applyRootMotion = isInteracting;
        anim.SetBool("isInteracting", isInteracting);
        //使用标准化时间创建从当前状态到任何其他状态的淡入淡出效果。
        anim.CrossFade(targetAnim, 0.2f);
    }


    #region Anim Events
    public virtual void OnAnimEnter() { }
    
    public virtual void OnAnimExit() { }

    public void SetInterruptCurrentAnimAsTrue()
    {
        this.canInterruptCurrentAnim = true;
    }
    #endregion
}