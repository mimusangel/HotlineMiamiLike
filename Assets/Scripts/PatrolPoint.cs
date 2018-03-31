using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour {

	public PatrolPoint nextPoint;
	public float randomRadius;

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawIcon(transform.position, "arrow.png", true);
		Gizmos.DrawWireSphere(transform.position, randomRadius);
		if (nextPoint) {
			Gizmos.DrawLine(transform.position, nextPoint.transform.position);
		}
	}
}
