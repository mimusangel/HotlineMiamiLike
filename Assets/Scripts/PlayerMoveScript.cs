using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour {

	public GameObject weaponSlot;
	public GameObject weaponSound;
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
			if (fireWait <= 0) {
				fireWait = weaponInventory.timeToShot;
				if (weaponInventory.weaponType == WeaponScript.Type.Dist) {
					if (weaponInventory.bulletUsedByShot > 1)
							StartCoroutine(weaponShotRate(lookDir));
						else
							weaponShot(lookDir, 1);
				} else {
					weaponShot(lookDir, 0);
				}
			}
		}
		if (Input.GetMouseButtonDown (1)) {
			dropWeapon ();
		}
	}

	IEnumerator weaponShotRate(Vector2 lookDir)
	{
		for (int i = 0; i < weaponInventory.bulletUsedByShot; i++)
		{
			if (weaponInventory)
				weaponShot(lookDir, 1);
			yield return new WaitForSeconds(0.1f);
		}
	}
	private void weaponShot(Vector2 lookDir, int useMun)
	{
		if (weaponInventory.bulletNumber <= 0 && weaponInventory.weaponType == WeaponScript.Type.Dist)
		{
			GetComponent<AudioSource>().Play();
			return;
		}
		Vector3 newPos = transform.position;
		Vector2 lookDirNorm = lookDir.normalized;
		newPos.x += lookDirNorm.x * 0.5f;
		newPos.y += lookDirNorm.y * 0.5f;
		GameObject newBullet;
		float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
		newBullet = GameObject.Instantiate (bullet, newPos, Quaternion.AngleAxis(angle, Vector3.forward));
		newBullet.GetComponent<SpriteRenderer> ().sprite = weaponInventory.bulletSprite;
		BulletScript bs = newBullet.GetComponent<BulletScript> ();
		bs.dir = lookDir.normalized;
		bs.speed = weaponInventory.bulletSpeed;
		bs.origin = gameObject;
		bs.setLifeTime(weaponInventory.bulletLifeTime);
		bs.type = weaponInventory.bulletType;
		// Sound
		GameObject newBulletSound = GameObject.Instantiate(weaponSound, transform.position, transform.rotation);
		AudioSource audio = newBulletSound.GetComponent<AudioSource>();
		audio.clip = weaponInventory.weaponShotSound;
		audio.Play();
		Destroy(newBulletSound, 1.0f);
		// Remove Mun
		weaponInventory.bulletNumber -= useMun;
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
		Vector2 mouseInScreen = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 lookDir = new Vector2(mouseInScreen.x - transform.position.x, mouseInScreen.y - transform.position.y);
		weaponInventory.dropWeapon(lookDir.normalized);
		weaponInventory = null;
		weaponSlot.SetActive (false);
	}

	public WeaponScript getPlayerWeapon()
	{
		return (weaponInventory);
	}
}
