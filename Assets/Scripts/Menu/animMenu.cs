using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animMenu : MonoBehaviour
{
    private Animator anim;

    private int showHash = Animator.StringToHash("Open");

    public void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    public void OnEnable()
    {
        anim.SetTrigger(showHash);
    }
}
