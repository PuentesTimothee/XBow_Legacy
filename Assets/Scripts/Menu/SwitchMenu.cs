using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwitchMenu : MonoBehaviour
{

    [Tooltip("Menu that will appear at the animation's end.")]
    public GameObject menu;
    [SerializeField] private Animator anim_;
    private bool triggered = false;

    private int animHash = Animator.StringToHash("displaceLogo");
    private int bopHash = Animator.StringToHash("hoverLogo");
    
    // Start is called before the first frame update
    void Awake()
    {
        anim_ = GetComponent<Animator>();
    }

    public void TriggerAnimation()
    {
        anim_.ResetTrigger(bopHash);
        triggered = true;
        anim_.SetTrigger(animHash);
        menu.SetActive(true);
    }

    public void OnMouseEnter()
    {
        if (!triggered)
            anim_.SetTrigger(bopHash);
    }

    public void OnMouseExit()
    {
        anim_.ResetTrigger(bopHash);
    }
}
