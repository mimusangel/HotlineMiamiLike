using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRandomScript : MonoBehaviour {

	public string[]				weaponsName;
	public Sprite[]				weaponSprites;
	public Sprite[]				weaponSlotSprites;
	public Sprite[]				bulletSprites;
	public WeaponScript.Type[]	weaponsType;
	public int[]				bulletNumbers;
	public float[]				timeToShots;
	public int[]				bulletUsedByShots;
	public float[]				bulletsSpeed;
	public float[]				bulletsLifeTime;
	public float[]				shotSoundRanges;
	public float[] 				weaponFrictions;
	public WeaponScript.BulletType[]	bulletTypes;

	public AudioClip[]			weaponShotSounds;

	void Start () {
		int id = Random.Range (0, weaponSprites.Length);
		WeaponScript ws = GetComponent<WeaponScript> ();
		ws.weaponName = weaponsName[id];
		ws.weaponSprite = weaponSprites [id];
		ws.weaponSlotSprite = weaponSlotSprites[id];
		ws.bulletSprite = bulletSprites[id];
		ws.weaponType = weaponsType[id];
		ws.bulletNumber = bulletNumbers[id];
		ws.timeToShot = timeToShots[id];
		ws.bulletUsedByShot = bulletUsedByShots[id];
		ws.bulletSpeed = bulletsSpeed [id];
		ws.bulletLifeTime = bulletsLifeTime[id];
		ws.weaponShotSound = weaponShotSounds[id];
		ws.shotSoundRange = shotSoundRanges[id];
		ws.weaponFriction = weaponFrictions[id];
		ws.bulletType = bulletTypes[id];
		GetComponent<SpriteRenderer> ().sprite = weaponSprites [id];
	}
}
