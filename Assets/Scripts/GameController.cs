using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<ActivationSequenceInfo> ActivationSequences;
    public List<GameObject> DynamicBlocks;
    public List<float> BlockTimeCounter;
    public List<int> BlockFrameCounter;

    public Text deathCounterText;
    public Text timerText;

    public float gameTime;

    public GameObject spawnPoint;
    public GameObject playerPrefab;
    public GameObject[] stages;
    public Camera camera;

    private bool playerAlive = false;
    private GameObject currentPlayer;
    private BoxCollider2D currentPlayerCollider;
    private Player currentPlayerScript;

    public int timesDied = 0;
    public string timeFormatted;

    public bool KnockBackActive = false;
    public float KnockBackSpeed = 35f;

    private int currentStage;

    public GameObject gameUI;

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
//        Debug.Log("Updated activation sequences");
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
                Vector2 currentPlayerPosition = new Vector2();
                Vector2 blockPosition = new Vector2();
                if(!currentPlayer) return;
                currentPlayerPosition.x = currentPlayer.transform.position.x;
                currentPlayerPosition.y = currentPlayer.transform.position.y;
                float maxX = 0, maxY = 0, minX = 0, minY = 0;
                bool y_ok = false, x_ok = false;
                blockPosition.x = DynamicBlocks[i].transform.position.x;
                blockPosition.y = DynamicBlocks[i].transform.position.y;
                PolygonCollider2D blockCollider = DynamicBlocks[i].GetComponent<PolygonCollider2D>(); 
                if(blockCollider.points.Length >= 3)
                {
                    foreach(Vector2 p in blockCollider.points)
                    {
                        if(p.x < minX) minX = p.x;
                        if(p.y < minY) minY = p.y;
                    }
                    foreach(Vector2 p in blockCollider.points)
                    {
                        if(p.x > maxX) maxX = p.x;
                        if(p.y > maxY) maxY = p.y;
                    }
                    if(currentPlayerPosition.x >= blockPosition.x + minX || currentPlayerPosition.x + currentPlayerCollider.size.x >= blockPosition.x + minX)
                        if(currentPlayerPosition.x < blockPosition.x + maxX || currentPlayerPosition.x + currentPlayerCollider.size.x < blockPosition.x + maxX)
                            x_ok = true;
                    if(x_ok) 
                        if(currentPlayerPosition.y >= blockPosition.y + minY || currentPlayerPosition.y + currentPlayerCollider.size.y >= blockPosition.y + minY)
                            if(currentPlayerPosition.y < blockPosition.y + maxY || currentPlayerPosition.y + currentPlayerCollider.size.y < blockPosition.y + maxY)
                                y_ok = true;    
                }
                if(sequence[BlockFrameCounter[i]] == true)
                {
                    if(x_ok && y_ok)
                    {
//                        Debug.Log("Pushing player back...");
                        Vector2 currentPlayerMiddlePoint = new Vector2(currentPlayerPosition.x + (currentPlayerCollider.size.x / 2f), currentPlayerPosition.y + (currentPlayerCollider.size.y / 2f));
                        Vector2 currentBlockMiddlePoint = new Vector2(blockPosition.x + (maxX / 2f), blockPosition.y + (maxY / 2f));
                        
                        Vector2 dir = currentPlayerMiddlePoint - currentBlockMiddlePoint;
                        
                        dir /= dir.magnitude;
  //                      Debug.Log("Direction " + dir + " dir mag " + dir.magnitude);
                        currentPlayerScript.KnockBackActive = true;
                        currentPlayer.transform.Translate(dir * KnockBackSpeed * Time.deltaTime);
                    }
                    else
                    {
                        currentPlayerScript.KnockBackActive = false;
                    }
                }
                DynamicBlocks[i].SetActive(sequence[BlockFrameCounter[i]]);
            }
        }
    }

    void Update()
    {
        PollActivationSequences();
        GameTimer();

    }

    public void DisableAllStages()
    {
        for(int i = 0; i < stages.Length; i++)
        {
            Destroy(currentPlayer);
            stages[i].SetActive(false);
        }
    }

 

    void GameTimer()
    {
        gameTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(gameTime / 60F);
        int seconds = Mathf.FloorToInt(gameTime - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        timeFormatted = niceTime;
        timerText.text = "Time: " + niceTime;
    }
    public void SetCurrentStage(int i)
    {
        currentStage = i;
    }

    public void StartGame()
    {
        Destroy(currentPlayer);
        playerAlive = false;
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
    }

    public void Start()
    {
        gameUI.SetActive(true);
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
        if (currentStage == stages.Length)
        {
            yield return new WaitForSeconds(0.5f);
            gameUI.GetComponent<PauseMenu>().ToGameOverScreen();
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
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

        
    }

    // Respawn the player and adjust the camera back to player and set camera limits for current stage
    public void RespawnPlayer()
    {
        if (!playerAlive)
        {
            spawnPoint = stages[currentStage].transform.GetChild(0).gameObject;
            GameObject player = (GameObject)Instantiate(playerPrefab, spawnPoint.gameObject.transform.position, spawnPoint.gameObject.transform.rotation);
            currentPlayer = player;
            currentPlayerScript = player.GetComponent<Player>();
            currentPlayerCollider = currentPlayer.GetComponent<BoxCollider2D>();
            camera.GetComponent<SmoothFollow>().SetCameraLimits(currentStage);
            camera.GetComponent<SmoothFollow>().SetPlayer(player.gameObject);
            playerAlive = true;
        }


    }
}
