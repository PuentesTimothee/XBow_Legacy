using ScoringSystem;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace SceneManagers
{
    public enum SceneType
    {
        Menu,
        Game
    }

    public class SceneManager : MonoBehaviour
    {
        public static SceneManager ActualScene;

        public SceneType SceneType = SceneType.Game;
        public Transform StartPlayerPosition;
        public GameObject PlayerPrefab;

        public int LevelIndex = 1;
        private Player _steamVrPlayer;

        private void Awake()
        {
            ActualScene = this;
        
            _steamVrPlayer = Player.Instance;
            if (_steamVrPlayer == null)
            {
                var go = Instantiate(PlayerPrefab);
                _steamVrPlayer = go.GetComponent<Player>();
            }       
        }
    
        private void Start()
        {
            _steamVrPlayer.WeaponsSlot.enabled = (SceneType == SceneType.Game);
            if (SceneType == SceneType.Game)
            {
                ScoreController.StartForLevel(LevelIndex);
                Player.Instance.HealthBar.SetupForGame();
            }
            else
                Player.Instance.HealthBar.Disable();

            _steamVrPlayer.transform.position = StartPlayerPosition.position;
            _steamVrPlayer.transform.rotation = StartPlayerPosition.rotation;
            Time.timeScale = 1f;
        }

        private void ExitScene()
        {
            Time.timeScale = 1f;
            if (_steamVrPlayer)
                _steamVrPlayer.WeaponsSlot.enabled = false;
            if (SceneType == SceneType.Game)
                ScoreController.ExitLevel();
        }

        public void LoadScene(string sceneName)
        {
            ExitScene();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void QuitScene()
        {
            if (ActualScene == this)
                ActualScene = null;
    
            ExitScene();
            Application.Quit();
        }

        public void Reload()
        {
            LoadScene(gameObject.scene.name);
        }
    }
}