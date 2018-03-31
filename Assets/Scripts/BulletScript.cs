using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {


	public Vector2 dir;
	public float speed = 10.0f;
	public GameObject origin = null;
	public WeaponScript.BulletType	type;
	public GameObject explode;

	void Start() {
		
	}

	void Update () {
		GetComponent<Rigidbody2D> ().velocity = dir * speed;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject == origin || origin == null)
			return;
		if (coll.gameObject.tag == "Enemy") {
			Destroy(coll.gameObject);
		}
		if (coll.gameObject.tag == "Player") {
			Destroy(coll.gameObject);
		}
		if (type == WeaponScript.BulletType.Explode)
		{

		}
		if (type != WeaponScript.BulletType.Laser)
			Destroy (gameObject);
		else if (coll.gameObject.tag != "Enemy" && coll.gameObject.tag != "Player")
			Destroy (gameObject);
	}

	public void setLifeTime(float time) {
		Destroy (gameObject , time);
	}
}
