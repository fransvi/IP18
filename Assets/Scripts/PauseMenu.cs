using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseMenu : MonoBehaviour {


    public static bool GamePaused = false;
    public static bool GameStarted = false;

    public GameObject pauseMenuUI;
    public GameObject startMenuUI;
    public GameObject gameController;
    public GameObject hudUI;
    public GameObject levelSelectUI;
    public GameObject gameOverUI;
    public Text gamerOverUIText;

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
    public void StartLevelSelected(int i)
    {
        Debug.Log("Starting stage" + i);
        gameController.GetComponent<GameController>().SetCurrentStage(i);
        levelSelectUI.SetActive(false);
        StartGame();

    }

    public void StartGame()
    {
        gameController.GetComponent<GameController>().StartGame();
        GameStarted = true;
        startMenuUI.SetActive(false);
    }

    public void LevelSelectMenuOn(bool b)
    {
        if(b)
        {
            startMenuUI.SetActive(false);
            levelSelectUI.SetActive(true);
        }
        else
        {
            startMenuUI.SetActive(true);
            levelSelectUI.SetActive(false);
        }

    }
    public void ToGameOverScreen()
    {
        gameController.GetComponent<GameController>().DisableAllStages();
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(true);
        gamerOverUIText.text = "You died "+gameController.GetComponent<GameController>().timesDied+" times and wasted "+ gameController.GetComponent<GameController>().timeFormatted+ " minutes of your life. Thank you for playing!";
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        levelSelectUI.SetActive(false);
        gameController.GetComponent<GameController>().DisableAllStages();
        startMenuUI.SetActive(true);
        Time.timeScale = 1f;
        GamePaused = false;
        GameStarted = false;
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
