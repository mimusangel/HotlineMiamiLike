using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Stairs : MonoBehaviour {

	public string linkedScene;
	public Transform spawn;
	public Transform player;
	public UnityEvent onExit;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetString("lastLoadedScene") == linkedScene) {
			player.transform.position = spawn.position;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name);
			DeathScript.DestroyPlayer();
			onExit.Invoke();
		}
	}

	public void LoadRelatedLevel() {
		SceneManager.LoadScene(linkedScene);
	}
}
