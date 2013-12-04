using UnityEngine;
using System.Collections;

public class FireEvader : MonoBehaviour {

	public GameObject Evader;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.D)){

			Instantiate(Evader, transform.position, transform.rotation);

		}

	}
}