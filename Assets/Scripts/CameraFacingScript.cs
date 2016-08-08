using UnityEngine;
using System.Collections;

public class CameraFacingScript : MonoBehaviour {

    Camera camToFace;
	// Use this for initialization
	void Start () {
        camToFace = GameObject.Find("Camera (eye)").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 lookToward = camToFace.transform.position;
        lookToward.y = transform.position.y;
        transform.LookAt(lookToward);
        transform.Rotate(Vector3.up, 180.0f);
	}
}
