using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonScript : MonoBehaviour {

	public GameObject[]				menus;
	int								iMenu = 0;

	public UnityEngine.UI.Slider	musicSlider;
	public UnityEngine.UI.Slider	soundsSlider;
	public AudioSource				audioSource;

	void Update()
	{
	}

	public void play()
	{
		SceneManager.LoadScene("Scenes/Level1");
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
}
