using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour {

	public GameObject playerChar;
	public float moveSpeed;
	public PatrolPoint startPatrolPoint;
	public float patrolWaitingTime;
	public bool patrolRandom;

	// private values
	private int patrolIndex;
	private Vector2 targetPoint;
	private float lastMoveTime;
	private bool currentlyMoving;
	private List<Vector2> patrolPoints;
	private List<float> patrolRandomWanderingRadius;

	// Component ShortCuts
	private Animator animatorComponent;
	private Rigidbody2D bodyComponent;

	// Use this for initialization
	void Start () {

		// private values inits
		patrolIndex = 0;
		targetPoint = gameObject.transform.position;
		currentlyMoving = false;
		lastMoveTime = Time.time;
		patrolPoints = new List<Vector2>();
		patrolRandomWanderingRadius = new List<float>();
		if (startPatrolPoint) {
			PatrolPoint point = startPatrolPoint;
			do {
				patrolPoints.Add(point.transform.position);
				patrolRandomWanderingRadius.Add(point.randomRadius);
				point = point.nextPoint;
			} while (point != startPatrolPoint && point != null);
		}

		// Component ShortCuts inits
		animatorComponent = GetComponent<Animator>();
		bodyComponent = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {

		// continue patrol
		if (currentlyMoving) {
			Vector2 dir = targetPoint - (Vector2)gameObject.transform.position;
			transform.eulerAngles = new Vector3(0, 0, 90 + Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
			if (dir.magnitude < 0.2f) {
				currentlyMoving = false;
				lastMoveTime = Time.time;
			}
			dir.Normalize();
			bodyComponent.velocity = dir * moveSpeed;
			animatorComponent.SetBool("run", true);
		} else {
			bodyComponent.angularVelocity = 0;
			bodyComponent.velocity = Vector2.zero;
			animatorComponent.SetBool("run", false);
			if (Time.time - lastMoveTime > patrolWaitingTime) {
				if (patrolPoints.Count > 0) {
					currentlyMoving = true;
					targetPoint = patrolPoints[patrolIndex];
					if (patrolRandomWanderingRadius.Count > patrolIndex) {
						float alpha = Random.Range(0, Mathf.PI * 2);
						float radius = Random.Range(0, patrolRandomWanderingRadius[patrolIndex]);
						targetPoint.x += Mathf.Cos(alpha) * radius;
						targetPoint.y += Mathf.Sin(alpha) * radius;
					}
					if (patrolRandom)
						patrolIndex = Random.Range(0, patrolPoints.Count);
					else
						patrolIndex = (patrolIndex + 1) % patrolPoints.Count;
				}
			}
		}
	}

	void NoiseListener(Vector2 noisePosition, float noiseRange) {

	}

}
