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

	public static WeaponRandomScript	instance { get; private set; }

	private void Awake() {
		if (instance == null)
			instance = this;
	}

	public void randomWeapon(WeaponScript ws) {
		int id = Random.Range (0, weaponSprites.Length);
		getWeapon(ws, id);
	}

	public void getWeapon(WeaponScript ws, int id) {
		
		id = Mathf.Clamp(id, 0, weaponsName.Length - 1);
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
		ws.GetComponent<SpriteRenderer> ().sprite = weaponSprites [id];
	}
}
