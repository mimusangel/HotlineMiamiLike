using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windowScript : MonoBehaviour {
	SpriteRenderer		spriteR;
	public Sprite		broken;

	// Use this for initialization
	void Start () {
		spriteR = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag != "Enemy" && coll.gameObject.tag != "Player")
		{
			Broken();
		}
	}

	public void Broken()
	{
		spriteR.sprite = broken;
		GetComponent<Collider2D>().enabled = false;
	}
}
