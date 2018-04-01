using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAlerter : MonoBehaviour {

	public static SoundAlerter	instance { get; private set; }

	private void Awake() {
		if (instance == null)
			instance = this;
	}

	public delegate void SoundEvent(Vector2 soundPosition, float soundRange);
	public event SoundEvent OnSoundEvent;

	public void CreateSoundAt(Vector2 soundPosition, float soundRange) {
		OnSoundEvent(soundPosition, soundRange);
	}
}
