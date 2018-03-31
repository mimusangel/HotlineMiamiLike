using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonScript : MonoBehaviour {
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		
	}
	public void play()
	{
		// Destroy(audioSource);
		SceneManager.LoadScene("Scenes/Level1");
	}

	public void exit()
	{
		Application.Quit();
	}
	public void toMainMenu()
	{
		SceneManager.LoadScene("Scenes/MainMenu");
	}
	public void toOptionMenu()
	{
		SceneManager.LoadScene("Scenes/OptionMenu");
	}
}
