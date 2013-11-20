using UnityEngine;
using UnityEditor;
using System.Collections;

public class BasicMovement : MonoBehaviour {
	
	public Vector3 targetPos;
	public float acceleration;
	
	private static readonly float pursueRadius = 20;
	private static readonly float evadeRadius = 10;
	private GameObject myObjectVisual;
	
	// Use this for initialization
	void Start () {
		this.rigidbody.freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		//stay near its friends
		Vector3 vel = Vector3.zero;
		Collider[] pursueColliders = Physics.OverlapSphere(this.rigidbody.position, pursueRadius);
		foreach(Collider c in pursueColliders){
			Vector3 pursueVec = c.rigidbody.position - this.rigidbody.position;
			pursueVec.Normalize();
			vel += pursueVec;
			//vel = Quaternion.Inverse(this.rigidbody.rotation)*vel;
		}
		//but not too close
		Collider[] evadeColliders = Physics.OverlapSphere(this.rigidbody.position, evadeRadius);
		foreach(Collider c in evadeColliders){
			Vector3 evadeVec = c.rigidbody.position - this.rigidbody.position;
			evadeVec.Normalize();
			vel -= evadeVec*1.5f;
			//vel = Quaternion.Inverse(this.rigidbody.rotation)*vel;
		}
		
		
		//got toward target.
		Vector3 targetVec = targetPos - this.rigidbody.position;
		targetVec.Normalize();
		vel += targetVec;
		//vel += Vector3.up;
		
		Quaternion curRotation = transform.rotation;
		Vector3 temp = Vector3.Cross(Vector3.up, targetVec);
		Quaternion targetRotation = new Quaternion(temp.x, temp.y, temp.z, 0);
		targetRotation.w = Mathf.Sqrt(Mathf.Pow(Vector3.Magnitude(Vector3.up), 2) * Mathf.Pow(Vector3.Magnitude(targetVec), 2) + 
			Vector3.Dot(Vector3.up, targetVec));
		
		
		transform.rotation = new Quaternion(0, 0, 0, 0);
		this.rigidbody.AddRelativeForce(acceleration * vel);
		transform.rotation = Quaternion.RotateTowards(curRotation, targetRotation, 10);
		
		
		//int i = 0;
		//PrefabType i = PrefabUtility.GetPrefabType(nearbyColliders[0].gameObject);
		//Debug.Log(i.ToString());
		//string s = i.ToString();
		
		
		//vel = this.rigidbody.velocity;
		//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rigidbody.velocity, Vector3.up), Time.deltaTime * 10f);
		//Vector3 rot = new Vector3(vel.y, vel.x, 0);
		//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rot, Vector3.up), Time.deltaTime * 100f);
	}
}
