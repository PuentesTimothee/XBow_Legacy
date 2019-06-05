using SceneManagers;
using UnityEngine;
using Valve.VR;

namespace Menu
{
    [RequireComponent(typeof(Animator))]
    public class PauseMenu : MonoBehaviour
    {
        public SteamVR_Action_Boolean PauseAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Pause");
        public GameObject PivotPause;
        public GameObject PivotGameOver;
        public GameObject PivotWin;
        private Animator _animator;
        private bool _gamePaused;
        private bool _gameEnd;

        // Start is called before the first frame update
        void Start()
        {
            _gamePaused = false;
            _gameEnd = false;
        }

        private void Update()
        {
            if (PauseAction.stateDown)
            {
                if (!_gameEnd)
                {
                    if (_gamePaused)
                        UnPauseGame();
                    else
                        PauseGame();
                }
                             
            }
        }
        
        private void PauseGame()
        {
            _gamePaused = true;
            PivotPause.SetActive(true);
            Time.timeScale = 0.0f;
        }

        private void UnPauseGame()
        {
            _gamePaused = false;
            PivotPause.SetActive(false);
            Time.timeScale = 1.0f;
        }

        public void GameOver()
        {
            _gameEnd = true;
            Time.timeScale = 0.0f;
            PivotGameOver.SetActive(true);
        }

        public void Win()
        {
            _gameEnd = true;
            PivotWin.SetActive(true);
            Time.timeScale = 0.0f;
        }

        //Buttons 

        public void Resume()
        {
            UnPauseGame();
        }

        public void Restart()
        {
            SceneManager.ActualScene.Reload();
        }
        
        public void MainMenu()
        {
            SceneManager.ActualScene.LoadScene("Menu");
        }

        public void QuitGame()
        {
            SceneManager.ActualScene.QuitScene();
        }

        public void MainMenuDeath()
        {
            SceneManager.ActualScene.LoadScene("MenuDeath");
        }

        public void NextLevel()
        {
            int level = SceneManager.ActualScene.LevelIndex + 1;
            string nextLevel = "Scene" + level.ToString();
            SceneManager.ActualScene.LoadScene(nextLevel);
        }
    }
}
