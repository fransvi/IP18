using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject spawnPoint;
    public GameObject playerPrefab;
    public GameObject[] stages;
    public Camera camera;

    private int currentStage;

    // Use this for initialization
    void Start () {
        RespawnPlayer();
        currentStage = 0;
	}

    public void ReloadStage()
    {
        RespawnPlayer();
    }

    // Set active the current stage player is in
    public void NextStage()
    {
        currentStage += 1;
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
        RespawnPlayer();
    }

    // Respawn the player and adjust the camera back to player
    public void RespawnPlayer()
    {
        Debug.Log("Current stage" + currentStage);
        spawnPoint = stages[currentStage].transform.GetChild(0).gameObject;
        GameObject player = (GameObject)Instantiate(playerPrefab, spawnPoint.gameObject.transform.position, spawnPoint.gameObject.transform.rotation);
        camera.GetComponent<SmoothFollow>().SetPlayer(player.gameObject);

    }
}
