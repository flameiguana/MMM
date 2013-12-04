using UnityEngine;
using System.Collections;

public class MissleLauncher : MonoBehaviour {
	public GameObject SeekMPrefab;
	public GameObject Target;

	public int numMissiles = 8;
	public float launchRadius = 5;


	void Start () {
		//SeekMPrefab = GameObject.Find("SeekRocket"); want a general prefab
	}
	
	// Update is called once per frame

	void Update () {
		if(Input.GetKeyDown(KeyCode.D)){
			for(int i = 0; i < numMissiles; i++){
				float angle = (float)(i)/(float)(numMissiles)*Mathf.PI*2;
				Vector3 newPosition = transform.localPosition;
				newPosition.x = transform.localPosition.x + Mathf.Cos(angle)*launchRadius;
				newPosition.y = transform.localPosition.y + Mathf.Sin(angle)*launchRadius;
				GameObject temp = (GameObject)Instantiate(SeekMPrefab, newPosition, transform.rotation);

				//this script needs to be hand changed depending on the target, since we're not using rigidbodies
				temp.GetComponent<Flock>().target = Target.GetComponent<BackForth>();
			}
		}
	}
}
