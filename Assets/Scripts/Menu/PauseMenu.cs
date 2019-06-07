using SceneManagers;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(Animator))]
    public class PauseMenu : MonoBehaviour
    {
        public SteamVR_Action_Boolean PauseAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Pause");
        public GameObject PivotPause;
        public GameObject PivotGameOver;
        public GameObject PivotWin;
        public Text ScoreDisplayDeath;
        public Text ScoreDisplayWin;

        private Animator _animator;
        private bool _gamePaused;
        private bool _gameEnd;
        private bool _textSet;

        // Start is called before the first frame update
        void Start()
        {
            _gamePaused = false;
            _gameEnd = false;
            _textSet = false;
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
            if (_gameEnd)
            {
                if (!_textSet)
                {
                    ScoreDisplayDeath.text = "Score : " + ScoringSystem.ScoreController.Instance.GetScore().ToString();
                    ScoreDisplayWin.text = "Score : " + ScoringSystem.ScoreController.Instance.GetScore().ToString();
                    _textSet = true;
                }
            }
        }
        
        private void PauseGame()
        {
            _gamePaused = true;
            Player.Instance.WeaponsSlot.enabled = false;
            PivotPause.SetActive(true);
            Time.timeScale = 0.0f;
        }

        private void UnPauseGame()
        {
            _gamePaused = false;
            Player.Instance.WeaponsSlot.enabled = true;
            PivotPause.SetActive(false);
            Time.timeScale = 1.0f;
        }

        public void GameOver()
        {
            Debug.Log("GAME OVER");
            _gameEnd = true;
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
