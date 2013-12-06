using UnityEngine;
using UnityEditor;
using System.Collections;

public class Flock : MonoBehaviour, IVehicle {
	
	public IVehicle target;
	public GameObject effect;
	
	private static readonly float pursueRadius = 30;
	private static readonly float evadeRadius = 10;
	private static readonly float stopFlockingRadius = 50;
	private static readonly float evadePower = .6f;
	private static readonly float pursuePower = .5f;
	private static readonly float acceleration = 10;
	private static readonly float maxPursueVelocity = 10;
	private static readonly float timeout = 2;

	private Vector3 targetPos;
	private float initialDistFromTarget;
	private bool reachedTarget;
	private bool changedTargets;
	private float startTime;


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
		startTime = Time.time;
	}

	void OnTriggerEnter(Collider other){
		if(changedTargets) return;
		if(other.gameObject.tag == "Decoy"){
			DecoyMissile decoy = other.gameObject.GetComponent<DecoyMissile>();
			decoy.follow_count += 1;
			target = decoy;
			changedTargets = true;
		}
	}

	void OnCollisionEnter(Collision other){
		Destroy(gameObject);
		Instantiate(effect, this.rigidbody.position, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
		//check if it's target is null. If so, dont move
		if(target == null || target.vehicleGameObject == null){
			return;
		}
		//set the targetPos if it hasn't been set yet
		if(targetPos == Vector3.zero){
			initialDistFromTarget = (targetPos - this.position).magnitude;
		}
		targetPos = target.position;

		//check if the flocket has timed out. If so, destroy itself
		if(Time.time - startTime > timeout){
			Destroy(gameObject);
			Instantiate(effect, this.rigidbody.position, Quaternion.identity);
		}

		//see if we can explode it when it gets too far away
		if((targetPos - this.position).magnitude < initialDistFromTarget*.25)
			reachedTarget = true;

		//check if its overshot it's target after getting close
		if((targetPos - this.position).magnitude > initialDistFromTarget*.3 && reachedTarget){
			Destroy(gameObject);
			Instantiate(effect, this.rigidbody.position, Quaternion.identity);
			Debug.Log("overshot");
		}

		Vector3 vel = Vector3.zero;

		//if you keep flocking too close to the target, you'll miss
		if((targetPos - this.position).magnitude > stopFlockingRadius){
			//stay near its friends
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
		}
		
		//go toward target.
		Vector3 targetVec = (targetPos - this.rigidbody.position).normalized*maxPursueVelocity;
		vel += targetVec;

		//go away from obstacles
		Vector3 fwd = transform.TransformDirection(Vector3.up);
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, fwd, out hitInfo, 250)){
			if(hitInfo.collider.gameObject != target.vehicleGameObject){
				Vector3 offset = transform.position - hitInfo.point;
				Vector3 avoider = Vector3.ClampMagnitude(Vector3.Cross(offset, hitInfo.point), maxPursueVelocity);
				vel += avoider;
				print("There is something in front of the object!");
			}
			else{
				Debug.Log("I made a mistake");
			}
		}


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
