using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		
	}
	
	public void Unlock() {
		rb.bodyType = RigidbodyType2D.Dynamic;
	}
}
