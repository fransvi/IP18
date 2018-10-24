﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<ActivationSequenceInfo> ActivationSequences;
    public List<GameObject> DynamicBlocks;
    public List<float> BlockTimeCounter;
    public List<int> BlockFrameCounter;

    public Text deathCounterText;

    public GameObject spawnPoint;
    public GameObject playerPrefab;
    public GameObject[] stages;
    public Camera camera;

    private bool playerAlive = false;
    private GameObject currentPlayer;

    private int timesDied = 0;

    private int currentStage;

    public void UpdateActivationSequences()
	{
		GameObject [] allDynamicBlocks = GameObject.FindGameObjectsWithTag("Dynamic Block");
		
		List<GameObject> activeDynamicBlocks = new List<GameObject>();

		foreach(GameObject g in allDynamicBlocks)
		{
			if(g.activeInHierarchy)
			{
		        ActivationSequenceInfo i = g.GetComponent<ActivationSequenceInfo>();
				ActivationSequences.Add(i);
                DynamicBlocks.Add(g);
                BlockTimeCounter.Add(0.0f);
                BlockFrameCounter.Add(0);
			}
		}
        Debug.Log("Updated activation sequences");
	}

    private void PollActivationSequences()
    {
        for(int i = 0; i < ActivationSequences.Count; ++i)
        {
            float deltaTime = Time.deltaTime;
            BlockTimeCounter[i] += deltaTime;
            if(ActivationSequences[i].SecondsPerFrame < BlockTimeCounter[i])
            {
                ++BlockFrameCounter[i];
                BlockTimeCounter[i] = 0.0f;
                bool [] sequence = ActivationSequences[i].Sequence;
                if(BlockFrameCounter[i] >= sequence.Length)
                {
                    BlockFrameCounter[i] =  0;
                }
                //DynamicBlocks[i].SetActive(false);
                DynamicBlocks[i].SetActive(sequence[BlockFrameCounter[i]]);
            }
        }
    }

    void Update()
    {
        PollActivationSequences();
        
    }

    public void StartGame()
    {
        UpdateActivationSequences();
        RespawnPlayer();
        currentStage = 0;
    }

    public void Start()
    {
        UpdateActivationSequences();
    }

    //Used when player dies to increase death count
    public void PlayerDead()
    {
        StartCoroutine(WaitForRespawn());
        timesDied += 1;
        deathCounterText.text = "Times died : " + timesDied;
        //currentPlayer.GetComponent<ParticleSystem>().Play();
    }


    //Transition and camera adjustment when player dies.
    IEnumerator WaitForRespawn()
    {
        playerAlive = false;
        GetComponent<AutoFade>().BeginFade(1);
        yield return new WaitForSeconds(0.5f);
        camera.GetComponent<SmoothFollow>().SetPlayer(spawnPoint);
        GetComponent<AutoFade>().BeginFade(-1);
        yield return new WaitForSeconds(0.5f);
        RespawnPlayer();
    }

    public void ReloadStage()
    {
        UpdateActivationSequences();
        RespawnPlayer();
    }



    // Advance the game to next level and respawn the player on the stage.
    public void NextStage()
    {

        StartCoroutine(StageChange());
        
    }

    IEnumerator StageChange()
    {
        currentStage += 1;
        Debug.Log("Stage change to " + currentStage);
        GetComponent<AutoFade>().BeginFade(1);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < stages.Length; ++i)
        {
            if (i == currentStage)
            {
                stages[i].SetActive(true);
            }
            else
            {
                stages[i].SetActive(false);
            }
        }
        UpdateActivationSequences();
        RespawnPlayer();
        GetComponent<AutoFade>().BeginFade(-1);
        yield return new WaitForSeconds(0.5f);
        
    }

    // Respawn the player and adjust the camera back to player and set camera limits for current stage
    public void RespawnPlayer()
    {
        if (!playerAlive)
        {
            spawnPoint = stages[currentStage].transform.GetChild(0).gameObject;
            GameObject player = (GameObject)Instantiate(playerPrefab, spawnPoint.gameObject.transform.position, spawnPoint.gameObject.transform.rotation);
            currentPlayer = player;
            camera.GetComponent<SmoothFollow>().SetCameraLimits(currentStage);
            camera.GetComponent<SmoothFollow>().SetPlayer(player.gameObject);
            playerAlive = true;
        }


    }
}
