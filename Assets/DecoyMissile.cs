using UnityEngine;
using System.Collections;

public class DecoyMissile : MonoBehaviour, IVehicle {
  // implement IVehicle interface
  public float maxForce { get; set; }
  public float maxSpeed { get; set; }
  public float mass { get; set; }
  public float radius { get; set; }
  public Vector3 position { get; set; }
  public Vector3 velocity { get; set; }

  public IVehicle target;
  public GameObject effect;
  public float SEEK_SPEED = 7.5f;

  public int follow_count { get; set; }
  public int MAX_FOLLOW = 3;
	
  void Start() {
    follow_count = 0;
    maxSpeed = SEEK_SPEED;
    mass = 1.0f;
    
    // find a target TODO: dynamic targeting
    GameObject other = GameObject.Find("Seeker");
    target = other.GetComponent<SimpleSeeking>();
    // TODO: add explosion effect
  }
	
  void OnCollisionEnter(Collision other){
    Destroy(gameObject);
    Instantiate(effect, position, Quaternion.identity);
    Debug.Log("Decoy missile collided");
  }
	
  void Update () {
    Vector3 steering_force, acceleration;
    if (follow_count < MAX_FOLLOW) {
      steering_force = SteeringForces.seek(this, target.position);
    } else {
      steering_force = SteeringForces.flee(this, target.position);
    }
 
    acceleration = steering_force/mass;
    velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);
		
    // make this fps independent
    transform.position = transform.position + velocity * Time.deltaTime;
    position = transform.position; // update for use in steering functions
		
    // update rotations
    transform.up = velocity.normalized;
  }
}