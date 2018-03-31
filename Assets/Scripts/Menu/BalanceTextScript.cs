using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceTextScript : MonoBehaviour {
	float		rotationSpeed = 0.02f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.rotation.z > 0.022f || transform.rotation.z < -0.022f)
			rotationSpeed *= -1;
		transform.Rotate(0, 0, rotationSpeed);
	}
}
