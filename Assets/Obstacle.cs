using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour, IVehicle {

	public GameObject vehicleGameObject{get; set;}
	public float maxForce {get; set;}
	public float maxSpeed{get; set;}
	public float mass{get; set;}
	public float radius{get; set;}
	
	public Vector3 position{get; set;}
	public Vector3 velocity{get; set;}

	// Use this for initialization
	void Start () {
		vehicleGameObject = gameObject;
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
