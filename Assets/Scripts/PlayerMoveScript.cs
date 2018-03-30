using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour {

	public GameObject weaponSlot;
	public float speed = 5.0f;

	Rigidbody2D rb;
	Animator	anim;


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
	}
}
