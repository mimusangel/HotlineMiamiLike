using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallScript : MonoBehaviour {
	SpriteRenderer		spriteR;
	public Sprite		cracked;
	public Sprite		broken;
	bool				isCracked = false;

	// Use this for initialization
	void Start () {
		spriteR = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag != "Enemy" && coll.gameObject.tag != "Player" && coll.gameObject.tag != "Map")
		{
			getHit();
		}
	}

	public void getHit()
	{
		if (isCracked)
		{
			spriteR.sprite = broken;
			GetComponent<Collider2D>().enabled = false;
		}
		else
		{
			spriteR.sprite = cracked;
			isCracked = true;
		}
	}
}