using UnityEngine;
using System.Collections;

public class SimpleSeeking : MonoBehaviour, IVehicle {
	
	
	public IVehicle target;
	
	//Implement all getters and setters.
	public float maxForce {get; set;}
	public float maxSpeed{get; set;}
	public float mass{get; set;}
	public float radius{get; set;}
	
	public Vector3 position{get; set;}
	public Vector3 velocity{get; set;}
	
	
	void Start() {
		maxSpeed = 7.5f;
		mass = 1.0f;
		
		//Get specific cube object
		GameObject other = GameObject.Find("Cube");
		//Obtains component BackForth from Cube, which also implements IVehicle
		target = other.GetComponent<BackForth>();
	}
	
	
	void OnCollisionEnter(Collision other){
		Debug.Log ("Collided");	
	}
	
	void Update () {
		//Access steering forces library and adjust it.
		Vector3 steeringForce = SteeringForces.seek(this, target.position);
		Vector3 acceleration = steeringForce/mass;
		velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);
		
		//make this fps independent
		transform.position = transform.position + velocity * Time.deltaTime;
		position = transform.position; //update for use in steering functions
		
		//Update rotations
		transform.up = velocity.normalized;
	}
}

//http://www.red3d.com/cwr/steer/gdc99/