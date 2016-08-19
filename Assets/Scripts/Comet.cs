using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Comet : MonoBehaviour {

	public float cometSpeed = 0.03f;

	private float Nz;
	private float Ex;
	private float Sz;
	private float Wx;

	float eastWestDirection = 1.0f;
	float northSouthDirection = 1.0f;
	List<float> boundries;

	// Use this for initialization
	void Start () {
		boundries = WorldBounds.instance.Boundries ();
		Nz = boundries [0];
		Ex = boundries [1];
		Sz = boundries [3];
		Wx = boundries [4];
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * cometSpeed * eastWestDirection);
		transform.Translate (Vector3.forward * Time.deltaTime * cometSpeed * northSouthDirection);

		if (transform.position.z < Nz) {
			northSouthDirection *= -1;
		}
		if (transform.position.x > Ex) {
			eastWestDirection *= -1;
		}
		if (transform.position.z > Sz) {
			northSouthDirection *= -1;
		}
		if (transform.position.x < Wx) {
			eastWestDirection *= -1;
		}
	}
}
