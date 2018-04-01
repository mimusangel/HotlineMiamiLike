using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Tank))]
public class TankEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Tank myScript = (Tank)target;
        if(GUILayout.Button("Fire"))
        {
			myScript.Fire();
        }

		if(GUILayout.Button("Aim"))
        {
			myScript.Aim();
        }
    }
}
#endif

public class Tank : MonoBehaviour {

	public float turretSpeed;
	public float rotationSpeed;
	public float movementSpeed;
	public float fireRate = 1;
	private float lastFireDelay;
	public float bulletSpeed = 15;

	public Transform target;
	public Transform turret;
	public Transform canonTip;

	public GameObject rocketPrefab;
	public GameObject weaponSound;
	public Sprite rocketSprite;
	public AudioClip fireSound;
	private Rigidbody2D rb;

	public UnityEvent onFire;

	public PatrolPoint entryPoint;
	private List<Transform> wayPoints = new List<Transform>();

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		wayPoints.Add(entryPoint.transform);
		while (entryPoint.nextPoint.transform != wayPoints[0]) {
			entryPoint = entryPoint.nextPoint;
			wayPoints.Add(entryPoint.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (target)
		{
			MoveToClosestWayPoint();
			Aim();
			if (lastFireDelay > fireRate && AimAligned())
				Fire();
			lastFireDelay += Time.deltaTime;
		}
	}

	public void MoveToClosestWayPoint() {
		Transform closestWaypoint;
		closestWaypoint = wayPoints[0];
		foreach(Transform wayPoint in wayPoints) {
			if (Vector2.Distance(wayPoint.position, target.position) < Vector2.Distance(closestWaypoint.position, target.position)) {
				closestWaypoint = wayPoint;
			}
		}
		float angleRad = Mathf.Atan2(closestWaypoint.transform.position.y - transform.position.y, closestWaypoint.transform.position.x - transform.position.x);
		float angleDeg = (180 / Mathf.PI) * angleRad + 180;
		float angleDiff = transform.rotation.eulerAngles.z - angleDeg;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angleDeg), Time.time * rotationSpeed / Mathf.Abs(angleDiff));
		if (Vector2.Distance(closestWaypoint.position, transform.position) < 5)
			return;
		Vector2 dir = (closestWaypoint.position - transform.position).normalized;
		rb.velocity = dir * movementSpeed * Time.deltaTime;
	}

	public void Aim() {
		float angleRad = Mathf.Atan2(target.transform.position.y - turret.transform.position.y, target.transform.position.x - turret.transform.position.x);
		float angleDeg = (180 / Mathf.PI) * angleRad;
		turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, Quaternion.Euler(0, 0, angleDeg), Time.time * turretSpeed);
	}

	public bool AimAligned() {
		float angleCanonRad = Mathf.Atan2(canonTip.transform.position.y - turret.transform.position.y, canonTip.transform.position.x - turret.transform.position.x);
		float angleCanonDeg = (180 / Mathf.PI) * angleCanonRad;
		float anglePlayerRad = Mathf.Atan2(target.transform.position.y - turret.transform.position.y, target.transform.position.x - turret.transform.position.x);
		float anglePlayerDeg = (180 / Mathf.PI) * anglePlayerRad;
		if (Mathf.Abs(anglePlayerDeg - angleCanonDeg) < 5)
			return true;
		return false;
	}

	public void Fire() {
		float angleRad = Mathf.Atan2(canonTip.transform.position.y - turret.transform.position.y, canonTip.transform.position.x - turret.transform.position.x);
		float angleDeg = (180 / Mathf.PI) * angleRad;
		GameObject newBullet = GameObject.Instantiate(rocketPrefab, canonTip.position, Quaternion.AngleAxis(angleDeg, Vector3.forward));
		newBullet.GetComponent<SpriteRenderer> ().sprite = rocketSprite;
		BulletScript bs = newBullet.GetComponent<BulletScript> ();
		bs.dir = (canonTip.position - turret.position).normalized;
		bs.speed = bulletSpeed;
		bs.origin = gameObject;
		bs.setLifeTime(5);
		bs.type = WeaponScript.BulletType.Explode;
		GameObject newBulletSound = GameObject.Instantiate(weaponSound, transform.position, transform.rotation);
		AudioSource audio = newBulletSound.GetComponent<AudioSource>();
		audio.volume = PlayerPrefs.GetFloat("soundsVolume");
		audio.clip = fireSound;
		audio.Play();
		Destroy(newBulletSound, 1.0f);
		onFire.Invoke();
		lastFireDelay = 0;
		Debug.Log(GetComponent<DeathScript>().Hp);
	}
}
