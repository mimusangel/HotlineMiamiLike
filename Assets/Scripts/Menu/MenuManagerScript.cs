using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagerScript : MonoBehaviour {

	public static MenuManagerScript	mm;

	public GameObject				menu;
	public GameObject				gameOverMenu;
	public GameObject				missionCompleteMenu;

	private void Awake() {
		if (mm == null)
			mm = this;
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void setGameOverMenu(bool on)
	{
		menu.SetActive(on);
		gameOverMenu.SetActive(on);
	}

	public void setMissionCompleteMenu(bool on)
	{
		menu.SetActive(on);
		missionCompleteMenu.SetActive(on);
	}
}
