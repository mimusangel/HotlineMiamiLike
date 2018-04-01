using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Shaker))]
public class SaveUserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Shaker myScript = (Shaker)target;
        if(GUILayout.Button("Shake"))
        {
			myScript.Shake();
        }
    }
}
#endif

public class Shaker : MonoBehaviour {

	public AudioSource shakeSource;

	// Use this for initialization
	void Start () {
		
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.O))
			Shake();
	}

	public void Shake() {
		shakeSource.Play();
		StartCoroutine(ShakeRoutine());
	}

	IEnumerator ShakeRoutine() {
		List<Vector3> shakePositions = new List<Vector3>();
		List<float> shakeStrengths = new List<float>();
		for (int i = 0; i < 30; i++) {
			shakePositions.Add(transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));	
		}
		for (int i = 0; i < 5; i++) {
			shakeStrengths.Add(5.0f);
		}
		for (int i = 0; i < 20; i++) {
			shakeStrengths.Add(15.0f);
		}
		for (int i = 0; i < 5; i++) {
			shakeStrengths.Add(3.0f);
		}
		int j = 0;
		foreach(Vector3 shakePos in shakePositions) 
		{
			transform.position = Vector3.MoveTowards(transform.position, shakePos, shakeStrengths[j] * Time.deltaTime);
			j++;
			yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
		};
	}
}
