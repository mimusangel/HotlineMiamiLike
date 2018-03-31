using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour {

	public PatrolPoint nextPoint;
	public float randomRadius;

	void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "arrow.png", true);
		if (nextPoint != null) {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, nextPoint.transform.position);
		}
	}
}
