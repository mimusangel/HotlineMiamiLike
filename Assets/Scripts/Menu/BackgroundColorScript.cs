using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorScript : MonoBehaviour {
	Camera			cam;
	float			duration = 10.0F;
	public Color[]	colors;
	int				index = 0;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float t = Mathf.PingPong(Time.time, duration) / duration;
		cam.backgroundColor = Color.Lerp(colors[index], colors[(index + 1) % colors.Length], t);
	}
}