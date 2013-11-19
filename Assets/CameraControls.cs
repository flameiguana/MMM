using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {
	// camera position "constants"
	private Vector3 BIRD_VEC { get { return new Vector3(0, 20, 0); } }
	private Vector3 DIAGONAL_VEC { get { return new Vector3(0, 20, 20); } } 
	private Vector3 SIDE_VEC { get { return new Vector3(0, 0, 20); } }

	// current zoom vector
	private Vector3 zoomVec = Vector3.zero;
	private const float moveSpeed = 10;

	// Use this for initialization
	void Start() {
		SideView();
	}
	
	// Update is called once per frame
	void Update() {
		// handle zooming
		float vDir = 0;
		if (Input.GetKey("[+]")) vDir = 1;
		if (Input.GetKey("[-]")) vDir = -1;
		Camera.main.transform.position += zoomVec * moveSpeed * vDir * Time.deltaTime;

		// check for change in camera control
		if (Camera.main != null)
		{
			if (Input.GetKeyDown("[7]")) {
				BirdsEyeView();
			} else if (Input.GetKeyDown("[8]")) {
				SideView();
			} else if (Input.GetKeyDown("[9]")) {
				DiagonalView();
			} else {
				// do nothing
			}
		}
	}

	void BirdsEyeView() {
		Camera.main.transform.position = BIRD_VEC;
		Camera.main.transform.LookAt(Vector3.zero);

		zoomVec = new Vector3(0, 1, 0);
	}

	void DiagonalView() {
		Camera.main.transform.position = DIAGONAL_VEC;
		Camera.main.transform.LookAt(Vector3.zero);

		zoomVec = new Vector3(0, 1, 1);
	}

	void SideView() {
		Camera.main.transform.position = SIDE_VEC;
		Camera.main.transform.LookAt(Vector3.zero);

		zoomVec = new Vector3(0, 0, 1);
	}
}