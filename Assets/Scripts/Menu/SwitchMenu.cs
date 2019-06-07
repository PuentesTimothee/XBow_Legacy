using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwitchMenu : MonoBehaviour
{

    [Tooltip("Menu that will appear at the animation's end.")]
    public GameObject menu;

    public IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1f);
        menu.SetActive(true);
    }
}
