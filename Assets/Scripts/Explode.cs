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

	private void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Player")
		{
			if (testHit(other.gameObject))
			{
				other.gameObject.GetComponent<DeathScript>().Death();
			}
		}
		else if (other.gameObject.tag == "Enemy")
		{
			if (testHit(other.gameObject))
			{
				Debug.Log("kill " + other.gameObject.name);
				other.gameObject.GetComponent<DeathScript>().Death();
			}
		}
		else if (other.gameObject.tag == "Map")
		{
			windowScript window = other.gameObject.GetComponent<windowScript>();
			if (window)
			{
				window.Broken();
			}
		}
	}

	bool testHit(GameObject other)
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, (other.transform.position - transform.position).normalized, GetComponent<CircleCollider2D>().radius);
		if (hit.collider != null)
		{
			if (hit.collider.gameObject.tag == "Player" && hit.collider.gameObject.tag == "Enemy")
			{
				return (true);
			}
		}
		return (false);
	}
}
