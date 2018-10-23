using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {


    private GameObject gameController;


    private void Start()
    {
        gameController = GameObject.Find("GameController");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

            if (col.gameObject.layer == LayerMask.NameToLayer("Spike"))
            {
                
                gameController.GetComponent<GameController>().PlayerDead();
            GameObject[] clones = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject clone in clones)
            {
                Destroy(clone);
            }
            gameController.GetComponent<GameController>().PlayerDead();
            }


            if (col.gameObject.layer == LayerMask.NameToLayer("Goal"))
            {
            gameController.GetComponent<GameController>().PlayerDead();
            GameObject[] clones = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject clone in clones)
            {
                Destroy(clone);
            }
            gameController.GetComponent<GameController>().NextStage();
            }

    }


}
