using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {

	public Text	txtWeaponName;
	public Text txtWeaponMun;
	public PlayerMoveScript	player;
	
	// Update is called once per frame
	void Update () {
		WeaponScript weapon = player.getPlayerWeapon();
		if (weapon)
		{
			txtWeaponName.text = weapon.weaponName;
			if (weapon.weaponType == WeaponScript.Type.Dist)
				txtWeaponMun.text = weapon.bulletNumber.ToString();
			else
				txtWeaponMun.text = "-";
		}
		else
		{
			txtWeaponName.text = "No Weapon!";
			txtWeaponMun.text = "";
		}
	}
}
