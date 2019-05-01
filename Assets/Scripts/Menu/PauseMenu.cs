using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;

    private bool gamePaused;

    // Start is called before the first frame update
    void Start()
    {
        gamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PauseGame()
    {
        gamePaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void UnpauseGame()
    {
        gamePaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    //Buttons 

    public void Resume()
    {
        UnpauseGame();
    }

    public void MainMenu()
    {

    }

    public void QuitGame()
    {
        
    }
}
