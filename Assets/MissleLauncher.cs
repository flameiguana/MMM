using UnityEngine;
using System.Collections;

public class MissleLauncher : MonoBehaviour {
	public GameObject SeekMPrefab;


	void Start () {
		//SeekMPrefab = GameObject.Find("SeekRocket"); want a general prefab
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.D))
			Instantiate(SeekMPrefab, transform.position, transform.rotation);
	}
}
