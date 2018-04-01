using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonScript : MonoBehaviour {

	public GameObject[]				menus;
	int								iMenu = 0;

	public UnityEngine.UI.Slider	musicSlider = null;
	public UnityEngine.UI.Slider	soundsSlider = null;
	public AudioSource				audioSource;

	void Start()
	{
		if (!PlayerPrefs.HasKey("musicVolume"))
			PlayerPrefs.SetFloat("musicVolume", 0.5f);
		if (!PlayerPrefs.HasKey("soundsVolume"))
			PlayerPrefs.SetFloat("soundsVolume", 0.5f);
		if (musicSlider)
			musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
		if (soundsSlider)
			soundsSlider.value = PlayerPrefs.GetFloat("soundsVolume");
	}
	public void play()
	{
		PlayerPrefs.DeleteKey("lastLoadedScene");
		SceneManager.LoadScene("Scenes/Level1");
	}

	public void loadLevel(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void exit()
	{
		Application.Quit();
	}

	public void toMenu(int i)
	{
		if (menus.Length > i)
		{
			menus[iMenu].SetActive(false);
			iMenu = i;
			menus[i].SetActive(true);
		}
	}

	public void musicVolume()
	{
		if (menus.Length > 0)
		{
			PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
			PlayerPrefs.Save();
			audioSource.volume = PlayerPrefs.GetFloat("musicVolume");
		}
	}

	public void soundsVolume()
	{
		PlayerPrefs.SetFloat("soundsVolume", soundsSlider.value);
		PlayerPrefs.Save();
	}

	public void backToMenu()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("Scenes/MainMenu");
	}

	public void restart()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
