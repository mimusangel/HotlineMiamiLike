using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

	public enum Type {Dist, Katana, Hadoken};

	public Sprite	weaponSprite;
	public Sprite	weaponSlotSprite;
	public Sprite	bulletSprite;
	public Type		weaponType;
	public int		bulletNumber;
	public float	timeToShot = 0.25f;
	public int		bulletUsedByShot = 1;
	public float	bulletSpeed = 10.0f;
	public float	bulletLifeTime = 10.0f;

	GameObject player;
	
	// Update is called once per frame
	void Update () {
		if (player) {
			if (Input.GetKeyDown (KeyCode.E) || Input.GetMouseButtonDown (0)) {
				player.GetComponent<PlayerMoveScript> ().pickWeapon (this);
			}
			else if (Input.GetKeyDown(KeyCode.C)) {
				player.GetComponent<PlayerMoveScript> ().changeWeapon(this);
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			player = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (player == other.gameObject) {
			player = null;
		}
	}

}
