using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {

	public GameObject follow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (follow) {
			Vector3 pos = follow.transform.position;
			pos.z = transform.position.z;
			transform.position = pos;
		}
	}
}
