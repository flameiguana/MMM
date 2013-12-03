﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public class Flock : MonoBehaviour, IVehicle {
	
	public Vector3 targetPos;
	public float acceleration;
	
	public GameObject effect;
	
	private static readonly float pursueRadius = 20;
	private static readonly float evadeRadius = 10;
	private static readonly float evadePower = .3f;

	//Implement all getters and setters.
	public float maxForce {get; set;}
	public float maxSpeed{get; set;}
	public float mass{get; set;}
	public float radius{get; set;}

	public Vector3 position{get; set;}
	public Vector3 velocity{get; set;}


	// Use this for initialization
	void Start () {
		this.rigidbody.freezeRotation = true;
	}

	void OnCollisionEnter(Collision other){
		Destroy(gameObject);
		Instantiate(effect, this.rigidbody.position, Quaternion.identity);
		Debug.Log ("Collided");	
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
		}
		//but not too close
		Collider[] evadeColliders = Physics.OverlapSphere(this.rigidbody.position, evadeRadius);
		foreach(Collider c in evadeColliders){
			Vector3 evadeVec = c.rigidbody.position - this.rigidbody.position;
			float relativePower = evadeRadius - evadeVec.magnitude;
			evadeVec.Normalize();
			vel -= evadeVec*evadePower*relativePower;
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
