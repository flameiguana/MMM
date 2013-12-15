using UnityEngine;
using System.Collections;

public class CircleFlight : MonoBehaviour, IVehicle{
  	public GameObject vehicleGameObject{get; set;}
	public float maxForce {get; set;}
	public float maxSpeed{get; set;}
	public float mass{get; set;}
	public float radius{get; set;}
	
	public Vector3 position{get; set;}
	public Vector3 velocity{get; set;}

	
	public float smooth = .1f;


	// Use this for initialization
	void Start () {
		vehicleGameObject = this.gameObject;
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (Vector3.zero, Vector3.up, 20 * Time.deltaTime);
		position = transform.position;
	}
}
