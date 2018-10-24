using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {


    public static bool GamePaused = false;
    public static bool GameStarted = false;

    public GameObject pauseMenuUI;
    public GameObject startMenuUI;
    public GameObject gameController;
    public GameObject hudUI;

	void Update () {

        if(Input.GetKeyDown(KeyCode.O))
        {
            StartGame();
        }

        if(Input.GetKeyDown(KeyCode.Escape) && GameStarted)
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if( GameStarted && !GamePaused)
        {
            hudUI.SetActive(true);
        }
        else
        {
            hudUI.SetActive(false);
        }
		
	}

    public void StartGame()
    {
        gameController.GetComponent<GameController>().StartGame();
        GameStarted = true;
        startMenuUI.SetActive(false);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void QuitGame()
    {
        //closing the game for demo not for final version
        Application.Quit();
    }
}
