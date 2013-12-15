using UnityEngine;
using System.Collections;

public class SimpleSeeking : MonoBehaviour, IVehicle {
	
	
	public IVehicle target;
	public GameObject effect;

	public GameObject vehicleGameObject{get; set;}

	//Implement all getters and setters.
	public float maxForce {get; set;}
	public float maxSpeed{get; set;}
	public float mass{get; set;}
	public float radius{get; set;}
	
	public Vector3 position{get; set;}
	public Vector3 velocity{get; set;}

	public bool changedTargets = false;
	
	void Start() {
		vehicleGameObject = gameObject;
		maxSpeed = 15.5f;
		mass = 1.0f;
	}
	
	void OnCollisionEnter(Collision other){
		Destroy(gameObject);
		Instantiate(effect, position, Quaternion.identity);
	}


	void Update () {
		if(target.vehicleGameObject != null){
			//Access steering forces library and adjust it.
			Vector3 steeringForce = SteeringForces.seek(this, target.position);
			Vector3 acceleration = steeringForce/mass;
			velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);
		}
		//make this fps independent
		transform.position = transform.position + velocity * Time.deltaTime;
		position = transform.position; //update for use in steering functions
		
		//Update rotations
		transform.up = velocity.normalized;
		
	}
}


//http://www.red3d.com/cwr/steer/gdc99/