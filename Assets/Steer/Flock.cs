using UnityEngine;
using UnityEditor;
using System.Collections;

public class Flock : MonoBehaviour, IVehicle {
	
	public IVehicle target;
	public GameObject effect;
	
	private static readonly float pursueRadius = 30;
	private static readonly float evadeRadius = 10;
	private static readonly float evadePower = .2f;
	private static readonly float pursuePower = .4f;
	private static readonly float acceleration = 10;
	private static readonly float maxPursueVelocity = 10;

	private Vector3 targetPos;
	private float initialDistFromTarget;
	private bool reachedTarget;
	private bool changedTargets;


 	public GameObject vehicleGameObject{get; set;}  
	//Implement all getters and setters.
	public float maxForce {get; set;}
	public float maxSpeed{get; set;}
	public float mass{get; set;}
	public float radius{get; set;}

	public Vector3 position{get{return this.rigidbody.position;} set{this.rigidbody.position = value;}}
	public Vector3 velocity{get; set;}


	void Start () {
		this.rigidbody.freezeRotation = true;
	}

	void OnTriggerEnter(Collider other){
		if(changedTargets) return;
		if(other.gameObject.tag == "Decoy"){
			//Debug.Log ("Hi");
			DecoyMissile decoy = other.gameObject.GetComponent<DecoyMissile>();
			decoy.follow_count += 1;
			target = decoy;
		}
	}

	void OnCollisionEnter(Collision other){
		Destroy(gameObject);
		Instantiate(effect, this.rigidbody.position, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
		if(target.vehicleGameObject == null)
			return;
		//set the targetPos if it hasn't been set yet
		if(targetPos == Vector3.zero){
			targetPos = target.position;
			initialDistFromTarget = (targetPos - this.position).magnitude;
		}

		//see if we can explode it when it gets too far away
		if((targetPos - this.position).magnitude < initialDistFromTarget*.25)
			reachedTarget = true;

		//check if its overshot it's target after getting close
		if((targetPos - this.position).magnitude > initialDistFromTarget*.4 && reachedTarget){
			Destroy(gameObject);
			Instantiate(effect, this.rigidbody.position, Quaternion.identity);
			Debug.Log("overshot");
		}

		//stay near its friends
		Vector3 vel = Vector3.zero;
		Collider[] pursueColliders = Physics.OverlapSphere(this.rigidbody.position, pursueRadius);
		foreach(Collider c in pursueColliders){
			if(c.gameObject.GetComponent<Rigidbody>() != null){
				Vector3 pursueVec = c.gameObject.rigidbody.position - this.rigidbody.position;
				pursueVec = pursueVec.normalized * pursuePower;
				vel += pursueVec;
			}
		}
		//but not too close
		Collider[] evadeColliders = Physics.OverlapSphere(this.rigidbody.position, evadeRadius);
		foreach(Collider c in evadeColliders){
			if(c.gameObject.tag.Equals("flocket")){
				Vector3 evadeVec = c.rigidbody.position - this.rigidbody.position;
				float relativePower = evadeRadius - evadeVec.magnitude;
				evadeVec.Normalize();
				vel -= evadeVec*evadePower*relativePower;
			}
		}
		
		
		//got toward target.
		Vector3 targetVec = targetPos - this.rigidbody.position;
		if(targetVec.magnitude > maxPursueVelocity){
			targetVec = targetVec.normalized*maxPursueVelocity;
		}

		vel += targetVec;
		
		Quaternion curRotation = transform.rotation;
		Vector3 temp = Vector3.Cross(Vector3.up, vel);
		Quaternion targetRotation = new Quaternion(temp.x, temp.y, temp.z, 0);
		targetRotation.w = Mathf.Sqrt(Mathf.Pow(Vector3.Magnitude(Vector3.up), 2) * Mathf.Pow(Vector3.Magnitude(vel), 2) + 
		                              Vector3.Dot(Vector3.up, vel));
		

		Quaternion tempQuat = Quaternion.RotateTowards(curRotation, targetRotation, 1);
		if(!double.IsNaN(tempQuat.w)){
			transform.rotation = new Quaternion(0, 0, 0, 0);
			this.rigidbody.AddRelativeForce(acceleration * vel);
			transform.rotation = tempQuat;
		}
	}
}
