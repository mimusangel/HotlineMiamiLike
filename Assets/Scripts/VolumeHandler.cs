using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		foreach (AudioSource source in sources)
			source.volume = PlayerPrefs.GetFloat("musicVolume");
	}
}
