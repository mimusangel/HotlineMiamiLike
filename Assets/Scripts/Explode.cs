using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

	public Sprite[] anim;
	public float	animTime;

	float			time = 0.0f;
	float			animFrame;
	// Use this for initialization
	void Start () {
		animFrame = animTime / (float)(anim.Length + 1);
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		int frameAnim = Mathf.RoundToInt(time / animFrame);
		if (frameAnim >= anim.Length)
			frameAnim = anim.Length - 1;
		GetComponent<SpriteRenderer>().sprite = anim[frameAnim];
		if (time > animTime)
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player")
		{
			if (testHit(other.gameObject))
			{
				other.gameObject.GetComponent<DeathScript>().Death();
			}
		}
		else if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Tank")
		{
			Debug.Log("hit");
			if (testHit(other.gameObject))
			{
				Debug.Log("hit after");
				other.gameObject.GetComponent<DeathScript>().Death();
			}
		}
		else if (other.gameObject.tag == "Map")
		{
			windowScript window = other.gameObject.GetComponent<windowScript>();
			wallScript wall = other.gameObject.GetComponent<wallScript>();
			if (window)
				window.Broken();
			else if (wall)
				wall.getHit();
		}
	}

	bool testHit(GameObject other)
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, (other.transform.position - transform.position).normalized, GetComponent<CircleCollider2D>().radius + 3);
		Debug.Log("testHit");
		if (hits.Length > 1)
		if (hits[1].collider != null)
		{
			Debug.Log("got hits[1]");
			Debug.Log(hits[1].collider.gameObject.tag);
			Debug.Log(hits[1].collider.gameObject.name);
			if (hits[1].collider.gameObject.tag == "Player" || hits[1].collider.gameObject.tag == "Enemy" || hits[1].collider.gameObject.tag == "Tank")
			{
				return (true);
			}
		}
		return (false);
	}
}
