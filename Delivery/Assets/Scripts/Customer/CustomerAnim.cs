using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CustomerAnim : MonoBehaviour
{
    [Header("== LIST OF ANIMATIONS")]
    public string IDLE;
    public string WRONGITEM;
    public string RIGHTPACKAGE;
    string currentAnim;
    public Animator anim => GetComponent<Animator>();
    
    public void SwitchAnim(string animName)
    {
        //if (currentAnim == animName) return;
        currentAnim = animName;
        anim.Play(animName);
    }
}
