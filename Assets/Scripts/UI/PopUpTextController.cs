using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextController : MonoBehaviour
{
    public PopUpText popUpTextPrefab;

    private static GameObject canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void CreatePopupText(string text, Transform location)
    {
        PopUpText instance = Instantiate(popUpTextPrefab);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = location.position;
        instance.SetText(text);
    }
}
