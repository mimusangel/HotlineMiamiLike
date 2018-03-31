using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

	public enum Type {Dist, Katana, Hadoken};
	public enum BulletType {Base, Laser, Explode};
	public string	weaponName;
	public Sprite	weaponSprite;
	public Sprite	weaponSlotSprite;
	public Sprite	bulletSprite;
	public Type		weaponType;
	public int		bulletNumber;
	public float	timeToShot = 0.25f;
	public int		bulletUsedByShot = 1;
	public float	bulletSpeed = 10.0f;
	public float	bulletLifeTime = 10.0f;
	public float	shotSoundRange = 5.0f;
	public float 	weaponFriction = 0.8f; 
	public BulletType bulletType = BulletType.Base;

	public AudioClip	weaponShotSound;

	GameObject player;
	
	Rigidbody2D	rb2d;

	float	rotate = 0.0f;
	float	rotateSpeed;

	private void Start() {
		rb2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		if (player != null) {
			PlayerMoveScript pms = player.GetComponent<PlayerMoveScript> ();
			if (pms)
			{
				if (Input.GetKeyDown (KeyCode.E) || Input.GetMouseButtonDown (0)) 
					pms.pickWeapon (this);
				else if (Input.GetKeyDown(KeyCode.C)) 
					pms.changeWeapon(this);
			}
		}

		if (rb2d.velocity != Vector2.zero)
		{
			rb2d.velocity *= weaponFriction;
			if (rotateSpeed != 0)
			{
				rotate += rotateSpeed;
				transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
			}
			if (rb2d.velocity.magnitude < 0.5f)
				rb2d.velocity = Vector2.zero;
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			player = other.gameObject;
		}
		if (other.gameObject.tag == "Enemy") {
			if (weaponType == Type.Katana) {
				// Tuer l'enemy
				other.gameObject.GetComponent<DeathScript>().Death();
				rb2d.velocity = Vector2.zero;
			}
			else
			{
				// Etourdir l'enemy
				// other.gameObject.GetComponent<EnemyMoveScript>().SetS
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (player == other.gameObject) {
			player = null;
		}
	}

	public void dropWeapon(Vector2 dir)
	{
		rb2d.velocity = dir * 20.0f;
		if (weaponType == Type.Katana)
		{
			rotateSpeed = 0.0f;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		else
			rotateSpeed = Random.Range(20.0f, 25.0f);
	}
}
