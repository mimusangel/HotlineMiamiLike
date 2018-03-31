using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public Vector2 dir;
	public float speed = 10.0f;
	public GameObject origin = null;

	void Start() {
		
	}

	void Update () {
		GetComponent<Rigidbody2D> ().velocity = dir * speed;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject == origin || origin == null)
			return;
		if (coll.gameObject.tag == "Enemy") {

		}
		if (coll.gameObject.tag == "Player") {

		}
		Destroy (gameObject);
	}

	public void setLifeTime(float time) {
		Destroy (gameObject , time);
	}
}
