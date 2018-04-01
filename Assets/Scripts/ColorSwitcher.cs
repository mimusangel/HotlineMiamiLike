using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitcher : MonoBehaviour {

	private Color targetColor;
	private float switchCooldown;

	// Use this for initialization
	void Start () {
		switchCooldown = 0.0f;
	}

	// Update is called once per frame
	void Update () {
		switchCooldown -= Time.deltaTime;
		if (switchCooldown <= 0.0f) {
			Color c = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
			targetColor = c;
			switchCooldown = 1.0f;
		}
		Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, targetColor, 0.01f);
	}
}
