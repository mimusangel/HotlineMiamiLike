using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ExitCar : MonoBehaviour {

	public string nextScene;
	public List<GameObject> objectsToDisable;
	public UnityEvent onExit;

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
			DeathScript.DestroyPlayer();
			GetComponent<Animator>().SetTrigger("Jump");
			Camera.main.GetComponent<CameraFollowScript>().follow = gameObject;
		}
	}

	public void DisableObjects() {
		objectsToDisable.ForEach(item => item.SetActive(false));
	}

	public void LoadNextScene() {
		onExit.Invoke();
	}


}
