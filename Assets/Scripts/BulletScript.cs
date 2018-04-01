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
		if (origin == null)
			Destroy(gameObject);
		if (coll == null || coll.gameObject == null || coll.gameObject == origin)
			return;
		if (coll.gameObject.tag == "Enemy" && origin.tag == "Enemy")
			return ;
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponent<DeathScript>().Death();
		}
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.GetComponent<DeathScript>().Death();
		}
		if (type == WeaponScript.BulletType.Explode) {
			GameObject.Instantiate(explode, transform.position, transform.rotation);
		}
		if (type != WeaponScript.BulletType.Laser)
			Destroy (gameObject);
		else if (coll.gameObject.tag != "Enemy" && coll.gameObject.tag != "Player")
			Destroy (gameObject);
		else if (coll.gameObject.tag == "Map")
			Destroy (gameObject);
	}

	public void setLifeTime(float time) {
		Destroy (gameObject , time);
	}
}
