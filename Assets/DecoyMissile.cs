using UnityEngine;
using System.Collections;

public class DecoyMissile : MonoBehaviour, IVehicle
{


	public GameObject vehicleGameObject{ get; set; }

	public float maxForce { get; set; }

	public float maxSpeed { get; set; }

	public float mass { get; set; }

	public float radius { get; set; }

	public Vector3 position { get; set; }

	public Vector3 velocity { get; set; }

	public IVehicle target;
	IVehicle secondaryTarget;
	public GameObject effect;
	public float SEEK_SPEED;

	public int follow_count { get; set; }

	public int MAX_FOLLOW = 3;
	
	void Start ()
	{
		vehicleGameObject = gameObject;
		follow_count = 0;
		maxSpeed = SEEK_SPEED;
		mass = 1.0f;

		//hard coded
		GameObject other = GameObject.Find ("DecoyTarget");
		secondaryTarget = other.GetComponent<Obstacle> ();
	}
	
	void OnCollisionEnter (Collision other)
	{
		Destroy (gameObject);
		Instantiate (effect, position, Quaternion.identity);
	}


	public void addFollower()
	{
		follow_count++;
		if(follow_count >= MAX_FOLLOW)
			maxSpeed += .8f;
	}

	void OnTriggerEnter(Collider other){
		//This branching would not be necessary if we found a more generic of organizing rockets
		if(other.gameObject.tag == "seek"){
			SimpleSeeking seek = other.gameObject.GetComponent<SimpleSeeking>();
			if(seek.changedTargets)
				return;
			else {
				addFollower();
				seek.target = this;
				seek.changedTargets = true;
			}
		}

		else if(other.gameObject.tag == "flocket"){
			Flock flock = other.gameObject.GetComponent<Flock>();
			if(flock.changedTargets)
				return;
			else{
				addFollower();
				flock.target = this;
				flock.changedTargets = true;
			}
		}
	}

	void Update ()
	{
		Vector3 steering_force, acceleration;
		if (follow_count >= MAX_FOLLOW) {
			target = secondaryTarget;
		}
		steering_force = SteeringForces.seek (this, target.position);
		acceleration = steering_force / mass;
		velocity = Vector3.ClampMagnitude (velocity + acceleration, maxSpeed);
	
		transform.position = transform.position + velocity * Time.deltaTime;
		position = transform.position; // update for use in steering functions
		
		// update rotations
		transform.up = velocity.normalized;
		}



}