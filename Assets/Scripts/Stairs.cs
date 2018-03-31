using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairs : MonoBehaviour {

	public string linkedScene;

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			Debug.Log("goind to scene " + linkedScene);
			SceneManager.LoadScene(linkedScene);
		}
	}
}
