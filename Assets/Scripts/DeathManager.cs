using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour {

	public static DeathManager instance { get; private set; }

	public int	enemyNB = -1;
	

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
		DeathScript.DestroyPlayer();
		enemyNB = -1;
	}
}
