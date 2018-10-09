using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour {

	public bool[] Sequence;
	public float SecondsPerFrame = 0.5f;
	public float AnimationDuration = 0.3f;
	private bool animationDirection;
	private float animationCounter;
	private float counter = 0.0f;
	private int cursor = 0;

	void Start () {
		
	}
	
	void Update () {
		
		if(counter > SecondsPerFrame)	
		{
			counter = 0;
			++cursor;
			if(cursor >= Sequence.Length) cursor = 0;
			gameObject.SetActive(Sequence[cursor]);

		}
		counter += Time.deltaTime;
	}
}
