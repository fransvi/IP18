using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	public float ParallaxSmoothing = 1f;

	private GameObject[] allParallaxBackgrounds;
	private List<GameObject> currentParallaxBackgrounds;
	private List<float> currentParallaxBackgroundDepth;
	private ParallaxInfo parallaxInformation;
	private Transform camera;
	private Vector3 lastCameraPosition;
	
	public void UpdateParallaxBackgrounds()
	{
		allParallaxBackgrounds = GameObject.FindGameObjectsWithTag("Parallax");
		currentParallaxBackgroundDepth = new List<float>();
		currentParallaxBackgrounds = new List<GameObject>();

		foreach(GameObject g in allParallaxBackgrounds)
		{
			if(g.activeInHierarchy)
			{
				ParallaxInfo i = g.GetComponent<ParallaxInfo>();
				if(i == null)
				{
					currentParallaxBackgroundDepth.Add(1.0f);
				}
				else
				{
					currentParallaxBackgroundDepth.Add(i.Depth);
				}
				currentParallaxBackgrounds.Add(g);
			}
		}
	}

	void Awake() {
		camera = Camera.main.transform;
	}

	void Start() {
		lastCameraPosition = camera.position;
		UpdateParallaxBackgrounds();
	}
	
	void Update() {
		for(int i=0; i < currentParallaxBackgrounds.Count; ++i)
		{
			Vector3 direction = lastCameraPosition - camera.position;
			if(direction.magnitude>0) direction /= direction.magnitude;
			else direction = Vector3.zero;
			float distance = Vector3.Distance(lastCameraPosition, camera.position) * currentParallaxBackgroundDepth[i];
			Vector3 targetPosition = currentParallaxBackgrounds[i].transform.position + (direction * distance);
			//currentParallaxBackgrounds[i].transform.position = Vector3.Lerp(currentParallaxBackgrounds[i].transform.position, targetPosition, ParallaxSmoothing * Time.deltaTime);
			currentParallaxBackgrounds[i].transform.position = targetPosition;
		}
		lastCameraPosition = camera.position;
	}
}
