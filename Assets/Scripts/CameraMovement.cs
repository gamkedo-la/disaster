using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float movementSpeed = 800.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Input.GetAxis ("Horizontal") * transform.right * Time.deltaTime * movementSpeed;
		transform.position += Input.GetAxis ("Vertical") * transform.forward * Time.deltaTime * movementSpeed;
		float yHere = Terrain.activeTerrain.SampleHeight (transform.position);
		Vector3 fixedHeight = transform.position;
		fixedHeight.y = yHere;
		transform.position = fixedHeight;
	}
}
