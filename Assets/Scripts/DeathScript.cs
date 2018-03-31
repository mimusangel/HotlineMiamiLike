using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour {
	public GameObject deathAudioSource;
	public AudioClip[] deathAudioList;
	static int	enemyNB = -1;
	static GameObject	player;
	private void Start() {
		if (enemyNB == -1)
			enemyNB = GameObject.FindGameObjectsWithTag("Enemy").Length;
		if (player == null && gameObject.tag == "Player")
			player = gameObject;
	}

	public void Death()
	{
		if (deathAudioSource)
		{
			GameObject go = GameObject.Instantiate(deathAudioSource, transform.position, Quaternion.identity);
			AudioSource audio = go.GetComponent<AudioSource>();
			audio.clip = deathAudioList[Random.Range(0, deathAudioList.Length)];
			audio.volume = PlayerPrefs.GetFloat("soundsVolume");
			audio.Play();
			Destroy(go, 1.0f);
		}
		if (gameObject.tag == "Player")
		{
			MenuManagerScript.mm.setGameOverMenu(true);
		}
		if (gameObject.tag == "Enemy")
		{
			enemyNB--;
			checkWin();
		}
		Destroy(gameObject);
	}

	void checkWin()
	{
		if (enemyNB > 0)
			return ;
		MenuManagerScript.mm.setMissionCompleteMenu(true);
		if (player)
			Destroy(player);
	}

	public static void DestroyPlayer()
	{
		Destroy(player);
	}
}
