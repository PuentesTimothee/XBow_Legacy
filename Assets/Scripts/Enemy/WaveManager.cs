using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class WaveManager : MonoBehaviour
    {
        public enum WaveState
        {
            SPAWNING,
            WAITING,
            COUNTING
        };

        [Serializable]
        public class Wave
        {
            public string Name;
            public GameObject[] Enemies;        // The enemy prefab to be spawned
            public int Count;               // Number of enemies for this wave
            public float Delay;             // Time between each spawning enemy

            [HideInInspector] public WaveState State;
        }

        public Wave[] waves;
        public float timeBetweenWaves;      // Time between each waves
        public Transform[] spawnPoints;

        public bool ifinityMode = false;

        public Text panelText; // Change of Wave

        private WaveState state = WaveState.COUNTING;
        private int currentWave = 0;
        private float waveCountDown;

        private float searchCountdown = 1f;
        private bool _dead;

        public event Action<int> OnNewWave;

        private Menu.PauseMenu _winMenu;
        
        void Start()
        {
            waveCountDown = timeBetweenWaves;
            _winMenu = GameObject.Find("Canvas").GetComponent<Menu.PauseMenu>();
            _dead = false;
        }

        private void Update()
        {
            if (_dead)
                return;

            if (state == WaveState.WAITING)
            {
                // check if ennemies are still alive;
                if(!EnemyIsAlive())
                    // Begin is new round;
                    WaveCompleted();
                else
                    return;
            }

            if (waveCountDown <= 0)
            {
                if (state != WaveState.SPAWNING)
                {
                    // Start spawning wave
                    if (OnNewWave != null)
                        OnNewWave(currentWave);
                    if (currentWave < waves.Length)
                        StartCoroutine(SpawnWave(waves[currentWave]));
                    else {
                        if (ifinityMode)
                        {
                            panelText.text = "Infinity Mode !";
                            GetComponent<EnemyManager>().enabled = true;
                            GetComponent<WaveManager>().enabled = false;
                        } else
                        {
                            //Do the win menu
                            _winMenu.Win();
                        }
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
            Debug.Log("wave : " + waves[currentWave].Name + " completed");

            panelText.text = "Wave completed !";
            Debug.Log(panelText.text);
            state = WaveState.COUNTING;
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
            state = WaveState.SPAWNING;

            for (int i = 0; i < wave.Count; i++)
            {
                Debug.Log("Spawning Enemy");
                int spawnPointIndex = Random.Range(0, spawnPoints.Length);
                int randomEnnemyType = Random.Range(0, wave.Enemies.Length);
                Instantiate(wave.Enemies[randomEnnemyType], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
                yield return new WaitForSeconds(wave.Delay);
            }

            state = WaveState.WAITING;
            yield break;
        }

        bool EnemyIsAlive()
        {
            searchCountdown -= Time.deltaTime;

            if (searchCountdown <= 0f)
            {
                searchCountdown = 1f;                        // check if enemies are alive once every second
                if(GameObject.FindGameObjectsWithTag("enemy").Length == 0)
                    return false;
            }
            return true;
        }

        public void SetDead()
        {
            _dead = true;
        }
    }
}
