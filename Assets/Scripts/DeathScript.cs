using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour {
	public GameObject deathAudioSource;
	public AudioClip[] deathAudioList;
	public void Death()
	{
		if (deathAudioSource)
		{
			GameObject go = GameObject.Instantiate(deathAudioSource, transform.position, Quaternion.identity);
			go.GetComponent<AudioSource>().clip = deathAudioList[Random.Range(0, deathAudioList.Length)];
			go.GetComponent<AudioSource>().Play();
		}
		if (gameObject.tag == "Player")
		{
			MenuManagerScript.mm.setGameOverMenu(true);
		}
		Destroy(gameObject);
	}
}
