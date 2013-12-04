using UnityEngine;
using System.Collections;

public class MissleLauncher : MonoBehaviour {
	public GameObject SeekMPrefab;


	void Start () {
		//SeekMPrefab = GameObject.Find("SeekRocket"); want a general prefab
	}
	
	// Update is called once per frame

	void Update () {
		Vector3 newPosition = transform.localPosition;

		if(Input.GetKeyDown(KeyCode.D)){
			newPosition.x = transform.localPosition.x + 5.0f;
			Instantiate(SeekMPrefab, newPosition, transform.rotation);
			newPosition.x = transform.localPosition.x - 5.0f;
			Instantiate(SeekMPrefab, newPosition, transform.rotation);
		}
	}
}
