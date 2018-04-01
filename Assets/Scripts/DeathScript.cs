using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathScript : MonoBehaviour {
	public int Hp = 1;
	public GameObject deathAudioSource;
	public AudioClip[] deathAudioList;
	public static GameObject	player;
	public UnityEvent onDead;
	
	private void Start() {
		if (player == null && gameObject.tag == "Player")
			player = gameObject;
	}

	public void Death()
	{
		Hp --;
		if (Hp > 0)
			return;
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
			DeathManager.instance.enemyNB--;
			DeathManager.instance.checkWin();
		}
		onDead.Invoke();
		Destroy(gameObject);
	}

	public static void DestroyPlayer()
	{
		Destroy(player);
	}
}
