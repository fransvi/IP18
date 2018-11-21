using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapdoor_script : MonoBehaviour {
    
    public GameObject sphere;

	void disablePlane () {
		transform.parent.gameObject.GetComponent<MeshCollider>().enabled = false;
        
	}
	void enablePlane () {
		transform.parent.gameObject.GetComponent<MeshCollider>().enabled = true;
	}
}
