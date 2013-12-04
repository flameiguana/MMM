using UnityEngine;
using UnityEditor;
using System.Collections;

public class Flock : MonoBehaviour, IVehicle {
	
	public IVehicle target;
	public GameObject effect;
	
	private static readonly float pursueRadius = 20;
	private static readonly float evadeRadius = 10;
	private static readonly float evadePower = .3f;
	private static readonly float acceleration = 35;

	private Vector3 targetPos;


	//Implement all getters and setters.
	public float maxForce {get; set;}
	public float maxSpeed{get; set;}
	public float mass{get; set;}
	public float radius{get; set;}

	public Vector3 position{get; set;}
	public Vector3 velocity{get; set;}


	// Use this for initialization
	void Start () {
		GameObject other = GameObject.Find("Cube");
		//Obtains component BackForth from Cube, which also implements IVehicle
		target = other.GetComponent<BackForth>();

		this.rigidbody.freezeRotation = true;
		targetPos = target.position;

		//Get specific cube object
	}

	void OnCollisionEnter(Collision other){

		Destroy(gameObject);
		Instantiate(effect, this.rigidbody.position, Quaternion.identity);
		Debug.Log ("Flocket Collided");	
	}

	// Update is called once per frame
	void Update () {
		
		//stay near its friends
		Vector3 vel = Vector3.zero;
		Collider[] pursueColliders = Physics.OverlapSphere(this.rigidbody.position, pursueRadius);
		foreach(Collider c in pursueColliders){
			if(c.gameObject.GetComponent<Rigidbody>() != null){
				Vector3 pursueVec = c.gameObject.rigidbody.position - this.rigidbody.position;
				pursueVec.Normalize();
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
		targetVec.Normalize();
		vel += targetVec;
		
		Quaternion curRotation = transform.rotation;
		Vector3 temp = Vector3.Cross(Vector3.up, targetVec);
		Quaternion targetRotation = new Quaternion(temp.x, temp.y, temp.z, 0);
		targetRotation.w = Mathf.Sqrt(Mathf.Pow(Vector3.Magnitude(Vector3.up), 2) * Mathf.Pow(Vector3.Magnitude(targetVec), 2) + 
		                              Vector3.Dot(Vector3.up, targetVec));
		
		
		transform.rotation = new Quaternion(0, 0, 0, 0);
		this.rigidbody.AddRelativeForce(acceleration * vel);
		transform.rotation = Quaternion.RotateTowards(curRotation, targetRotation, 1);
	}
}
