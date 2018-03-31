using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {

	public Texture2D	cursorIcon;
	public GameObject follow;
	
	float maxDistance = 2.0f;

	// Use this for initialization
	void Start () {
		if (cursorIcon)
			Cursor.SetCursor(cursorIcon, Vector2.zero, CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
		if (follow) {
			// Camera sur le joueur
			Vector3 lastPos = follow.transform.position;
			lastPos.z = transform.position.z;
			transform.position = lastPos;
			// Camera selon la distance de la souris.
			Vector3 mouseInScreen = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mouseInScreen.z = follow.transform.position.z;
			Vector3 distVec = mouseInScreen - follow.transform.position;
			float dist = distVec.magnitude / 2.0f;
			if (dist > maxDistance)
				dist = maxDistance;
			distVec.Normalize();
			// Set Pos
			Vector3 pos = follow.transform.position + distVec * dist;
			pos.z = transform.position.z;
			// Set Cam
			transform.position = pos;
		}
	}
}
