using UnityEngine;
using System.Collections;

public class MissleLauncher : MonoBehaviour {
	public GameObject Target;

	public int numMissiles = 8;
	public float launchRadius = 5;

	private GameObject flocket;
	private GameObject seekMissile;
	private GameObject decoyMissile;
	private GameObject evadeMissile;

	void Start () {
		flocket = (GameObject)Resources.Load("Flocket");
		seekMissile = (GameObject)Resources.Load("SeekMissile");
		decoyMissile = (GameObject)Resources.Load("DecoyMissile");
		evadeMissile = (GameObject)Resources.Load("Evaderz");
	}
	
	// Update is called once per frame

	void Update () {
		//flock!
		if(Input.GetKeyDown(KeyCode.F)){
			for(int i = 0; i < numMissiles; i++){
				float angle = (float)(i)/(float)(numMissiles)*Mathf.PI*2;
				Vector3 newPosition = transform.localPosition;
				newPosition.x = transform.localPosition.x + Mathf.Cos(angle)*launchRadius - 1 + 2*Random.value;
				newPosition.y = transform.localPosition.y + Mathf.Sin(angle)*launchRadius - 1 + 2*Random.value;
				GameObject temp = (GameObject)Instantiate(flocket, newPosition, transform.rotation);

				//this script needs to be hand changed depending on the target, since we're not using rigidbodies
				temp.GetComponent<Flock>().target = Target.GetComponent<CircleFlight>();
			}
		}

		//seek missile
		if(Input.GetKeyDown(KeyCode.S)){
			float angle = (float)(Random.value)/(float)(numMissiles)*Mathf.PI*2;
			Vector3 newPosition = transform.localPosition;
			newPosition.x = transform.localPosition.x + Mathf.Cos(angle)*launchRadius;
			newPosition.y = transform.localPosition.y + Mathf.Sin(angle)*launchRadius;
			GameObject temp = (GameObject)Instantiate(seekMissile, newPosition, transform.rotation);
			
			//this script needs to be hand changed depending on the target, since we're not using rigidbodies
			temp.GetComponent<SimpleSeeking>().target = Target.GetComponent<CircleFlight>();
		}

		//decoy missile
		if(Input.GetKeyDown(KeyCode.Z)){
			float angle = (float)(Random.value)/(float)(numMissiles)*Mathf.PI*2;
			Vector3 newPosition = transform.localPosition;
			newPosition.x = transform.localPosition.x + Mathf.Cos(angle)*launchRadius;
			newPosition.y = transform.localPosition.y + Mathf.Sin(angle)*launchRadius;
			GameObject temp = (GameObject)Instantiate(decoyMissile, newPosition, transform.rotation);
			
			//this script needs to be hand changed depending on the target, since we're not using rigidbodies
			temp.GetComponent<DecoyMissile>().target = Target.GetComponent<CircleFlight>();
		}

		//evade missile
		if(Input.GetKeyDown(KeyCode.E)){
			float angle = (float)(Random.value)/(float)(numMissiles)*Mathf.PI*2;
			Vector3 newPosition = transform.localPosition;
			newPosition.x = transform.localPosition.x + Mathf.Cos(angle)*launchRadius;
			newPosition.y = transform.localPosition.y + Mathf.Sin(angle)*launchRadius;
			GameObject temp = (GameObject)Instantiate(evadeMissile, newPosition, transform.rotation);
			
			//this script needs to be hand changed depending on the target, since we're not using rigidbodies
			temp.GetComponent<Evade>().target = Target.GetComponent<CircleFlight>();
		}

	}
}
