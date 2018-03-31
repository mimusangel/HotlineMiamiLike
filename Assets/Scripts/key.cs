using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class key : MonoBehaviour {

	public UnityEvent onCollected;

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			onCollected.Invoke();
			Destroy(gameObject);
		}
	}
}
