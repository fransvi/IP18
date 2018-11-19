using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {


    private GameObject gameController;
    public Sprite[] deathPieceSprites;
    public GameObject deathPiece;
    public GameObject prefabSpawnPoint;


    private void Start()
    {
        gameController = GameObject.Find("GameController");
    }
    

    private void GenerateDeathPrefabs()
    {
        for(int i = 0; i < deathPieceSprites.Length; i++)
        {
            GameObject piece = (GameObject)Instantiate(deathPiece, prefabSpawnPoint.gameObject.transform.position, prefabSpawnPoint.gameObject.transform.rotation);
            piece.gameObject.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            piece.GetComponent<SpriteRenderer>().sprite = deathPieceSprites[i];
            piece.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(10f, -10f), Random.Range(-25f, -50f));
            Destroy(piece, 10f);
        }
    }

    //Handle all the collisions player does, with spikes and goal for respawning or level change.
    private void OnTriggerEnter2D(Collider2D col)
    {

            if (col.gameObject.layer == LayerMask.NameToLayer("Spike"))
            {
                
                GameObject[] clones = GameObject.FindGameObjectsWithTag("Player");
                GenerateDeathPrefabs();

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

            if (col.gameObject.layer == LayerMask.NameToLayer("Trigger"))
            {
                Debug.Log("Entering Trigger");
                if (col.transform.parent != null)
                {
                    col.gameObject.GetComponentInParent<PlatformController>().TriggerBlockMovement();
                }

            }

    }


}
