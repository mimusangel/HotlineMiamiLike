using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventMessageHandler : MonoBehaviour {

	public Text frontText;
	public Text backText;

	// Use this for initialization
	void Start () {
		
	}
	
	public void DisplayMessage(string message) {
		frontText.text = message;
		backText.text = message;
		gameObject.SetActive(true);
		StartCoroutine(DisplayMessageRoutine());
	}

	IEnumerator DisplayMessageRoutine() {
		yield return new WaitForSeconds(3);
		gameObject.SetActive(false);
	}
}
