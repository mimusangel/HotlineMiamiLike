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

	// Use this for initialization
	void Start () {
		
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.O))
			Shake();
	}

	public void Shake() {
		StartCoroutine(ShakeRoutine());
	}

	IEnumerator ShakeRoutine() {
		Vector3 startPosition = transform.position;
		List<Vector3> shakePositions = new List<Vector3>() {
			transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0),
			transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0),
			transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0),
			transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0),
			transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0),
			transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0),
			transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0),
		};
		foreach(Vector3 shakePos in shakePositions) 
		{
			transform.position = Vector3.MoveTowards(startPosition, shakePos, 5.0f * Time.deltaTime);
			yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
		};
	}
}
