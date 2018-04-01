using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour {

	public float moveSpeed;
	public PatrolPoint startPatrolPoint;
	public float patrolWaitingTime;
	public bool patrolRandom;
	public float sightRadius;
	public float senseRadius;
	public float sightAngle;
	public SpriteRenderer alertedIcon;
	public SpriteRenderer headRenderer;
	public SpriteRenderer bodyRenderer;

	// private values

	private enum AlertState {
		idlePatrol,
		checkNoise,
		attack
	}
	private AlertState alertState;
	private bool stunned;
	private float stunnedTime;

	// idlePatrol case
	private int patrolIndex;
	private Vector2 targetPoint;
	private float lastMoveTime;
	private bool currentlyMoving;
	private List<Vector2> patrolPoints;
	private List<float> patrolRandomWanderingRadius;

	// checkingNoise case
	private Vector2 checkingNoisePosition;
	private List<Vector2> pathPositions; // always go to the first then remove it
	private float checkingWaitTime;

	// attack case
	private Vector2 lastPlayerSeenPosition;
	private float shootCooldown;

	// Component ShortCuts
	private GameObject playerChar;
	private Animator animatorComponent;
	private Rigidbody2D bodyComponent;

	// Weapon
	public GameObject weaponSlot;
	WeaponScript weaponInventory = null;
	public GameObject	bullet;
	public GameObject	weaponSound;
	public bool initWeapon = false;

	void OnDrawGizmos() {
		float lookDirection = (transform.eulerAngles.z - 90) * Mathf.Deg2Rad;
		Vector2 farPointLeft = transform.position;
		farPointLeft.x += Mathf.Cos(lookDirection - 0.5f * Mathf.Deg2Rad * sightAngle) * sightRadius;
		farPointLeft.y += Mathf.Sin(lookDirection - 0.5f * Mathf.Deg2Rad * sightAngle) * sightRadius;
		Vector2 farPointRight = transform.position;
		farPointRight.x += Mathf.Cos(lookDirection + 0.5f * Mathf.Deg2Rad * sightAngle) * sightRadius;
		farPointRight.y += Mathf.Sin(lookDirection + 0.5f * Mathf.Deg2Rad * sightAngle) * sightRadius;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, farPointLeft);
		Gizmos.DrawLine(transform.position, farPointRight);

		for (float i = -0.5f; i < 0.4f; i += 0.1f) {
			farPointLeft = transform.position;
			farPointLeft.x += Mathf.Cos(lookDirection + i * Mathf.Deg2Rad * sightAngle) * sightRadius;
			farPointLeft.y += Mathf.Sin(lookDirection + i * Mathf.Deg2Rad * sightAngle) * sightRadius;
			farPointRight = transform.position;
			farPointRight.x += Mathf.Cos(lookDirection + (i + 0.1f) * Mathf.Deg2Rad * sightAngle) * sightRadius;
			farPointRight.y += Mathf.Sin(lookDirection + (i + 0.1f) * Mathf.Deg2Rad * sightAngle) * sightRadius;
			Gizmos.DrawLine(farPointLeft, farPointRight);
		}

		Gizmos.DrawWireSphere(transform.position, senseRadius);
	}

	// Use this for initialization
	void Start () {
		// private values inits
		alertState = AlertState.idlePatrol;
		stunned = false;
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
		pathPositions = new List<Vector2>();
		checkingWaitTime = 0.0f;
		shootCooldown = 0.0f;

		// Component ShortCuts inits
		playerChar = GameObject.FindGameObjectWithTag("Player");
		animatorComponent = GetComponent<Animator>();
		bodyComponent = GetComponent<Rigidbody2D>();

		// Weapon
		weaponInventory = GetComponent<WeaponScript>();

		// Sprite Random
		headRenderer.sprite = EnemyRandomScript.instance.getRandomHead();
		bodyRenderer.sprite = EnemyRandomScript.instance.getRandomBody();

		SetState(alertState);
		SoundAlerter.instance.OnSoundEvent += NoiseListener;
	}

	private void OnDestroy() {
		SoundAlerter.instance.OnSoundEvent -= NoiseListener;
	}

	void Update () {
		if (!initWeapon)
		{
			if (weaponInventory.selectWeapon > -1)
				WeaponRandomScript.instance.getWeapon(weaponInventory, weaponInventory.selectWeapon);
			else
				WeaponRandomScript.instance.randomWeapon(weaponInventory);
			weaponSlot.GetComponent<SpriteRenderer> ().sprite = weaponInventory.weaponSlotSprite;
			initWeapon = true;
		}
		// if (Input.GetMouseButtonDown(1)) {
		// 	Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// 	NoiseListener(worldPos, 2.0f);
		// }
		alertedIcon.transform.position = transform.position + new Vector3(0, 1, 0);
		alertedIcon.transform.rotation = Quaternion.identity;
		if (stunned) {
			stunnedTime -= Time.deltaTime;
			if (stunnedTime <= 0.0f)
				SetStunned(false);
		} else {
			UpdateMove();
			if (playerChar)
			{
				if (alertState != AlertState.attack && CanSeePosition(playerChar.transform.position)) {
					SetState(AlertState.attack);
				}
			}
		}
	}

	void UpdateMove() {
		switch (alertState) {
			case AlertState.idlePatrol:
				UpdatePatrolMove();
				break;
			case AlertState.checkNoise:
				if (pathPositions.Count > 0)
					UpdatePatrolMove();
				else {
					checkingWaitTime -= Time.deltaTime;
					bodyComponent.velocity = Vector2.zero;
					bodyComponent.angularVelocity = 0.0f;
					if (checkingWaitTime <= 0.0f)
						SetState(AlertState.idlePatrol);
				}
				break;
			case AlertState.attack:
				if (playerChar && CanSeePosition(playerChar.transform.position)) {
					UpdateAttackPlayer();
					UpdateMoveToward(lastPlayerSeenPosition);
				} else {
					checkingNoisePosition = lastPlayerSeenPosition;
					SetState(AlertState.checkNoise);
				}
				break;
		}
	}

	void UpdateAttackPlayer() {
		lastPlayerSeenPosition = playerChar.transform.position;
		Vector2 dir = lastPlayerSeenPosition - (Vector2)gameObject.transform.position;
		transform.eulerAngles = new Vector3(0, 0, 90 + Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
		shootCooldown -= Time.deltaTime;
		if (shootCooldown <= 0.0f) {
			if (weaponInventory.bulletUsedByShot > 1)
				StartCoroutine(weaponShotRate(dir.normalized));
			else
			{
				if (weaponInventory.bulletType == WeaponScript.BulletType.Fragment)
				{
					weaponShot(Quaternion.AngleAxis(15.0f, Vector3.forward) * dir.normalized);
					weaponShot(Quaternion.AngleAxis(-15.0f, Vector3.forward) * dir.normalized);
				}
				weaponShot(dir.normalized);
			}
			shootCooldown = weaponInventory.timeToShot;
		}
	}

	void UpdateMoveToward(Vector2 walkTargetPoint) {
		Vector2 dir = walkTargetPoint - (Vector2)gameObject.transform.position;
		transform.eulerAngles = new Vector3(0, 0, 90 + Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
		if (dir.magnitude < 0.2f) {
			currentlyMoving = false;
			lastMoveTime = Time.time;
		}
		dir.Normalize();
		bodyComponent.velocity = dir * moveSpeed;
		bodyComponent.angularVelocity = 0.0f;
		animatorComponent.SetBool("run", true);
	}

	void UpdatePatrolMove() {
		if (pathPositions.Count > 0) {
			UpdateMoveToward(pathPositions[0]);
			if (Vector2.Distance((Vector2)gameObject.transform.position, pathPositions[0]) < 0.5f)
				pathPositions.RemoveAt(0);
		} else if (currentlyMoving) {
			UpdateMoveToward(targetPoint);
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

	private void SetState(AlertState newState) {
		switch (newState) {
			case AlertState.idlePatrol:
				if (alertState != AlertState.idlePatrol) {
					// TODO: pathPositions = pathfinding to targetPoint
					pathPositions.Clear();
				}
				alertedIcon.transform.localScale = new Vector3(0, 0, 0);
				break;
			case AlertState.checkNoise:
				pathPositions.Clear();
				// TODO: pathPositions = pathfinding to checkingNoisePosition
				pathPositions.Add(checkingNoisePosition);
				alertedIcon.color = Color.yellow;
				BumpAlertedIcon();
				checkingWaitTime = 4.0f;
				break;
			case AlertState.attack:
				if (alertState != AlertState.attack) {
					alertedIcon.color = Color.red;
					BumpAlertedIcon();
				}
				break;
		}
		alertState = newState;
	}

	public bool CanSeePosition(Vector2 position) {
		Vector2 selfPos = transform.position;
		if (Vector2.Distance(selfPos, position) > sightRadius)
			return false;
		float lookDirection = (transform.eulerAngles.z - 90) * Mathf.Deg2Rad;
		Vector2 farPoint = selfPos;
		farPoint.x += Mathf.Cos(lookDirection);
		farPoint.y += Mathf.Sin(lookDirection);
		float angle = Vector2.Angle(selfPos - farPoint, selfPos - position);
		if (angle > sightAngle / 2) {
			if (Vector2.Distance(selfPos, position) > senseRadius)
				return false;
		}
		Vector2 direction = position - selfPos;
		RaycastHit2D[] hits = Physics2D.RaycastAll(selfPos, direction.normalized, direction.magnitude);
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider.gameObject.tag == "Map")
				return false;
		}
		return true;
	}

	public void NoiseListener(Vector2 noisePosition, float noiseRange) {
		if (alertState != AlertState.attack)
			if (Vector2.Distance(transform.position, noisePosition) <= noiseRange) {
				checkingNoisePosition = noisePosition;
				SetState(AlertState.checkNoise);
			}
	}

	public void SetStunned(bool s, float time = 0) {
		stunned = s;
		if (s) {
			bodyComponent.velocity = new Vector2(0, 0);
			bodyComponent.angularVelocity = 360 * 2;
			stunnedTime = time;
		} else {
			bodyComponent.velocity = new Vector2(0, 0);
			bodyComponent.angularVelocity = 0.0f;
		}
	}

	public void BumpAlertedIcon() {
		StartCoroutine(bumpAlertedIconRoutine());
	}

	private IEnumerator bumpAlertedIconRoutine() {
		for (float i = 0; i < 3.0f; i += 0.07f) {
			float s = -12.0f * i * i + 7.0f * i + 1;
			if (s < 1.0f)
				s = 1.0f;
			alertedIcon.transform.localScale = new Vector3(s, s, 1);
			yield return new WaitForSeconds(0.025f);
		}
		alertedIcon.transform.localScale = new Vector3(0, 0, 1);
	}

	IEnumerator weaponShotRate(Vector2 lookDir)
	{
		for (int i = 0; i < weaponInventory.bulletUsedByShot; i++)
		{
			if (weaponInventory)
				weaponShot(lookDir);
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void weaponShot(Vector2 lookDir)
	{
		Vector3 newPos = transform.position;
		Vector2 lookDirNorm = lookDir.normalized;
		newPos.x += lookDirNorm.x * 0.5f;
		newPos.y += lookDirNorm.y * 0.5f;
		GameObject newBullet;
		float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
		newBullet = GameObject.Instantiate (bullet, newPos, Quaternion.AngleAxis(angle, Vector3.forward));
		newBullet.GetComponent<SpriteRenderer> ().sprite = weaponInventory.bulletSprite;
		BulletScript bs = newBullet.GetComponent<BulletScript> ();
		bs.dir = lookDir.normalized;
		bs.speed = weaponInventory.bulletSpeed;
		bs.origin = gameObject;
		bs.setLifeTime(weaponInventory.bulletLifeTime);
		bs.type = weaponInventory.bulletType;
		// Sound
		SoundAlerter.instance.CreateSoundAt(transform.position, weaponInventory.shotSoundRange);
		GameObject newBulletSound = GameObject.Instantiate(weaponSound, transform.position, transform.rotation);
		AudioSource audio = newBulletSound.GetComponent<AudioSource>();
		audio.clip = weaponInventory.weaponShotSound;
		audio.volume = PlayerPrefs.GetFloat("soundsVolume");
		audio.Play();
		Destroy(newBulletSound, 1.0f);
	}

}
