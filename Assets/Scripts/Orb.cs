using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int health = 20;
    public float timer = 20;
    public float chance = 50;

    public GameObject prefabVFX;

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
        if (prefabVFX != null)
        {
            GameObject endVFX = Instantiate(prefabVFX, this.transform.position, this.transform.rotation);
            Destroy(endVFX, 2);
        }
        Debug.Log("Damage trigger");
        Destroy(this.gameObject);
    }
}
