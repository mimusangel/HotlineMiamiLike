using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barril : MonoBehaviour {

	public float explosionRadius = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	void OnDrawGizmos() {
		Color color = new Color(Color.yellow.a, Color.yellow.g, Color.yellow.b, 0.3f);
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

	public void OnCollisionEnter2D(Collision2D coll) {
		BulletScript bullet = coll.collider.GetComponent<BulletScript>();
		if (bullet)
		{
			GetComponent<Animator>().SetTrigger("Explode");
			Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
			foreach (Collider2D collision in collisions) {
				if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy") {
					var hits = Physics2D.RaycastAll(transform.position, collision.transform.position - transform.position, explosionRadius);
					if (hits.Length < 2)
						continue;
					if (hits[1].collider.gameObject.tag == "Player" || hits[1].collider.gameObject.tag == "Enemy")
						collision.GetComponent<DeathScript>().Death();
				}
			}
		}
	}

	public void EndExplosion() {
		Destroy(gameObject);
	}
}
