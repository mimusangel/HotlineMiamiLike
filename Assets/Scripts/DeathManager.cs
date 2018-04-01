using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour {

	public static DeathManager instance { get; private set; }

	public int	enemyNB = -1;

	public AudioClip	winSound;	
	public AudioClip	loseSound;	

	private void Awake() {
		if (instance == null)
			instance = this;
	}

	void Start () {
		enemyNB = GameObject.FindGameObjectsWithTag("Enemy").Length;
	}

	public void checkWin()
	{
		if (!DeathScript.player)
			return;
		if (enemyNB > 0)
			return ;
		MenuManagerScript.mm.setMissionCompleteMenu(true);
		DeathScript.playSound(winSound);
		DeathScript.DestroyPlayer();
		enemyNB = -1;
	}

	public void lose()
	{
		DeathScript.playSound(loseSound);
		Time.timeScale = 0.1f;
	}
}
