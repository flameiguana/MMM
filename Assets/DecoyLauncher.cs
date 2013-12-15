using UnityEngine;
using System.Collections;

public class DecoyLauncher : MonoBehaviour {
  public GameObject DecoyMPrefab;
  public GameObject targetObject; 
  public CameraControls camera;
  public float launchRadius = 5;
  public float RELOAD_SPEED = 1.0f;
  private float reload_time;

  void Start () {
    //SeekMPrefab = GameObject.Find("SeekRocket"); want a general prefab
    reload_time = 0.0f;
  }
	
  // Update is called once per frame
  void Update () {
    if (reload_time > RELOAD_SPEED) {
      if (Input.GetKeyDown(KeyCode.D)) {
		float angle = (float)(Random.value)/Mathf.PI*2;
		Vector3 newPosition = transform.localPosition;
		Debug.Log (transform.localPosition);
		newPosition.x = transform.localPosition.x + Mathf.Cos(angle)*launchRadius - 1 + 2*Random.value;
		newPosition.y = transform.localPosition.y + Mathf.Sin(angle)*launchRadius - 1 + 2*Random.value;
		GameObject temp = (GameObject)Instantiate(DecoyMPrefab, newPosition, transform.rotation);
		temp.GetComponent<DecoyMissile>().target = targetObject.GetComponent<CircleFlight>();
        reload_time = 0.0f;
		camera.target = temp.GetComponent<DecoyMissile>();
      }
    } else {
      reload_time += Time.deltaTime;
    }
  }
}
