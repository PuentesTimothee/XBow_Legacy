using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmoothMouseLook : MonoBehaviour
{
    public float sensitivityX = 2F;
    public float sensitivityY = 2F;

    float rotationX = 0F;
    float rotationY = 0F;

    Quaternion originalRotation;

    void Update()
    {
        rotationY += Input.GetAxis("MouseY") * sensitivityY;
        rotationX += Input.GetAxis("MouseX") * sensitivityX;

        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        originalRotation = transform.localRotation;
    }
}
