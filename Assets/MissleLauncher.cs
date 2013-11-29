using UnityEngine;
using System.Collections;

public class MissleLauncher : MonoBehaviour {
	GameObject SeekMPrefab;

	void Start () {
		seekprefab = GameObject.Find("SeekRocket");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.D))
			Instantiate(SeekMPrefab, transform.position, transform.rotation);
	}
}
