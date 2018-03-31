using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMotifScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.localScale += new Vector3(0.02f, 0.02f, 0);
		if (transform.localScale.x > 4f)
			transform.localScale = new Vector3(0.1f, 0.1f, 0);
	}
}
