using SceneManagers;
using UnityEngine;
using Valve.VR;

namespace Menu
{
    [RequireComponent(typeof(Animator))]
    public class PauseMenu : MonoBehaviour
    {
        public SteamVR_Action_Boolean PauseAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Pause");
        public GameObject Pivot;
        private Animator _animator;
        private bool _gamePaused;

        // Start is called before the first frame update
        void Start()
        {
            _gamePaused = false;
        }

        private void Update()
        {
            if (PauseAction.stateDown)
            {
                if (_gamePaused)
                    UnPauseGame();
                else
                    PauseGame();
            }
        }
        
        private void PauseGame()
        {
            _gamePaused = true;
            Pivot.SetActive(true);
            Time.timeScale = 0.0f;
        }

        private void UnPauseGame()
        {
            _gamePaused = false;
            Pivot.SetActive(false);
            Time.timeScale = 1.0f;
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
            SceneManager.ActualScene.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
            SceneManager.ActualScene.QuitScene();
        }
    }
}
