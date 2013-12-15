using UnityEngine;
using System.Collections;

public class BackForth : MonoBehaviour, IVehicle{

	public GameObject vehicleGameObject{get; set;}
	public float maxForce {get; set;}
	public float maxSpeed{get; set;}
	public float mass{get; set;}
	public float radius{get; set;}
	
	public Vector3 position{get; set;}
	public Vector3 velocity{get; set;}
	Vector3 pointB;
	
	public float smooth = .1f;
	
	// Use this for initialization
	void Start () {
		vehicleGameObject = gameObject;
		pointB = transform.position;
		pointB.y += 10;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.M)) {
			if(transform.position.z == 1000)
				transform.position = Vector3.zero;
			else
				transform.position = new Vector3(0,0,1000);
		}
		//transform.position = Vector3.Lerp(transform.position, pointB, smooth * Time.deltaTime);
		//position = transform.position;
		transform.Rotate(new Vector3(0, 0.1f, 0));
	}
}
