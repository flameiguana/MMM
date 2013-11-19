using UnityEngine;
using System.Collections;

//Put anything you find useful here
public interface IVehicle {

	float maxForce {get; set;}
	float maxSpeed{get; set;}
	float mass{get; set;}
	float radius{get; set;}
	
	Vector3 velocity{get; set;}
	Vector3 position{get; set;}
	
}
