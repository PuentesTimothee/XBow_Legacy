using UnityEngine;
using Valve.VR.InteractionSystem;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemies;                // The enemy prefab to be spawned.
    public float spawnTime = 3f;            // How long between each spawn.
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.

    // private HealthBar playerHealth;       // Reference to the player's heatlh.

    void Start()
    {
        // playerHealth = player.GetComponent<HealthBar>();

        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", spawnTime, spawnTime);

    }


    void Spawn()
    {
        // // If the player has no health left...
        // if (playerHealth.GetHealth() <= 0f)
        // {
        //     // ... exit the function.
        //     return;
        // }

        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemies.Length);
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(enemies[enemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

}
