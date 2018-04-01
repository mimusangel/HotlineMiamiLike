using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomScript : MonoBehaviour {

	public Sprite[]	bodySprites;
	public Sprite[]	headSprites;

	public static EnemyRandomScript instance { get; private set; }

	private void Awake() {
		if (instance == null)
			instance = this;
	}

	public Sprite getRandomBody() {
		return (bodySprites[Random.Range(0, bodySprites.Length)]);
	}

	public Sprite getRandomHead() {
		return (headSprites[Random.Range(0, headSprites.Length)]);
	}
}
