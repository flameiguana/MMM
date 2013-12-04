using UnityEngine;
using System.Collections;

public class DecoyLauncher : MonoBehaviour {
  public GameObject DecoyMPrefab;
  public float RELOAD_SPEED = 1.0f;
  private float reload_time;

  void Start () {
    //SeekMPrefab = GameObject.Find("SeekRocket"); want a general prefab
    reload_time = 0.0f;
  }
	
  // Update is called once per frame
  void Update () {
    if (reload_time > RELOAD_SPEED) {
      if (Input.GetKeyDown(KeyCode.F)) {
        Instantiate(DecoyMPrefab, transform.position, transform.rotation);
        reload_time = 0.0f;
      }
    } else {
      reload_time += Time.deltaTime;
    }
  }
}
