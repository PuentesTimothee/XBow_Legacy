using ScoringSystem;
using UnityEngine;
using Valve.VR.InteractionSystem;

public enum SceneType
{
    Menu,
    Game
}

public class SceneManager : MonoBehaviour
{
    public SceneType SceneType = SceneType.Game;
    public Transform StartPlayerPosition;
    public GameObject PlayerPrefab;

    public int LevelIndex = 1;
    private Player _steamVrPlayer;
    
    private void Awake()
    {
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
            ScoreController.StartForLevel(LevelIndex);
        _steamVrPlayer.transform.position = StartPlayerPosition.position;
        _steamVrPlayer.transform.rotation = StartPlayerPosition.rotation;
    }

    public void ExitScene()
    {
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
        ExitScene();
        Application.Quit();
    }
}
