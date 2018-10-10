using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<ActivationSequenceInfo> ActivationSequences;
    public List<GameObject> DynamicBlocks;
    public List<float> BlockTimeCounter;
    public List<int> BlockFrameCounter;

    public GameObject spawnPoint;
    public GameObject playerPrefab;
    public GameObject[] stages;
    public Camera camera;

    private bool playerAlive = false;
    private GameObject currentPlayer;

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
    public void PlayerDead()
    {
        playerAlive = false;
        //currentPlayer.GetComponent<ParticleSystem>().Play();
    }

    public void ReloadStage()
    {
        UpdateActivationSequences();
        RespawnPlayer();
    }

    // Set active the current stage player is in
    public void NextStage()
    {
        currentStage += 1;
        Debug.Log("Stage change to " + currentStage);
        for(int i = 0; i < stages.Length; ++i)
        {
            if(i == currentStage)
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
    }

    // Respawn the player and adjust the camera back to player
    public void RespawnPlayer()
    {
        if (!playerAlive)
        {
            spawnPoint = stages[currentStage].transform.GetChild(0).gameObject;
            GameObject player = (GameObject)Instantiate(playerPrefab, spawnPoint.gameObject.transform.position, spawnPoint.gameObject.transform.rotation);
            currentPlayer = player;
            camera.GetComponent<SmoothFollow>().SetPlayer(player.gameObject);
            camera.GetComponent<SmoothFollow>().SetCameraLimits(currentStage);
            playerAlive = true;
        }


    }
}
