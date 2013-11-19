using UnityEngine;
using System.Collections;

public class BackForth : MonoBehaviour, IVehicle{
	
	public float maxSpeed {get; set;}
	public float mass {get; set;}
	public float radius {get; set;}
	
	public float maxForce {get; set;}
	public Vector3 position{get; set;}
	public Vector3 velocity{get; set;}
	
	Vector3 pointB;
	
	public float smooth = .1f;
	
	// Use this for initialization
	void Start () {
		pointB = transform.position;
		pointB.y += 10;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, pointB, smooth * Time.deltaTime);
		position = transform.position;
	}
}
