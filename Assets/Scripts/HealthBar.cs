using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    static public HealthBar playerHealthBar;

    public SceneManager sceneManager;

    public float maxHealth;

    private float health;

    private void Start()
    {
        health = maxHealth;
		    playerHealthBar = this;
	}

    // Update is called once per frame
    void Update()
    {

        if (health < 1.0)
        {
          sceneManager.LoadScene("MenuDeath");
          Debug.Log("You're DEAD !! GAME OVER !!!");
          return;
        }

    }

    public float GetHealth()
    {
        return health;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("player takes " + damage + " damages");
        if (damage > health)
        {

            health = 0;
        }
        else
        {
            health -= damage;
        }
    }

    public void Heal(float healing)
    {
        if (healing + health > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += healing;
        }
    }
}
