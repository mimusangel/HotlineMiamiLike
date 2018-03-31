using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour {

	public GameObject weaponSlot;
	public float speed = 5.0f;

	Rigidbody2D rb;
	Animator	anim;

	WeaponScript weaponInventory = null;
	public GameObject	bullet;
	float fireWait = 0.0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		weaponSlot.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 dir = Vector2.zero;
		if (Input.GetKey (KeyCode.W))
			dir += Vector2.up;
		if (Input.GetKey (KeyCode.S))
			dir += Vector2.down;
		if (Input.GetKey (KeyCode.A))
			dir += Vector2.left;
		if (Input.GetKey (KeyCode.D))
			dir += Vector2.right;
		if (dir != Vector2.zero) {
			dir.Normalize ();
			rb.velocity = dir * speed;
			anim.SetBool ("run", true);
		} else {
			rb.velocity = Vector2.zero;
			anim.SetBool ("run", false);
		}

		Vector2 mouseInScreen = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 lookDir = new Vector2(mouseInScreen.x - transform.position.x, mouseInScreen.y - transform.position.y);
		if (lookDir != Vector2.zero) {
			float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
		}

		fireWait -= Time.deltaTime;
		if (fireWait < 0)
			fireWait = 0;
		if (Input.GetMouseButton (0) && weaponInventory) {
			if (fireWait <= 0 && weaponInventory.bulletNumber > 0) {
				fireWait = weaponInventory.timeToShot;
				Vector3 newPos = transform.position;
				Vector2 lookDirNorm = lookDir.normalized;
				newPos.x += lookDirNorm.x * 0.75f;
				newPos.y += lookDirNorm.y * 0.75f;
				GameObject newBullet;
				if (lookDir != Vector2.zero) {
					float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
					newBullet = GameObject.Instantiate (bullet, newPos, Quaternion.AngleAxis(angle, Vector3.forward));
				}
				else
					newBullet = GameObject.Instantiate (bullet, newPos, transform.rotation);
				newBullet.GetComponent<SpriteRenderer> ().sprite = weaponInventory.bulletSprite;
				BulletScript bs = newBullet.GetComponent<BulletScript> ();
				bs.dir = lookDir.normalized;
				bs.speed = weaponInventory.bulletSpeed;
				bs.origin = gameObject;
				weaponInventory.bulletNumber -= 1;
			}
		}
		if (Input.GetMouseButtonDown (1)) {
			dropWeapon ();
		}
	}

	public void pickWeapon(WeaponScript weapon) {
		if (weaponInventory)
			return;
		weaponInventory = weapon;
		weaponInventory.gameObject.SetActive (false);
		weaponSlot.SetActive (true);
		weaponSlot.GetComponent<SpriteRenderer> ().sprite = weaponInventory.weaponSlotSprite;
	}

	public void changeWeapon(WeaponScript weapon)
	{
		dropWeapon();
		pickWeapon(weapon);
	}

	public void dropWeapon() {
		if (!weaponInventory)
			return;
		weaponInventory.gameObject.SetActive (true);
		weaponInventory.gameObject.transform.position = transform.position;
		weaponInventory = null;
		weaponSlot.SetActive (false);
	}
}
