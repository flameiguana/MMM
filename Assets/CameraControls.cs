using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
  // camera position "constants"
  private Vector3 BIRD_VEC { get { return new Vector3(0, 20, 0); } }
  private Vector3 DIAGONAL_VEC { get { return new Vector3(0, 20, 20); } } 
  private Vector3 SIDE_VEC { get { return new Vector3(0, 0, 20); } }

  // zoom vector "constants"
  private Vector3 BIRD_ZOOM { get { return new Vector3(0, 1, 0); } }
  private Vector3 DIAGONAL_ZOOM { get { return new Vector3(0, 1, 1); } }
  private Vector3 SIDE_ZOOM { get { return new Vector3(0, 0, 1); } }

  // current zoom vector
  private Vector3 zoom_vec = Vector3.zero;
  private const float move_speed = 10;

  // camera mode controls
  private bool missile_destroyed;
  private int STATIC = 0, DYNAMIC = 1;
  private int mode;  
  private SimpleSeeking target;
  public GameObject missile;

  // Use this for initialization
  void Start() {
    target = null;
    BirdsEyeView();
    missile_destroyed = false;
  }
	
  // Update is called once per frame
  void Update() {
    // handle zooming
    float vDir = 0;
    if (Input.GetKey("a")) vDir = 1;
    if (Input.GetKey("b")) vDir = -1;
    transform.position += zoom_vec * move_speed * vDir * Time.deltaTime;

    // check for change in camera control
    if (Camera.main != null) {
        if (Input.GetKeyDown("2")) {
          BirdsEyeView();
        } else if (Input.GetKeyDown("3")) {
          SideView();
        } else if (Input.GetKeyDown("4")) {
          DiagonalView();
        } else if (mode == DYNAMIC || Input.GetKeyDown("1")) {
          MissileView();
        } else {
          // static camera, don't need to do anything
        }  
      }
  }

  void BirdsEyeView() {
    transform.position = BIRD_VEC;
    transform.LookAt(Vector3.zero);

    mode = STATIC;
    target = null;
    zoom_vec = BIRD_ZOOM;
  }

  void DiagonalView() {
    transform.position = DIAGONAL_VEC;
    transform.LookAt(Vector3.zero);

    mode = STATIC;
    target = null;
    zoom_vec = DIAGONAL_ZOOM;
  }

  void SideView() {
    transform.position = SIDE_VEC;
    transform.LookAt(Vector3.zero);

    mode = STATIC;
    target = null;
    zoom_vec = SIDE_ZOOM;
  }

  void MissileView() {
    // the distance behind the target
    float distance = 1.0f;
    // the height we want the camera to be above the target
    float height = 0.3f;

    if (target == null) {
      // find a random missile TODO: fix
      if (missile_destroyed == true) {
        missile_destroyed = false;
        SideView();
      } else {
        GameObject obj = (GameObject)Instantiate(missile, transform.position, transform.rotation);
        target = obj.GetComponent<SimpleSeeking>();
        missile_destroyed = true;
      }
    }
	
    // set the position of the camera
    transform.position = new Vector3(target.position.x, target.position.y + height, 
                                     target.position.z + distance);
    
    // set the look at point
    //transform.LookAt(target.position + target.velocity, new Vector3(0,1,0));
    transform.LookAt(target.position + target.velocity, target.transform.forward);

    // reorient the camera
    Quaternion reorient = Quaternion.LookRotation(transform.forward, new Vector3(0,1,0));
    transform.rotation = reorient;

    mode = DYNAMIC;
    zoom_vec = transform.forward;
  }
}