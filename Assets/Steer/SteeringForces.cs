using UnityEngine;
using System.Collections;


//A list of functions for calculating forces
public class SteeringForces {
	
	//The simple seek
	public static Vector3 seek(IVehicle self, Vector3 target){
		Vector3 desiredVelocity = Vector3.Normalize(target - self.position) * self.maxSpeed;
		//return the difference needed to reach that velocity
    	return desiredVelocity - self.velocity; 
	}
	
	
	
}
