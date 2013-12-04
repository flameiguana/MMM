using UnityEngine;
using System.Collections;

public class Evade : MonoBehaviour, IVehicle {
	
	public GameObject vehicleGameObject{get; set;}  
	public IVehicle target;
	public IVehicle movearound;

	public GameObject effect;

	public float maxForce {get; set;}
	public float maxSpeed{get; set;}
	public float mass{get; set;}
	public float radius{get; set;}

	public Vector3 position{get; set;}
	public Vector3 velocity{get; set;}
	public float avoiding = 0;
	
	void Start() {
		maxSpeed = 7.5f;
		mass = 1.0f;
		

		GameObject other = GameObject.Find("plane");
		GameObject other2 = GameObject.Find ("block");
		//Obtains component BackForth from Cube, which also implements IVehicle

		target = other.GetComponent<CircleFlight>();

	}
	
	
	void OnCollisionEnter(Collision other){

		if(other.gameObject.name != "plane2"){

		Destroy(gameObject);


		//Instantiate(effect, position, Quaternion.identity);

		Debug.Log ("Collided" + other.gameObject.tag);
		}
	}

	void OnTriggerEnter(Collider col){

		//Place gameobject's name inside string
		string objectname = col.gameObject.name;


		//Check to see if the string has the name we are looking for
		if(objectname == "block"){
		GameObject obstacle = GameObject.Find(objectname);

		movearound = obstacle.GetComponent<CircleFlight>();
		avoiding = 1;
		}
	}

	void OnTriggerExit(Collider col){




	}

	void Update () {

			

			Vector3 steeringForce = SteeringForces.seek(this, target.position);
			
		if(avoiding == 1){

			Vector3 offset = transform.position - movearound.position;
			Vector3 avoider = Vector3.Cross(offset, movearound.position);
			avoider = avoider.normalized;
			avoider *= maxSpeed * 1.4f;

			Vector3 acceleration = (steeringForce - avoider)/mass;
			
			velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);
			

			transform.position = transform.position + velocity * Time.deltaTime;
			position = transform.position; //update for use in steering functions
			

			transform.up = velocity.normalized;
			print("here");

		} else {
			
			
			Vector3 acceleration = steeringForce/mass;
			
			velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);
			
			//make this fps independent
			transform.position = transform.position + velocity * Time.deltaTime;
			position = transform.position; //update for use in steering functions
			
			//Update rotations
			transform.up = velocity.normalized;

		}

	}

	
}
