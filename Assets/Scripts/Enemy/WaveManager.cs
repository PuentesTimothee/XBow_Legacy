using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class WaveManager : MonoBehaviour
{
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject[] enemies;        // The enemy prefab to be spawned
        public int count;               // Number of enemies for this wave
        public float delay;             // Time between each spawning enemy
    }

    public Wave[] waves;
    public float timeBetweenWaves;      // Time between each waves
    public Transform[] spawnPoints;

    public Text panelText; // Change of Wave

    private SpawnState state = SpawnState.COUNTING;
    private int currentWave = 0;
    private float waveCountDown;

    private float searchCountdown = 1f;

    void Start()
    {
        waveCountDown = timeBetweenWaves;

    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            // check if ennemies are still alive;
            if(!EnemyIsAlive())
            {
                // Begin is new round;
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountDown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                // Start spawning wave
                if (currentWave < waves.Length)
                  StartCoroutine(SpawnWave(waves[currentWave]));
                else {
                  panelText.text = "Infinity Mode !";
                  GetComponent<EnemyManager>().enabled = true;
                  GetComponent<WaveManager>().enabled = false;
                }
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("wave : " + waves[currentWave].name + " completed");

        panelText.text = "Wave completed !";
        Debug.Log(panelText.text);
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;
        if (GameObject.FindGameObjectsWithTag("Music").Length != 0) {
          Debug.Log(GameObject.FindGameObjectsWithTag("Music")[0]);
          GameObject.FindGameObjectsWithTag("Music")[0].GetComponent<SoundPlayOneshot>().Play();
        }

        if (currentWave < waves.Length)
        {
            currentWave++;
            panelText.text = "Wave " + currentWave;
        }
        else
        {
            Debug.Log("ROUND COMPLETED !! Looping ...");
            // do something in here for the level to end !! for now it's just looping another round of waves
            currentWave = 0;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.count; i++)
        {
            Debug.Log("Spawning Enemy");
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            int randomEnnemyType = Random.Range(0, wave.enemies.Length);
            Instantiate(wave.enemies[randomEnnemyType], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            yield return new WaitForSeconds(wave.delay);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;                        // check if enemies are alive once every second
            if(GameObject.FindGameObjectsWithTag("enemy").Length == 0)
            {
                return false;
            }
        }
        return true;
    }
}
