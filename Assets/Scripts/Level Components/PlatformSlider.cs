using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSlider : MonoBehaviour {

	private Vector3 OriginalPosition;
	private float OriginalRotation;
	[SerializeField]
	private Vector3 DeltaPosition;
	[SerializeField]
	private float DeltaRotation;
	[SerializeField]
	private float Speed = 1.0f;
	private float Phase = 0.5f;
	private bool Direction = false;

	void Start () {
		OriginalPosition = transform.position;
		OriginalRotation = transform.rotation.eulerAngles.z;
	}
	
	void Update () {
		Vector3 dir;
		Quaternion rotation;

		transform.position = Vector3.Lerp(OriginalPosition, OriginalPosition + DeltaPosition, Phase);
		rotation = Quaternion.EulerAngles(0,0,OriginalRotation + (DeltaRotation * Phase));
		transform.rotation = rotation;

		if(Direction == true) Phase += Time.deltaTime * Speed;
		else Phase -= Time.deltaTime * Speed;

		if(Phase >= (1-Mathf.Epsilon) || Phase <= Mathf.Epsilon) {
			Direction = !Direction;
		}
	}
}
