using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int health = 20;
    public float timer = 20;
    public float chance = 50;

    private float timerCount;

    private void Awake()
    {
        timerCount = timer;
    }

    private void Update()
    {
        timerCount -= Time.deltaTime;

        if(timerCount < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Damage")
        {
            Debug.Log("projectile trigger");
            Destroy(this.gameObject);
        }

    }
}
